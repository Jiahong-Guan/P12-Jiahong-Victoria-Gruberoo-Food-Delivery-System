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
        public List<Order> OrderList { get; private set; } = new List<Order>();
        public List<SpecialOffer> SpecialOfferList { get; private set; } = new List<SpecialOffer>();


        //default constructor
        public Restaurant()
        {
            RestaurantId = "";
            RestaurantName = "";
            RestaurantEmail = "";
        }

        //parameterised constructor
        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;
        }

        //methods
        public void DisplayOrders()
        {

        }

        public void DisplaySpecialOrders() 
        {

        }

        public void DisplayMenu()
        {

        }

        public void AddOrder(string Menu) 
        {

        }

        public bool RemoveOrder(string Menu) 
        {
            return false;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
