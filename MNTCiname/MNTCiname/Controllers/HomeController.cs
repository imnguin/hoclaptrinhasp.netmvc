using MNTCiname.Models;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MNTCiname.Controllers
{
    public class HomeController : Controller
    {
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        MNTCinemaDataContext db = new MNTCinemaDataContext();
        public ActionResult Header()
        {
            ViewBag.Category = db.TheLoais.ToList();
            ViewBag.phim = db.Phims.ToList();
            return PartialView();
        }
        public ActionResult Index()
        {
            ViewBag.Category = db.TheLoais.Take(4).ToList();
            ViewBag.a = db.Phims.Take(3).ToList();
            return View(db.Phims.ToList());
        }

        public ActionResult About()
        {


            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public ActionResult EventCategory()
        {
            return View();
        }
        public ActionResult MovieCategory(String searchString)
        {
            ViewData["error1"] = "Vui lòng nhập từ khóa";
            var model = db.Phims.Where(p => p.TenPhim.Contains(searchString));
            if (model.Count() > 0)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult MovieSingle(int id)
        {
            Phim phim = db.Phims.SingleOrDefault(a => a.ID == id);
            return View(phim);
        }
        public ActionResult UpdateProfile(FormCollection collection)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["nguoidung"];
            var hoten = collection["HoTen"];
            var gioitinh = collection["GioiTinh"];
            var diachi = collection["DiaChi"];
            var sdt = collection["SDT"];

            NguoiDung updadenguoidung = db.NguoiDungs.SingleOrDefault(a => a.Email == nguoiDung.Email);
            updadenguoidung.HoTen = hoten;
            updadenguoidung.GioiTinh = gioitinh;
            updadenguoidung.DiaChi = diachi;
            updadenguoidung.SDT = sdt;
            db.SubmitChanges();
            Session["nguoidung"] = updadenguoidung;
            return RedirectToAction("Account");
        }
        public ActionResult Account()
        {
            Session["Url"] = null;
            Session["Url"] = Request.Url.ToString();
            if (Session["nguoidung"] == null || Session["nguoidung"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public ActionResult DoiMK()
        {
            Session["Url"] = null;
            Session["Url"] = Request.Url.ToString();
            if (Session["nguoidung"] == null || Session["nguoidung"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public ActionResult ConfirmDoiMK(FormCollection collection)
        {
            NguoiDung nguoiDung = (NguoiDung)Session["nguoidung"];
            var mkc = collection["MatKhauCu"];
            var mkm = collection["MatKhauMoi"];
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            NguoiDung updadenguoidung = db.NguoiDungs.SingleOrDefault(a => a.Email == nguoiDung.Email);
            if (updadenguoidung.MatKhau.Equals(GetMD5.Mahoa(mkc)))
            {
                if (!hasLowerChar.IsMatch(mkm) || !hasUpperChar.IsMatch(mkm) || !hasNumber.IsMatch(mkm) || !hasSymbols.IsMatch(mkm))
                {
                    ViewBag.Message = "Chứa ít nhất 1 ký tự hoa, 1 kí tự số và 1 ký tự đặc biệt!";
                    return RedirectToAction("DoiMK");
                }
                else
                {
                    updadenguoidung.MatKhau = GetMD5.Mahoa(mkm);
                    UpdateModel(updadenguoidung);
                    db.SubmitChanges();
                    Session["nguoidung"] = null;
                    return RedirectToAction("Login","User");
                }
            }
            else
            {
                ViewBag.Message = "Mật khẩu không đúng";
                return RedirectToAction("DoiMK");
            }
        }
        public DateTime[] GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates.ToArray();
        }
        public ActionResult MovieBooking(int id)
        {
            Session["Url"] = null;
            Session["Url"] = Request.Url.ToString();
            if (Session["nguoidung"] == null || Session["nguoidung"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var phim = db.Phims.SingleOrDefault(a => a.ID == id);
            ViewBag.DSrap = db.RapPhims.Select(a => a.TenRap).ToList();
            var lichChieus = new List<LichChieu>();
            var query = (from r in db.RapPhims
                         join p in db.Phongs on r.ID equals p.ID_Rap
                         join k in db.KeHoaches on p.ID equals k.ID_Phong
                         join ph in db.Phims on k.ID_Phim equals ph.ID
                         join t in db.ThoiGians on k.ID equals t.ID_KeHoach
                         where ph.ID == id
                         select new
                         {
                             r.TenRap,
                             t.GioChieu,
                             p.ID_Rap,
                             k.ID_Phong,
                             k.ID_Phim,
                             t.ID_KeHoach
                         }).ToList();
            foreach (var item in query)
            {
                LichChieu lichChieu = new LichChieu(item.TenRap, item.GioChieu, item.ID_Rap, item.ID_Phong, item.ID_Phim, item.ID_KeHoach);

                lichChieus.Add(lichChieu);
            }
            ViewBag.lich = lichChieus;
            var ngaychieu = db.KeHoaches.SingleOrDefault(a => a.ID_Phim == id);
            ViewBag.ngaychieu = GetDatesBetween(ngaychieu.NgayBatDau, ngaychieu.NgayKetThuc);
            return View(phim);
        }
        public List<string> Lstphim(string SearchString)
        {
            return db.Phims.Where(a => a.TenPhim.Contains(SearchString)).Select(a => a.TenPhim).ToList();
        }
        public JsonResult DSphim(string q)
        {
            var data = Lstphim(q);
            return Json(new
            {
                data = data,
                Status = true
            });
        }
        public void luuthongtin(int idrap, int idphong, int idphim, int idkehoach, TimeSpan giochieu, string tenrap, DateTime ngay)
        {
            var phong = db.Phongs.SingleOrDefault(a => a.ID == idphong);
            var phim = db.Phims.SingleOrDefault(a => a.ID == idphim);
            Thongtinve thongtinve = new Thongtinve(idphim, idphong, idkehoach, tenrap, phim.TenPhim, phong.TenPhong, giochieu, ngay);
            Session["Temp"] = null;
            Session["Temp"] = thongtinve;
        }
        public ActionResult SeatBooking(int idrap, int idphong, int idphim, int idkehoach, TimeSpan giochieu, string tenrap, DateTime ngaychieu)
        {
            luuthongtin(idrap, idphong, idphim, idkehoach, giochieu, tenrap, ngaychieu);
            Session["Url"] = null;
            Session["Url"] = Request.Url.ToString();
            ViewBag.Check = db.DatChos.ToList();
            ViewBag.gio = giochieu;
            var phim = db.Phims.SingleOrDefault(x => x.ID == idphim);
            ViewBag.tenphim = phim.TenPhim;
            ViewBag.ngaychieu = ngaychieu;
            return View(db.GheNgois.Where(x => x.ID_Phong == idphong).ToList());
        }
        public ActionResult Booking(FormCollection collection)
        {
            var chonghe = collection["cb"];
            if (chonghe != null)
            {
                List<GheNgoi> gheNgois = new List<GheNgoi>();
                string[] soghe = chonghe.Split(',');
                for (int i = 0; i < soghe.Length; i++)
                {
                    var gheNgoi = db.GheNgois.SingleOrDefault(a => a.ID == Convert.ToInt32(soghe[i]));
                    gheNgois.Add(gheNgoi);
                }
                int giatien = 0;
                foreach (var item in gheNgois)
                {
                    giatien = giatien + (int)item.Gia;
                }
                ViewBag.gia = giatien;
                Session["ghe"] = null;
                Session["ghe"] = gheNgois;
            }
            return View();
        }
        public ActionResult ThanhToan()
        {
            string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
            string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
            string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
            string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMHHmmss");
            string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
            string notifyurl = ConfigurationManager.AppSettings["notifyUrl"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();

            NguoiDung nguoiDung = (NguoiDung)Session["nguoidung"];
            Thongtinve thongtinve = (Thongtinve)Session["Temp"];
            List<GheNgoi> gheNgois = (List<GheNgoi>)Session["ghe"];
            int diem = 0;
            double tongtien = 0;
            string donhang = "DH ";
            int i = 0;
            foreach (var item in gheNgois)
            {
                i++;
                tongtien = tongtien + (double)item.Gia;
                DatCho datCho = new DatCho();
                datCho.ID_NguoiDung = nguoiDung.ID;
                datCho.ID_KeHoach = thongtinve.idkehoach;
                datCho.ID_GheNgoi = item.ID;
                datCho.GioChieu = thongtinve.gioChieu;
                datCho.Ngay = thongtinve.ngay;
                datCho.Gia = item.Gia;
                datCho.NgayDat = DateTime.Now;
                db.DatChos.InsertOnSubmit(datCho);
                db.SubmitChanges();
                if(i < gheNgois.Count())
                {
                    donhang = donhang + datCho.ID + ",";
                }
                if (i == gheNgois.Count())
                {
                    donhang = donhang + datCho.ID;
                }
                if (item.Hang.Equals("Ghế thường"))
                {
                    diem = nguoiDung.DiemTieuDung + 1;
                }
                if(item.Hang.Equals("Ghế vip"))
                {
                    diem = nguoiDung.DiemTieuDung + 2;
                }
            }
            nguoiDung.DiemTieuDung = diem;
            db.SubmitChanges();

            string amount = tongtien.ToString();
            string orderid = donhang.ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity Cryto = new MoMoSecurity();
            string signature = Cryto.signSHA256(rawHash, serectKey);

            JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount},
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }
                };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);
            if (signature != Request["signature".ToString()])
            {
                ViewBag.message = "THÔNG TIN REQUEST KHÔNG HỢP LỆ ";
            }
            else if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "THANH TOÁN THẤT BẠI";
            }
            else
            {
                ViewBag.message = "THANH TOÁN THÀNH CÔNG";
            }
            return View();
        }
        public JsonResult NotifyUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);

            if (signature == Request["signature".ToString()])
            {
                string orderId = Request.QueryString["orderId"].ToString();
                string[] capnhatthanhtoan = orderId.Split(',');
                foreach(var item in capnhatthanhtoan)
                {
                    DatCho datCho = db.DatChos.SingleOrDefault(a => a.ID == int.Parse(item));
                    datCho.ThanhToan = true;
                    db.SubmitChanges();
                }
            }
            else
            {
                string orderId = Request.QueryString["orderId"].ToString();
                string[] capnhatthanhtoan = orderId.Split(',');
                foreach (var item in capnhatthanhtoan)
                {
                    DatCho datCho = db.DatChos.SingleOrDefault(a => a.ID == int.Parse(item));
                    datCho.ThanhToan = false;
                    db.SubmitChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DatCho()
        {
            NguoiDung nguoiDung = (NguoiDung)Session["nguoidung"];
            Thongtinve thongtinve = (Thongtinve)Session["Temp"];
            List<GheNgoi> gheNgois = (List<GheNgoi>)Session["ghe"];
            string qrText = thongtinve.tenRap + "," + thongtinve.tenPhim + "," + thongtinve.ngay + "," + thongtinve.gioChieu + ",";
            int diem = 0;
            string soghe = "";
            string donhang = "DH";
            int i = 0;
            decimal gia = 0;
            foreach (var item in gheNgois)
            {
                i++;
                if(item.Hang == "Ghế vip")
                {
                    diem = diem + nguoiDung.DiemTieuDung + 1;
                }
                if(item.Hang == "Ghế Thường")
                {
                    diem = diem + nguoiDung.DiemTieuDung + 2;
                }    
                DatCho datCho = new DatCho();
                datCho.ID_NguoiDung = nguoiDung.ID;
                datCho.ID_KeHoach = thongtinve.idkehoach;
                datCho.ID_GheNgoi = item.ID;
                datCho.GioChieu = thongtinve.gioChieu;
                datCho.Ngay = thongtinve.ngay;
                datCho.Gia = item.Gia;
                datCho.NgayDat = DateTime.Now;
                datCho.ThanhToan = false;
                db.DatChos.InsertOnSubmit(datCho);
                db.SubmitChanges();
                gia = gia + item.Gia;
                if (i < gheNgois.Count())
                {
                    soghe = soghe + item.Hang +"("+ item.SoGhe+"),";
                    donhang = donhang + datCho.ID + ",";
                }
                if (i == gheNgois.Count())
                {
                    soghe = soghe + item.Hang + "(" + item.SoGhe + ")";
                    donhang = donhang + datCho.ID;
                }
            }
            nguoiDung.DiemTieuDung = diem;
            qrText = qrText + "," + donhang + "," + gia + "," + "Chưa thanh toán";
            ViewBag.Gia = gia;
            ViewBag.DH = donhang;
            db.SubmitChanges();
            Session["ghe"] = gheNgois;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            ViewBag.QRCode = BitmapToBytes(qrCodeImage);

            return View();
        }
    }

}