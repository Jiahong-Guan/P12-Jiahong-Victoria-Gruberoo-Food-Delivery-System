//==========================================================
// Student Number : S10275126J
// Student Name   : Guan Jiahong
// Partner Name   : Victoria Kate Lee Hui Xin
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruberoo.FoodDeliverySystem
{
    internal class Customer
    {
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }

        public Dictionary<int, Order> Orders { get; private set; } = new Dictionary<int, Order>();

        public Customer()   
        {
            CustomerName = "";
            EmailAddress = "";
        }

        public Customer(string customerName, string emailAddress)
        {
            CustomerName = customerName;
            EmailAddress = emailAddress;
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order.OrderId, order);
        }

        public void DisplayAllOrders()
        {
            Console.WriteLine("All Orders");
            Console.WriteLine("==========");
            Console.WriteLine($"{"Order ID",-10} {"Customer",-13} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-9} Status");
            Console.WriteLine($"{"--------",-10} {"----------",-13} {"-------------",-15} {"------------------",-20} {"------",-9} ---------");

            if (Orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (KeyValuePair<int, Order> kvp in Orders)
            {
                Order order = kvp.Value;

                Console.WriteLine(order.ToString());
            }
        }

        public bool RemoveOrder(Order order)
        {
            return Orders.Remove(order.OrderId);
        }

        public override string ToString()
        {
            return $"Email: {EmailAddress} Name: {CustomerName}";
        }
    }
}
