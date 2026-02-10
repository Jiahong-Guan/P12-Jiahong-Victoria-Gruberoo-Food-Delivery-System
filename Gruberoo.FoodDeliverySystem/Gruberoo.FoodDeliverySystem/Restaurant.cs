//==========================================================
// Student Number : S10259724J
// Student Name   : Victoria Kate Lee Hui Xin
// Partner Name   : Guan Jiahong
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruberoo.FoodDeliverySystem
{
    internal class Restaurant
    {
        //fields
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

        public List<Menu> MenuList { get; private set; } = new List<Menu>();
        public Queue<Order> OrderQueue { get; set; } = new Queue<Order>();

        public Stack<Order> RefundStack { get; set; } = new Stack<Order>();
        public List<SpecialOffer> SpecialOfferList { get; private set; } = new List<SpecialOffer>();


        //default constructor
        public Restaurant()
        {
            RestaurantId = "";
            RestaurantName = "";
            RestaurantEmail = "";
            MenuList.Add(new Menu("M001", "Main Menu"));
        }

        //parameterised constructor
        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;
            MenuList.Add(new Menu("M001", "Main Menu"));
        }

        //methods
        public void DisplayOrders()
        {
            Console.WriteLine($"Orders for {RestaurantName}:");
            Console.WriteLine("==============================");

            if (OrderQueue.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (Order order in OrderQueue)
            {
                Console.WriteLine($"Order ID: {order.OrderId}");
                Console.WriteLine($"Customer: {order.Customer.EmailAddress}");
                Console.WriteLine($"Delivery: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Status: {order.OrderStatus}");
                Console.WriteLine($"Total: ${order.OrderTotal:F2}");
                Console.WriteLine("--------------------------");
            }
        }

        public void DisplaySpecialOrders() 
        {
            Console.WriteLine($"Special Offers for {RestaurantName}:");
            Console.WriteLine("===================================");

            if (SpecialOfferList.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }

            foreach (SpecialOffer offer in SpecialOfferList)
            {
                Console.WriteLine($"Code: {offer.OfferCode}");
                Console.WriteLine($"Description: {offer.OfferDesc}");
                Console.WriteLine($"Discount: {offer.Discount}%");
                Console.WriteLine("--------------------------");
            }
        }

        public void DisplayMenu()
        {
            foreach (Menu menu in MenuList)
            {
                menu.DisplayFoodItems();
            }
        }

        public void AddMenu(Menu menu) 
        {
            MenuList.Add(menu);
        }

        public bool RemoveMenu(Menu menu) 
        {
            return MenuList.Remove(menu);
        }

        public override string ToString()
        {
            return $"{RestaurantName} (ID: {RestaurantId}) - {RestaurantEmail}";
        }
    }
}
