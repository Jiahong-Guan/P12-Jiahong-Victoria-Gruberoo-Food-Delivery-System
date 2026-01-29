//==========================================================
// Student Number : S10259724J
// Student Name   : Victoria Kate Lee Hui Xin
// Partner Name   : Guan Jiahong
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Gruberoo.FoodDeliverySystem
{
    internal class OrderedFoodItem : FoodItem
    {
        //fields
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

        //default constructor
        public OrderedFoodItem() : base()
        {
            QtyOrdered = 0;
            SubTotal = 0;
        }

        //parameterised constructor
        public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, string customise, int qtyOrdered, double subTotal) : base(itemName, itemDesc, itemPrice, customise)
        {
            QtyOrdered = qtyOrdered;
            SubTotal = subTotal;
        }

        //methods
        public double CalculateSubTotal()
        {
            return QtyOrdered * ItemPrice;
        }
    }
}
