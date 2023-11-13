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

    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();

        //index
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }
        //index dua vao status=1,2, con status=0 -> thùng rác
        public List<Suppliers> getList(string status = "ALL")
        {
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Suppliers
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Suppliers
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Suppliers.ToList();
                        break;

                    }
            }

            return list;
        }


        //details
        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);

            }

        }

        //create
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);

            return db.SaveChanges();
        }


        //edit
        public int Update(Suppliers row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
    }


}