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
    public class LinkDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Links> getList()
        {
            return db.Links.ToList();
        }
        //Hiển thị danh sách toàn bộ Loại sản phẩm: SELCT * FROM
        public List<Links> getList(string status = "All")
        {
            List<Links> list = null;
            return list;
        }
        //Hiển thị danh sách 1 mẩu tin (bản ghi)
        public Links getRow(int tableid, string typelink)
        {
            return db.Links
                .Where(m => m.TableId == tableid && m.Type == typelink)
                .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //Hiển thị danh sách 1 mẩu tin (bản ghi) với kiểu string = slug
        public Links getRow(string slug)
        {

            return db.Links
                .Where(m => m.Slug == slug)
                .FirstOrDefault();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Thêm mới một mẩu tin
        public int Insert(Links row)
        {
            db.Links.Add(row);
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Cập nhật một mẩu tin
        public int Update(Links row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /////////////////////////////////////////////////////////////////////////////////////
        ///Xoá một mẩu tin ra khỏi CSDL
        public int Delete(Links row)
        {
            db.Links.Remove(row);
            return db.SaveChanges();
        }
    }
}