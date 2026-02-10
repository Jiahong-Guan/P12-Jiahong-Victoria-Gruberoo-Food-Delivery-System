//==========================================================
// Student Number : S10259724J
// Student Name   : Victoria Kate Lee Hui Xin
// Partner Name   : Guan Jiahong
//==========================================================
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
        public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, string customise, int qtyOrdered) : base(itemName, itemDesc, itemPrice, customise)
        {
            QtyOrdered = qtyOrdered;
            SubTotal = CalculateSubTotal();
        }

        //methods
        public double CalculateSubTotal()
        {
            return QtyOrdered * ItemPrice;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - {QtyOrdered}";
        }
    }
}
