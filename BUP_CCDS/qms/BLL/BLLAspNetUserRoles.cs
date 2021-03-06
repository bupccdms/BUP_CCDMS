﻿using qms.DAL;
using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLAspNetUserRoles
    {
        public List<AspNetRole> GetAllUser()
        {
            DALAspNetUserRoles dal = new DALAspNetUserRoles();
            DataTable dt = dal.GetAllUser();
            return ObjectMappingList(dt);
        }


        public List<AspNetRole> GetRolesByUserId(string userId)
        {
            DALAspNetUserRoles dal = new DALAspNetUserRoles();
            DataTable dt = dal.GetRolesByUserId(userId);
            return ObjectMappingList(dt);
        }


        internal List<AspNetRole> ObjectMappingList(DataTable dt)
        {
            List<AspNetRole> list = new List<AspNetRole>();
            foreach (DataRow row in dt.Rows)
            {
                AspNetRole user = new AspNetRole();
                user.Id = (row["Id"] == DBNull.Value ? null : row["Id"].ToString());
                user.Name = (row["Name"] == DBNull.Value ? null : row["Name"].ToString());
                


                list.Add(user);

            }
            return list;
        }
    }
}