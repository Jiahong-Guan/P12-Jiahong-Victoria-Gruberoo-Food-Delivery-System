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
            EmailAddress = "";
            CustomerName = "";
        }

        public Customer(string emailAddress, string customerName)
        {
            EmailAddress = emailAddress;
            CustomerName = customerName;
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order.OrderId, order);
        }

        public void DisplayAllOrders()
        {
            Console.WriteLine("Order");
            foreach (KeyValuePair<int, Order> items in Orders)
            {
                throw new NotImplementedException();
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
