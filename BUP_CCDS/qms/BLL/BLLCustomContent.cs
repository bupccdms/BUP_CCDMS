using qms.DAL;
//using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLCustomContent
    {
        public List<VMCustomContent> GetAll()
        {
            DALCustomContent dal = new DALCustomContent();
            DataTable dt = dal.GetAll();
            return ObjectMappingListForIndex(dt);
        }

        internal VMCustomContent ObjectMapping(DataRow row)
        {

            VMCustomContent customContent = new VMCustomContent();
            customContent.custom_content_id = Convert.ToInt32(row["CUSTOM_CONTENT_ID"] == DBNull.Value ? 0 : row["CUSTOM_CONTENT_ID"]);
            customContent.is_url = Convert.ToInt32(row["IS_URL"] == DBNull.Value ? 0 : row["IS_URL"]);
            if (customContent.is_url == 1)
            {
                customContent.url = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            }
            else
            {
                customContent.content = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            }
           
            return customContent;
        }
        internal VMCustomContent ObjectMappingForIndex(DataRow row)
        {

            VMCustomContent customContent = new VMCustomContent();
            customContent.custom_content_id = Convert.ToInt32(row["CUSTOM_CONTENT_ID"] == DBNull.Value ? 0 : row["CUSTOM_CONTENT_ID"]);
            
            customContent.is_url = Convert.ToInt32(row["IS_URL"] == DBNull.Value ? 0 : row["IS_URL"]);
            customContent.content = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            //if (customContent.is_url == 1)
            //{
            //    customContent.url = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            //}
            //else
            //{
            //    customContent.content = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            //}

            return customContent;
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

        internal VMCustomContent ObjectMappingVM(DataRow row)
        {

            VMCustomContent customContent = new VMCustomContent();
            int customContentId = Convert.ToInt32(row["CUSTOM_CONTENT_ID"] == DBNull.Value ? 0 : row["CUSTOM_CONTENT_ID"]);
            customContent.is_url = Convert.ToInt32(row["IS_URL"] == DBNull.Value ? 0 : row["IS_URL"]);
            customContent.content = (row["CONTENT"] == DBNull.Value ? "" : row["CONTENT"].ToString());
            customContent.url = (row["URL"] == DBNull.Value ? "" : row["URL"].ToString());
            return customContent;
        }

        internal List<VMCustomContent> ObjectMappingList(DataTable dt)
        {
            List<VMCustomContent> list = new List<VMCustomContent>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }

        internal List<VMCustomContent> ObjectMappingListForIndex(DataTable dt)
        {
            List<VMCustomContent> list = new List<VMCustomContent>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMappingForIndex(row));

            }
            return list;
        }
        public VMCustomContent GetById(int id)
        {
            DALCustomContent dal = new DALCustomContent();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        //public VMScroll GetByDepartmentId(int department_id)
        //{
        //    DALCustomContent dal = new DALCustomContent();
        //    DataTable dt = dal.GetByDepartmentId(department_id);
        //    if (dt.Rows.Count > 0)
        //        return ObjectMappingVM(dt.Rows[0]);
        //    else return null;
        //}
        public void Create(VMCustomContent customContent)
        {
            DALCustomContent dal = new DALCustomContent();
            int custom_content_id = dal.Insert(customContent);
            customContent.custom_content_id = custom_content_id;
            //AddEditScrollBn(customContent);
        }
        public void Edit(VMCustomContent customContent)
        {
            DALCustomContent dal = new DALCustomContent();
            dal.Update(customContent);
            //AddEditScrollBn(customContent);
        }

        //internal void AddEditScrollBn(tblCustomContent customContent)
        //{
        //    string filePath = HttpContext.Current.Server.MapPath((HttpContext.Current.Request.ApplicationPath == @"/" ? "" : HttpContext.Current.Request.ApplicationPath) + @"/ScrollsBN/" + customContent.scroll_id.ToString() + ".txt");
        //    if (File.Exists(filePath))
        //    {
        //        File.WriteAllText(filePath, customContent.content_bn);
        //    }
        //    else
        //    {
        //        StreamWriter sw = File.CreateText(filePath);
        //        sw.Write(customContent.content_bn);
        //        sw.Close();
        //    }
        //}
        public void Remove(int id)
        {
            DALCustomContent dal = new DALCustomContent();
            dal.Delete(id);

        }

        internal void deleteScrollBn(int custom_content_id)
        {
            string filePath = HttpContext.Current.Server.MapPath((HttpContext.Current.Request.ApplicationPath == @"/" ? "" : HttpContext.Current.Request.ApplicationPath) + @"/ScrollsBN/" + custom_content_id.ToString() + ".txt");
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