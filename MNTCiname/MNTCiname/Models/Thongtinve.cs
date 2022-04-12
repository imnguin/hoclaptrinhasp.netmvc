using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNTCiname.Models
{
    public class Thongtinve
    {
        public int idphim { get; set; }
        public int idphong { get; set; }
        public int idkehoach { get; set; }
        public string tenRap { get; set; }
        public string tenPhim { get; set; }
        public string phong { get; set; }
        public TimeSpan gioChieu { get; set; }
        public DateTime ngay { get; set; }
        public Thongtinve(int idphim, int idphong, int idkehoach,string tenrap, string tenphim, string phong, TimeSpan giochieu, DateTime ngay)
        {
            this.idphim = idphim;
            this.idphong = idphong;
            this.idkehoach = idkehoach;
            this.tenRap = tenrap;
            this.tenPhim = tenphim;
            this.phong = phong;
            this.gioChieu = giochieu;
            this.ngay = ngay;
        }
    }
}