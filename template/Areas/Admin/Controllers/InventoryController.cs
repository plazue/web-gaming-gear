using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using template.Models;
using System.Data.Entity; // Bắt buộc phải có để sử dụng được phương thức .Include() cho Entity Framework

namespace template.Areas.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Admin/Inventory

        public ActionResult Index()
        {
           
            var products = db.Products.Include(p => p.Category).ToList();

            return View(products);
        }
        [HttpPost]
        public ActionResult UpdateStock(int id, int quantity)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.StockQuantity = (product.StockQuantity ?? 0) + quantity;
                if (product.StockQuantity < 0) product.StockQuantity = 0;
                db.SaveChanges();
                return Json(new { success = true, newStock = product.StockQuantity });
            }
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }
    }
}