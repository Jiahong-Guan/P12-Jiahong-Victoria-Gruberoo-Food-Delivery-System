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
    internal class OrderHistoryAnalyzer
    {
        // orderFrequency -> CustomerEmail -> RestaurantId -> FoodItemId -> Frequency
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> orderFrequency;
        public OrderHistoryAnalyzer()
        {
            orderFrequency = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
        }

        public void RecordOrder(Order order)
        {
            string customerEmail = order.Customer.EmailAddress;
            string restaurantId = order.Restaurant.RestaurantId;

            // initialize if dont exist
            if (!orderFrequency.ContainsKey(customerEmail))
            {
                orderFrequency[customerEmail] = new Dictionary<string, Dictionary<string, int>>();
            }

            if (!orderFrequency[customerEmail].ContainsKey(restaurantId))
            {
                orderFrequency[customerEmail][restaurantId] = new Dictionary<string, int>();
            }

            // count every item in the order
            foreach (OrderedFoodItem ofi in order.OrderedItems)
            {
                string itemName = ofi.ItemName;

                if (orderFrequency[customerEmail][restaurantId].ContainsKey(itemName))
                {
                    orderFrequency[customerEmail][restaurantId][itemName] += ofi.QtyOrdered;
                }
                else
                {
                    orderFrequency[customerEmail][restaurantId][itemName] = ofi.QtyOrdered;
                }
            }
        }

        public List<KeyValuePair<string, int>> GetFrequentItems(string customerEmail, string restaurantId)
        {
            if (!orderFrequency.ContainsKey(customerEmail) || !orderFrequency[customerEmail].ContainsKey(restaurantId))
            {
                return new List<KeyValuePair<string, int>>();
            }

            // sort items by freq (descending) and get top 3
            return orderFrequency[customerEmail][restaurantId]
                .OrderByDescending(kvp => kvp.Value)
                .Take(3)
                .ToList();
        }

        public bool HasOrderedBefore(string customerEmail, string restaurantId)
        {
            return orderFrequency.ContainsKey(customerEmail) && orderFrequency[customerEmail].ContainsKey(restaurantId);
        }

        public List<string> GetCustomerRestaurants(string customerEmail)
        {
            if (orderFrequency.ContainsKey(customerEmail)) {
                return orderFrequency[customerEmail].Keys.ToList();
            }
            return new List<string>();
        }
    }
}
