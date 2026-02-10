//==========================================================
// Student Number : S10275126J
// Student Name   : Guan Jiahong
// Partner Number : S10259724J
// Partner Name   : Victoria Kate Lee Hui Xin
//==========================================================

using Gruberoo.FoodDeliverySystem;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Security.AccessControl;
using System.Xml.Serialization;

List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();
SortedDictionary<int, Order> orderList = new SortedDictionary<int, Order>();

double deliveryFee = 5.00;

void DisplayMenu()
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Bulk process unprocessed orders");
    Console.WriteLine("8. Display total order amounts");
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
                string items = string.Join(",", parts.Skip(9)).Trim('"');

                string paymentMethod = "CC";

                Order order = new Order(orderId, customer, restaurant, deliveryDate, deliveryTime, deliveryAddress, creationDateTime, totalAmount, status, paymentMethod, true);

                if (!string.IsNullOrEmpty(items) && items != "\"\"")
                {
                    ParseFoodItems(items, restaurant, order);
                }

                orderList.Add(order.OrderId, order);
                customer.AddOrder(order);
                restaurant.OrderQueue.Enqueue(order);

                // All cancelled orders are pushed to refund stack
                if (status == "Cancelled" || status == "Rejected")
                {
                    restaurant.RefundStack.Push(order);
                }
            }
        }
        Console.WriteLine($"{orderList.Count} orders loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading orders: {ex.Message}");
    }
}

