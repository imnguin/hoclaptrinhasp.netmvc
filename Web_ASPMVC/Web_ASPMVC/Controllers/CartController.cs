using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Web_ASPMVC.Models;

namespace Web_ASPMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        WebASPMVCDataContext data = new WebASPMVCDataContext();
        public ActionResult Index()
        {
            return View();
        }
        //Lay danh sach san pham co trong gio hang
        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }
        //tinh tong so luong san pham
        private int TotalQty()
        {
            int iTotalQty = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTotalQty = lstCart.Sum(a => a.iQtyPrdouct);
            }
            return iTotalQty;
        }
        //tinh tong tien san pham trong gio hang
        private double TotalPrice()
        {
            double iTotalPrice = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTotalPrice = lstCart.Sum(a => a.dTotalPriceProduct);
            }
            return iTotalPrice;
        }
        //Gio hang
        public ActionResult cart(string strURL)
        {
            List<Cart> lstCart = GetCart();
            if (lstCart.Count == 0)
            {
                ViewData["Thongbao"] = "Giỏ hàng rỗng";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TotalQty = TotalQty();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.URL = strURL;
            return View(lstCart);
        }
        //Them san pham vao gio hang
        public ActionResult AddCart(int iIdPro, string strURL, FormCollection Collection)
        {
            List<Cart> lstCart = GetCart();
            Cart product = lstCart.Find(a => a.iIdProduct == iIdPro);
            Product pd = data.Products.SingleOrDefault(a => a.ID == iIdPro);
            var color = Collection["txtColor"];
            var qty = Collection["txtQty"];
            if (product == null)
            {
                if (qty == null)
                {
                    if(pd.Items_Left > 0)
                    {
                        product = new Cart(iIdPro, 1);
                    }
                    else
                    {
                        ViewData["Thongbao"] = "Sản phẩm tạm hết hàng";
                    }
                }
                else
                {
                    int iQty = int.Parse(qty.ToString());
                    if (pd.Items_Left >= iQty)
                    {
                        product = new Cart(iIdPro, iQty);
                    }
                    else
                    {
                        ViewData["Thongbao"] = "Số lượng tại cửa hàng không đủ!";
                    }    
                }
                product.sColor = color;
                if (product.sColor == null)
                {
                    product.sColor = "Đỏ";
                    lstCart.Add(product);
                    return Redirect(strURL);
                }
                else
                {
                    lstCart.Add(product);
                    return Redirect(strURL);
                }

            }
            else
            {
                pd.Items_Left -= product.iQtyPrdouct;
                if (qty != null)
                {
                    int iQty = int.Parse(qty.ToString());
                    if (pd.Items_Left >= iQty)
                    {
                        product.iQtyPrdouct += iQty;
                    }
                    else
                    {
                        ViewData["Thongbao"] = "Số lượng tại cửa hàng không đủ";
                    }    
                }
                else
                {
                    product.iQtyPrdouct++;
                }
                return Redirect(strURL);
            }
        }
        
        //Xóa Giỏ Hàng
        public ActionResult DeleteCart(int iIdProduct)
        {
            //Lay gio hang tu Session
            List<Cart> lstcart = GetCart();
            //Kiem tra sach da co trong sesion gio hang
            Cart product = lstcart.SingleOrDefault(n => n.iIdProduct == iIdProduct);
            if (product != null)
            {
                lstcart.RemoveAll(n => n.iIdProduct == iIdProduct);
                return RedirectToAction("cart");
            }
            if (lstcart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
        //Cập nhật giỏ hàng
        public ActionResult UpdateCart(int iIdProduct, FormCollection f)
        {
            Product pd = data.Products.SingleOrDefault(a => a.ID == iIdProduct);
            //Lay gio hang tu Session
            List<Cart> lstcart = GetCart();
            //Kiem tra sach da co trong Sesion gio hang
            Cart product = lstcart.SingleOrDefault(n => n.iIdProduct == iIdProduct);
            //Neu ton tai thi cho sua so luong
            if (product != null)
            {
                pd.Items_Left -= product.iQtyPrdouct;
                int iQty = int.Parse(f["txtQty"].ToString());
                if(pd.Items_Left >= iQty)
                {
                    product.iQtyPrdouct = iQty;
                }
                product.sColor = f["txtColor"].ToString();
            }
            return RedirectToAction("Cart");
        }
        // Cập nhật phương thức đặt hàng

        //Hiển thị View Đặt hàng để cập nhật các thông tin đơn hàng

        [HttpGet]
         public ActionResult Order()
         {
             //Kiểm tra đăng nhập
             if (Session["Username"] == null || Session["Username"].ToString() == "")
             {
                 return RedirectToAction("Userlogin", "Home");
             }
             if (Session["Cart"] == null)
             {
                 return RedirectToAction("Index", "Home");

             }
             List<Cart> lstcart = GetCart();
             ViewBag.TotalQty = TotalQty();
             ViewBag.TotalPrice = TotalPrice();
             return View(lstcart);
         }
         //đặt hàng
         public ActionResult Order(FormCollection collection)
         {
             //Thêm đơn hàng
             
             Order ddh = new Order();
             Customer kh = (Customer)Session["Username"];
             List<Cart> gh = GetCart();
             ddh.IdCustomer = kh.ID;
             ddh.Order_Time = DateTime.Now;
             var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Shiptime"]);
             ddh.Order_Status = false;
             ddh.Payment_Status = false;
             data.Orders.InsertOnSubmit(ddh);
             data.SubmitChanges();

             //Thêm chi tiết đơn hàng
             foreach (var item in gh)
             {
                 OrderDetail ctdh = new OrderDetail();
                 ctdh.IdOrder = ddh.ID;
                 ctdh.IdProduct = item.iIdProduct;
                 ctdh.Qty = item.iQtyPrdouct;
                 ctdh.Price = (decimal)item.dPriceProduct;
                 ctdh.Color = item.sColor;
                 Product pd = data.Products.SingleOrDefault(a => a.ID == ctdh.IdProduct);
                 pd.Items_Left -= ctdh.Qty;
                 data.OrderDetails.InsertOnSubmit(ctdh);
                 UpdateModel(pd);
                 data.SubmitChanges();
            }
            data.SubmitChanges();
            Session["Cart"] = null;
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Client/NewOrder.html"));
            content = content.Replace("{{CustomerName}}",kh.Name);
            content = content.Replace("{{Phone}}", kh.Phone);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{Address}}", kh.Address);
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendMail(toEmail, "Đơn hàng mới", content);
            new MailHelper().SendMail(kh.Email, "Đơn hàng từ shop batonmobile", content);
            return RedirectToAction("OrderConfirmation", "Cart");
         }
        public ActionResult OrderConfirmation()
        {
            return View();
        }
        public ActionResult CartPartial()
        {
            ViewBag.TotalQty = TotalQty();
            ViewBag.TotalPrice = TotalPrice();
            return PartialView();
        }

        private class MailHelper
        {
            public void SendMail(string toEmaiAddress, string subject, string content)
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

                bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

                string body = content;
                MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmaiAddress));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                var client = new SmtpClient();
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = enabledSsl;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                client.Send(message);
            }
        }
    }
}