using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using ThuchanhPTUDW.Library;

namespace ThuchanhPTUDW.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ProductDAO productsDAO = new ProductDAO();
        MenusDAO menusDAO = new MenusDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        PostDAO postsDAO = new PostDAO();
        /// /////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Index
        public ActionResult Index()
        {
            ViewBag.CatList = categoriesDAO.getList("Index");
            ViewBag.SupList = suppliersDAO.getList("Index");
            ViewBag.ProList = productsDAO.getList("Index");
            ViewBag.TopList = topicsDAO.getList("Index");
            List<Menus> menu = menusDAO.getList("Index");
            return View(menusDAO.getList("Index"));
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //Them Loai san pham
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCategory"]))
                {
                    var listitem = form["nameCategory"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Slug;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục loại sản phẩm");
                }
            }

            //Them Nha cung cap
            if (!string.IsNullOrEmpty(form["ThemSupplier"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameSupplier"]))
                {
                    var listitem = form["nameSupplier"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Suppliers suppliers = suppliersDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = suppliers.Name;
                        menu.Link = suppliers.Slug;
                        menu.TypeMenu = "supplier";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn nhà cung cấp");
                }
            }

            //Them San pham
            if (!string.IsNullOrEmpty(form["ThemProduct"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameProduct"]))
                {
                    var listitem = form["nameProduct"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Products products = productsDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = products.Name;
                        menu.Link = products.Slug;
                        menu.TypeMenu = "product";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn sản phẩm");
                }
            }

            //Them Custom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCustom"]) && !string.IsNullOrEmpty(form["linkCustom"]))
                {
                    //tao ra menu custom
                    Menus menu = new Menus();
                    menu.Name = form["nameCustom"];
                    menu.Link = form["linkCustom"];
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentID = 0;
                    menu.Order = 0;
                    menu.CreateAt = DateTime.Now;
                    menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.UpdateAt = DateTime.Now;
                    menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.Status = 2; //tam thoi chua xuat ban
                    //Them vao DB
                    menusDAO.Insert(menu);

                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa đầy đủ thông tin cho menu");
                }
            }

            //tra ve trang Index
            return RedirectToAction("Index", "Menu");
        }

        ////////////////////////////////////////////////////////////////
        //GET: Admin/Menu/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 1<->2
                menus.Status = (menus.Status == 1) ? 2 : 1;

                //cap nhat gia tri UpdateAt
                menus.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                menusDAO.Update(menus);

                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

                return RedirectToAction("Index");
            }
        }


        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }
            return View(menus);
        }

        // GET: Admin/Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TableID,TypeMenu,Position,Link,ParentID,Order,CreateAt,CreateBy,UpdateAt,UpdateBy,Status")] Menus menus)
        {
            if (ModelState.IsValid)
            {
                menusDAO.Insert(menus);
                return RedirectToAction("Index");
            }

            return View(menus);
        }

        // GET: Admin/Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");

            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }

            Menus menus = menusDAO.getRow(id);

            if (menus == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }
            return View(menus);
        }

        // POST: Admin/Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus)
        {
            if (ModelState.IsValid)
            {

                if (menus.ParentID == null)
                {
                    menus.ParentID = 0;
                }
                if (menus.Order == null)
                {
                    menus.Order = 1;
                }
                else
                {
                    menus.Order += 1;
                }

                //Xy ly cho muc UpdateAt
                menus.UpdateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                menus.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thành công");

                //Cap nhat du lieu
                menusDAO.Update(menus);

                return RedirectToAction("Index");
            }

            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            return View(menus);
        }
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy menu");
                return RedirectToAction("Index");
            }
            return View(menus);
        }

        // POST: Admin/Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);

            //tim thay mau tin thi xoa, cap nhat cho Links
            menusDAO.Delete(menus);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa menu thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/DelTrash/5:Thay doi trang thai cua mau tin = 0
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);

            //thay doi trang thai Status tu 1,2 thanh 0
            menus.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;

            //Goi ham Update trong MenusDAO
            menusDAO.Update(menus);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Menu");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menus/Trash/5:Hien thi cac mau tin có gia tri la 0
        public ActionResult Trash(int? id)
        {
            return View(menusDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Recover/5:Thay doi trang thai cua mau tin
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);
            //kiem tra id cua menus co ton tai?
            if (menus == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");

                //chuyen huong trang
                return RedirectToAction("Index");
            }
            //thay doi trang thai Status = 2
            menus.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            menus.UpdateAt = DateTime.Now;

            //Goi ham Update trong menusDAO
            menusDAO.Update(menus);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi menu thành công");

            //khi cap nhat xong thi chuyen ve Trash de phuc hoi tiep
            return RedirectToAction("Trash");
        }
    }
}
