using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM
        public List<Categories> getList(string status = "All")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories
                        .Where(m => m.Status != 0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Categories.ToList();
                        break;
                    }
            }
            return list;
        }
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Categories getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi) với kiểu string = slug
        public Categories getRow(string slug)
        {

            return db.Categories
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        public List<Categories> getListByPareantId(int parentid = 0)
        {
            return db.Categories
              .Where(m => m.ParentId == parentid && m.Status == 1)
              .OrderBy(m => m.Order)
              .ToList();
        }
    }
}