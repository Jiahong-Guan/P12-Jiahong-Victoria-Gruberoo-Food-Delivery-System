//==========================================================
// Student Number : S10275126J
// Student Name   : Guan Jiahong
// Partner Number : S10259724J
// Partner Name   : Victoria Kate Lee Hui Xin
//==========================================================

using Gruberoo.FoodDeliverySystem;

List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();
List<Order> orderList = new List<Order>();


void DisplayMenu()
{
    Console.WriteLine("===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice:");
}

void InitializeRestaurant(List<Restaurant> restaurantsList, string filePath)
{
    // Basic Function Qn 1
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    string[] lines = File.ReadAllLines(filePath);

    foreach (string line in lines)
    {
        string[] parts = line.Split(',');
        if (parts.Length >= 3)
        {
            string restaurantId = parts[0];
            string restaurantName = parts[1];
            string restaurantEmail = parts[2];
            Restaurant restaurant = new Restaurant(restaurantId, restaurantName, restaurantEmail);
            
            // TODO: Add new menu

            restaurantsList.Add(restaurant);
        }
    }
    Console.WriteLine($"{restaurantsList.Count} restaurants loaded!");
}

void InitializeFoodItems(string filePath)
{
    // Basic Function Qn 1
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    int count = 0;
    string[] lines = File.ReadAllLines(filePath);

    foreach (string line in lines)
    {
        string[] parts = line.Split(',');
        if (parts.Length >= 5)
        {
            int restaurantId = int.Parse(parts[0]);
            string foodItemName = parts[1];
            string foodItemDescription = parts[2];
            double foodItemPrice = double.Parse(parts[3]);
            FoodItem foodItem = new FoodItem(restaurantId, foodItemName, foodItemDescription, foodItemPrice);
            
            // TODO: Add food item to restaurant menu

            count++;
        }
    }
    Console.WriteLine($"{count} food items loaded!");
}

void InitializeCustomers(List<Customer> customerList, string filePath)
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    string[] lines = File.ReadAllLines(filePath);
    foreach (string line in lines.Skip(1))
    {
        string[] parts = line.Split(",");
        if (parts.Length >= 2)
        {
            string name = parts[0];
            string email = parts[1];
            Customer customer = new Customer(name, email);
            customerList.Add(customer);
        }
    }


}
void InitializeOrders(List<Order> orderList, string filePath)
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    string[] lines = File.ReadAllLines(filePath);
    foreach (string line in lines.Skip(1))
    {
        string[] parts = line.Split(",");
        if (parts.Length >= 10)
        {
            string orderID = parts[0];
            string email = parts[1];
            string restaurantID = parts[2];
            string deliveryDate = parts[3];
            string deliveryTime = parts[4];
            string deliveryAddress = parts[5];
            string createdDateTime = parts[6];
            string totalAmount = parts[7];
            string status = parts[8];
            string items = parts[9];

            // TODO: Probelm for future me sort the data out (items)
            Order orders = new Order(orderID, email, restaurantID, deliveryDate, deliveryTime, deliveryAddress, createdDateTime, totalAmount, status, items);
            orderList.Add(orders);
        }
        
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

void DisplayAllOrders()
{
    // Basic Function Qn 4
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
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
    string resttID = Console.ReadLine();
    foreach (Restaurant restaurant in restaurants)
    {
        if (restaurant.RestaurantId == resttID)
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
InitializeFoodItems("./Data/foodItems.csv");
InitializeCustomers(customers, "./Data/customers.csv");
InitializeOrders(orderList, "./Data/orders.csv");

while (true)
{
    DisplayMenu();

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            DisplayAllRestaurantsAndMenuItems(restaurants);
            break;
        case "2":
            DisplayAllOrders();
            break;
        case "4":
            ProcessOrder();
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