void ParseFoodItems(string itemsData, Restaurant restaurant, Order order)
{
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    string cleanItemsData = itemsData.Trim('"');
    string[] itemPairs = cleanItemsData.Split('|', StringSplitOptions.RemoveEmptyEntries);

    foreach (string pair in itemPairs)
    {
        string[] itemDetails = pair.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (itemDetails.Length >= 2)
        {
            string itemName = itemDetails[0].Trim();
            if (int.TryParse(itemDetails[1].Trim(), out int quantity))
            {
                // Search for food item in all restaurant menus
                FoodItem foodItem = null;
                foreach (Menu menu in restaurant.MenuList)
                {
                    foodItem = menu.FoodItemList.FirstOrDefault(fi => fi.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                    if (foodItem != null)
                        break;
                }

                if (foodItem != null)
                {
                    OrderedFoodItem orderedItem = new OrderedFoodItem(
                        foodItem.ItemName,
                        foodItem.ItemDesc,
                        foodItem.ItemPrice,
                        "",
                        quantity);

                    order.OrderedItems.Add(orderedItem);
                }
            }
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
    string custEmail = Console.ReadLine()?.Trim();

    Customer customer = customers.FirstOrDefault(c => c.EmailAddress == custEmail);
    if (customer == null)
    {
        Console.WriteLine("Customer not found. Please check the email address.");
        return;
    }

    Console.Write("Enter Restaurant ID: ");
    string restId = Console.ReadLine()?.Trim().ToUpper();

    Restaurant restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restId);

    if (restaurant == null)
    {
        Console.WriteLine("Invalid restaurant ID.");
        return;
    }

    DateOnly deliveryDate;

    while (true)
    {
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        if (DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out deliveryDate))
        {
            if (deliveryDate >= DateOnly.FromDateTime(DateTime.Now))
                break;
            else
                Console.WriteLine("Delivery date must be today or in the future.");
        }
        else
        {
            Console.WriteLine("Invalid date format. Please use dd/mm/yyyy.");
        }
    }

    TimeOnly deliveryTime;
    while (true)
    {
        Console.Write("Enter Delivery Time (hh:mm): ");
        if (TimeOnly.TryParse(Console.ReadLine(), out deliveryTime))
            break;
        else
            Console.WriteLine("Invalid time format. Please use hh:mm.");
    }

    Console.Write("Enter Delivery Address: ");
    string deliveryAddr = Console.ReadLine()?.Trim();

    Console.WriteLine("\nAvailable Food Items:");
    Console.WriteLine("---------------------");
    foreach (Menu menu in restaurant.MenuList)
    {
        List<FoodItem> foodItemList = menu.FoodItemList;
        for (int i = 0; i < menu.FoodItemList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {foodItemList[i].ItemName} - ${foodItemList[i].ItemPrice.ToString("0.00")}");
        }
    }

    List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();

    // Select food items
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");
        // 3 conditions -> check if cannot convert to int, less than 0, number not from restaurant menu list
        if (!int.TryParse(Console.ReadLine(), out int itemNo) || itemNo < 0 || itemNo > restaurant.MenuList[0].FoodItemList.Count)
        {
            Console.WriteLine("Invalid item number.");
            continue;
        }

        if (itemNo == 0)
            break;

        FoodItem selectedItem = restaurant.MenuList[0].FoodItemList[itemNo - 1];
        Console.Write($"Enter quantity: ");

        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            continue;
        }

        // Special Request
        string customise = "";
        Console.Write("Add special request? [Y/N]: ");
        if (Console.ReadLine()?.Trim().ToUpper() == "Y")
        {
            Console.Write("Special Request: ");
            customise = Console.ReadLine()?.Trim() ?? "";
        }

        OrderedFoodItem orderedItem = new OrderedFoodItem(
            selectedItem.ItemName,
            selectedItem.ItemDesc,
            selectedItem.ItemPrice,
            customise,
            quantity
        );
        orderedItem.CalculateSubTotal();
        orderedItems.Add(orderedItem);
    }

    if (orderedItems.Count == 0)
    {
        Console.WriteLine("No items ordered. Order cancelled.");
        return;
    }

    // calculate total (creating a default order object to calculate total)
    Order tempOrder = new Order();
    foreach (var item in orderedItems)
    {
        tempOrder.AddOrderedFoodItem(item);
    }

    double orderTotal = tempOrder.CalculateOrderTotal();
    Console.WriteLine($"\nOrder Total: ${(orderTotal - deliveryFee).ToString("0.00")} + ${deliveryFee.ToString("0.00")} (delivery) = ${orderTotal.ToString("0.00")}");

    Console.Write("Proceed to payment? [Y/N]: ");

    if (Console.ReadLine()?.Trim().ToUpper() != "Y")
    {
        Console.WriteLine("\nOrder cancelled.");
        return;
    }
    
    Console.WriteLine("\nPayment Method:");
    Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
    
    string paymentMethod = Console.ReadLine()?.Trim().ToUpper();
    if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
    {
        Console.WriteLine("Invalid payment method.");
        return;
    }

    int newOrderId = orderList.Count > 0 ? orderList.Max(o => o.Value.OrderId) + 1 : 1001;

    Order newOrder = new Order(
        newOrderId,
        customer,
        restaurant,
        deliveryDate,
        deliveryTime,
        deliveryAddr,
        DateTime.Now,
        orderTotal,
        "Pending",
        paymentMethod,
        true
    );

    foreach (var item in orderedItems)
    {
        newOrder.AddOrderedFoodItem(item);
    }

    orderList.Add(newOrder.OrderId, newOrder);
    customer.AddOrder(newOrder);
    restaurant.OrderQueue.Enqueue(newOrder);

    // save to csv
    try
    {
        string itemsString = string.Join("|", orderedItems.Select(oi => $"{oi.ItemName}, {oi.QtyOrdered}"));
        string orderLine = $"{newOrder.OrderId},{customer.EmailAddress},{restaurant.RestaurantId},{deliveryDate:dd/MM/yyyy},{deliveryTime:HH:mm}," +
                         $"{deliveryAddr},{newOrder.OrderDateTime:dd/MM/yyyy HH:mm},{orderTotal},Pending,\"{itemsString}\"";

        string filePath = "./Data/orders.csv";
        // If files does not exist
        if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
        {
            string header = "OrderId,CustomerEmail,RestaurantId,DeliveryDate,DeliveryTime,DeliveryAddress,CreatedDateTime,TotalAmount,Status,Items";
            File.WriteAllText(filePath, header + orderLine);
        }
        else
        {
            File.AppendAllText(filePath, orderLine);
        }
    } catch (Exception ex)
    {
        Console.WriteLine($"Error saving order: {ex.Message}");
    }

    Console.WriteLine($"\nOrder {newOrder.OrderId} created successfully! Status: Pending");
}

