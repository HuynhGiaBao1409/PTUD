using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;

namespace MyClass.DAO
{

    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();

        //index
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }
        //index dua vao status=1,2, con status=0 -> thùng rác
        public List<Categories> getList(string status = "ALL")
        {
            List<Categories> list = null;
            switch (status) {
                case "Index":
                    {
                        list= db.Categories
                            .Where(m=>m.Status !=0)
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


        //details
        public  Categories getRow(int? id)
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

        //create
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            
            return db.SaveChanges();
        }


        //edit
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public  int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }

        
}