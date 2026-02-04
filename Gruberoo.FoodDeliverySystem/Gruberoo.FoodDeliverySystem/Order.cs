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
    internal class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }

        // OrderStatus: Pending | Preparing | Cancelled | Delivered
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }

        // OrderPayment: [CC] Credit Card | [PP] PayPal | [CD] Cash on Delivery
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }
        
        public Customer Customer { get; set; }

        public Restaurant Restaurant { get; private set; }

        public List<OrderedFoodItem> OrderedItems { get; private set; } = new List<OrderedFoodItem>();

        public SpecialOffer SpecialOffer { get; set; }

        public Order()
        {
            OrderId = 0;
            Customer = new Customer();
            Restaurant = new Restaurant();
            DeliveryDateTime = DateTime.Now;
            DeliveryAddress = "";
            OrderDateTime = DateTime.Now;
            OrderTotal = 0;
            OrderStatus = "Preparing";
            OrderPaymentMethod = "CC";
            OrderPaid = false;
        }

        public Order(int orderId, Customer customer, Restaurant restaurant, DateOnly deliveryDate, TimeOnly deliveryTime, string deliveryAddress,  DateTime orderDateTime, double orderTotal, string orderStatus, string orderPaymentMethod, bool orderPaid)
        {
            OrderId = orderId;
            Customer = customer;
            Restaurant = restaurant;
            DeliveryDateTime = deliveryDate.ToDateTime(deliveryTime);
            DeliveryAddress = deliveryAddress;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            OrderPaymentMethod = orderPaymentMethod;
            OrderPaid = orderPaid;
        }

        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (OrderedFoodItem item in OrderedItems)
            {
                total += item.SubTotal;
            }

            // Delivery Fee
            total += 5.00;
            return Math.Round(total, 2);
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedItems.Add(item);
            OrderTotal = CalculateOrderTotal();
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            bool removed = OrderedItems.Remove(item);
            if (removed)
            {
                OrderTotal = CalculateOrderTotal();
            }
            return removed;
        }

        public void DisplayOrderedFoodItems()
        {
            for (int i = 0; i < OrderedItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {OrderedItems[i].ToString()}");
            }
        }

        public override string ToString()
        {
            return $"{OrderId,-10} {Customer.CustomerName,-13} {Restaurant.RestaurantName,-15} {DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"),-20} ${OrderTotal.ToString("0.00"),-8} {OrderStatus}";
        }
    }
}
