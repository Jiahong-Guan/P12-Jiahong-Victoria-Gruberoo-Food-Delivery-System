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
    internal class FoodItem
    {
        //fields
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public string Customise { get; set; }

        //default constructor
        public FoodItem() 
        {
            ItemName = "";
            ItemDesc = "";
            ItemPrice = 0;
            Customise = "";
        }

        //parameterised constructor
        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise) 
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = customise;
        }

        //methods
        public override string ToString()
        {
            return "";
        }
    }
}
