//==========================================================
// Student Number : S10275126J
// Student Name   : Guan Jiahong
// Partner Number : S10259724J
// Partner Name   : Victoria Kate Lee Hui Xin
//==========================================================

using Gruberoo.FoodDeliverySystem;
using System.Globalization;

List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();
SortedDictionary<int, Order> orderList = new SortedDictionary<int, Order>();


void DisplayMenu()
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
}

void InitializeRestaurant(List<Restaurant> restaurantsList, string filePath)
{
    // Basic Function Qn 1
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    try
    {
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines.Skip(1))
        {
            string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length >= 3)
            {
                string restaurantId = parts[0];
                string restaurantName = parts[1];
                string restaurantEmail = parts[2];
                Restaurant restaurant = new Restaurant(restaurantId, restaurantName, restaurantEmail);

                restaurantsList.Add(restaurant);
            }
        }
        Console.WriteLine($"{restaurantsList.Count} restaurants loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading restaurants: {ex.Message}");
    }
}

void InitializeFoodItems(List<Restaurant> restaurantList, string filePath)
{
    // Basic Function Qn 1
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    int count = 0;

    try
    {
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines.Skip(1))
        {
            string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length >= 4)
            {
                string restaurantId = parts[0];
                string foodItemName = parts[1];
                string foodItemDescription = parts[2];
                if (!double.TryParse(parts[3], out double foodItemPrice))
                {
                    Console.WriteLine("Invalid food item price format. Skipping record...");
                    continue;
                }
                FoodItem foodItem = new FoodItem(foodItemName, foodItemDescription, foodItemPrice, "");
                Restaurant? restaurant = restaurantList.FirstOrDefault(r => r.RestaurantId == restaurantId);
                if (restaurant != null)
                {
                    restaurant.MenuList[0].AddFoodItem(foodItem);
                    count++;
                }
            }
        }
        Console.WriteLine($"{count} food items loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading food items: {ex.Message}");

    }
}

void InitializeCustomers(List<Customer> customerList, string filePath)
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    try
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines.Skip(1))
        {
            string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length >= 2)
            {
                string name = parts[0];
                string email = parts[1];
                Customer customer = new Customer(name, email);
                customerList.Add(customer);
            }
        }
        Console.WriteLine($"{customerList.Count} customers loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading customers: {ex.Message}");
    }
}

void InitializeOrders(SortedDictionary<int, Order> orderList, List<Customer> customerList, List<Restaurant> restaurantList, string filePath)
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    try
    {
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines.Skip(1))
        {
            string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length >= 10)
            {
                if (!int.TryParse(parts[0], out int orderId))
                {
                    Console.WriteLine("Invalid order ID format. Skipping record...");
                    continue;
                }
                string email = parts[1];
                Customer customer = customerList.FirstOrDefault(c => c.EmailAddress == email);

                if (customer == null)
                {
                    Console.WriteLine($"Customer not found: {email}. Skipping record...");
                    continue;
                }

                string restaurantId = parts[2];
                Restaurant restaurant = restaurantList.FirstOrDefault(r => r.RestaurantId == restaurantId);

                if (restaurant == null)
                {
                    Console.WriteLine($"Restaurant not found: {restaurantId}. Skipping record...");
                    continue;
                }

                if (!DateOnly.TryParseExact(parts[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly deliveryDate))
                {
                    Console.WriteLine("Invalid delivery date format. Skipping record...");
                    continue;
                }

                if (!TimeOnly.TryParse(parts[4], out TimeOnly deliveryTime))
                {
                    Console.WriteLine("Invalid delivery time format. Skipping record...");
                    continue;
                }

                string deliveryAddress = parts[5];

                if (!DateTime.TryParseExact(parts[6], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime creationDateTime))
                {
                    Console.WriteLine("Invalid created date time format. Skipping record...");
                    continue;
                }

                if (!double.TryParse(parts[7], out double totalAmount))
                {
                    Console.WriteLine("Invalid total amount format. Skipping record...");
                    continue;
                }
                string status = parts[8];
                string items = parts[9]; // TODO: Items consist of all after 10th item

                string paymentMethod = "CC";
                // TODO: Probelm for future me sort the data out (items)

                Order order = new Order(orderId, customer, restaurant, deliveryDate, deliveryTime, deliveryAddress, creationDateTime, totalAmount, status, paymentMethod, true);
                orderList.Add(order.OrderId, order);
                customer.AddOrder(order);
                restaurant.OrderQueue.Enqueue(order);
            }
        }
        Console.WriteLine($"{orderList.Count} orders loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading orders: {ex.Message}");
    }
}

