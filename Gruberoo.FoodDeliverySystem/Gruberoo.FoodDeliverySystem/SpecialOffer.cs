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
    internal class SpecialOffer
    {
        public string OfferCode { get; set; }
        public string OfferDesc { get; set; }
        public double Discount { get; set; }


        public SpecialOffer()
        {
            OfferCode = "";
            OfferDesc = "";
            Discount = 0;
        }

        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            OfferCode = offerCode;
            OfferDesc = offerDesc;
            Discount = discount;
        }

        public override string ToString()
        {
            return $"Code: {OfferCode} Desc: {OfferDesc} Discount: {Discount}";
        }
    }
}
