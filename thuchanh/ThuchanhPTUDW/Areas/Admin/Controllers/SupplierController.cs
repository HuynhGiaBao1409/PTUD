using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using ThuchanhPTUDW.Library;

namespace ThuchanhPTUDW.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động 1 số trường
                //-----Create At
                suppliers.CreateAt = DateTime.Now;
                //-----Create By
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //-----Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //-----UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //-----UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //Thêm mới nhà cung cấp vào DB
                suppliersDAO.Insert(suppliers);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //-----Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //-----UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //-----UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //Truoc khi cap nhat lai anh moi thi xoa anh cu
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier/";
                if (img.ContentLength != 0 && suppliers.Image!=null)//ton tai mot anh tu truoc
                {
                    //xoa anh cu
                    string PathFile = Path.Combine(Server.MapPath(PathDir),suppliers.Image);
                    System.IO.File.Delete(PathFile);
                }
                //upload anh moi của nha cung cap

                 //xu ly cho phan upload hình ảnh

                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh                  
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //Cập nhật mẩu tin vào DB
                suppliersDAO.Update(suppliers);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Cập nhật nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Truy van mau tin theo Id
            Suppliers suppliers = suppliersDAO.getRow(id);

            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);

        }
        //////////////////////////////////////////////////////////////////////
        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Suppliers suppliers = suppliersDAO.getRow(id);

            if (suppliersDAO.Delete(suppliers) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/supplier/";
                //cap nhat thi phai xoa file cu
                if (suppliers.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            suppliers.Status = 0;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá nhà cung cấp thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            suppliers.Status = 2;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi nhà cung cấp thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }
    }
}