void ProcessOrder()
{
    // Basic Function Qn 6
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");

    Console.Write("Enter Restaurant ID: ");
    string restId = Console.ReadLine()?.Trim().ToUpper();

    Restaurant restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restId);
    if (restaurant == null)
    {
        Console.WriteLine("Restaurant not found.");
        return;
    }

    if (restaurant.OrderQueue.Count == 0)
    {
        Console.WriteLine("No orders to process.");
        return;
    }

    foreach (Order order in restaurant.OrderQueue.ToList())
    {
        Console.WriteLine($"\nOrder {order.OrderId}:");
        Console.WriteLine($"Customer: {order.Customer.CustomerName}");
        order.DisplayOrderedFoodItems();
        Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
        Console.WriteLine($"Order Status: {order.OrderStatus}");

        Console.Write("\n[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string choice = Console.ReadLine()?.Trim().ToUpper();

        switch (choice)
        {
            case "C":
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Preparing";
                    Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                }
                else
                {
                    Console.WriteLine($"Cannot confirm order. Current status: {order.OrderStatus}");
                }
                break;

            case "R":
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Rejected";
                    restaurant.RefundStack.Push(order);
                    Console.WriteLine($"Order {order.OrderId} rejected. Refund of ${order.OrderTotal.ToString("0.00")} processed.");
                }
                else
                {
                    Console.WriteLine($"Cannot reject order. Current status: {order.OrderStatus}");
                }
                break;

            case "D":
                if (order.OrderStatus == "Preparing")
                {
                    order.OrderStatus = "Delivered";
                    Console.WriteLine($"Order {order.OrderId} delivered.");
                }
                else
                {
                    Console.WriteLine($"Cannot deliver order. Current status: {order.OrderStatus}");
                }
                break;

            case "S":
                Console.WriteLine("Skipping this order.");
                break;

            default:
                Console.WriteLine("Invalid choice. Skipping...");
                break;

        }
    }
}

