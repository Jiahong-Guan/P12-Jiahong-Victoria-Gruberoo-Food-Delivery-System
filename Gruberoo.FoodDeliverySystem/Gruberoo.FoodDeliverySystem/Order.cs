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
        public required string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public required string DeliveryAddress { get; set; }

        // OrderPayment: [CC] Credit Card | [PP] PayPal | [CD] Cash on Delivery
        public required string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }

        public List<OrderedFoodItem> OrderedItems { get; private set; } = new List<OrderedFoodItem>();

        public Order()
        {
            OrderId = 0;
            OrderDateTime = DateTime.Now;
            OrderTotal = 0;
            OrderStatus = "";
        }

        public Order(int orderId, DateTime orderDateTime, double orderTotal, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
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
            return total;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return OrderedItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            foreach (OrderedFoodItem item in OrderedItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public override string ToString()
        {
            return $"Order To be implement";
        }
    }
}
