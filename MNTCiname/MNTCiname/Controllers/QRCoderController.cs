using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using QRCoder;

namespace MNTCiname.Controllers
{
    public class QRCoderController : Controller
    {
        // GET: QRCoder
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public byte[] CreateQRCode(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return BitmapToBytes(qrCodeImage);
        }
        /*code hiển thị mã QR @{
          if (Model != null)
          {
            <h3>QR Code Successfully Generated</h3>
            <img src="@String.Format("data:image/png;base64,{0}", Convert.ToBase64String(Model))" />
          }
        }*/
        public ActionResult Index()
        {
            return View();
        }
    }
}