void ModifyOrder(List<Customer> customerList, List<Restaurant> restaurantsList)
{
    // Basic Function Qn 7
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string custEmail = Console.ReadLine()?.Trim();

    Customer customer = customerList.FirstOrDefault(c => c.EmailAddress == custEmail);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    // Show pending orders
    var pendingOrders = customer.Orders.Values.Where(o => o.OrderStatus == "Pending").ToList();
    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found.");
        return;
    }

    Console.WriteLine("Pending Orders:");
    foreach (var order in pendingOrders)
    {
        Console.WriteLine($"{order.OrderId}");
    }

    Console.Write("Enter Order ID: ");
    if (!int.TryParse(Console.ReadLine(), out int orderId))
    {
        Console.WriteLine("\nInvalid Order ID.");
        return;
    }

    Order orderToModify = pendingOrders.FirstOrDefault(o => o.OrderId == orderId);
    if (orderToModify == null)
    {
        Console.WriteLine("\nOrder not found or not pending.");
        return;
    }

    Console.WriteLine("Order Items:");
    orderToModify.DisplayOrderedFoodItems();

    //address + delivery date/time
    Console.WriteLine("Address:");
    Console.WriteLine(orderToModify.DeliveryAddress);

    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(orderToModify.DeliveryDateTime.ToString("dd/M/yyyy, HH:mm"));

    Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid option.");
        return;
    }

    switch (choice)
    {
        case 1:
            Console.WriteLine("\nAvailable Food Items:");
            Console.WriteLine("---------------------");
            foreach (Menu menu in orderToModify.Restaurant.MenuList)
            {
                List<FoodItem> foodItemList = menu.FoodItemList;
                for (int i = 0; i < menu.FoodItemList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {foodItemList[i].ItemName} - ${foodItemList[i].ItemPrice.ToString("0.00")}");
                }
            }

            Dictionary<string, int> originalQuantities = new Dictionary<string, int>();
            Dictionary<string, double> originalItemTotals = new Dictionary<string, double>();

            foreach (OrderedFoodItem ofi in orderToModify.OrderedItems)
            {
                originalQuantities[ofi.ItemName] = ofi.QtyOrdered;
                originalItemTotals[ofi.ItemName] = ofi.SubTotal;
            }

            bool changesMade = false;
            double originalOrderTotal = orderToModify.OrderTotal;

            void AbortModification()
            {
                orderToModify.OrderedItems.Clear();
                foreach (var kvp in originalQuantities)
                {
                    // Find the original item details
                    var foodItem = orderToModify.Restaurant.MenuList[0].FoodItemList
                        .FirstOrDefault(fi => fi.ItemName == kvp.Key);
                    if (foodItem != null)
                    {
                        double originalSubTotal = originalItemTotals[kvp.Key];
                        OrderedFoodItem revertedItem = new OrderedFoodItem(
                            kvp.Key,
                            foodItem.ItemDesc,
                            foodItem.ItemPrice,
                            foodItem.Customise,
                            kvp.Value
                        );
                        revertedItem.CalculateSubTotal();
                        orderToModify.AddOrderedFoodItem(revertedItem);
                    }
                }
                orderToModify.OrderTotal = originalOrderTotal;
                return;
            }

            while (true)
            {
                Console.Write("\nEnter item number (0 to finish): ");

                if (!int.TryParse(Console.ReadLine(), out int itemNo) || itemNo < 0 || itemNo > orderToModify.Restaurant.MenuList[0].FoodItemList.Count)
                {
                    Console.WriteLine("Invalid item number.");
                    continue;
                }

                if (itemNo == 0)
                    break;

                FoodItem selectedItem = orderToModify.Restaurant.MenuList[0].FoodItemList[itemNo - 1];
                OrderedFoodItem existingOrderedItem = orderToModify.OrderedItems.FirstOrDefault(oi => oi.ItemName == selectedItem.ItemName);

                var suffix = existingOrderedItem != null ? " (0 to remove)" : "";
                Console.Write($"Enter quantity{suffix}: ");

                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                if (quantity == 0)
                {
                    if (existingOrderedItem != null)
                    {
                        bool removed = orderToModify.RemoveOrderedFoodItem(existingOrderedItem);

                        if (removed)
                        {
                            Console.WriteLine($"\nRemoved {selectedItem.ItemName} from order.");
                            changesMade = true;
                        }
                    } 
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                        continue;
                    }
                }
                else
                {
                    // item is already in order so update quantity
                    if (existingOrderedItem != null)
                    {
                        existingOrderedItem.QtyOrdered = quantity;
                        existingOrderedItem.CalculateSubTotal();
                        orderToModify.OrderTotal = orderToModify.CalculateOrderTotal();
                        Console.WriteLine($"Updated {selectedItem.ItemName} quantity to {quantity}.");
                        changesMade = true;
                    }
                    // If item not in order add it in
                    else
                    {
                        string customise = "";
                        Console.Write("Add special request? [Y/N]: ");

                        if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                        {
                            Console.Write("Special Request: ");
                            customise = Console.ReadLine()?.Trim() ?? "";
                        }

                        OrderedFoodItem newOrderedItem = new OrderedFoodItem(
                            selectedItem.ItemName,
                            selectedItem.ItemDesc,
                            selectedItem.ItemPrice,
                            customise,
                            quantity
                        );
                        newOrderedItem.CalculateSubTotal();
                        orderToModify.AddOrderedFoodItem(newOrderedItem);

                        Console.WriteLine($"\nAdded {quantity}x {selectedItem.ItemName} to order.");
                        changesMade = true;
                    }
                }
            }

            // Check if order is empty after modify
            if (orderToModify.OrderedItems.Count == 0)
            {
                Console.WriteLine("\nWarning: Order has no items after modification.");
                Console.Write("Do you want to cancel the entire order? [Y/N]");

                if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                {
                    orderToModify.OrderStatus = "Cancelled";
                    orderToModify.Restaurant.RefundStack.Push(orderToModify);

                    Queue<Order> newQueue = new Queue<Order>();
                    foreach (Order order in orderToModify.Restaurant.OrderQueue)
                    {
                        if (order.OrderId != orderToModify.OrderId)
                        {
                            newQueue.Enqueue(order);
                        }
                    }
                    orderToModify.Restaurant.OrderQueue = newQueue;
                    orderToModify.Customer.Orders.Remove(orderToModify.OrderId);
                    orderList.Remove(orderToModify.OrderId);
                    Console.WriteLine($"Order {orderId} has been cancelled.");
                }
                else
                {
                    Console.WriteLine("\nAborting modification.");
                    AbortModification();
                    return;
                }
            }

            if (changesMade)
            {
                Dictionary<string, int> newQuantities = new Dictionary<string, int>();
                Dictionary<string, double> newItemTotals = new Dictionary<string, double>();

                foreach (OrderedFoodItem ofi in orderToModify.OrderedItems)
                {
                    newQuantities[ofi.ItemName] = ofi.QtyOrdered;
                    newItemTotals[ofi.ItemName] = ofi.SubTotal;
                }

                double totalRefundAmount = 0;
                double totalAdditionalPayment = 0;

                // check items that exist in both original and new orders
                foreach (string itemName in originalQuantities.Keys.ToList())
                {
                    if (newQuantities.ContainsKey(itemName))
                    {
                        int originalQty = originalQuantities[itemName];
                        int newQty = newQuantities[itemName];
                        double itemPrice = orderToModify.OrderedItems.First(oi => oi.ItemName == itemName).ItemPrice;

                        if (newQty < originalQty)
                        {
                            // quantity decrease so refund
                            int qtyDifference = originalQty - newQty;
                            double refundAmount = qtyDifference * itemPrice;
                            totalRefundAmount += refundAmount;
                        }
                        else if (newQty > originalQty)
                        {
                            // quantity increase so payment
                            int qtyDifference = newQty - originalQty;
                            double additionalAmount = qtyDifference * itemPrice;
                            totalAdditionalPayment += additionalAmount;
                        }
                    }
                    else
                    {
                        // Item been removed - refund entire fooditem
                        double refundAmount = originalItemTotals[itemName];
                        totalRefundAmount += refundAmount;
                    }
                }

                // New Items not in original
                foreach (string itemName in newQuantities.Keys)
                {
                    if (!originalQuantities.ContainsKey(itemName))
                    {
                        double additionalAmount = newItemTotals[itemName];
                        totalAdditionalPayment += additionalAmount;
                    }
                }

                if (totalRefundAmount > 0)
                {
                    Console.WriteLine($"\nRefund Amount: ${totalRefundAmount.ToString("0.00")}");
                }

                if (totalAdditionalPayment > 0)
                {
                    Console.WriteLine($"Additional Payments: ${totalAdditionalPayment}");
                }

                // calculate net diff from refund and addional payments
                double netDiff = totalAdditionalPayment - totalRefundAmount;

                if (netDiff > 0)
                {
                    // customer need pay
                    Console.WriteLine($"\nNet Additional Payment Required: ${netDiff.ToString("0.00")}");
                    Console.Write("Proceed with additional payment? [Y/N]: ");

                    if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                    {
                        Console.WriteLine("\nPayment method:");
                        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");

                        string paymentMethod = Console.ReadLine()?.Trim().ToUpper();
                        if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
                        {
                            Console.WriteLine("Invalid payment method. Aborting modification.");
                            return;
                        }
                        orderToModify.OrderPaymentMethod = paymentMethod;
                        orderToModify.OrderTotal = orderToModify.CalculateOrderTotal();
                    }
                    else
                    {
                        Console.WriteLine("Aborting modification.");
                        AbortModification();
                        return;
                    }
                }
                else if (netDiff < 0)
                {
                    // Refund to customer
                    double refundAmount = Math.Abs(netDiff);
                    Console.WriteLine($"\nRefund of ${refundAmount.ToString("0.00")} will be processed.");
                }

                UpdateOrderInCSV(orderToModify);
                Console.WriteLine($"\nOrder {orderId} has been successfully updated.");
            }
            break;
        case 2:
            Console.Write("Enter new Delivery Address: ");
            string newAddress = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(newAddress))
            {
                orderToModify.DeliveryAddress = newAddress;
                Console.WriteLine($"\nOrder {orderToModify.OrderId} updated. New Address: {newAddress}.");
                UpdateOrderInCSV(orderToModify);
            }
            else
            {
                Console.WriteLine("Delivery address cannot be empty.");
            }
            break;
        case 3:
            Console.Write("Enter new Delivery Time (hh:mm): ");

            if (TimeOnly.TryParse(Console.ReadLine(), out TimeOnly newTime))
            {
                orderToModify.DeliveryDateTime = DateOnly.FromDateTime(orderToModify.DeliveryDateTime).ToDateTime(newTime);
                Console.WriteLine($"\nOrder {orderToModify.OrderId} updated. New Delivery Time: {newTime.ToString("HH:mm")}");
                UpdateOrderInCSV(orderToModify);
            }
            else
            {
                Console.WriteLine("Invalid time format.");
            }
            break;
    }
}

