using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using ThuchanhPTUDW.Library;

namespace ThuchanhPTUDW.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        LinkDAO linksDAO = new LinkDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                
            }
            return View(categories);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                categories.CreateAt = DateTime.Now;
                //-----Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //-----ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //-----Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //-----UpdateAt
                categories.UpdateAt = DateTime.Now;
                //-----UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success","Tạo mới sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }


        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                categories.CreateAt = DateTime.Now;
                //-----Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //-----ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //-----Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //-----UpdateAt
                categories.UpdateAt = DateTime.Now;
                //-----UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (categoriesDAO.Update(categories) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(categories.Id, "category");
                    //cap nhat lai thong tin
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                }
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
           
            
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Trash");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            if (categoriesDAO.Delete(categories) == 1)
            {
                Links links = linksDAO.getRow(categories.Id, "category");
                //Xoa luon cho Links
                linksDAO.Delete(links);
            }
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
           
            return RedirectToAction("Trash");
        }

        // GET: Admin/Category/Status/5

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái
            categories.Status = (categories.Status == 1) ? 2 : 1;
            ///cập nhật updateAt  
            categories.UpdateAt=DateTime.Now;
            ///cập nhật update By
            categories.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        // GET: Admin/Category/DelTrash/5

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái
            categories.Status = 0;
            ///cập nhật updateAt  
            categories.UpdateAt = DateTime.Now;
            ///cập nhật update By
            categories.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }
        // GET: Admin/Category/Undo/5

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái status = 2
            categories.Status = 2;
            ///cập nhật updateAt  
            categories.UpdateAt = DateTime.Now;
            ///cập nhật update By
            categories.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
            //tro ve trang index
            return RedirectToAction("Trash");

        }
    }
}