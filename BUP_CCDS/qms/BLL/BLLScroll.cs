using qms.DAL;
using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLScroll
    {
        public List<tblScroll> GetAll()
        {
            DALScroll dal = new DALScroll();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal tblScroll ObjectMapping(DataRow row)
        {

            tblScroll scroll = new tblScroll();
            scroll.scroll_id = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
            scroll.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            //scroll.content_bn = GetScrollBn(scroll.scroll_id);
            scroll.is_publish = Convert.ToInt32(row["IS_PUBLISH"] == DBNull.Value ? 0 : row["IS_PUBLISH"]);
            scroll.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            scroll.is_global = Convert.ToInt32(row["IS_GLOBAL"] == DBNull.Value ? 0 : row["IS_GLOBAL"]);
            if (row.Table.Columns.Contains("is_active")) scroll.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            
            return scroll;
        }

        public string GetScrollBn(int scroll_id)
        {
            string filePath = HttpContext.Current.Server.MapPath((HttpContext.Current.Request.ApplicationPath == @"/" ? "" : HttpContext.Current.Request.ApplicationPath) + @"/ScrollsBN/" + scroll_id.ToString() + ".txt");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "";
            }
        }

        internal VMScroll ObjectMappingVM(DataRow row)
        {

            VMScroll scroll = new VMScroll();
            int scrollId = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
            scroll.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            //scroll.content_bn = GetScrollBn(scrollId);
            scroll.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            if (row.Table.Columns.Contains("is_active")) scroll.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            return scroll;
        }

        internal List<tblScroll> ObjectMappingList(DataTable dt)
        {
            List<tblScroll> list = new List<tblScroll>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public tblScroll GetById(int id)
        {
            DALScroll dal = new DALScroll();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public VMScroll GetByDepartmentId(int department_id)
        {
            DALScroll dal = new DALScroll();
            DataTable dt = dal.GetByDepartmentId(department_id);
            if (dt.Rows.Count > 0)
                return ObjectMappingVM(dt.Rows[0]);
            else return null;
        }
        
        public VMScroll GetByUserId(string user_id)
        {
            DALScroll dal = new DALScroll();
            DataTable dt = dal.GetByUserId(user_id);
            if (dt.Rows.Count > 0)
                return ObjectMappingVM(dt.Rows[0]);
            else return null;
        }

        public void Create(tblScroll scroll)
        {
            DALScroll dal = new DALScroll();
            int scroll_id = dal.Insert(scroll);
            scroll.scroll_id = scroll_id;
            //AddEditScrollBn(scroll);
        }
        public void Edit(tblScroll scroll)
        {
            DALScroll dal = new DALScroll();
            dal.Update(scroll);
            //AddEditScrollBn(scroll);
        }
        public void Force(int department_id, tblScroll scroll)
        {
            DALScroll dal = new DALScroll();
            dal.Force(department_id, scroll);
        }
        public void SetActivation(int scroll_id, int is_active)
        {
            DALScroll dal = new DALScroll();
            dal.SetActivation(scroll_id, is_active);
        }
       
        internal void AddEditScrollBn(tblScroll scroll)
        {
            string filePath = HttpContext.Current.Server.MapPath((HttpContext.Current.Request.ApplicationPath == @"/" ? "" : HttpContext.Current.Request.ApplicationPath) + @"/ScrollsBN/" + scroll.scroll_id.ToString() + ".txt");
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, scroll.content_bn);
            }
            else
            {
                StreamWriter sw = File.CreateText(filePath);
                sw.Write(scroll.content_bn);
                sw.Close();
            }
        }
        public void Remove(int id)
        {
            DALScroll dal = new DALScroll();
            dal.Delete(id);

        }

        internal void deleteScrollBn(int scroll_id)
        {
            string filePath = HttpContext.Current.Server.MapPath((HttpContext.Current.Request.ApplicationPath == @"/" ? "" : HttpContext.Current.Request.ApplicationPath) + @"/ScrollsBN/" + scroll_id.ToString() + ".txt");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        
    }
}