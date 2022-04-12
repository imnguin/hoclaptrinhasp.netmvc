using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNTCiname.Models
{
    public class LichChieu
    {
        private string tenRap;
        private TimeSpan gioChieu;
        private int iDRap;
        private int iDPhong;
        private int iDphim;
        private int iDKeHoach;
        public string TenRap { get => tenRap; set => tenRap = value; }
        public TimeSpan GioChieu { get => gioChieu; set => gioChieu = value; }
        public int IDRap { get => iDRap; set => iDRap = value; }
        public int IDPhong { get => iDPhong; set => iDPhong = value; }
        public int IDphim { get => iDphim; set => iDphim = value; }
        public int IDKeHoach { get => iDKeHoach; set => iDKeHoach = value; }

        public LichChieu(string tenrap,TimeSpan giochieu, int idrap, int idphong, int idphim, int idkehoach) 
        { 
            this.TenRap = tenrap;
            this.GioChieu = giochieu;
            this.IDRap = idrap;
            this.IDPhong = idphong;
            this.IDphim = idphim;
            this.IDKeHoach = idkehoach;
        }
    }
}