using MNTCiname.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNTCiname.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        MNTCinemaDataContext db = new MNTCinemaDataContext();
        // LẤY DANH SÁCH THỂ LOẠI PHIM
        public ActionResult ListTheLoai()
        {
            return View(db.TheLoais.ToList());
        }
        public ActionResult ThemTheLoai()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemTheLoai(TheLoai theLoai)
        {
            db.TheLoais.InsertOnSubmit(theLoai);
            db.SubmitChanges();
            return RedirectToAction("ListTheLoai");
        }
        
        [HttpGet]
        public ActionResult DeleteTheLoai(int id)
        {
            TheLoai theLoai = db.TheLoais.SingleOrDefault(a => a.ID == id);
            if (theLoai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(theLoai);
        }
        [HttpPost, ActionName("DeleteTheLoai")]
        public ActionResult ConfirmDeleteTheLoai(int id)
        {
            TheLoai theLoai = db.TheLoais.SingleOrDefault(a => a.ID == id);
            if (theLoai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Phim> phims = db.Phims.Where(a => a.ID_TheLoai == id).ToList();
            if (phims.Count > 0)
            {
                return View();
                //theLoai.Trangthai = false;
            }
            else
            {
                db.TheLoais.DeleteOnSubmit(theLoai);
            }
            db.SubmitChanges();
            return RedirectToAction("ListTheLoai");
        }
        [HttpGet]
        public ActionResult SuaTheLoai(int id)
        {
            TheLoai theLoai = db.TheLoais.SingleOrDefault(a => a.ID == id);
            if (theLoai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(theLoai);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTheLoai(TheLoai theLoai, int id)
        {
            TheLoai tl = db.TheLoais.SingleOrDefault(a => a.ID == id);
            tl.TheLoai1 = theLoai.TheLoai1;
            db.SubmitChanges();
            return RedirectToAction("ListTheLoai");
        }
        public ActionResult IndexAdmin()
        {
            return View();
        }
        // code xử lí phần tài khoản người dùng
        public ActionResult ListUser()
        {
            return View(db.NguoiDungs.ToList());
        }
        public ActionResult ListAdmin()
        {
            return View(/*db.Admins.ToList()*/);
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
        public ActionResult DeleteAdmin()
        {
            return View();
        }
        public ActionResult EditAdmin()
        {
            return View();
        }
        //RAP PHIM
        public ActionResult DsRap()
        {
            var list = db.RapPhims.ToList();
            return View(list);
        }
        //thêm mới rạp phim
        [HttpGet]
        public ActionResult ThemRap()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemRap(RapPhim rap)
        {
            db.RapPhims.InsertOnSubmit(rap);
            db.SubmitChanges();
            return RedirectToAction("DsRap","Admin");
        }
        // sửa rạp phim
        [HttpGet]
        public ActionResult SuaRap(int id)
        {
            RapPhim rapPhim = db.RapPhims.SingleOrDefault(a => a.ID == id);
            if (rapPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(rapPhim);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaRap(RapPhim rapPhim, int id)
        {
            RapPhim rap = db.RapPhims.SingleOrDefault(a => a.ID == id);
            rap.TenRap = rapPhim.TenRap;
            rap.DiaChi = rapPhim.DiaChi;
            db.SubmitChanges();
            return RedirectToAction("DsRap");
        }
        // xóa rạp phim
        [HttpGet]
        public ActionResult XoaRap(int id)
        {
            RapPhim rapPhim = db.RapPhims.SingleOrDefault(a => a.ID == id);
            if (rapPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(rapPhim);
        }
        [HttpPost, ActionName("XoaRap")]
        public ActionResult ConfirmXoaRap(int id)
        {
            RapPhim rapPhim = db.RapPhims.SingleOrDefault(a => a.ID == id);
            if (rapPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Phong> phongs = db.Phongs.Where(a => a.ID_Rap == id).ToList();
            if (phongs.Count > 0)
            {
                return View();
                //theLoai.Trangthai = false;
            }
            else
            {
                db.RapPhims.DeleteOnSubmit(rapPhim);
            }
            db.SubmitChanges();
            return RedirectToAction("DsRap");
        }
        // code xử lí phòng
        public ActionResult DsPhim()
        {
            var list = db.Phongs.ToList();
            return View(list);
        }
        //thêm mới rạp phim
        [HttpGet]
        public ActionResult ThemPhong()
        {
            ViewBag.Rap = new SelectList(db.RapPhims.ToList(), "ID", "TenRap");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemPhong(Phong phong)
        {
            ViewBag.Rap = new SelectList(db.RapPhims.ToList(), "ID", "TenRap");
            if (ModelState.IsValid)
            {
                db.Phongs.InsertOnSubmit(phong);
                db.SubmitChanges();
            }
            return RedirectToAction("DsPhong", "Admin");
        }
        // sửa rạp phim
        [HttpGet]
        public ActionResult SuaPhong(int id)
        {
            Phong phong = db.Phongs.SingleOrDefault(a => a.ID == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phong);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaPhong(Phong phong, int id)
        {
            Phong p = db.Phongs.SingleOrDefault(a => a.ID == id);
            p.TenPhong = phong.TenPhong;
            p.ID_Rap = phong.ID_Rap;
            db.SubmitChanges();
            return RedirectToAction("DsPhong");
        }
        // xóa rạp phim
        [HttpGet]
        public ActionResult XoaPhong(int id)
        {
            Phong phong = db.Phongs.SingleOrDefault(a => a.ID == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phong);
        }
        [HttpPost, ActionName("XoaPhong")]
        public ActionResult ConfirmXoaPhong(int id)
        {
            Phong phong = db.Phongs.SingleOrDefault(a => a.ID == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GheNgoi> phongs = db.GheNgois.Where(a => a.ID_Phong == id).ToList();
            if (phongs.Count > 0)
            {
                return View();
                //theLoai.Trangthai = false;
            }
            else
            {
                db.Phongs.DeleteOnSubmit(phong);
            }
            db.SubmitChanges();
            return RedirectToAction("DsPhong");
        }
    }
}