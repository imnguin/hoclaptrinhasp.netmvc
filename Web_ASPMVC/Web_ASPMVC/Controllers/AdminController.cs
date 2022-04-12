using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Models;
using PagedList.Mvc;

namespace Web_ASPMVC.Controllers
{
    public class AdminController : Controller
    {

        WebASPMVCDataContext data = new WebASPMVCDataContext();
        // GET: Admin
        // LẤY RA DANH SÁCH ĐƠN ĐẶT HÀNG
        public List<lstOrder> GetLstOrders()
        {
            var lstOrders = from p in data.Products
                            join od in data.OrderDetails on p.ID equals od.IdProduct
                            join o in data.Orders on od.IdOrder equals o.ID
                            join c in data.Customers on o.IdCustomer equals c.ID
                            select new lstOrder()
                            {
                                IdOrder = o.ID,
                                NameProduct = p.Name,
                                Color = od.Color,
                                NameCustomer = c.Name,
                                Address = c.Address,
                                Phone = c.Phone,
                                Qty = od.Qty,
                                Price = od.Price,
                                OrderTime = o.Order_Time,
                                PaymentStatus = o.Payment_Status,
                                OrderStatus = o.Order_Status
                            };
            return lstOrders.OrderByDescending(a => a.OrderTime).ToList();
        }
        // TRANG CHỦ CỦA TRANG QUAN TRỊ
        public ActionResult Index()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Count = GetLstOrders().Where(a => a.OrderStatus == false).Count();
            var listcustomer = data.Customers.ToList();
            ViewBag.customer = listcustomer.Count();
            ViewBag.lstOrder = GetLstOrders().Where(a => a.OrderStatus == false).Take(3);
            ViewBag.lstCustomer = GetListCustomer().OrderByDescending(a => a.ID).Take(3);
            return View(data.Products.Where(a => a.Status == true).ToList());
        }
        //CHI TIẾT ĐƠN ĐẶT HÀNG
        public ActionResult OrderDetails(int id)
        {
            ViewBag.lstorder = GetLstOrders().Where(a => a.IdOrder == id);
            return View();
        }
        // SET TRẠNG THÁI ĐƠN HÀNG VỀ TRẠNG THÁI ĐÃ GIAO
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OrderStatus(int id)
        {
            Order order = data.Orders.SingleOrDefault(a => a.ID == id);
            order.Order_Status = true;
            UpdateModel(order);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        //danh sách đơn đặt hàng, đơn hàng được sắp xếp theo trình tự mới nhất đến cũ
        public ActionResult ListOrder(int? page)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<lstOrder> lstOrders = GetLstOrders();
            int pageNum = (page ?? 1);
            int pageSize = 6;
            return View(lstOrders.ToPagedList(pageNum, pageSize));
        }
        //danh mục sản phẩm
        public ActionResult ListCategory()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Category> lstcategory = data.Categories.ToList();
            return View(lstcategory);
        }
        //Thêm danh mục
        public ActionResult CreateCategory()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCategory(Category category)
        {
            data.Categories.InsertOnSubmit(category);
            data.SubmitChanges();
            return RedirectToAction("ListCategory");
        }
        // Xóa danh mục
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            Category category = data.Categories.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }
        [HttpPost, ActionName("DeleteCategory")]
        public ActionResult ConfirmDeleteCategory(int id)
        {
            Category category = data.Categories.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Product> products = data.Products.Where(a => a.IdCategory == id).ToList();
            if(products.Count > 0)
            {
                category.Status = false;
            }
            else
            {
                data.Categories.DeleteOnSubmit(category);
            }   
            data.SubmitChanges();
            return RedirectToAction("ListCategory");
        }
        // PHƯƠNG THỨC SỬA DANH MỤC SẢN PHẨM VỚI ID TRUYỀN VÀO
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            Category category = data.Categories.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(int id, Category ct)
        {
            Category category = data.Categories.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            category.Name = ct.Name;
            category.Status = ct.Status;
            data.SubmitChanges();
            return RedirectToAction("ListCategory");
        }
        //DANH SÁCH SẢN PHẨM THEO PHÂN TRANG
        public ActionResult ListProduct(int? page)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNum = (page ?? 1);
            var lstproduct = data.Products.Where(a => a.Status == true).ToList();
            int pageSize = 4;
            return View(lstproduct.ToPagedList(pageNum, pageSize));
        }
        //TẠO MỚI MỘT SẢN PHẨM
        [HttpGet]
        public ActionResult CreateProduct()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.IdCategory = new SelectList(data.Categories.ToList().OrderBy(a => a.Name), "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase fileUpload)
        {
            ViewBag.IdCategory = new SelectList(data.Categories.ToList().OrderBy(a => a.Name), "ID", "Name");
            if (fileUpload == null)
            {
                ViewBag.thongbao = "Vui lòng chọn ảnh đại diện";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Product"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    product.Thumbnail = fileName;
                    data.Products.InsertOnSubmit(product);
                    data.SubmitChanges();
                }
                return RedirectToAction("ListProduct");
            }
        }
        //SỬA SẢN PHẨM VỚI ID TRUYỀN VÀO
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ViewBag.IdCategory = new SelectList(data.Categories.ToList().OrderBy(a => a.Name), "ID", "Name");
            Product sp = data.Products.SingleOrDefault(n => n.ID == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(Product sp, HttpPostedFileBase fileUpload, int id)
        {
            ViewBag.IdCategory = new SelectList(data.Categories.ToList().OrderBy(a => a.Name), "ID", "Name");
            Product product = data.Products.SingleOrDefault(a => a.ID == id);
            if (fileUpload != null)
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Product"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    product.Thumbnail = fileName;
                }
            }
            product.IdCategory = sp.IdCategory;
            product.Name = sp.Name;
            product.Status = sp.Status;
            product.Description = sp.Description;
            product.UpdateTime = sp.UpdateTime;
            product.Items_Left = sp.Items_Left;
            product.Title = sp.Title;
            product.Color = sp.Color;
            product.Group = sp.Group;
            product.Price = sp.Price;
            data.SubmitChanges();
            return RedirectToAction("ListProduct");
        }
        //CHI TIẾT SẢN PHẨM VỚI ID TRUYỀN VÀO
        public ActionResult DetailProduct(int ID)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            Product product = data.Products.SingleOrDefault(a => a.ID == ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult DeleteProduct(int ID)
        {
            Product product = data.Products.SingleOrDefault(a => a.ID == ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult ConfirmDeleteProduct(int ID)
        {
            Product product = data.Products.SingleOrDefault(a => a.ID == ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<OrderDetail> orderDetails = data.OrderDetails.Where(a => a.IdProduct == ID).ToList();
            if(orderDetails.Count > 0)
            {
                product.Status = false;
            }else
            {
                data.Products.DeleteOnSubmit(product);
            } 
            data.SubmitChanges();
            return RedirectToAction("ListProduct");
        }
        //LẤY RA DANH SÁCH CÁC TÀI KHOẢN ADMIN
        public ActionResult ListUser(int? page)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNum = (page ?? 1);
            var lstuser = data.Users.ToList();
            int pageSize = 4;
            return View(lstuser.ToPagedList(pageNum, pageSize));
        }

        //Thêm tài khoản ADMIN
        public ActionResult CreateUser()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateUser(User user, HttpPostedFileBase fileUpload, FormCollection collection)
        {
            
            if (fileUpload == null)
            {
                ViewBag.thongbao = "Vui lòng chọn ảnh đại diện";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/User"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    user.Avatar = fileName;
                    var password = collection["Password"];
                    user.PassWord = GetMD5(password);

                    data.Users.InsertOnSubmit(user);
                    data.SubmitChanges();
                }
                return RedirectToAction("ListUser");
            }
        }

        //Chỉnh Sửa tài khoản ADMIN
        [HttpGet]
        public ActionResult EditUser(int id)
        {
            User user = data.Users.SingleOrDefault(a => a.ID == id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(user);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditUser(User u, HttpPostedFileBase fileUpload, int id)
        {
            User user = data.Users.SingleOrDefault(a => a.ID == id);
            if (fileUpload != null)
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/User"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    user.Avatar = fileName;
                }
            }
            user.Name = u.Name;
            user.UserName = u.UserName;
            data.SubmitChanges();
            return RedirectToAction("ListUser");
        }
        //PHƯƠNG THỨC XÓA TÀI KHOẢN ADMIN VỚI ID TRUYỀN VÀO
        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            User user = data.Users.SingleOrDefault(a => a.ID == id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult ConfirmDeleteUser(int id)
        {
            User user = data.Users.SingleOrDefault(a => a.ID == id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Users.DeleteOnSubmit(user);
            data.SubmitChanges();
            return RedirectToAction("ListUser");
        }
        // LẤY DANH SÁCH CÁC KHÁCH HÀNG
        public List<Customer> GetListCustomer()
        {
            return data.Customers.ToList();
        }
        public ActionResult ListCustomer(int? page)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNum = (page ?? 1);
            int pageSize = 4;
            List<Customer> customers = GetListCustomer();
            return View(customers.ToPagedList(pageNum, pageSize));
        }

        //TRẢ VỀ SESSION TÀI KHOÀN ADMIN VÀ HIỂN THỊ TẠI TRANG QUẢN TRỊ
        public ActionResult AdminPartial()
        {
            return PartialView();
        }
        //CHỨC NĂNG ĐĂNG NHẬP TRANG QUAN TRỊ
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
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
                User user = data.Users.SingleOrDefault(a => a.UserName == username && a.PassWord == GetMD5(password));
                if (user != null)
                {
                    Session["admin"] = user;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Title = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }

        // CHỨC NĂNG ĐĂNG XUẤT TÀI KHOẢN TẠI TRANG QUẢN TRỊ
        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        //HÀM MÃ HÓA DỮ LIỆU MD5
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
    }
}