void UpdateOrderInCSV(Order updatedOrder)
{
    // Helper function for Updating Order to CSV
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    try
    {
        string[] allLines = File.ReadAllLines("./Data/orders.csv");
        for (int i = 0; i < allLines.Length; i++)
        {
            // skip header
            if (i == 0) continue;

            string[] parts = allLines[i].Split(',');
            if (parts.Length >= 1 && int.TryParse(parts[0], out int lineOrderId))
            {
                if (lineOrderId == updatedOrder.OrderId)
                {
                    string itemsString = string.Join("|", updatedOrder.OrderedItems.Select(oi => $"{oi.ItemName}, {oi.QtyOrdered}"));
                    allLines[i] = $"{updatedOrder.OrderId}," +
                                 $"{updatedOrder.Customer.EmailAddress}," +
                                 $"{updatedOrder.Restaurant.RestaurantId}," +
                                 $"{updatedOrder.DeliveryDateTime.ToString("dd/MM/yyyy")}," +
                                 $"{updatedOrder.DeliveryDateTime.ToString("HH:mm")}," +
                                 $"{updatedOrder.DeliveryAddress}," +
                                 $"{updatedOrder.OrderDateTime.ToString("dd/MM/yyyy HH:mm")}," +
                                 $"{updatedOrder.OrderTotal.ToString("0.00")}," +
                                 $"{updatedOrder.OrderStatus}," +
                                 $"{itemsString}";

                    // Write back to csv file
                    File.WriteAllLines("./Data/orders.csv", allLines);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error updating order in CSV: {ex.Message}");
    }
}

void DeleteOrder(List<Customer> customerList)
{
    // Basic Function Qn 8
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string custEmail = Console.ReadLine()?.Trim();

    Customer customer = customerList.FirstOrDefault(c => c.EmailAddress == custEmail);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    // Show pending orders
    var pendingOrders = customer.Orders.Values.Where(o => o.OrderStatus == "Pending").ToList();
    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found.");
        return;
    }

    Console.WriteLine("Pending Orders:");
    foreach (var order in pendingOrders)
    {
        Console.WriteLine($"{order.OrderId}");
    }

    Console.Write("Enter Order ID: ");
    if (!int.TryParse(Console.ReadLine(), out int orderId))
    {
        Console.WriteLine("\nInvalid Order ID.");
        return;
    }

    Order orderToDelete = pendingOrders.FirstOrDefault(o => o.OrderId == orderId);
    if (orderToDelete == null)
    {
        Console.WriteLine("\nOrder not found or not pending.");
        return;
    }

    Console.WriteLine($"\nCustomer: {customer.CustomerName}");
    Console.WriteLine("Ordered Items:");
    orderToDelete.DisplayOrderedFoodItems();
    Console.WriteLine($"Delivery date/time: {orderToDelete.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${orderToDelete.OrderTotal:F2}");
    Console.WriteLine($"Order Status: {orderToDelete.OrderStatus}");

    Console.Write("Confirm deletion? [Y/N]: ");
    if (Console.ReadLine()?.Trim().ToUpper() == "Y")
    {
        orderToDelete.OrderStatus = "Cancelled";

        Restaurant restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == orderToDelete.Restaurant.RestaurantId);
        if (restaurant != null)
        {
            restaurant.RefundStack.Push(orderToDelete);
        }

        Console.WriteLine($"\nOrder {orderId} cancelled. Refund of ${orderToDelete.OrderTotal:F2} processed.");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }
}

void BulkProcessOrders()
{
    // Advanced Function Qn 1
    //==========================================================
    // Student Number : S10275126J
    // Student Name   : Guan Jiahong
    //==========================================================
    
}

void DisplayTotalOrderAmounts(List<Restaurant> restaurantsList)
{
    // Advanced Function Qn 2
    //==========================================================
    // Student Number : S10259724J
    // Student Name   : Victoria Kate Lee Hui Xin
    //==========================================================
    Console.WriteLine("\nTotal Order Amounts");
    Console.WriteLine("===================");

    double totalGruberooEarnings = 0;
    double totalAllRefunds = 0;
    double totalAllRestaurantEarnings = 0;

    foreach (Restaurant restaurant in restaurantsList)
    {
        double restaurantEarnings = 0;
        double totalRefunds = 0;

        List<Order> deliveredOrders = restaurant.OrderQueue.Where(o => o.OrderStatus == "Delivered").ToList();
        
        foreach (Order order in deliveredOrders)
        {
            // Gruberoo calculation:
            // 30% of each order amount (excluding delivery fee) + 5$ delivery fee.
            double gruberooCommision = order.OrderTotal * 0.30;

            totalGruberooEarnings += gruberooCommision;
            restaurantEarnings += order.OrderTotal;
        }

        // refunds
        foreach (Order refundedOrder in restaurant.RefundStack)
        {
            totalRefunds += refundedOrder.OrderTotal;
        }

        Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");
        Console.WriteLine($"   - Total order amount: ${restaurantEarnings.ToString("0.00")}");
        Console.WriteLine($"   - Total refund amount: ${totalRefunds.ToString("0.00")}\n");

        totalAllRefunds += totalRefunds;
        totalAllRestaurantEarnings += restaurantEarnings;
    }

    // display all restaurant amount and final amount Gruberoo earned
    Console.WriteLine("\n========================================");
    Console.WriteLine($"GRAND TOTALS:");
    Console.WriteLine($"  Total order amount: ${totalAllRestaurantEarnings.ToString("0.00")}");
    Console.WriteLine($"  Total refunds: ${totalAllRefunds.ToString("0.00")}");
    Console.WriteLine($"  Final Gruberoo Earnings: ${totalGruberooEarnings.ToString("0.00")}");
    Console.WriteLine("========================================");
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

    string? choice = Console.ReadLine().Trim();

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
            ModifyOrder(customers, restaurants);
            break;
        case "6":
            DeleteOrder(customers);
            break;
        case "7":
            BulkProcessOrders();
            break;
        case "8":
            DisplayTotalOrderAmounts(restaurants);
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