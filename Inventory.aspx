<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="GroceryStoreApp.Inventory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div style="font-family: Arial; padding: 20px;">
    <h1>Grocery Store Manager</h1>

    <fieldset style="background-color: #e6f7ff; padding: 10px;">
        <legend><b>Search Product</b></legend>
        Search Name: <asp:TextBox ID="txtSearch" runat="server" placeholder="e.g. Oil"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        <asp:Button ID="btnReset" runat="server" Text="Show All" OnClick="btnReset_Click" />
    </fieldset>
    <br />

    <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="False" 
                  DataKeyNames="ProductID"
                  OnRowDataBound="gvInventory_RowDataBound"
                  OnSelectedIndexChanged="gvInventory_SelectedIndexChanged"
                  Width="100%" CellPadding="6" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
        <Columns>
            <asp:BoundField DataField="ProductID" HeaderText="ID" />
            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
            <asp:BoundField DataField="Price" HeaderText="Price" />
            <asp:BoundField DataField="Quantity" HeaderText="Stock Qty" />
            
            <%-- The Green 'Select' Button --%>
            <asp:ButtonField Text="Select" CommandName="Select" ButtonType="Button" 
                             ControlStyle-BackColor="#4CAF50" ControlStyle-ForeColor="White" />
        </Columns>
    </asp:GridView>
    <br />

    <fieldset>
        <legend><b>Add / Update Item</b></legend>
        <asp:HiddenField ID="hfSelectedID" runat="server" />
        
        Name: <asp:TextBox ID="txtProdName" runat="server"></asp:TextBox>
        Price: <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
        Qty: <asp:TextBox ID="txtQty" runat="server"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="Add New Item" OnClick="btnAdd_Click" BackColor="#008CBA" ForeColor="White" />
    </fieldset>
    <br />

    <fieldset style="background-color: #fff4e6; padding: 10px;">
        <legend><b>Sell Item</b></legend>
        Selected ID: <asp:TextBox ID="txtSellID" runat="server" ReadOnly="true" BackColor="#E0E0E0"></asp:TextBox>
        Selected Name: <asp:TextBox ID="txtSellName" runat="server" ReadOnly="true" BackColor="#E0E0E0"></asp:TextBox>
        <br /><br />
        Quantity to Sell: <asp:TextBox ID="txtSellQty" runat="server"></asp:TextBox>
        <asp:Button ID="btnSell" runat="server" Text="Generate Bill" OnClick="btnSell_Click" BackColor="#f44336" ForeColor="White" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
    </fieldset>
</div>
    </form>
</body>
</html>
