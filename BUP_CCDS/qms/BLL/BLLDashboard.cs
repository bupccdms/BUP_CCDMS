using qms.DAL;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLDashboard
    {
        public List<VMDashboardAdmin> GetAdminDashboard()
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetAdminDashboard();
            return ObjectMappingListAdmin(dt);
        }
        public List<VMDashboardAdmin> GetAll()
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }
        internal List<VMDashboardAdmin> ObjectMappingList(DataTable dt)
        {
            List<VMDashboardAdmin> list = new List<VMDashboardAdmin>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardAdmin dashboard = new VMDashboardAdmin();
                {
                    dashboard.department_playlist_id = Convert.ToInt32(row["DEPARTMENT_playlist_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_playlist_ID"]);
                    dashboard.playlist_id = Convert.ToInt32(row["playlist_ID"] == DBNull.Value ? 0 : row["playlist_ID"]);
                    dashboard.playlist_name = (row["playlist_name"] == DBNull.Value ? "" : row["playlist_name"].ToString());
                    dashboard.department_id = Convert.ToInt32(row["DEPARTMENT_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_ID"]);
                    dashboard.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
                    dashboard.device_name = (row["DEVICE_NAME"] == DBNull.Value ? "" : row["DEVICE_NAME"].ToString());
                    dashboard.location = (row["LOCATION"] == DBNull.Value ? "" : row["LOCATION"].ToString());
                    dashboard.device_no = (row["DEVICE_NO"] == DBNull.Value ? "" : row["DEVICE_NO"].ToString());
                    dashboard.file_name = (row["FILE_NAME"] == DBNull.Value ? "" : row["FILE_NAME"].ToString());
                    dashboard.file_type = (row["FILE_TYPE"] == DBNull.Value ? "" : row["FILE_TYPE"].ToString());
                    dashboard.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
                }

                list.Add(dashboard);

            }
            return list;
        }
        public List<VMDashboardDepartmentAdminDevices> GetDepartmentAdminDashboard(int department_id, List<VMDashboardDepartmentAdminServicesTokens> servicesTokens, List<VMDashboardDepartmentAdminServicesWaitings> servicesWaitings)
        {
            DALDashboard dal = new DALDashboard();
            DataSet ds = dal.GetDepartmentAdminDashboard(department_id);
            ObjectMappingListServicesTokens(ds.Tables["ServicesTokens"], servicesTokens);
            ObjectMappingListServicesWaitings(ds.Tables["ServicesWaitings"], servicesWaitings);
            return ObjectMappingListDevices(ds.Tables["DEVICES"]);
        }

        public List<VMDashboardDepartmentService> GetDepartmentServiceList(int department_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetDepartmentServiceList(department_id);

            List<VMDashboardDepartmentService> list = new List<VMDashboardDepartmentService>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDepartmentService dashboard = new VMDashboardDepartmentService()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardDeviceStatus> GetDeviceStatusList(int department_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetDeviceStatusList(department_id);

            List<VMDashboardDeviceStatus> list = new List<VMDashboardDeviceStatus>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDeviceStatus dashboard = new VMDashboardDeviceStatus()
                {
                    department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString()),
                    device_name = (row["device_name"] == DBNull.Value ? null : row["device_name"].ToString()),
                    playlist_name = (row["playlist_name"] == DBNull.Value ? null : row["playlist_name"].ToString()),
                    scroll = (row["scroll"] == DBNull.Value ? null : row["scroll"].ToString())
                    
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserServiceDetail> GetDepartmentServiceDetailList(int department_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetDepartmentServiceDetailList(department_id);

            List<VMDashboardUserServiceDetail> list = new List<VMDashboardUserServiceDetail>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserServiceDetail dashboard = new VMDashboardUserServiceDetail()
                {
                    token_prefix = (row["TOKEN_PREFIX"] == DBNull.Value ? null : row["TOKEN_PREFIX"].ToString()),
                    token_no = (row["TOKEN_NO"] == DBNull.Value ? 0 : Convert.ToInt32(row["TOKEN_NO"])),
                    customer_type = (row["CUSTOMER_TYPE"] == DBNull.Value ? null : row["CUSTOMER_TYPE"].ToString()),
                    device = (row["DEVICE"] == DBNull.Value ? null : row["DEVICE"].ToString()),
                    department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? null : row["DEPARTMENT_NAME"].ToString()),
                    service = (row["SERVICE"] == DBNull.Value ? null : row["SERVICE"].ToString()),
                    issue_time = Convert.ToDateTime(row["ISSUE_TIME"] == DBNull.Value ? null : row["ISSUE_TIME"].ToString()),
                    start_time = Convert.ToDateTime(row["START_TIME"] == DBNull.Value ? null : row["START_TIME"].ToString()),
                    end_time = Convert.ToDateTime(row["END_TIME"] == DBNull.Value ? null : row["END_TIME"].ToString()),
                    service_status = (row["SERVICE_STATUS"] == DBNull.Value ? null : row["SERVICE_STATUS"].ToString()),
                };

                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardDeviceService> GetDeviceServiceList(int device_id, out string serving_time)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetDeviceServiceList(device_id, out serving_time);

            List<VMDashboardDeviceService> list = new List<VMDashboardDeviceService>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDeviceService dashboard = new VMDashboardDeviceService()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardDeviceToken> GetDeviceTokenList(int device_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetDeviceTokenList(device_id);

            List<VMDashboardDeviceToken> list = new List<VMDashboardDeviceToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDeviceToken dashboard = new VMDashboardDeviceToken()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"])),
                    serving = (row["serving"] == DBNull.Value ? 0 : Convert.ToInt32(row["serving"])),
                    missing = (row["missing"] == DBNull.Value ? 0 : Convert.ToInt32(row["missing"])),
                    waiting = (row["waiting"] == DBNull.Value ? 0 : Convert.ToInt32(row["waiting"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserService> GetUserServiceList(string user_id, out string serving_time)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetUserServiceList(user_id, out serving_time);

            List<VMDashboardUserService> list = new List<VMDashboardUserService>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserService dashboard = new VMDashboardUserService()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserToken> GetUserTokenList(string user_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetUserTokenList(user_id);

            List<VMDashboardUserToken> list = new List<VMDashboardUserToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserToken dashboard = new VMDashboardUserToken()
                {
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    served = (row["served"] == DBNull.Value ? 0 : Convert.ToInt32(row["served"])),
                    serving = (row["serving"] == DBNull.Value ? 0 : Convert.ToInt32(row["serving"])),
                    missing = (row["missing"] == DBNull.Value ? 0 : Convert.ToInt32(row["missing"])),
                    waiting = (row["waiting"] == DBNull.Value ? 0 : Convert.ToInt32(row["waiting"]))
                };
                list.Add(dashboard);

            }

            return list;
        }

        public List<VMDashboardUserServiceDetail> GetUserServiceDetailList(string user_id)
        {
            DALDashboard dal = new DALDashboard();
            DataTable dt = dal.GetUserServiceDetailList(user_id);

            List<VMDashboardUserServiceDetail> list = new List<VMDashboardUserServiceDetail>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardUserServiceDetail dashboard = new VMDashboardUserServiceDetail()
                {
                    token_prefix = (row["TOKEN_PREFIX"] == DBNull.Value ? null : row["TOKEN_PREFIX"].ToString()),
                    token_no = (row["TOKEN_NO"] == DBNull.Value ? 0 : Convert.ToInt32(row["TOKEN_NO"])),
                    customer_type = (row["CUSTOMER_TYPE"] == DBNull.Value ? null : row["CUSTOMER_TYPE"].ToString()),
                    device = (row["DEVICE"] == DBNull.Value ? null : row["DEVICE"].ToString()),
                    department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? null : row["DEPARTMENT_NAME"].ToString()),
                    service = (row["SERVICE"] == DBNull.Value ? null : row["SERVICE"].ToString()),
                    issue_time = Convert.ToDateTime(row["ISSUE_TIME"] == DBNull.Value ? null : row["ISSUE_TIME"].ToString()),
                    start_time = Convert.ToDateTime(row["START_TIME"] == DBNull.Value ? null : row["START_TIME"].ToString()),
                    end_time = Convert.ToDateTime(row["END_TIME"] == DBNull.Value ? null : row["END_TIME"].ToString()),
                    service_status = (row["SERVICE_STATUS"] == DBNull.Value ? null : row["SERVICE_STATUS"].ToString())
                };
                list.Add(dashboard);

            }

            return list;
        }


        internal List<VMDashboardAdmin> ObjectMappingListAdmin(DataTable dt)
        {
            List<VMDashboardAdmin> list = new List<VMDashboardAdmin>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardAdmin dashboard = new VMDashboardAdmin()
                {
                    department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"])),
                    services = (row["services"] == DBNull.Value ? 0 : Convert.ToInt32(row["services"]))
                };

                list.Add(dashboard);

            }
            return list;
        }

        internal List<VMDashboardDepartmentAdminDevices> ObjectMappingListDevices(DataTable dt)
        {
            List<VMDashboardDepartmentAdminDevices> list = new List<VMDashboardDepartmentAdminDevices>();
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDepartmentAdminDevices dashboard = new VMDashboardDepartmentAdminDevices()
                {
                    device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                list.Add(dashboard);

            }
            return list;
        }

        internal void ObjectMappingListServicesTokens(DataTable dt, List<VMDashboardDepartmentAdminServicesTokens> servicesTokens)
        {
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDepartmentAdminServicesTokens dashboard = new VMDashboardDepartmentAdminServicesTokens()
                {
                    service_id = (row["service_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["service_id"])),
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                servicesTokens.Add(dashboard);

            }
        }

        internal void ObjectMappingListServicesWaitings(DataTable dt, List<VMDashboardDepartmentAdminServicesWaitings> servicesWaitings)
        {
            foreach (DataRow row in dt.Rows)
            {
                VMDashboardDepartmentAdminServicesWaitings dashboard = new VMDashboardDepartmentAdminServicesWaitings()
                {
                    service_id = (row["service_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["service_id"])),
                    service_name = (row["service_name"] == DBNull.Value ? null : row["service_name"].ToString()),
                    tokens = (row["tokens"] == DBNull.Value ? 0 : Convert.ToInt32(row["tokens"]))
                };

                servicesWaitings.Add(dashboard);

            }
        }
    }
}