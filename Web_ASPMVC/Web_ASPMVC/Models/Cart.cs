using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ASPMVC.Models
{
    public class Cart
    {
        WebASPMVCDataContext data = new WebASPMVCDataContext();

        public int iIdProduct { set; get; }
        public string sNameProduct { set; get; }
        public string sThumbnailPrdouct { set; get; }
        public string sColor { get; set; }
        public Double dPriceProduct { set; get; }
        public int iQtyPrdouct { set; get; }
        public Double dTotalPriceProduct
        {
            get { return iQtyPrdouct * dPriceProduct; }
        }

        public Cart(int iIdPro, int iQty)
        {
            iIdProduct = iIdPro;
            Product product = data.Products.Single(a => a.ID == iIdProduct);
            sNameProduct = product.Name;
            sThumbnailPrdouct = product.Thumbnail;
            sColor = null;
            dPriceProduct = double.Parse(product.Price.ToString());
            iQtyPrdouct = iQty;
        }
    }
}