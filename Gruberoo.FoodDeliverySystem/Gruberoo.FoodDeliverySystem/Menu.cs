//==========================================================
// Student Number : S10259724J
// Student Name   : Victoria Kate Lee Hui Xin
// Partner Name   : Guan Jiahong
//==========================================================
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

        //parameterized constructor
        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
        }

        //methods
        public void AddFoodItem(FoodItem foodItem)
        {
            FoodItemList.Add(foodItem);
        }

        public bool RemoveFoodItem(FoodItem foodItem)
        {
            return FoodItemList.Remove(foodItem);
        }

        public void DisplayFoodItems()
        {
            if (FoodItemList.Count == 0)
            {
                Console.WriteLine("   No food items available.");
                return;
            }

            foreach (FoodItem foodItem in FoodItemList)
            {
                Console.WriteLine($"   - {foodItem.ItemName}: {foodItem.ItemDesc} - ${foodItem.ItemPrice.ToString("0.00")}");
            }
        }
        public override string ToString()
        {
            return $"Menu ID: {MenuId} Menu Name: {MenuName}";
        }
    }
}
