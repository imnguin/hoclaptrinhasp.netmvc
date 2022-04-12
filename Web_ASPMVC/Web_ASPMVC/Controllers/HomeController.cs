using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Models;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Web_ASPMVC.Controllers
{
    public class HomeController : Controller
    {
        WebASPMVCDataContext data = new WebASPMVCDataContext();
        //phương thức lấy ra danh sách tất cả sản phẩm
        public List<Product> GetListProduct()
        {
            return data.Products.ToList();
        }
        //lấy dữ liệu trả về view trang chủ
        public ActionResult Index()
        {
            ViewBag.pro = GetListProduct().OrderByDescending(a => a.Group == "Mới").Take(3);
            ViewBag.GroupProduct = GetListProduct().Where(a => a.Group == "Nổi bật").Take(6);
            ViewBag.slides = data.Slides.Take(6).ToList();
            return View();
        }
        //lấy ra danh sách tất cả danh mục được sắp xếp theo ngày cập nhật
        private List<Product> GetListProductByCategory()
        {
            return data.Products.OrderByDescending(a => a.UpdateTime).ToList();
        }
        //trả về sản phẩm xem theo dạng danh sách tại website có phân trang
        public ActionResult listView(int? page)
        {
            int pageNum = (page ?? 1);
            var lstproduct = GetListProductByCategory();
            int pageSize = 11;
            return View(lstproduct.ToPagedList(pageNum, pageSize));
        }
        //trả về danh sách các sản phẩm theo danh mục
        public ActionResult product(int id)
        {
            return View(data.Products.Where(a=>a.IdCategory == id).ToList());
        }
        //trả về chi tiết sản phẩm với mã id truyền vào
        public ActionResult productDetails(int? id)
        {
            var dt = data.Products.Where(a => a.ID == id).ToList();
            ViewBag.details = dt;
            foreach (var item in dt)
            {
                if (item.Price == null || item.Price < 1)
                {
                    ViewData["Tilte1"] = "Đang cập nhật";
                }
                ViewBag.lsproduct = data.Products.Where(a => a.IdCategory == item.IdCategory).ToList();
            }
            return View();
        }
        //chức năng tìm kiếm sản phẩm theo tên
        public ActionResult Search(String searchString)
        {
            ViewData["error1"] = "Vui lòng nhập từ khóa";
            var model = data.Products.Where(p => p.Name.Contains(searchString) && p.Status == true);
            if(model.Count() <= 0)
            {
                ViewData["error2"] = "Không tìm thấy sản phẩm";
            }    
            return View(model);
        }    
        // hàm mã hóa dữ liệu MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        //trả về dữ liệu menu bên trái của webstie
        public ActionResult LeftMenu()
        {
            ViewBag.cate = data.Categories.Where(a => a.Status == true).ToList();
            return PartialView();
        }
        //trả về partialview sử dụng chức năng đăng nhập của trang web tại thanh menu
        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["Username"];
            var password = collection["Password"];
            Customer customer = data.Customers.SingleOrDefault(a => a.Username == username && a.Password == GetMD5(password));
            if (customer != null)
            {
                Session["Username"] = customer;
                return RedirectToAction("Index", "Home");
            }
            return PartialView();
        }
        //chức năng đăng ký thành viên của khách hàng
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(FormCollection collection, Customer customer)
        {
            var name = collection["Name"];
            var username = collection["Username"];
            var password = collection["Password"];
            var enterthepassword = collection["Enterthepassword"];
            var email = collection["Email"];
            var address = collection["Address"];
            var phone = collection["Phone"];
            var birthday = collection["Birthday"];
            if (string.IsNullOrEmpty(name))
            {
                ViewData["error1"] = "Họ tên không được để trống";
            }
            else if (string.IsNullOrEmpty(username))
            {
                ViewData["error2"] = "Tài khoản không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(password))
            {
                ViewData["error3"] = "Vui lòng nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(enterthepassword))
            {
                ViewData["error4"] = "Vui lòng nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["error5"] = "Email không được để trống";
            }
            else if (string.IsNullOrEmpty(phone))
            {
                ViewData["error6"] = "Vui lòng nhập số điện thoại";
            }
            else
            {
                if (password != enterthepassword)
                {
                    ViewData["error7"] = "Mật khẩu nhập lại không đúng!";
                }
                else
                {
                    customer.Name = name;
                    customer.Username = username;
                    customer.Password = GetMD5(password);
                    customer.Email = email;
                    customer.Address = address;
                    customer.Phone = phone;
                    customer.Birthday = DateTime.Parse(birthday);
                    data.Customers.InsertOnSubmit(customer);
                    data.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }
            return this.register();
        }
        //chức năng đăng nhập của khách hàng khi cần đặt hàng
        [HttpGet]
        public ActionResult Userlogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Userlogin(FormCollection collection)
        {
            var username = collection["Username"];
            var password = collection["Password"];
            if (string.IsNullOrEmpty(username))
            {
                ViewData["error1"] = "Tên đăng nhập không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(password))
            {
                ViewData["error2"] = "Vui lòng nhập mật khẩu!";
            }
            else
            {
                Customer customer = data.Customers.SingleOrDefault(a => a.Username == username && a.Password == GetMD5(password));
                if (customer != null)
                {
                    Session["Username"] = customer;
                    return RedirectToAction("Order", "Cart");
                }
                else
                {
                    ViewBag.Title = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }
        //chức năng đăng xuất của khách hàng
        public ActionResult Logout()
        {
            Session["Username"] = null;
            return RedirectToAction("Index", "Home");
        }
        //trả về thông tin của khách hàng khi khách hàng xem thông tin cá nhân
        public ActionResult ProfileCustomer()
        {
            return View();
        }
        //chức năng đổi mật khẩu của khách hàng
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChangePassword(FormCollection collection)
        {
            var username = collection["Username"];
            var password = collection["Password"];
            var newPassword = collection["NewPassword"];
            List<Customer> customers = data.Customers.ToList();
            Customer customer = customers.SingleOrDefault(a => a.Username == username);
            if (customer == null)
            {
                ViewData["ThongBao1"] = "Tài khoản không tồn tại";
            }
            else
            {
                if (customer.Password == GetMD5(password))
                {
                    customer.Password = GetMD5(newPassword);
                    data.SubmitChanges();
                    ViewData["ThongBao3"] = "Đặt lại mật khẩu thành công";
                }
                else
                {
                    ViewData["ThongBao2"] = "Mật khẩu không đúng";
                }
            }
            return View();
        }
        //trả về dữ liệu thông tin của cửa hàng
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forgotpassword(FormCollection collection)
        {
            List<Customer> customers = data.Customers.ToList();
            var email = collection["Email"];
            Customer customer = customers.SingleOrDefault(a => a.Email == email);
            if(customer!=null)
            {
                string newpass = "e10adc3949ba59abbe56e057f20f883e";
                customer.Password = newpass;
                data.SubmitChanges();
                ViewData["Thongbao2"] = "Kiểm tra email của bạn để xem mật khẩu mới";
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Client/Forgotpassword.html"));
                content = content.Replace("{{newpassword}}", newpass);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(email, "Lấy lại mật khẩu", content);
            }
            else
            {
                ViewData["Thongbao1"] = "Email không tồn tại!";
            }    
            return View();
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