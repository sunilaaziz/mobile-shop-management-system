using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileShopManagement
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  MODELS
    // ─────────────────────────────────────────────────────────────────────────────

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public Product(int id, string name, string brand, string model,
                       double price, int stock, string category, string description = "")
        {
            Id = id; Name = name; Brand = brand; Model = model;
            Price = price; Stock = stock; Category = category; Description = description;
        }
    }

    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }

        public Customer(int id, string name, string phone, string email, string address)
        {
            Id = id; Name = name; Phone = phone; Email = email;
            Address = address; RegisteredOn = DateTime.Now;
        }
    }

    class SaleItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double SubTotal => Product.Price * Quantity;
    }

    class Sale
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        public DateTime Date { get; set; } = DateTime.Now;
        public double Total => Items.Sum(i => i.SubTotal);
        public string PayMethod { get; set; }
        public bool Paid { get; set; }
    }

    class RepairJob
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public string DeviceName { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed, Delivered
        public double EstCost { get; set; }
        public double FinalCost { get; set; }
        public DateTime DateIn { get; set; } = DateTime.Now;
        public DateTime? DateOut { get; set; }
        public string Technician { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  DATABASE (in-memory store)
    // ─────────────────────────────────────────────────────────────────────────────

    static class DB
    {
        public static List<Product> Products = new List<Product>();
        public static List<Customer> Customers = new List<Customer>();
        public static List<Sale> Sales = new List<Sale>();
        public static List<RepairJob> RepairJobs = new List<RepairJob>();

        private static int _pid = 1, _cid = 1, _sid = 1, _rid = 1;
        public static int NextPID() => _pid++;
        public static int NextCID() => _cid++;
        public static int NextSID() => _sid++;
        public static int NextRID() => _rid++;

        public static void Seed()
        {
            Products.Add(new Product(NextPID(), "iPhone 15 Pro", "Apple", "A3290", 299999, 10, "Smartphone", "6.1\" Super Retina XDR display"));
            Products.Add(new Product(NextPID(), "Samsung Galaxy S24", "Samsung", "SM-S921", 195000, 15, "Smartphone", "6.2\" Dynamic AMOLED 2X"));
            Products.Add(new Product(NextPID(), "Xiaomi 14", "Xiaomi", "23127PN0CC", 92000, 8, "Smartphone", "Leica optics, Snapdragon 8 Gen 3"));
            Products.Add(new Product(NextPID(), "OnePlus 12", "OnePlus", "CPH2573", 98000, 6, "Smartphone", "Hasselblad cameras, 100W charging"));
            Products.Add(new Product(NextPID(), "Realme GT 6", "Realme", "RMX3850", 55000, 12, "Smartphone", "Snapdragon 8s Gen 3"));
            Products.Add(new Product(NextPID(), "Apple AirPods Pro 2", "Apple", "MQTP3", 45000, 20, "Accessory", "Active noise cancellation"));
            Products.Add(new Product(NextPID(), "Samsung Galaxy Buds2", "Samsung", "SM-R177", 15000, 18, "Accessory", "Compact true wireless earbuds"));
            Products.Add(new Product(NextPID(), "Anker 65W Charger", "Anker", "A2667", 3500, 30, "Charger", "GaN technology, dual USB-C"));
            Products.Add(new Product(NextPID(), "Spigen Tempered Glass", "Spigen", "AGL06832", 800, 50, "Accessory", "9H hardness screen protector"));
            Products.Add(new Product(NextPID(), "Baseus Power Bank 20K", "Baseus", "PPBD050501", 5500, 25, "Accessory", "20000mAh, 65W PD charging"));

            Customers.Add(new Customer(NextCID(), "Ali Hassan", "0300-1234567", "ali@email.com", "Block 5, Rawalpindi"));
            Customers.Add(new Customer(NextCID(), "Sara Ahmed", "0311-9876543", "sara@email.com", "G-10, Islamabad"));
            Customers.Add(new Customer(NextCID(), "Usman Khan", "0333-5552211", "usman@email.com", "Saddar, Rawalpindi"));
            Customers.Add(new Customer(NextCID(), "Ayesha Malik", "0321-7774433", "ayesha@email.com", "Blue Area, Islamabad"));
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  UI HELPERS
    // ─────────────────────────────────────────────────────────────────────────────

    static class UI
    {
        const int WIDTH = 70;

        public static void SetColor(ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
        { Console.ForegroundColor = fg; Console.BackgroundColor = bg; }

        public static void ResetColor() => Console.ResetColor();

        public static void Clear() => Console.Clear();

        public static void Header(string title)
        {
            Console.Clear();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("╔" + new string('═', WIDTH) + "╗");
            string pad = title.PadLeft((WIDTH + title.Length) / 2).PadRight(WIDTH);
            Console.WriteLine("║" + pad + "║");
            Console.WriteLine("╠" + new string('═', WIDTH) + "╣");
            string sub = "  MobileZone Pro — Shop Management System  ";
            string sub2 = sub.PadLeft((WIDTH + sub.Length) / 2).PadRight(WIDTH);
            Console.WriteLine("║" + sub2 + "║");
            Console.WriteLine("╚" + new string('═', WIDTH) + "╝");
            ResetColor();
            Console.WriteLine();
        }

        public static void SectionLine(string label = "")
        {
            SetColor(ConsoleColor.DarkCyan);
            if (string.IsNullOrEmpty(label))
                Console.WriteLine("  " + new string('─', WIDTH - 2));
            else
            {
                string line = $"  ── {label} " + new string('─', Math.Max(0, WIDTH - label.Length - 7));
                Console.WriteLine(line);
            }
            ResetColor();
        }

        public static void Success(string msg)
        { SetColor(ConsoleColor.Green); Console.WriteLine($"  ✔  {msg}"); ResetColor(); }

        public static void Error(string msg)
        { SetColor(ConsoleColor.Red); Console.WriteLine($"  ✘  {msg}"); ResetColor(); }

        public static void Info(string msg)
        { SetColor(ConsoleColor.Yellow); Console.WriteLine($"  ℹ  {msg}"); ResetColor(); }

        public static void MenuItem(int num, string label)
        {
            SetColor(ConsoleColor.DarkYellow);
            Console.Write($"  [{num}] ");
            ResetColor();
            Console.WriteLine(label);
        }

        public static void MenuItemStr(string key, string label)
        {
            SetColor(ConsoleColor.DarkYellow);
            Console.Write($"  [{key}] ");
            ResetColor();
            Console.WriteLine(label);
        }

        public static void Prompt(string msg = "Enter choice")
        {
            Console.WriteLine();
            SetColor(ConsoleColor.White);
            Console.Write($"  ► {msg}: ");
            ResetColor();
        }

        public static string ReadLine(string prompt = "Enter choice")
        {
            Prompt(prompt);
            return Console.ReadLine()?.Trim() ?? "";
        }

        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                string s = ReadLine(prompt);
                if (int.TryParse(s, out int v) && v >= min && v <= max) return v;
                Error($"Please enter a number between {min} and {max}.");
            }
        }

        public static double ReadDouble(string prompt)
        {
            while (true)
            {
                string s = ReadLine(prompt);
                if (double.TryParse(s, out double v) && v >= 0) return v;
                Error("Please enter a valid non-negative number.");
            }
        }

        public static void Pause()
        {
            Console.WriteLine();
            SetColor(ConsoleColor.DarkGray);
            Console.Write("  Press any key to continue...");
            ResetColor();
            Console.ReadKey(true);
        }

        public static void TableHeader(params (string label, int width)[] cols)
        {
            SetColor(ConsoleColor.DarkCyan);
            Console.Write("  ");
            foreach (var (label, width) in cols)
                Console.Write(label.PadRight(width));
            Console.WriteLine();
            Console.Write("  ");
            foreach (var (_, width) in cols)
                Console.Write(new string('─', width));
            Console.WriteLine();
            ResetColor();
        }

        public static void TableRow(ConsoleColor color, params (string value, int width)[] cols)
        {
            SetColor(color);
            Console.Write("  ");
            foreach (var (value, width) in cols)
                Console.Write((value ?? "").PadRight(width));
            Console.WriteLine();
            ResetColor();
        }

        public static string FormatCurrency(double amount) => $"Rs. {amount:N0}";
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  MODULES
    // ─────────────────────────────────────────────────────────────────────────────

    // ── INVENTORY ────────────────────────────────────────────────────────────────
    static class InventoryModule
    {
        public static void Menu()
        {
            while (true)
            {
                UI.Header("INVENTORY MANAGEMENT");
                UI.SectionLine("Products");
                UI.MenuItem(1, "View All Products");
                UI.MenuItem(2, "Add New Product");
                UI.MenuItem(3, "Edit Product");
                UI.MenuItem(4, "Delete Product");
                UI.MenuItem(5, "Search Product");
                UI.MenuItem(6, "Low Stock Alert");
                UI.MenuItem(7, "Restock Product");
                UI.SectionLine();
                UI.MenuItemStr("0", "Back to Main Menu");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": ViewAll(); break;
                    case "2": AddProduct(); break;
                    case "3": Edit(); break;
                    case "4": Delete(); break;
                    case "5": Search(); break;
                    case "6": LowStock(); break;
                    case "7": Restock(); break;
                    case "0": return;
                    default: UI.Error("Invalid choice."); UI.Pause(); break;
                }
            }
        }

        static void ViewAll(List<Product> list = null)
        {
            UI.Header("PRODUCT CATALOG");
            var products = list ?? DB.Products;
            if (!products.Any()) { UI.Info("No products found."); UI.Pause(); return; }

            UI.TableHeader(("ID", 5), ("Name", 24), ("Brand", 12), ("Category", 12), ("Price (Rs.)", 14), ("Stock", 7));
            foreach (var p in products)
            {
                var color = p.Stock <= 3 ? ConsoleColor.Red : p.Stock <= 8 ? ConsoleColor.Yellow : ConsoleColor.White;
                UI.TableRow(color,
                    (p.Id.ToString(), 5), (p.Name, 24), (p.Brand, 12),
                    (p.Category, 12), (p.Price.ToString("N0"), 14), (p.Stock.ToString(), 7));
            }
            UI.SectionLine();
            UI.Info($"Total products: {products.Count}  |  Total stock value: {UI.FormatCurrency(products.Sum(p => p.Price * p.Stock))}");
            UI.Pause();
        }

        static void AddProduct()
        {
            UI.Header("ADD NEW PRODUCT");
            string name = UI.ReadLine("Product Name");
            string brand = UI.ReadLine("Brand");
            string model = UI.ReadLine("Model Number");
            double price = UI.ReadDouble("Price (Rs.)");
            int stock = UI.ReadInt("Initial Stock", 0);
            Console.WriteLine();
            Console.WriteLine("  Categories: 1-Smartphone  2-Tablet  3-Accessory  4-Charger  5-Other");
            int cat = UI.ReadInt("Category", 1, 5);
            string[] cats = { "Smartphone", "Tablet", "Accessory", "Charger", "Other" };
            string desc = UI.ReadLine("Description (optional)");

            var p = new Product(DB.NextPID(), name, brand, model, price, stock, cats[cat - 1], desc);
            DB.Products.Add(p);
            UI.Success($"Product '{name}' added with ID #{p.Id}.");
            UI.Pause();
        }

        static void Edit()
        {
            UI.Header("EDIT PRODUCT");
            if (!DB.Products.Any()) { UI.Error("No products available."); UI.Pause(); return; }
            int id = UI.ReadInt("Enter Product ID to edit");
            var p = DB.Products.FirstOrDefault(x => x.Id == id);
            if (p == null) { UI.Error("Product not found."); UI.Pause(); return; }

            UI.Info($"Editing: {p.Name} | Current Price: {UI.FormatCurrency(p.Price)} | Stock: {p.Stock}");
            Console.WriteLine("  (Press Enter to keep current value)");

            string name = UI.ReadLine($"Name [{p.Name}]");
            string brand = UI.ReadLine($"Brand [{p.Brand}]");
            string priceStr = UI.ReadLine($"Price [{p.Price}]");
            string stockStr = UI.ReadLine($"Stock [{p.Stock}]");

            if (!string.IsNullOrEmpty(name)) p.Name = name;
            if (!string.IsNullOrEmpty(brand)) p.Brand = brand;
            if (double.TryParse(priceStr, out double pr)) p.Price = pr;
            if (int.TryParse(stockStr, out int st)) p.Stock = st;

            UI.Success("Product updated successfully.");
            UI.Pause();
        }

        static void Delete()
        {
            UI.Header("DELETE PRODUCT");
            int id = UI.ReadInt("Enter Product ID to delete");
            var p = DB.Products.FirstOrDefault(x => x.Id == id);
            if (p == null) { UI.Error("Product not found."); UI.Pause(); return; }

            string confirm = UI.ReadLine($"Delete '{p.Name}'? (yes/no)");
            if (confirm.ToLower() == "yes")
            {
                DB.Products.Remove(p);
                UI.Success("Product deleted.");
            }
            else UI.Info("Operation cancelled.");
            UI.Pause();
        }

        static void Search()
        {
            UI.Header("SEARCH PRODUCT");
            string query = UI.ReadLine("Search (name / brand / category)").ToLower();
            var results = DB.Products.Where(p =>
                p.Name.ToLower().Contains(query) ||
                p.Brand.ToLower().Contains(query) ||
                p.Category.ToLower().Contains(query)).ToList();

            if (!results.Any()) UI.Error("No matching products.");
            else ViewAll(results);
        }

        static void LowStock()
        {
            UI.Header("LOW STOCK ALERT");
            int threshold = UI.ReadInt("Low-stock threshold (units)", 1, 100);
            var low = DB.Products.Where(p => p.Stock <= threshold).OrderBy(p => p.Stock).ToList();
            if (!low.Any()) { UI.Success("All products are adequately stocked."); UI.Pause(); return; }
            UI.Info($"{low.Count} product(s) at or below {threshold} units:");
            ViewAll(low);
        }

        static void Restock()
        {
            UI.Header("RESTOCK PRODUCT");
            int id = UI.ReadInt("Enter Product ID");
            var p = DB.Products.FirstOrDefault(x => x.Id == id);
            if (p == null) { UI.Error("Product not found."); UI.Pause(); return; }

            UI.Info($"Current stock for '{p.Name}': {p.Stock}");
            int qty = UI.ReadInt("Units to add", 1);
            p.Stock += qty;
            UI.Success($"Stock updated. New total: {p.Stock} units.");
            UI.Pause();
        }
    }

    // ── CUSTOMERS ────────────────────────────────────────────────────────────────
    static class CustomerModule
    {
        public static void Menu()
        {
            while (true)
            {
                UI.Header("CUSTOMER MANAGEMENT");
                UI.SectionLine("Options");
                UI.MenuItem(1, "View All Customers");
                UI.MenuItem(2, "Add New Customer");
                UI.MenuItem(3, "Edit Customer");
                UI.MenuItem(4, "Search Customer");
                UI.MenuItem(5, "View Customer History");
                UI.SectionLine();
                UI.MenuItemStr("0", "Back to Main Menu");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": ViewAll(); break;
                    case "2": Add(); break;
                    case "3": Edit(); break;
                    case "4": Search(); break;
                    case "5": History(); break;
                    case "0": return;
                    default: UI.Error("Invalid choice."); UI.Pause(); break;
                }
            }
        }

        public static void ViewAll(List<Customer> list = null)
        {
            UI.Header("CUSTOMER LIST");
            var customers = list ?? DB.Customers;
            if (!customers.Any()) { UI.Info("No customers registered."); UI.Pause(); return; }

            UI.TableHeader(("ID", 5), ("Name", 20), ("Phone", 16), ("Email", 24), ("City", 14));
            foreach (var c in customers)
                UI.TableRow(ConsoleColor.White,
                    (c.Id.ToString(), 5), (c.Name, 20), (c.Phone, 16), (c.Email, 24),
                    (c.Address.Length > 13 ? c.Address.Substring(0, 13) : c.Address, 14));
            UI.SectionLine();
            UI.Info($"Total customers: {customers.Count}");
            UI.Pause();
        }

        static void Add()
        {
            UI.Header("ADD NEW CUSTOMER");
            string name = UI.ReadLine("Full Name");
            string phone = UI.ReadLine("Phone Number");
            string email = UI.ReadLine("Email");
            string address = UI.ReadLine("Address");

            var c = new Customer(DB.NextCID(), name, phone, email, address);
            DB.Customers.Add(c);
            UI.Success($"Customer '{name}' registered with ID #{c.Id}.");
            UI.Pause();
        }

        static void Edit()
        {
            UI.Header("EDIT CUSTOMER");
            int id = UI.ReadInt("Customer ID");
            var c = DB.Customers.FirstOrDefault(x => x.Id == id);
            if (c == null) { UI.Error("Customer not found."); UI.Pause(); return; }

            Console.WriteLine("  (Press Enter to keep current value)");
            string name = UI.ReadLine($"Name [{c.Name}]");
            string phone = UI.ReadLine($"Phone [{c.Phone}]");
            string email = UI.ReadLine($"Email [{c.Email}]");
            string addr = UI.ReadLine($"Address [{c.Address}]");

            if (!string.IsNullOrEmpty(name)) c.Name = name;
            if (!string.IsNullOrEmpty(phone)) c.Phone = phone;
            if (!string.IsNullOrEmpty(email)) c.Email = email;
            if (!string.IsNullOrEmpty(addr)) c.Address = addr;

            UI.Success("Customer updated.");
            UI.Pause();
        }

        static void Search()
        {
            UI.Header("SEARCH CUSTOMER");
            string q = UI.ReadLine("Search (name / phone)").ToLower();
            var res = DB.Customers.Where(c =>
                c.Name.ToLower().Contains(q) || c.Phone.Contains(q)).ToList();
            if (!res.Any()) UI.Error("No matching customers.");
            else ViewAll(res);
        }

        static void History()
        {
            UI.Header("CUSTOMER PURCHASE HISTORY");
            int id = UI.ReadInt("Customer ID");
            var c = DB.Customers.FirstOrDefault(x => x.Id == id);
            if (c == null) { UI.Error("Customer not found."); UI.Pause(); return; }

            var sales = DB.Sales.Where(s => s.Customer.Id == id).ToList();
            var jobs = DB.RepairJobs.Where(r => r.Customer.Id == id).ToList();

            UI.SectionLine($"Customer: {c.Name}  |  {c.Phone}");
            UI.SectionLine("Sales");
            if (!sales.Any()) UI.Info("No purchases yet.");
            else
            {
                UI.TableHeader(("Sale#", 7), ("Date", 14), ("Items", 8), ("Total", 14), ("Paid", 6));
                foreach (var s in sales)
                    UI.TableRow(ConsoleColor.White,
                        ($"#{s.Id}", 7), (s.Date.ToString("dd-MM-yyyy"), 14),
                        (s.Items.Count.ToString(), 8), (UI.FormatCurrency(s.Total), 14),
                        (s.Paid ? "Yes" : "No", 6));
            }

            UI.SectionLine("Repair Jobs");
            if (!jobs.Any()) UI.Info("No repair jobs.");
            else
            {
                UI.TableHeader(("Job#", 6), ("Device", 20), ("Status", 14), ("Cost", 14));
                foreach (var r in jobs)
                    UI.TableRow(ConsoleColor.White,
                        ($"#{r.Id}", 6), (r.DeviceName, 20), (r.Status, 14),
                        (UI.FormatCurrency(r.FinalCost > 0 ? r.FinalCost : r.EstCost), 14));
            }
            UI.Pause();
        }
    }

    // ── SALES ─────────────────────────────────────────────────────────────────────
    static class SalesModule
    {
        public static void Menu()
        {
            while (true)
            {
                UI.Header("SALES & BILLING");
                UI.SectionLine("Options");
                UI.MenuItem(1, "New Sale / Generate Bill");
                UI.MenuItem(2, "View All Sales");
                UI.MenuItem(3, "View Sale Detail");
                UI.MenuItem(4, "Mark Sale as Paid");
                UI.SectionLine();
                UI.MenuItemStr("0", "Back to Main Menu");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": NewSale(); break;
                    case "2": ViewAll(); break;
                    case "3": ViewDetail(); break;
                    case "4": MarkPaid(); break;
                    case "0": return;
                    default: UI.Error("Invalid choice."); UI.Pause(); break;
                }
            }
        }

        static void NewSale()
        {
            UI.Header("NEW SALE");

            // Select or create customer
            string cOpt = UI.ReadLine("Customer: [1] Existing  [2] Walk-in");
            Customer customer;
            if (cOpt == "1")
            {
                int cid = UI.ReadInt("Customer ID");
                customer = DB.Customers.FirstOrDefault(x => x.Id == cid);
                if (customer == null) { UI.Error("Customer not found."); UI.Pause(); return; }
            }
            else
            {
                string name = UI.ReadLine("Customer Name");
                string phone = UI.ReadLine("Phone");
                customer = new Customer(DB.NextCID(), name, phone, "", "Walk-in");
                DB.Customers.Add(customer);
            }

            var sale = new Sale { Id = DB.NextSID(), Customer = customer };

            // Add items
            while (true)
            {
                UI.SectionLine("Add Products");
                int pid = UI.ReadInt("Product ID (0 to finish)");
                if (pid == 0) break;
                var product = DB.Products.FirstOrDefault(p => p.Id == pid);
                if (product == null) { UI.Error("Product not found."); continue; }
                if (product.Stock == 0) { UI.Error("Out of stock!"); continue; }

                int qty = UI.ReadInt($"Quantity (available: {product.Stock})", 1, product.Stock);
                sale.Items.Add(new SaleItem { Product = product, Quantity = qty });
                product.Stock -= qty;
                UI.Success($"Added: {product.Name} x{qty} = {UI.FormatCurrency(product.Price * qty)}");
            }

            if (!sale.Items.Any()) { UI.Info("No items added. Sale cancelled."); UI.Pause(); return; }

            // Payment
            Console.WriteLine("  Payment: [1] Cash  [2] Card  [3] EasyPaisa  [4] JazzCash");
            int pm = UI.ReadInt("Method", 1, 4);
            string[] methods = { "Cash", "Card", "EasyPaisa", "JazzCash" };
            sale.PayMethod = methods[pm - 1];
            string paid = UI.ReadLine("Payment received now? (yes/no)");
            sale.Paid = paid.ToLower() == "yes";

            DB.Sales.Add(sale);
            PrintBill(sale);
        }

        static void PrintBill(Sale sale)
        {
            Console.Clear();
            UI.SetColor(ConsoleColor.Cyan);
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║               MobileZone Pro — Sales Receipt                   ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════════════════════╝");
            UI.ResetColor();
            Console.WriteLine($"  Invoice #: INV-{sale.Id:D4}          Date: {sale.Date:dd-MM-yyyy HH:mm}");
            Console.WriteLine($"  Customer : {sale.Customer.Name}          Phone: {sale.Customer.Phone}");
            Console.WriteLine($"  Payment  : {sale.PayMethod}                  Status: {(sale.Paid ? "PAID" : "PENDING")}");
            UI.SectionLine("Items");
            UI.TableHeader(("#", 4), ("Product", 28), ("Price", 14), ("Qty", 6), ("Sub-Total", 14));
            int i = 1;
            foreach (var item in sale.Items)
                UI.TableRow(ConsoleColor.White,
                    ((i++).ToString(), 4), (item.Product.Name, 28),
                    (UI.FormatCurrency(item.Product.Price), 14),
                    (item.Quantity.ToString(), 6),
                    (UI.FormatCurrency(item.SubTotal), 14));
            UI.SectionLine();
            Console.WriteLine($"{"",48}{"TOTAL:".PadLeft(14)} {UI.FormatCurrency(sale.Total),10}");
            Console.WriteLine();
            UI.SetColor(ConsoleColor.Green);
            Console.WriteLine("  Thank you for shopping at MobileZone Pro!");
            UI.ResetColor();
            UI.Pause();
        }

        static void ViewAll()
        {
            UI.Header("ALL SALES");
            if (!DB.Sales.Any()) { UI.Info("No sales recorded."); UI.Pause(); return; }
            UI.TableHeader(("ID", 6), ("Customer", 20), ("Date", 14), ("Items", 7), ("Total", 14), ("Method", 12), ("Paid", 6));
            foreach (var s in DB.Sales)
                UI.TableRow(s.Paid ? ConsoleColor.Green : ConsoleColor.Yellow,
                    ($"#{s.Id}", 6), (s.Customer.Name, 20), (s.Date.ToString("dd-MM-yyyy"), 14),
                    (s.Items.Count.ToString(), 7), (UI.FormatCurrency(s.Total), 14),
                    (s.PayMethod, 12), (s.Paid ? "YES" : "NO", 6));
            UI.SectionLine();
            double total = DB.Sales.Where(s => s.Paid).Sum(s => s.Total);
            UI.Info($"Total sales: {DB.Sales.Count}  |  Collected: {UI.FormatCurrency(total)}  |  Pending: {UI.FormatCurrency(DB.Sales.Where(s => !s.Paid).Sum(s => s.Total))}");
            UI.Pause();
        }

        static void ViewDetail()
        {
            UI.Header("SALE DETAIL");
            int id = UI.ReadInt("Sale ID");
            var s = DB.Sales.FirstOrDefault(x => x.Id == id);
            if (s == null) { UI.Error("Sale not found."); UI.Pause(); return; }
            PrintBill(s);
        }

        static void MarkPaid()
        {
            UI.Header("MARK SALE AS PAID");
            int id = UI.ReadInt("Sale ID");
            var s = DB.Sales.FirstOrDefault(x => x.Id == id);
            if (s == null) { UI.Error("Sale not found."); UI.Pause(); return; }
            if (s.Paid) { UI.Info("Already marked as paid."); UI.Pause(); return; }
            s.Paid = true;
            UI.Success($"Sale #{id} marked as PAID.");
            UI.Pause();
        }
    }

    // ── REPAIRS ──────────────────────────────────────────────────────────────────
    static class RepairModule
    {
        public static void Menu()
        {
            while (true)
            {
                UI.Header("REPAIR SERVICE DESK");
                UI.SectionLine("Options");
                UI.MenuItem(1, "Register New Repair Job");
                UI.MenuItem(2, "View All Repair Jobs");
                UI.MenuItem(3, "Update Job Status");
                UI.MenuItem(4, "Complete / Deliver Job");
                UI.MenuItem(5, "Search Repair Jobs");
                UI.SectionLine();
                UI.MenuItemStr("0", "Back to Main Menu");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": NewJob(); break;
                    case "2": ViewAll(); break;
                    case "3": UpdateStatus(); break;
                    case "4": Complete(); break;
                    case "5": Search(); break;
                    case "0": return;
                    default: UI.Error("Invalid choice."); UI.Pause(); break;
                }
            }
        }

        static void NewJob()
        {
            UI.Header("REGISTER REPAIR JOB");
            int cid = UI.ReadInt("Customer ID (0 to register new)");
            Customer c;
            if (cid == 0)
            {
                string name = UI.ReadLine("Customer Name");
                string phone = UI.ReadLine("Phone");
                c = new Customer(DB.NextCID(), name, phone, "", "");
                DB.Customers.Add(c);
            }
            else
            {
                c = DB.Customers.FirstOrDefault(x => x.Id == cid);
                if (c == null) { UI.Error("Customer not found."); UI.Pause(); return; }
            }

            string device = UI.ReadLine("Device (e.g. iPhone 13 Pro)");
            string issue = UI.ReadLine("Problem Description");
            string tech = UI.ReadLine("Technician Name");
            double est = UI.ReadDouble("Estimated Cost (Rs.)");

            var job = new RepairJob
            {
                Id = DB.NextRID(),
                Customer = c,
                DeviceName = device,
                Issue = issue,
                Status = "Pending",
                EstCost = est,
                Technician = tech
            };
            DB.RepairJobs.Add(job);
            UI.Success($"Repair Job #{job.Id} registered. Status: Pending.");
            UI.Pause();
        }

        static void ViewAll(List<RepairJob> list = null)
        {
            UI.Header("REPAIR JOBS");
            var jobs = list ?? DB.RepairJobs;
            if (!jobs.Any()) { UI.Info("No repair jobs."); UI.Pause(); return; }

            UI.TableHeader(("ID", 5), ("Customer", 18), ("Device", 20), ("Issue", 20), ("Status", 13), ("Cost", 12));
            foreach (var r in jobs)
            {
                var col = r.Status == "Completed" ? ConsoleColor.Green :
                          r.Status == "In Progress" ? ConsoleColor.Yellow :
                          r.Status == "Delivered" ? ConsoleColor.DarkGray : ConsoleColor.White;
                string issue = r.Issue.Length > 19 ? r.Issue.Substring(0, 19) : r.Issue;
                UI.TableRow(col,
                    ($"#{r.Id}", 5), (r.Customer.Name, 18), (r.DeviceName, 20),
                    (issue, 20), (r.Status, 13),
                    (UI.FormatCurrency(r.FinalCost > 0 ? r.FinalCost : r.EstCost), 12));
            }
            UI.Pause();
        }

        static void UpdateStatus()
        {
            UI.Header("UPDATE JOB STATUS");
            int id = UI.ReadInt("Job ID");
            var r = DB.RepairJobs.FirstOrDefault(x => x.Id == id);
            if (r == null) { UI.Error("Job not found."); UI.Pause(); return; }

            Console.WriteLine($"  Current Status: {r.Status}");
            Console.WriteLine("  [1] Pending  [2] In Progress  [3] Completed  [4] Delivered");
            int s = UI.ReadInt("New Status", 1, 4);
            string[] statuses = { "Pending", "In Progress", "Completed", "Delivered" };
            r.Status = statuses[s - 1];
            UI.Success($"Job #{id} status updated to '{r.Status}'.");
            UI.Pause();
        }

        static void Complete()
        {
            UI.Header("COMPLETE & DELIVER JOB");
            int id = UI.ReadInt("Job ID");
            var r = DB.RepairJobs.FirstOrDefault(x => x.Id == id);
            if (r == null) { UI.Error("Job not found."); UI.Pause(); return; }

            UI.Info($"Device: {r.DeviceName} | Estimated: {UI.FormatCurrency(r.EstCost)}");
            double final = UI.ReadDouble("Final Charge (Rs.)");
            r.FinalCost = final;
            r.Status = "Delivered";
            r.DateOut = DateTime.Now;
            UI.Success($"Job #{id} marked Delivered. Final charge: {UI.FormatCurrency(final)}.");
            UI.Pause();
        }

        static void Search()
        {
            UI.Header("SEARCH REPAIRS");
            string q = UI.ReadLine("Search (customer name / device / status)").ToLower();
            var res = DB.RepairJobs.Where(r =>
                r.Customer.Name.ToLower().Contains(q) ||
                r.DeviceName.ToLower().Contains(q) ||
                r.Status.ToLower().Contains(q)).ToList();
            if (!res.Any()) UI.Error("No matching jobs.");
            else ViewAll(res);
        }
    }

    // ── REPORTS ──────────────────────────────────────────────────────────────────
    static class ReportModule
    {
        public static void Menu()
        {
            while (true)
            {
                UI.Header("REPORTS & ANALYTICS");
                UI.SectionLine("Available Reports");
                UI.MenuItem(1, "Daily Sales Summary");
                UI.MenuItem(2, "Monthly Revenue Report");
                UI.MenuItem(3, "Top Selling Products");
                UI.MenuItem(4, "Inventory Valuation");
                UI.MenuItem(5, "Repair Revenue Report");
                UI.MenuItem(6, "Full Business Dashboard");
                UI.SectionLine();
                UI.MenuItemStr("0", "Back to Main Menu");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": DailySales(); break;
                    case "2": Monthly(); break;
                    case "3": TopProducts(); break;
                    case "4": Inventory(); break;
                    case "5": RepairRevenue(); break;
                    case "6": Dashboard(); break;
                    case "0": return;
                    default: UI.Error("Invalid choice."); UI.Pause(); break;
                }
            }
        }

        static void DailySales()
        {
            UI.Header("DAILY SALES SUMMARY");
            var today = DB.Sales.Where(s => s.Date.Date == DateTime.Today).ToList();
            UI.Info($"Date: {DateTime.Today:dd MMMM yyyy}");
            Console.WriteLine();
            Console.WriteLine($"  Total Transactions : {today.Count}");
            Console.WriteLine($"  Total Revenue      : {UI.FormatCurrency(today.Sum(s => s.Total))}");
            Console.WriteLine($"  Collected (Paid)   : {UI.FormatCurrency(today.Where(s => s.Paid).Sum(s => s.Total))}");
            Console.WriteLine($"  Pending Payments   : {UI.FormatCurrency(today.Where(s => !s.Paid).Sum(s => s.Total))}");
            Console.WriteLine($"  Items Sold         : {today.SelectMany(s => s.Items).Sum(i => i.Quantity)}");
            UI.Pause();
        }

        static void Monthly()
        {
            UI.Header("MONTHLY REVENUE");
            var groups = DB.Sales.GroupBy(s => new { s.Date.Year, s.Date.Month })
                                  .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month);
            UI.TableHeader(("Month", 16), ("Transactions", 14), ("Revenue", 16), ("Collected", 16));
            foreach (var g in groups)
            {
                string month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy");
                UI.TableRow(ConsoleColor.White,
                    (month, 16), (g.Count().ToString(), 14),
                    (UI.FormatCurrency(g.Sum(s => s.Total)), 16),
                    (UI.FormatCurrency(g.Where(s => s.Paid).Sum(s => s.Total)), 16));
            }
            UI.Pause();
        }

        static void TopProducts()
        {
            UI.Header("TOP SELLING PRODUCTS");
            var top = DB.Sales.SelectMany(s => s.Items)
                              .GroupBy(i => i.Product.Name)
                              .Select(g => new { Name = g.Key, Qty = g.Sum(i => i.Quantity), Rev = g.Sum(i => i.SubTotal) })
                              .OrderByDescending(x => x.Qty).Take(10).ToList();
            if (!top.Any()) { UI.Info("No sales data yet."); UI.Pause(); return; }
            UI.TableHeader(("Rank", 6), ("Product", 28), ("Units Sold", 12), ("Revenue", 16));
            int rank = 1;
            foreach (var x in top)
                UI.TableRow(rank <= 3 ? ConsoleColor.Yellow : ConsoleColor.White,
                    ($"#{rank++}", 6), (x.Name, 28), (x.Qty.ToString(), 12),
                    (UI.FormatCurrency(x.Rev), 16));
            UI.Pause();
        }

        static void Inventory()
        {
            UI.Header("INVENTORY VALUATION");
            UI.TableHeader(("Category", 14), ("Products", 10), ("Total Units", 13), ("Est. Value", 16));
            foreach (var g in DB.Products.GroupBy(p => p.Category))
                UI.TableRow(ConsoleColor.White,
                    (g.Key, 14), (g.Count().ToString(), 10),
                    (g.Sum(p => p.Stock).ToString(), 13),
                    (UI.FormatCurrency(g.Sum(p => p.Price * p.Stock)), 16));
            UI.SectionLine();
            Console.WriteLine($"  {"TOTAL INVENTORY VALUE",-37} {UI.FormatCurrency(DB.Products.Sum(p => p.Price * p.Stock)),16}");
            UI.Pause();
        }

        static void RepairRevenue()
        {
            UI.Header("REPAIR SERVICE REVENUE");
            var jobs = DB.RepairJobs;
            Console.WriteLine($"  Total Jobs Registered : {jobs.Count}");
            Console.WriteLine($"  Pending               : {jobs.Count(r => r.Status == "Pending")}");
            Console.WriteLine($"  In Progress           : {jobs.Count(r => r.Status == "In Progress")}");
            Console.WriteLine($"  Completed/Delivered   : {jobs.Count(r => r.Status == "Delivered")}");
            Console.WriteLine($"  Total Repair Revenue  : {UI.FormatCurrency(jobs.Where(r => r.FinalCost > 0).Sum(r => r.FinalCost))}");
            UI.Pause();
        }

        static void Dashboard()
        {
            UI.Header("BUSINESS DASHBOARD");
            UI.SetColor(ConsoleColor.Cyan);
            Console.WriteLine("  ┌─────────────────────────┬──────────────────────────┐");
            Console.WriteLine($"  │  Total Products : {DB.Products.Count,-7}  │  Total Customers : {DB.Customers.Count,-7}│");
            Console.WriteLine($"  │  Total Sales    : {DB.Sales.Count,-7}  │  Repair Jobs     : {DB.RepairJobs.Count,-7}│");
            Console.WriteLine("  ├─────────────────────────┴──────────────────────────┤");
            double totalRevenue = DB.Sales.Where(s => s.Paid).Sum(s => s.Total) +
                                  DB.RepairJobs.Where(r => r.FinalCost > 0).Sum(r => r.FinalCost);
            Console.WriteLine($"  │  💰 Total Collected Revenue : {UI.FormatCurrency(totalRevenue),-26}│");
            Console.WriteLine($"  │  📦 Inventory Value         : {UI.FormatCurrency(DB.Products.Sum(p => p.Price * p.Stock)),-26}│");
            Console.WriteLine($"  │  ⚠  Low Stock Items (<5)    : {DB.Products.Count(p => p.Stock < 5),-26}│");
            Console.WriteLine("  └──────────────────────────────────────────────────────┘");
            UI.ResetColor();
            UI.Pause();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  MAIN PROGRAM
    // ─────────────────────────────────────────────────────────────────────────────

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "MobileZone Pro — Shop Management System";
            DB.Seed();

            while (true)
            {
                UI.Header("MAIN MENU");
                UI.SectionLine("Modules");
                UI.MenuItem(1, "Inventory Management");
                UI.MenuItem(2, "Customer Management");
                UI.MenuItem(3, "Sales & Billing");
                UI.MenuItem(4, "Repair Service Desk");
                UI.MenuItem(5, "Reports & Analytics");
                UI.SectionLine();
                UI.MenuItemStr("0", "Exit System");

                string choice = UI.ReadLine();
                switch (choice)
                {
                    case "1": InventoryModule.Menu(); break;
                    case "2": CustomerModule.Menu(); break;
                    case "3": SalesModule.Menu(); break;
                    case "4": RepairModule.Menu(); break;
                    case "5": ReportModule.Menu(); break;
                    case "0":
                        UI.Clear();
                        UI.SetColor(ConsoleColor.Cyan);
                        Console.WriteLine("\n\n  Thank you for using MobileZone Pro.");
                        Console.WriteLine("  Session ended. Goodbye!\n\n");
                        UI.ResetColor();
                        return;
                    default:
                        UI.Error("Invalid choice. Please try again.");
                        UI.Pause();
                        break;
                }
            }
        }
    }
}