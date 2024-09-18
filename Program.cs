using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace BasicInventoryManagementSystem
{
    internal class Program
    {
        static List<(int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, bool LowStock)> ProductInformation = new List<(int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, bool LowStock)>();
        static List<(string CustomerName, int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, float TotalPaid)> Invoices = new List<(string CustomerName, int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, float TotalPaid)>();

        static void Main(string[] args)
        {
            int ProductCounter = 0;
            string ProdPath = "C:\\Users\\Codeline user\\Desktop\\Projects\\InventorySysV3\\ProductInformation.txt";
            string InvoicePath = "C:\\Users\\Codeline user\\Desktop\\Projects\\InventorySysV3\\AllInvoices.txt";

            CreateFiles(ProdPath, InvoicePath);

            bool Running = true;
            Console.WriteLine("\n");
            Console.WriteLine("     WELCOME TO SHAHAD'S E-SHOP!");
            Console.WriteLine("\n");
            Console.WriteLine("Lets start the setup!");
            Console.WriteLine("How many products would you like to setup? Enter S to skip.");
            string Skip = Console.ReadLine();
            if (Skip == "s") { }

            else
            {
                //GETTING THE INPUT FROM USER [NUMBER OF PRODUCTS TO BE INITIALLY SET UP
                int NumProducts = 0;
                try
                {

                    NumProducts = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                ProductGetInfo(NumProducts, ProductCounter);
                Console.WriteLine("Would you like to save your changes? Yes or No");
                string Save = Console.ReadLine().ToLower();
                if (Save != "no") { SavingProduct(ProdPath); }
            }

            //MAIN PART OF THE PROGRAM [ALL THE SET MENUS {CUSTOMER/ CLERK}  {CUSTOMER SERVICES}  {CLERK SERVICES} ]
            while (Running)
            {
                //Refreshing products 
                ProductInformation.Clear();
                LoadProducts(ProdPath);

                Console.WriteLine("     WELCOME TO SHAHAD'S E-SHOP! \n");
                Console.WriteLine("Please select one of the following numbers:  ");
                Console.WriteLine("1. Customer ");
                Console.WriteLine("2. Shop Clerk \n");
                Console.WriteLine("Enter option: ");


                //GETTING USER IDENTITY (CUSTOMER/CLERK)
                int UserIdentity = 0;
                try
                {
                    UserIdentity = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine("Sorry invalid input, please try again enter a number. \n More info: " + ex.Message); Console.Clear(); }

                if (UserIdentity == 1 || UserIdentity == 2) //Input validation
                {
                    //CUSTOMER FEATURES MENU
                    if (UserIdentity == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("     SHAHAD'S E-SHOP \n");
                        Console.WriteLine("\n  SERVICES: ");
                        Console.WriteLine("1. View all products");
                        Console.WriteLine("2. Search for product ");
                        Console.WriteLine("3. Back \n");
                        Console.WriteLine("Enter option: ");
                        int Option = 0;
                        try
                        {
                            Option = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message); Console.Clear();
                        }
                        switch (Option)
                        {
                            //DISPLAYS ALL PRODUCTS
                            case 1:
                                {
                                    Console.Clear();
                                    ViewProducts(ProdPath, InvoicePath);
                                    Console.WriteLine();
                                    break;
                                }

                            //ALLOWS USER TO SEARCH FOR PRODUCTS AND SEE AVAILABLE STOCK FOR EACH
                            case 2:
                                {
                                    Console.Clear();
                                    Search(ProdPath, InvoicePath);
                                    break;
                                }

                            //RETURNS USER TO MAIN MENU
                            case 3:
                                {
                                    Console.Clear();
                                    break;
                                }

                            default:
                                { 
                                    Console.WriteLine("Improper input please select one of the avialable options :(");
                                    break;
                                }

                        }
                    }



                    //SHOP OWNER FEATURES
                    else if (UserIdentity == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("     SHAHAD'S E-SHOP \n");
                        Console.WriteLine("\n  SERVICES: ");
                        Console.WriteLine("1. Check inventory");
                        Console.WriteLine("2. Edit products");
                        Console.WriteLine("3. Add new product ");
                        Console.WriteLine("4. View Invoices");
                        Console.WriteLine("5. Save and Exit");
                        Console.WriteLine("6. Back \n");
                        Console.WriteLine("Enter option: ");
                        int Option = 0;
                        try
                        {
                            Option = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); Console.Clear(); }
                        switch (Option)
                        {
                            //SHOWS ALL REPORT AND STOCK STATISTICS (BONUS GIVES A LOW STOCK WARNING)
                            case 1:
                                {
                                    Console.Clear();
                                    CheckInventory(ProdPath);
                                    break;
                                }

                            //ALLOWS USER TO EDIT PRODUCTS (NAME/ADD QUANTITY/PRICE)
                            case 2:
                                {
                                    Console.Clear();
                                    EditProduct(ProdPath);
                                    break;
                                }

                            //SETUP NEW PRODUCTS
                            case 3:
                                {
                                    Console.Clear();
                                    Console.WriteLine("How many products would you like to add?");
                                    int NumNewProds = 0;
                                    try
                                    {
                                        NumNewProds = int.Parse(Console.ReadLine());
                                        ProductGetInfo(NumNewProds, ProductCounter);

                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); Console.Clear(); }
                                    Console.WriteLine("Add more products? Enter yes or no.");
                                    string ContinueAdding = Console.ReadLine().ToLower();
                                    if (ContinueAdding == "yes")
                                    {
                                        int MoreProds = 0;
                                        Console.WriteLine("How many more products? ");
                                        try
                                        {
                                            MoreProds = int.Parse(Console.ReadLine());
                                            Console.Clear();
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        ProductGetInfo(MoreProds, ProductCounter);
                                    }
                                    else { Console.WriteLine("Improper input :("); Console.Clear(); }
                                    break;
                                }

                            //Prints all invoices 
                            case 4:
                                Console.Clear();
                                Console.WriteLine("     SHAHAD'S E-SHOP \n\n");
                                Console.WriteLine("INVOICES:\n");
                                PrintInvoices();
                                break;

                            //Saves everything to file and exits 
                            case 5:
                                Console.Clear();
                                Console.WriteLine("     SHAHAD'S E-SHOP \n\n");
                                Console.WriteLine("Saving...");
                                SavingProduct(ProdPath);
                                Running = false;
                                break;

                            //RETURNS USER TO MAIN MENU
                            case 6:
                                {
                                    Console.Clear();
                                    break;
                                }
                        }
                    }

                }
                else { Console.WriteLine("Invalid Input: please input either 1 or 2."); }

                Console.WriteLine("Would like to continue? Enter yes or no: ");
                string Continue = (Console.ReadLine()).ToLower();
                if (Continue == "yes") { Running = ContinueProgram(Continue); Console.Clear(); }
                else { }
            }
        }


        //CREATING PRODUCT AND INVOICE FILES 
        static public void CreateFiles(string ProdPath, string InvoicePath)
        {
            //CREATING PRODUCT INFO FILE
            //Contains: |ProductIDs|Product Names|Product Prices|ProductQuantity|If lowstock|
            if (!File.Exists(ProdPath))
            {
                File.Create(ProdPath).Close();
            }

            //CREATING INVOICE FILE
            //Record of: |Customer name| Date and time of purchase| Product purchased| Quantity purchased of each item| Individual item prices| Total price| --> product prices and names may change so detailed logs must be kept
            if (!File.Exists(InvoicePath))
            {
                File.Create(InvoicePath).Close();
            }

        }



        //THIS FUNCTION GATHERS INFORMATION NEEDED TO SET PRODUCTS UP - IT CALLS THE NEW_PRODUCT FUNCTION SO THAT IT CAN BE REPEATED FOR A NUMBER OF TIMES THE USER SPECIFIED 
        static public void ProductGetInfo(int NumProducts, int ProductCounter)
        {
            Console.Clear();
            for (int i = 1; i <= NumProducts; i++)
            {
                Console.WriteLine("     SHAHAD'S E-SHOP \n");
                Console.WriteLine("Setting Up Products: \n \n ");
                Console.WriteLine("Enter product " + i + "'s name: ");
                string ProdName = Console.ReadLine();
      
                Console.WriteLine("Enter product " + i + "'s price: ");
                float ProdPrice = 0;
                try
                {
                    ProdPrice = float.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }

                if (ProdPrice <= 0)
                {
                    while (ProdPrice <= 0)
                    {
                        Console.WriteLine("The product's price must be more than 0: ");
                        try
                        {
                            ProdPrice = float.Parse(Console.ReadLine());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); return; }
                    }
                }

                Console.WriteLine("Enter product " + i + "'s quantity: ");
                int ProdQuantity = 0;
                try
                {
                    ProdQuantity = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }
                NewProduct(ProdName, ProdPrice, ProdQuantity, ProductCounter);
                Console.WriteLine("Product " + i + "'s setup is complete. Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }


        }


        //THIS FUNCTION IS CALLED BY THE PRODUCT_GET_INFO AND SUPPLIES THE INFORMATION NEEDED TO SET THE PRODUCT UP
        static public void NewProduct(string ProdName, float ProdPrice, int ProdQuantity, int ProductCounter)
        {
            int ProdID = 0;
            bool IsLowStock = false; //=> LowStock = false
            try
            {
                ProdID = (ProductInformation.Count() + 10);
                // (ProductCounter + 10);
                //ProductCounter = ProductCounter + 1; //ID issue 
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }

            foreach (var name in ProductInformation)
            {
                if (name.ProductNames == ProdName)
                {
                    Console.WriteLine("Invalid input: name must be unique, no repeated product names");
                    while (name.ProductNames == ProdName) //ensures product name is unique 
                    {
                        Console.WriteLine("The products name must be unique please renter: ");
                        ProdName = Console.ReadLine();
                    }
                }
                else { break; }
            }

            if (ProdQuantity <= 5) { IsLowStock = true; }
            else { }

            ProductInformation.Add((ProdID, ProdName, ProdPrice, ProdQuantity, IsLowStock));
            Console.WriteLine("Product: " + ProdName + " has been added successfully :) \n");
        }



        //THIS ALLOWS THE PROGRAM TO LOOP
        static public bool ContinueProgram(string Continue)
        {
            if (Continue == "no") { Console.WriteLine("Thank you for visiting Shahad's e-shop. \n Come again soon!"); return false; }
            else if (Continue == "yes") { return true; }
            else { return true; }
        }




        //LISTS OUT PRODUCTS - ALSO ALLOWS USER TO BUY ITEMS TO MAKE PROCESS EASIER FOR THE USER 
        static public void ViewProducts(string ProdPath, string InvoicePath)
        {

            Console.WriteLine("     SHAHAD'S E-SHOP \n");
            Console.WriteLine("VIEW ALL PRODUCTS");

            Console.WriteLine("ID:\t Name:\t Prices: ");
            foreach (var product in ProductInformation)
            {
                Console.WriteLine($"{product.ProductIDs}\t   {product.ProductNames}\t    {product.ProductPrices}");

            }

            Console.WriteLine("\n");
            Console.WriteLine("Would you like to buy anything? \n");
            Console.WriteLine("Enter Yes or No:");
            string Answer = Console.ReadLine();

            if (Answer.ToLower() == "yes")
            {
                Console.WriteLine("Enter product ID");
                int ProdID = 0;
                int IndexOfProd = 0;
                try
                {
                    ProdID = int.Parse(Console.ReadLine());

                    IndexOfProd = ProductInformation.FindIndex(p => p.ProductIDs == ProdID);

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }
                int AvailableQuantity = ProductInformation[IndexOfProd].ProductQuantity;

                if (ProductInformation[IndexOfProd].ProductIDs == ProdID)
                {
                    Console.WriteLine("How many would you like?");
                    int NoItems = 0;
                    try
                    {
                        NoItems = int.Parse(Console.ReadLine());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); return; }

                    if (NoItems <= AvailableQuantity)
                    {
                        Console.WriteLine("We have enough for your order!");
                        Buy(NoItems, ProdID, ProdPath, InvoicePath);
                    }
                    else
                    {
                        if (AvailableQuantity > 0) // to find if there is any stock left (less than required)
                        {
                            Console.WriteLine("Sorry we don't have enough stock for your order. we currently have " + AvailableQuantity + " in stock.");
                            Console.WriteLine("Do you want to buy the remaining stock? \nEnter yes or no. ");
                            string GetRemaining = Console.ReadLine().ToLower();
                            if (GetRemaining == "yes") { NoItems = AvailableQuantity; Buy(NoItems, ProdID, ProdPath, InvoicePath); }
                            else { Console.WriteLine("Exiting..."); };
                        }
                        else { Console.WriteLine("Sorry this product is out of stock :("); }
                    }
                    Buy(NoItems, ProdID, ProdPath, InvoicePath);
                }
                else { Console.WriteLine("Sorry we don't have this product :("); }
            }
            else { }

        }




        //ALLOWS USER TO SEARCH FOR SPECIFIC ITEM - DISPLAYS ALL ITEMS FOR EASIER USE, LOOPS TO ALLOW MULTIPLE SEARCHES, ALLOWS USER TO BUY ITEM
        public static void Search(string ProdPath, string InvoicePath)
        {
            Console.WriteLine("List of products: ");
            Console.WriteLine("ID:\t Name:\t Prices: ");
            foreach (var product in ProductInformation)
            {
                Console.WriteLine($"{product.ProductIDs}\t {product.ProductNames}\t {product.ProductPrices}");

            }
            Console.WriteLine("\n");
            Console.WriteLine("Enter product ID: ");
            int SearchID = 0;
            int IndexOfProd = 0;
            try
            {
                SearchID = int.Parse(Console.ReadLine());
                IndexOfProd = ProductInformation.FindIndex(p => p.ProductIDs == SearchID);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }
            int AvailableQuantity = ProductInformation[IndexOfProd].ProductQuantity;

            if (ProductInformation[IndexOfProd].ProductIDs == SearchID)
            {
                if (AvailableQuantity > 0)
                {
                    Console.WriteLine("This product is available :)\n We have: " + AvailableQuantity + " in stock.");
                    Console.WriteLine("Would you like to buy this product? Enter Yes or No.");
                    string GoToBuyNow = Console.ReadLine().ToLower();
                    if (GoToBuyNow == "yes")
                    {
                        Console.WriteLine("How many of this item would you like?");
                        int NoItems = 0;
                        try
                        {
                            NoItems = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); return; }


                        if (NoItems <= AvailableQuantity)
                        {
                            Console.WriteLine("We have enough for your order!");
                            Buy(NoItems, SearchID, ProdPath, InvoicePath);
                        }
                        else
                        {
                            Console.WriteLine("Sorry we don't have enough stock for your order. we currently have " + AvailableQuantity + " in stock.");
                            Console.WriteLine("Do you want to buy the remaining stock? \nEnter yes or no. ");
                            string GetRemaining = Console.ReadLine().ToLower();
                            if (GetRemaining == "yes")
                            { NoItems = AvailableQuantity; Buy(NoItems, SearchID, ProdPath, InvoicePath); }
                            else { Console.WriteLine("Exiting..."); };

                        }
                    }
                }
                else { Console.WriteLine("Sorry this product is out of stock :("); }
            }
            else { Console.WriteLine("Sorry we don't have this product :("); }
            Console.WriteLine("Would you like to continue searching? Enter Yes or No.");
            string ContinueSearching = Console.ReadLine().ToLower();
            if (ContinueSearching == "yes")
            {
                ViewProducts(ProdPath, InvoicePath);
            }
            else { }
        }




        //ALLOWS USER TO PURCHASE PRODUCT - ALSO PRINTS RECIPT, DEALS WITH STOCK ISSUES (NO STOCK/ NOT ENOUGH STOCK), IS CALLED BY OTHER FUNCTIONS FOR USERS EASE OF USE 
        public static void Buy(int NoItems, int ProdID, string ProdPath, string InvoicePath)
        {
            Console.Clear();
            Console.WriteLine("     SHAHAD'S E-SHOP \n");
            int Location = ProductInformation.FindIndex(p => p.ProductIDs == ProdID);
            string ProductName = ProductInformation[Location].ProductNames;
            float ProdPrice = ProductInformation[Location].ProductPrices;

            if (NoItems > 0)
            {
                if (NoItems <= ProductInformation[Location].ProductQuantity)
                {
                    Console.Clear();
                    Console.WriteLine("     SHAHAD'S E-SHOP \n");
                    Console.WriteLine("Your order is: x" + NoItems + " " + ProductName + " $" + ProdPrice);
                    Console.WriteLine("Enter Yes to proceed to checkout (Enter No to discard basket): ");
                    string CheckoutNow = (Console.ReadLine()).ToLower();

                    if (CheckoutNow == "yes")
                    {
                        float Total = NoItems * ProdPrice;
                        //DateTime Now = DateTime.Now;
                        Console.Clear();
                        Console.WriteLine("     SHAHAD'S E-SHOP \n");
                        Console.WriteLine("Checkout: \n \n ");
                        Console.WriteLine("Your order is: x" + NoItems + " " + ProductName + " $" + ProdPrice);
                        Console.WriteLine("Total: $" + Total);
                        Console.WriteLine("Please enter your name: ");
                        string CustomerName = Console.ReadLine();
                        Console.WriteLine("Press any key to print your recipt");

                        ProcessSale(NoItems, ProdPrice, ProductName, Location, ProdPath, InvoicePath, CustomerName);

                    }
                    else
                    { Console.ReadKey(); Console.WriteLine("Exiting..."); }

                }
                else
                {
                    Console.WriteLine("Sorry it looks like we don't have enough stock to fulfil the whole order :(");
                    Console.WriteLine("We currently have " + ProductInformation[Location].ProductQuantity + " of the product you want.");
                    Console.WriteLine("Would you like to purchase the available stock? Enter yes to continue and any other key to exit.");
                    string Response = (Console.ReadLine()).ToLower();
                    if (Response == "yes")
                    {
                        float Total = NoItems * ProdPrice;
                        DateTime Now = DateTime.Now;
                        Console.Clear();
                        Console.WriteLine("     SHAHAD'S E-SHOP \n");
                        Console.WriteLine("Checkout: \n \n ");
                        Console.WriteLine("Your order is: x" + NoItems + " " + ProductName + " $" + ProdPrice);
                        Console.WriteLine("Total: $" + Total);
                        Console.WriteLine("Please enter your name: ");
                        string CustomerName = Console.ReadLine();
                        Console.WriteLine("Press any key to print your recipt");

                        ProcessSale(NoItems, ProdPrice, ProductName, Location, ProdPath, InvoicePath, CustomerName);
                    }
                    else { Console.ReadKey(); Console.WriteLine("Exiting..."); }
                }
            }

            else
            {
                Console.WriteLine("Sorry but it looks like you have entered an invalid number of items :( \nPlease try again, enter a number larger than 0. ");
            }
            Console.WriteLine("Would you like to continue shopping? Enter Yes or No.");
            string ContinueShopping = Console.ReadLine().ToLower();
            if (ContinueShopping == "yes")
            {
                ViewProducts(ProdPath, InvoicePath);
            }
            else { }
        }




        //ALLOWS STORE STAFF TO CHECK INVENTORY - LISTS OUT ITEMS WITH STOCK LESS THAN 5 ITEMS - LINKS TO ADDING STOCK FOR USERS EASE OF USE
        public static void CheckInventory(string ProdPath)
        {
            float Total = 0;
            float SumTotal = 0;
            bool LowStockFalg = false;  
            Console.WriteLine("     SHAHAD'S E-SHOP \n");
            Console.WriteLine("Inventory: \n \n");
            Console.WriteLine("!!!Low Stock Warning!!!");
            foreach (var product in ProductInformation)
            {
                if (product.LowStock == true)
                { 
                    Console.WriteLine("Product: ID " + product.ProductIDs + " " + product.ProductNames + " $" + product.ProductPrices + " x" + product.ProductQuantity);
                    LowStockFalg |= true;
                }
                else {}
            }

            if (!LowStockFalg) // if no low stock items 
            {
                Console.WriteLine("No low stock items :)");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("All products: ");
            foreach (var product in ProductInformation)
            {
                Total = product.ProductQuantity * product.ProductPrices;
                Math.Round(Total, 2); //limits result to two decimal places 
                Console.WriteLine("Product: ID " + product.ProductIDs + " " + product.ProductNames + " $" + product.ProductPrices + " x" + product.ProductQuantity + " total stock value -> $" + Total);
                SumTotal = SumTotal+Total; //Calculates the total value of all stock

            }
            Console.WriteLine();
            Console.WriteLine("The total value of stock is: " + SumTotal + "\n");

            Console.WriteLine("Would you like to add stock?");
            string AddStock = Console.ReadLine().ToLower();

            if (AddStock == "yes") { EditProduct(ProdPath); }
            else { Console.Clear(); }
        }




        //ALLOWS USER TO EDIT EXISTING PRODUCTS 
        public static void EditProduct(string ProdPath)
        {
            bool Editing = true;
            while (Editing)
            {
                Console.Clear();
                Console.WriteLine("     SHAHAD'S E-SHOP \n");
                Console.WriteLine("\n  Edit Products: ");
                Console.WriteLine("Choose from the following");
                Console.WriteLine("1. Edit product quantity");
                Console.WriteLine("2. Edit product name");
                Console.WriteLine("3. Edit product price");
                Console.WriteLine("Enter number: ");
                int EditOption = 0;
                try
                {
                    EditOption = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }

                Console.Clear();
                Console.WriteLine("     SHAHAD'S E-SHOP \n");
                Console.WriteLine("\n  Edit Products: ");
                Console.WriteLine("Choose from the following products: ");
                for (int i = 0; i < ProductInformation.Count(); i++)
                {
                    Console.WriteLine("Product: ID " + ProductInformation[i].ProductIDs + " " + ProductInformation[i].ProductNames + " $" + ProductInformation[i].ProductPrices + " x" + ProductInformation[i].ProductQuantity);

                }
                Console.WriteLine("Enter Product ID: ");
                int EditProd = 0;
                try
                {
                    EditProd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); return; }

                int Location = ProductInformation.FindIndex(p => p.ProductIDs == EditProd);
                if (ProductInformation[Location].ProductIDs == EditProd) //check if product id is found 
                {
                    Console.Clear();
                    Console.WriteLine("     SHAHAD'S E-SHOP \n");
                    //int IndexOfEdit = ProductIDs.IndexOf(EditProd);
                    Console.WriteLine($"Current details:  ID:{ProductInformation[Location].ProductIDs} {ProductInformation[Location].ProductNames} ${ProductInformation[Location].ProductPrices} x{ProductInformation[Location].ProductNames}\n ");
                    switch (EditOption)
                    {
                        case 1:
                            Console.WriteLine("Edit Product Quantity \n");
                            Console.WriteLine("Number of products to be added: ");
                            int NewQuantity = 0;
                            try
                            {
                                NewQuantity = int.Parse(Console.ReadLine());
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); return; }
                            if (NewQuantity > 0)
                            {
                                ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductInformation[Location].ProductNames, ProductInformation[Location].ProductPrices, ProductQuantity: ProductInformation[Location].ProductQuantity + NewQuantity, ProductInformation[Location].LowStock);
                                Console.WriteLine($"Updated details:  ID:{ProductInformation[Location].ProductIDs} {ProductInformation[Location].ProductNames} ${ProductInformation[Location].ProductPrices} x{ProductInformation[Location].ProductPrices}\n ");
                                if (ProductInformation[Location].ProductQuantity >= 5) //seing if product needs to be added to the low stock flag
                                {
                                    if (ProductInformation[Location].LowStock == true) { ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductInformation[Location].ProductNames, ProductInformation[Location].ProductPrices, ProductInformation[Location].ProductQuantity, LowStock: false); } //remove low stock flag if total stock of prod is more than 5 
                                    else { }
                                }
                                Console.WriteLine("Would you like to save your changes? Yes or No");
                                string Save = Console.ReadLine().ToLower();
                                if (Save != "no") { SavingProduct(ProdPath); }
                            }
                            else { Console.WriteLine("Invalid input: value must be greater than 0"); }
                            break;

                        case 2:
                            Console.WriteLine("Edit Product Name \n");
                            Console.WriteLine("New product name: ");
                            string NewProdName = Console.ReadLine();
                            foreach (var name in ProductInformation)
                            {
                                if (name.ProductNames == NewProdName)
                                {
                                    Console.WriteLine("Invalid input: name must be unique, no repeated product names");
                                    while (name.ProductNames == NewProdName) //ensures product name is unique 
                                    {
                                        Console.WriteLine("The products name must be unique please renter: ");
                                        NewProdName = Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductNames: NewProdName, ProductInformation[Location].ProductPrices, ProductInformation[Location].ProductQuantity, ProductInformation[Location].LowStock);
                                    Console.WriteLine($"Updated details:  ID:{ProductInformation[Location].ProductIDs} {ProductInformation[Location].ProductNames} ${ProductInformation[Location].ProductPrices} x{ProductInformation[Location].ProductPrices}\n ");

                                    Console.WriteLine("Would you like to save your changes? Yes or No");
                                    string Save = Console.ReadLine().ToLower();
                                    if (Save != "no") { SavingProduct(ProdPath); }
                                }
                            }
                            break;

                        case 3:
                            Console.WriteLine("Edit Product Price \n");
                            Console.WriteLine("New product price: ");
                            float NewPrice = float.Parse(Console.ReadLine());
                            if (NewPrice > 0)
                            {
                                ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductInformation[Location].ProductNames, ProductPrices: NewPrice, ProductInformation[Location].ProductQuantity, ProductInformation[Location].LowStock);
                                Console.WriteLine($"Updated details:  ID:{ProductInformation[Location].ProductIDs} {ProductInformation[Location].ProductNames} ${ProductInformation[Location].ProductPrices} x{ProductInformation[Location].ProductPrices}\n ");

                                Console.WriteLine("Would you like to save your changes? Yes or No");
                                string Save = Console.ReadLine().ToLower();
                                if (Save != "no") { SavingProduct(ProdPath); }
                            }
                            else { Console.WriteLine("Invalid input: price must be more than 0"); }
                            break;

                        default:
                            Console.WriteLine("Invalid input: please choose one of the available options");
                            break;
                    }
                }
                else { Console.WriteLine("The product ID does not exist :("); }
                Console.WriteLine("Would you like to continue editing? Enter Yes or No.");
                string EditingResponse = Console.ReadLine().ToLower();
                if (EditingResponse == "yes") { }
                else { Editing = false; }
            }

        }
        


        //FILE READER
        public static void LoadProducts(string ProdPath)
        {
            try
            {
                if (File.Exists(ProdPath))
                {
                    using (StreamReader reader = new StreamReader(ProdPath))
                    {
                        string Line;
                        while ((Line = reader.ReadLine()) != null)
                        {
                            var parts = Line.Split('|');
                            if (parts.Length == 5)
                            {
                                //| ProductIDs | Product Names | Product Prices | ProductQuantity | If lowstock |
                                ProductInformation.Add((int.Parse(parts[0]), parts[1], float.Parse(parts[2]), int.Parse(parts[3]), bool.Parse(parts[4])));
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }




        //FILE WRITER 
        public static void SavingProduct(string ProdPath)
        {
            try 
            {
                using (StreamWriter writer = new StreamWriter(ProdPath))
                { 
                    for (int i = 0; i < ProductInformation.Count; i++) 
                    {
                        //int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, bool LowStock
                        writer.WriteLine($"{ProductInformation[i].ProductIDs}|{ProductInformation[i].ProductNames}|{ProductInformation[i].ProductPrices}|{ProductInformation[i].ProductQuantity}|{ProductInformation[i].LowStock}");
                    }
                    Console.WriteLine("Transaction completed sucessfully :)");
                    ProductInformation.Clear();
                }
            }catch (Exception ex) { Console.WriteLine(ex.Message); }
        }




        //UPDATES PRODUCT QUANTITY AND PRINTS RECIPT - USED AFTER SALE GOES THROUGH 
        public static void ProcessSale(int NoItems, float ProdPrice, string ProductName, int Location, string ProdPath, string InvoicePath, string CustomerName)
        {
            float Total = NoItems * ProdPrice;
            DateTime Now = DateTime.Now;
            int ProductID = ProductInformation[Location].ProductIDs;

            ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductInformation[Location].ProductNames, ProductInformation[Location].ProductPrices, (ProductInformation[Location].ProductQuantity - NoItems), ProductInformation[Location].LowStock); //Minusing from stock 

            if (ProductInformation[Location].ProductQuantity < 5) //seing if product needs to be added to the low stock flag
            {
                if (ProductInformation[Location].LowStock == false) { ProductInformation[Location] = (ProductInformation[Location].ProductIDs, ProductInformation[Location].ProductNames, ProductInformation[Location].ProductPrices, ProductInformation[Location].ProductQuantity, LowStock: true); } //change lowstock to true meaning less than 5 available 
                else { }
            }

            Console.Clear();
            Console.WriteLine("*  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *");
            Console.WriteLine("\n\n                         SHAHAD'S E-SHOP \n \n");
            Console.WriteLine("*  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  * \n");
            Console.WriteLine(Now.ToString("dd-MM-yyyy HH:mm:ss"));
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("Order: x" + NoItems + " " + ProductName + " $" + ProdPrice);
            Console.WriteLine("Total: $" + Total);
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine("Thank you for shopping at Shahad's E-Shop\n Come again soon :)");
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("*  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *");

            Invoices.Add((CustomerName, ProductID,  ProductName,  ProdPrice,  NoItems,  Total));

            AddInvoice(InvoicePath);
            SavingProduct(ProdPath);
            ProductInformation.Clear();
            LoadProducts(ProdPath);

        }



        //ADDING INVOICES
        public static void AddInvoice(string InvoicePath)
        {
            //(string CustomerName, int ProductIDs, string ProductNames, float ProductPrices, int ProductQuantity, float TotalPaid)
            try
            {
                using (StreamWriter writer = new StreamWriter(InvoicePath))
                {
                    for (int i = 0; i < Invoices.Count; i++)
                    {
                        writer.WriteLine($"{Invoices[i].CustomerName}|{Invoices[i].ProductIDs}|{Invoices[i].ProductNames}|{Invoices[i].ProductPrices}|{Invoices[i].ProductQuantity}|{Invoices[i].TotalPaid}");
                    }
                    Invoices.Clear();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }




        //PRINTING ALL INVOICES
        public static void PrintInvoices()
        { 
            //add code hereeee :)
        }
    }
}

