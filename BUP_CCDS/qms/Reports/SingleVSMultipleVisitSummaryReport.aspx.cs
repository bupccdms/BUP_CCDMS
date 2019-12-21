using qms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace qms.Reports
{
    public partial class SingleVSMultipleVisitSummaryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAllDepartment();

            }
        }

        private void GetAllDepartment()
        {
            try
            {
                List<tblDepartment> departmentList = new BLL.BLLDepartment().GetAllDepartment();
                if (departmentList.Count > 0)
                {
                    ddlDepartment.DataSource = departmentList.ToList();
                    ddlDepartment.DataTextField = "DEPARTMENT_NAME";
                    ddlDepartment.DataValueField = "DEPARTMENT_ID";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, new ListItem("All Department"));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}