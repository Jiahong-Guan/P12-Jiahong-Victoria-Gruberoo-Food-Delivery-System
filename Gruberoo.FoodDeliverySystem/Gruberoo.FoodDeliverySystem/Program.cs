//==========================================================
// Student Number : S10275126J
// Student Name   : Guan Jiahong
// Partner Number : S10259724J
// Partner Name   : Victoria Kate Lee Hui Xin
//==========================================================

using Gruberoo.FoodDeliverySystem;

List<Restaurant> restaurants = new List<Restaurant>();

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

void InitializeCustomers()
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
}
void InitializeOrders()
{
    // Basic Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
}

void DisplayAllRestaurantsAndMenuItems()
{
    // Basic Function Qn 3
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
}

void DisplayAllOrders()
{
    // Basic Function Qn 4
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
}

void CreateNewOrder()
{
    // Basic Function Qn 5
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
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
InitializeCustomers();
InitializeOrders();

while (true)
{
    DisplayMenu();

    string? choice = Console.ReadLine();
    switch (choice)
    {
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
