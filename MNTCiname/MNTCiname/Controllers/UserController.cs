using MNTCiname.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MNTCiname.Controllers
{
    public class UserController : Controller
    {
        // GET: UserLogin
        MNTCinemaDataContext db = new MNTCinemaDataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            var name = collection["Name"];
            var email = collection["Email"];
            var password = collection["Password"];
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            NguoiDung ktnguoiDungs = db.NguoiDungs.SingleOrDefault(a =>a.Email == email);
            if (string.IsNullOrEmpty(name))
            {
                ViewData["error1"] = "Vui lòng nhập tên!";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["error2"] = "Email không được để trống!";
            }
            else if (string.IsNullOrEmpty(password))
            {
                ViewData["error3"] = "Vui lòng nhập mật khẩu!";
            } 
            else if (!hasLowerChar.IsMatch(password) || !hasUpperChar.IsMatch(password) || !hasNumber.IsMatch(password) || !hasSymbols.IsMatch(password))
            {
                ViewData["ErrorMessage"] = "Chứa ít nhất 1 ký tự hoa, 1 kí tự số và 1 ký tự đặc biệt!";
            }
            else
            {
                if(ktnguoiDungs!=null)
                {
                    ViewData["error4"] = "Tài khoản đã tồn tại!";
                }
                else
                {

                    NguoiDung nguoiDung = new NguoiDung();
                    nguoiDung.HoTen = name;
                    nguoiDung.Email = email;
                    nguoiDung.MatKhau = GetMD5.Mahoa(password);
                    db.NguoiDungs.InsertOnSubmit(nguoiDung);
                    db.SubmitChanges();
                    return RedirectToAction("Index", "Home");
                }    
            }
            return this.Register();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(FormCollection collection)
        {
            var email = collection["Email"];
            var password = collection["Password"];
            NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(a => a.Email == email);
            if(string.IsNullOrEmpty(email))
            {
                ViewData["error1"] = "Vui lòng nhập email!";
            }
            else if(string.IsNullOrEmpty(password))
            {
                ViewData["error2"] = "Vui lòng nhập mật khẩu!";
            }
            else
            {
                if (nguoiDung == null)
                {
                    ViewData["error3"] = "Tài khoản không tồn tại. Vui lòng tạo tài khoản!";
                }
                else
                {
                    if (nguoiDung.MatKhau.Equals(GetMD5.Mahoa(password)))
                    {
                        Session["nguoidung"] = nguoiDung;
                        if(Session["Url"] == null || Session["Url"].ToString() == "")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(Session["Url"].ToString());
                        }    
                        
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "khẩu không chính xác!";
                    }
                }
            }
            return this.Login();
        }
        public ActionResult Logout()
        {
            Session["nguoidung"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        public ActionResult DatLaiMatKhau()
        {
            return View();
        }
        public ActionResult ConfirmDatLaiMatKhau(FormCollection collection)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["nguoidung"];
            var code = collection["Code"];
            var mkm = collection["MatKhauMoi"];
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            NguoiDung resetpass = db.NguoiDungs.SingleOrDefault(a => a.Email == nguoiDung.Email);
            if(resetpass.Code.Equals(code))
            {
                if (!hasLowerChar.IsMatch(mkm) || !hasUpperChar.IsMatch(mkm) || !hasNumber.IsMatch(mkm) || !hasSymbols.IsMatch(mkm))
                {
                    ViewBag.Message = "Chứa ít nhất 1 ký tự hoa, 1 kí tự số và 1 ký tự đặc biệt!";
                    return RedirectToAction("DatLaiMatKhau");
                }
                else
                {
                    resetpass.MatKhau = GetMD5.Mahoa(mkm);
                    UpdateModel(resetpass);
                    db.SubmitChanges();
                    Session["nguoidung"] = null;
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                ViewBag.Message = "Mã xác nhận không chính xác";
                return RedirectToAction("DatLaiMatKhau");
            }    
        }
        public ActionResult Sendcode(FormCollection collection)
        {
            var email = collection["Email"];
            NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(a => a.Email == email);
            if (nguoiDung == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại!";
                return View();
            }
            else
            {
                string Code = RandomCode.MaXacNhan();
                nguoiDung.Code = Code;
                UpdateModel(nguoiDung);
                db.SubmitChanges();
                Session["nguoidung"] = null;
                Session["nguoidung"] = nguoiDung;
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Mail/SendCode.html"));
                string title = Code + " là mã khôi phục tài khoản của bạn";
                content = content.Replace("{{HoTen}}", nguoiDung.HoTen);
                content = content.Replace("{{Code}}", Code);
                content = content.Replace("{{Email}}", nguoiDung.Email);
                new MailHelper().SendMail(nguoiDung.Email, title, content);
                return RedirectToAction("DatLaiMatKhau");
            }
        }
    }
}