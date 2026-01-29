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
    internal class Menu
    {
        //fields
        public string MenuId { get; set; }
        public string MenuName { get; set; }

        public List<FoodItem> FoodItemList { get; private set; } = new List<FoodItem>();




        //default constructor
        public Menu()
        {
            MenuId = "";
            MenuName = "";
        }

        //parameterised constructor
        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
        }

        //methods
        public void AddFoodItem(string FoodItem)
        {
            throw new NotImplementedException();
        }

        public bool GetFoodItem(string FoodItem)
        {
            throw new NotImplementedException();
        }

        public void DisplayFoodItems()
        {

        }
        public override string ToString()
        {
            return "";
        }
    }
}
