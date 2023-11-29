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
using System.IO;

namespace ThuchanhPTUDW.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        PostDAO postsDAO = new PostDAO();
        LinkDAO linksDAO = new LinkDAO();
        //Page khôgn có Chủ đề - Topic
        /////////////////////////////////////////////////////////////////////////////////////
        //Trả về dan hsasch các mẩu tin
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index", "Page")); //Hiển thị toàn bộ danh sách trang đơn
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Details/5: Hiển thị một mẩu tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy trang đơn");
                return RedirectToAction("Index");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy trang đơn");
                return RedirectToAction("Index");
            }
            return View(posts);
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Create: Thêm một mẩu tin
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Page/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xử lý cho mục Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //Chuyển đổi đưua vào trường Name để lạoi bỏ dấu, khoảng cách = dấu -
                //Xử lý cho phần Upload hình ảnh
                var img = Request.Files["img"]; //Lấy thông tin File
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiểm tra tập tin có hay không
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chỉnh sửa sau khi phát hiện điều chưa dùng của Edit: Thêm ID
                        //Tên file = Slug + ID + phần mở rộng của tập tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        string PathDir = "~/Pulbic/img/page/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Upload Hình
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc phần Upload hình ảnh

                //Xử lý cho mục PostType = Page (đối với Page)
                posts.PostType = "page";

                //Xử lý cho mục CreateAt
                posts.CreateAt = DateTime.Now;

                //Xử lý cho mục CreateBy
                posts.CreateBy = Convert.ToInt32(Session["UserId"]);

                //Xứ lý cho mục Topics
                if (postsDAO.Insert(posts) == 1)    //Khi thêm dữ liệu thành công
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //Cập nhật Link cho Page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm trang đơn thành công");
                return RedirectToAction("Index");
            }
            return View(posts);
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }

            //Khi nhất nút thay đổi Status cho một mẩu tin
            Posts posts = postsDAO.getRow(id);
            //Kiểm tra ID của posts có tồn tại?
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            //Thay đổi trạng thái Status từ 1 thành 2 và ngược lại
            posts.Status = (posts.Status == 1) ? 2 : 1;

            //Cập nhật giá trị cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;

            //Gọi hàm Update trong PostDAO
            postsDAO.Update(posts);

            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //Khi cập nhật xong thì chuyển về Index
            return RedirectToAction("Index", "Page");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Chỉnh sửa trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Chỉnh sửa trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            return View(posts);
        }
        // POST: Admin/Page/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xử lý cho mục Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //Chuyển đổi đưua vào trường Name để lạoi bỏ dấu, khoảng cách = dấu -
                //Xử lý cho phần Upload hình ảnh
                var img = Request.Files["img"]; //Lấy thông tin file
                string PathDir = "~/Public/img/page/";
                if (img.ContentLength != 0)
                {
                    //Update thì phải xoá ảnh cũ
                    if (posts.Image != null)
                    {
                        string DelPath = Path.Combine(Server.MapPath(PathDir), posts.Image);
                        System.IO.File.Delete(DelPath);
                    }
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //Kiểm tra tập tin có hay không?
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chỉnh sửa sau khi phát hiện điều chưa dùng của Edit: Thêm ID
                        //Tên file = Slug + ID + phần mở rộng của tập tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //Upload hình
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc phần Upload hình ảnh

                //Xử lý cho mục UpdateAt
                posts.UpdateAt = DateTime.Now;

                //Xử lý cho mục UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //Xử lý cho mục Links
                if (postsDAO.Update(posts) == 1)//Khi sửa dữ liệu thành công
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    //Thay đổi thông tin kiểu Page
                    links.Type = "page";
                    linksDAO.Insert(links);
                }
                //Thông báo thành công
                TempData["message"] = new XMessage("success", "Chỉnh sửa trang đơn thành công");
                return RedirectToAction("Index");
            }
            return View(posts);
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/DelTrash/5:Thay đổi trạng thái của mẩu tin = 0
        public ActionResult DelTrash(int? id)
        {
            //Khi nhấp nút thay đổi Status cho một mẩu tin
            Posts posts = postsDAO.getRow(id);
            //Thay đổi trạng thái Status từ 1,2 thành 0
            posts.Status = 0;
            //Cập nhật giá trị cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Gọi hàm Update trong PostDAO
            postsDAO.Update(posts);
            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa trang đơn thành công");
            //Chuyển hướng trang
            return RedirectToAction("Index", "Page");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Posts/Trash/5: Hiển thị các mẩu tin có giá trị là 0
        public ActionResult Trash(int? id)
        {
            return View(postsDAO.getList("Trash", "Page"));
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Undo/5: Chuyển trạng thái Status = 0 thành = 2
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }

            //Khi nhấp nút thay đổi Status cho một mẩu tin
            Posts posts = postsDAO.getRow(id);
            //Kiểm tra id của page có tồn tại
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            //Thay đổi trạng thái Status từ 1 thành 2 và ngược lại
            posts.Status = 2;
            //Cập nhật giá trị cho UpdateAt/By
            posts.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            posts.UpdateAt = DateTime.Now;
            //Gọi hàm Update trong postsDAO
            postsDAO.Update(posts);
            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi trang đơn thành công");
            //Chuyển hướng về trang Trash
            return RedirectToAction("Trash", "Page");
        }
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Page/Delete/5: Xoá một mẩu tin
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá trang đơn thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index", "Page");
            }
            return View(posts);
        }
        // POST: Admin/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);

            //Tìm thấy mẩu tin thì xoá, cập nhật Links
            if (postsDAO.Delete(posts) == 1)
            {
                Links links = linksDAO.getRow(posts.Id, "page");
                //Xoá luôn cho Links
                linksDAO.Delete(links);

                //Đường dẫn đén ảnh cần xoá
                string PathDir = "~/Public/img/page/";
                //Cập nhật thì phải xoá file cũ
                if (posts.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), posts.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa trang dơn thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}