using qms.DAL;
using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLDeviceScroll
    {
        public List<VMDeviceScroll> GetAll()
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMDeviceScroll ObjectMapping(DataRow row)
        {

            VMDeviceScroll displayFooter = new VMDeviceScroll();
            displayFooter.device_scroll_id = Convert.ToInt32(row["DEVICE_SCROLL_ID"] == DBNull.Value ? 0 : row["DEVICE_SCROLL_ID"]);
            displayFooter.scroll_id = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
            //displayFooter.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            displayFooter.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            displayFooter.address = (row["ADDRESS"] == DBNull.Value ? "" : row["ADDRESS"].ToString());
            displayFooter.location = (row["LOCATION"] == DBNull.Value ? "" : row["LOCATION"].ToString());
            displayFooter.device_no = (row["DEVICE_NO"] == DBNull.Value ? "" : row["DEVICE_No"].ToString());
            displayFooter.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
            displayFooter.device_id = Convert.ToInt32(row["DEVICE_ID"] == DBNull.Value ? 0 : row["DEVICE_ID"]);
            displayFooter.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
            displayFooter.device_name = (row["DEVICE_NAME"] == DBNull.Value ? "" : row["DEVICE_NAME"].ToString());
            if (row.Table.Columns.Contains("is_active")) displayFooter.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            displayFooter.is_publish = Convert.ToInt32(row["IS_PUBLISH"] == DBNull.Value ? 0 : row["IS_PUBLISH"]);
            return displayFooter;
        }

        internal List<VMDeviceScroll> ObjectMappingList(DataTable dt)
        {
            List<VMDeviceScroll> list = new List<VMDeviceScroll>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public VMDeviceScroll GetById(int id)
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMDeviceScroll displayFooter)
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            int scroll_id = dal.Insert(displayFooter);
            displayFooter.scroll_id = scroll_id;
        }
        public void Edit(VMDeviceScroll displayFooter)
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            dal.Update(displayFooter);

        }
        public void Remove(int id)
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            dal.Delete(id);

        }

        public List<VMDeviceScroll> GetAllbyDevice(int id)
        {
            DALDeviceScroll dal = new DALDeviceScroll();
            DataTable dt = dal.GetAllbyDevice(id);
            return ObjectMappingListVM(dt);
        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}


        internal List<VMDeviceScroll> ObjectMappingListVM(DataTable dt)
        {
            List<VMDeviceScroll> list = new List<VMDeviceScroll>();
            foreach (DataRow row in dt.Rows)
            {
                VMDeviceScroll deviceScroll = new VMDeviceScroll();
                deviceScroll.device_id = Convert.ToInt32(row["device_id"] == DBNull.Value ? 0 : row["device_id"]);
                deviceScroll.device_scroll_id = Convert.ToInt32(row["DEVICE_SCROLL_ID"] == DBNull.Value ? 0 : row["DEVICE_SCROLL_ID"]);
                deviceScroll.scroll_id = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
                deviceScroll.status = Convert.ToInt32(row["STATUS"] == DBNull.Value ? 0 : row["STATUS"]);

                list.Add(deviceScroll);

            }
            return list;
        }

    }
}