using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductDAO
    {
        private MyDBContext db = new MyDBContext();

        //index
        public List<Products> getList()
        {
            return db.Products.ToList();
        }
        //index dua vao status=1,2, con status=0 -> thùng rác
        public List<Products> getList(string status = "ALL")
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products.ToList();
                        break;

                    }
            }

            return list;
        }

        public Products getRow(string slug)
        {
            return db.Products.Where(m=>m.Slug == slug && m.Status==1).FirstOrDefault();
        }
        //details
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);

            }

        }

        //create
        public int Insert(Products row)
        {
            db.Products.Add(row);

            return db.SaveChanges();
        }


        //edit
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}
