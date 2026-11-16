using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using template.Models;

namespace template.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private PhongCachXanhDBEntities1 db = new PhongCachXanhDBEntities1();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var staffList = db.Users.Where(u => u.Role == "Admin" || u.Role == "Staff" || u.Role == "Nhân viên" || u.Role == "Super Admin").ToList();

            // Đếm số lượng Super Admin để truyền qua ViewBag kiểm soát nút Xóa
            int superAdminCount = db.Users.Count(u => u.Role == "Super Admin");
            ViewBag.CanDeleteSuperAdmin = (superAdminCount > 1);

            ViewBag.ShowDetails = staffList.Count >= 5;

            return View(staffList);
        }

        public ActionResult Details1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Include(u => u.Orders).FirstOrDefault(u => u.UserID == id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                // Tự động gán thời gian hiện tại cho CreatedAt
                user.CreatedAt = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Email,Password,Role,CreatedAt")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra an toàn: nếu là Super Admin duy nhất thì không cho phép xóa
            if (user.Role == "Super Admin" && db.Users.Count(u => u.Role == "Super Admin") <= 1)
            {
                TempData["Error"] = "Không thể xóa Super Admin duy nhất trong hệ thống!";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                if (user.Role == "Super Admin" && db.Users.Count(u => u.Role == "Super Admin") <= 1)
                {
                    return RedirectToAction("Index");
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Admin/Users/Index1 (Quản lý khách hàng)
        public ActionResult Index1()
        {
            var customers = db.Users.Where(u => u.Role == "Customer" || u.Role == "Khách hàng" || u.Role == null).ToList();

            return View(customers);
        }
    }
}