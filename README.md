# 🛒 Grocery Store Inventory & POS System

A full-stack web application designed to manage grocery store operations, including inventory tracking, billing, and stock management. Built using **ASP.NET (Web Forms)** and **ADO.NET** for high-performance, raw SQL interaction without ORM overhead.

## 🚀 Key Features
* **Smart Inventory Management:** Prevents data redundancy by automatically detecting if a product exists and updating stock instead of creating duplicates.
* **Point of Sale (POS) Billing:** A "Search & Select" interface allows for fast billing without memorizing Product IDs.
* **Real-Time Stock Alerts:** Visual indicators (Red Highlights) in the grid automatically flag items with low stock (< 5 units).
* **Direct SQL Performance:** Utilizes `SqlDataReader` and `SqlParameter` for secure, fast data retrieval.

## 📸 Project Screenshots

### 1. Smart Dashboard & Low Stock Alert
*The system highlights low-stock items in RED for immediate attention.*
![Dashboard View]
<img width="1911" height="1017" alt="Screenshot 2026-01-29 231650" src="https://github.com/user-attachments/assets/3a4a268a-75b2-4055-8e63-2d63238b8883" />


### 2. Point of Sale (POS) Logic
*Clicking "Select" auto-fills the billing section and switches the system to "Update Mode".*
![Billing Logic]
<img width="1919" height="1015" alt="Screenshot 2026-01-29 231842" src="https://github.com/user-attachments/assets/ce48a2ee-5906-4b3c-94f2-6afa8a2a877c" />

## 🛠 Tech Stack
* **Frontend:** ASP.NET, HTML, CSS
* **Backend:** C#, ADO.NET
* **Database:** Microsoft SQL Server (MSSQL)
* **Tools:** Visual Studio 2022

## ⚙️ How to Run
1.  Clone the repository.
2.  Run the SQL script (included) to create the database.
3.  Update `Web.config` with your Server Name.
4.  Press **Ctrl+F5** in Visual Studio.