void DisplayAllRestaurantsAndMenuItems(List<Restaurant> restaurantsList)
{
    // Basic Function Qn 3
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================

    Console.WriteLine("All Restaurants and Menu Items");
    Console.WriteLine("==============================");
    foreach(Restaurant restaurant in restaurantsList)
    {
        Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");
        restaurant.DisplayMenu();
        Console.WriteLine();
    }

}

void DisplayAllOrders(SortedDictionary<int, Order> orderList)
{
    // Basic Function Qn 4
    // Display every order form every customer in ascending order via orderId
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-10} {"Customer",-13} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-9} Status");
    Console.WriteLine($"{"--------",-10} {"----------",-13} {"-------------",-15} {"------------------",-20} {"------",-9} ---------");

    foreach (KeyValuePair<int, Order> kvp in orderList)
    {
        Order order = kvp.Value;
        Console.WriteLine(order);
    }
}

void CreateNewOrder(List<Restaurant> restaurants)
{
    // Basic Function Qn 5
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");

    Console.Write("Enter Customer Email: ");
    string custEmail = Console.ReadLine();

    Console.Write("Enter Restaurant ID: ");
    string restID = Console.ReadLine();
    foreach (Restaurant restaurant in restaurants)
    {
        if (restaurant.RestaurantId == restID)
        {
            Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
            string delvDate = Console.ReadLine();

            Console.Write("Enter Delivery Time (hh:mm): ");
            string delvTime = Console.ReadLine();

            Console.Write("Enter Delivery Address: ");
            string delvAddr = Console.ReadLine();

            Console.WriteLine("Available Food Items:");

            foreach (Menu menu in restaurant.MenuList)
            {
                List<FoodItem> foodItemList = menu.FoodItemList;
                for (int i = 0; i < menu.FoodItemList.Count; i++)
                {
                    Console.WriteLine($"{i} {foodItemList[i].ItemName} - ${foodItemList[i].ItemPrice}");
                }
            }

            while (true)
            {
                Console.Write("Enter item number (0 to finish) : ");
                int itemNo = int.Parse(Console.ReadLine());

                if (itemNo == 0)
                {
                    Console.Write("Any special request? [Y/N] : ");
                    string choice = Console.ReadLine();
                    if (choice.ToLower() == "y") {
                        Console.Write("Special Request: ");
                        string specReq = Console.ReadLine();
                        // TODO: Later come back for this

                    }
                    
                    break;
                }

                Console.Write("Enter quantity: ");
                int quan = int.Parse(Console.ReadLine());
                
                FoodItem foodItem = new FoodItem();

            }
            


            return;
        }

    }
    Console.WriteLine("Invalid restaurant ID.");
}

void ProcessOrder()
{
    // Basic Function Qn 6
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
}

void ModifyOrder()
{
    // Basic Function Qn 7
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
}

void DeleteOrder()
{
    // Basic Function Qn 8
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
}


// Main program execution
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
InitializeRestaurant(restaurants, "./Data/restaurants.csv");
InitializeFoodItems(restaurants,"./Data/foodItems.csv");
InitializeCustomers(customers, "./Data/customers.csv");
InitializeOrders(orderList, customers, restaurants, "./Data/orders.csv");  

while (true)
{
    DisplayMenu();

    string? choice = Console.ReadLine();

    // spacer
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            DisplayAllRestaurantsAndMenuItems(restaurants);
            break;
        case "2":
            DisplayAllOrders(orderList);
            break;
        case "3":
            CreateNewOrder(restaurants);
            break;
        case "4":
            ProcessOrder();
            break;
        case "5":
            ModifyOrder();
            break;
        case "6":
            DeleteOrder();
            break;
        case "0":
            Console.WriteLine("\n\nExiting the program. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid choice! Please try again.");
            break;
    }

    // spacer
    Console.WriteLine();
}
