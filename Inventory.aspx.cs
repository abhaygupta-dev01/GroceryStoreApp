using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Web.UI.WebControls;

namespace GroceryStoreApp
{
    public partial class Inventory : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["GroceryConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
            }
        }

        // --- 1. SEARCH & LOAD LOGIC ---
        void LoadGrid(string searchTerm = "")
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM ProductInventory";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE ProductName LIKE @search";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvInventory.DataSource = dt;
                gvInventory.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(txtSearch.Text.Trim());
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadGrid();
            ClearInputs();
        }

        // --- 2. SELECT LOGIC (The Magic Part) ---
        protected void gvInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the hidden ID from the selected row
            string id = gvInventory.DataKeys[gvInventory.SelectedIndex].Value.ToString();

            // Get values from the grid cells (Cell 1 = Name, Cell 2 = Price)
            string name = gvInventory.SelectedRow.Cells[1].Text;
            string price = gvInventory.SelectedRow.Cells[2].Text;

            // FILL ADD/UPDATE BOXES
            hfSelectedID.Value = id;
            txtProdName.Text = name;
            txtPrice.Text = price;
            txtQty.Text = ""; // Leave quantity empty so you can type how much to add
            btnAdd.Text = "Update Stock"; // Change button text to show we are updating

            // FILL SELL BOXES
            txtSellID.Text = id;
            txtSellName.Text = name;

            lblMessage.Text = "Item Selected! You can now Sell it or Update its stock.";
            lblMessage.ForeColor = Color.Blue;
        }

        // --- 3. ADD / UPDATE LOGIC ---
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProdName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtQty.Text))
            {
                lblMessage.Text = "Please fill all boxes!";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd;

                // CHECK: Are we Updating (ID exists) or Adding New (ID is empty)?
                if (!string.IsNullOrEmpty(hfSelectedID.Value))
                {
                    // UPDATE LOGIC
                    string updateQuery = "UPDATE ProductInventory SET Quantity = Quantity + @qty, Price = @price WHERE ProductID = @id";
                    cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@id", hfSelectedID.Value);
                    lblMessage.Text = "Stock Updated Successfully!";
                }
                else
                {
                    // INSERT LOGIC
                    string insertQuery = "INSERT INTO ProductInventory (ProductName, Price, Quantity) VALUES (@name, @price, @qty)";
                    cmd = new SqlCommand(insertQuery, con);
                    lblMessage.Text = "New Product Added Successfully!";
                }

                // Common Parameters
                cmd.Parameters.AddWithValue("@name", txtProdName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQty.Text));

                cmd.ExecuteNonQuery();
            }

            ClearInputs();
            LoadGrid(txtSearch.Text); // Reload grid (keeping search results)
            lblMessage.ForeColor = Color.Green;
        }

        // --- 4. SELL LOGIC ---
        protected void btnSell_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSellID.Text))
            {
                lblMessage.Text = "Please Select an Item first!";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            int sellQty = Convert.ToInt32(txtSellQty.Text);
            int prodID = Convert.ToInt32(txtSellID.Text);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                // Check Stock
                SqlCommand checkCmd = new SqlCommand("SELECT Quantity, Price FROM ProductInventory WHERE ProductID = @id", con);
                checkCmd.Parameters.AddWithValue("@id", prodID);
                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    int currentQty = Convert.ToInt32(reader["Quantity"]);
                    decimal price = Convert.ToDecimal(reader["Price"]);
                    reader.Close();

                    if (currentQty >= sellQty)
                    {
                        // Update Stock
                        SqlCommand updateCmd = new SqlCommand("UPDATE ProductInventory SET Quantity = Quantity - @qty WHERE ProductID = @id", con);
                        updateCmd.Parameters.AddWithValue("@qty", sellQty);
                        updateCmd.Parameters.AddWithValue("@id", prodID);
                        updateCmd.ExecuteNonQuery();

                        decimal bill = price * sellQty;
                        lblMessage.Text = $"Sold! Total Bill: ₹{bill}";
                        lblMessage.ForeColor = Color.Green;

                        LoadGrid(txtSearch.Text); // Refresh grid
                    }
                    else
                    {
                        lblMessage.Text = "Error: Not enough stock!";
                        lblMessage.ForeColor = Color.Red;
                    }
                }
            }
        }

        // --- 5. RED ALERT LOGIC ---
        protected void gvInventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Cell[3] is Quantity. If < 5, turn RED.
                int quantity = Convert.ToInt32(e.Row.Cells[3].Text);
                if (quantity < 5)
                {
                    e.Row.BackColor = Color.Salmon;
                    e.Row.ForeColor = Color.White;
                }
            }
        }

        void ClearInputs()
        {
            txtProdName.Text = ""; txtPrice.Text = ""; txtQty.Text = "";
            txtSellID.Text = ""; txtSellName.Text = ""; txtSellQty.Text = "";
            hfSelectedID.Value = ""; // Reset hidden ID
            btnAdd.Text = "Add New Item"; // Reset button text
        }
    }
}