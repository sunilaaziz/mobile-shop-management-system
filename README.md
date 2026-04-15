# 📱 MobileZone Pro — Mobile Shop Management System

## 📌 Project Overview
MobileZone Pro is a **console-based mobile shop management system** developed in **C# (.NET)**.  
It is designed to manage a complete mobile shop including:

- Inventory management  
- Customer management  
- Sales & billing system  
- Repair service management  
- Business reporting & analytics  

This project uses an **in-memory database (List collections)** so no SQL Server or external database is required.

---

## 🚀 Features

### 📦 Inventory Management
- Add new products
- Edit / delete products
- View all inventory
- Search products
- Low stock alerts
- Restock system

### 👤 Customer Management
- Add / edit customers
- Search customers
- View customer purchase history
- View repair history

### 🧾 Sales & Billing System
- Create new sales (invoices)
- Auto bill generation
- Multiple payment methods:
  - Cash
  - Card
  - EasyPaisa
  - JazzCash
- Track paid & pending sales

### 🔧 Repair Management System
- Register repair jobs
- Update job status (Pending / In Progress / Completed / Delivered)
- Complete repair with final cost
- Track technician work

### 📊 Reports & Analytics
- Daily sales report
- Monthly revenue report
- Top selling products
- Inventory valuation
- Repair revenue report
- Full business dashboard

---

## 🛠️ Technologies Used
- **Language:** C#
- **Framework:** .NET Console Application
- **IDE:** Visual Studio
- **Database:** In-Memory Collections (List)

---

## 📂 Project Structure

```text
MobileShopManagement/
│
├── Models
│   ├── Product
│   ├── Customer
│   ├── Sale
│   ├── SaleItem
│   └── RepairJob
│
├── DB (In-memory database + Seed data)
│
├── UI (Console helper functions)
│
├── Modules
│   ├── InventoryModule
│   ├── CustomerModule
│   ├── SalesModule
│   ├── RepairModule
│   └── ReportModule
│
└── Program.cs (Main Entry Point)
