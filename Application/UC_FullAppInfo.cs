using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_FullAppInfo : UserControl
    {
        public UC_FullAppInfo()
        {
            InitializeComponent();
        }

        #region Properties
        public int LocalAppID { get; set; }
        public int PassedTests { get; set; }
        public string ClassName { get; set; }
        public int AppID { get; set; }
        public string Status { get; set; }
        public decimal Fees { get; set; }
        public string AppType { get; set; }
        public int PersonID { get; set; }
        public string Applicant { get; set; }
        public DateTime AppDate { get; set; }
        public DateTime LastStatusUpdate { get; set; }

        #endregion

        private void LoadPropertiesInfo()
        {
            LocalAppID = uC_D_L_AppInfo2.LocalAppID;
            ClassName = uC_D_L_AppInfo2.ClassName;
            PassedTests = uC_D_L_AppInfo2.PassedTests;

            AppID = uC_BasicAppInfo2.AppID;
            Status = uC_BasicAppInfo2.Status;
            Fees = uC_BasicAppInfo2.Fees;
            AppType = uC_BasicAppInfo2.AppType;
            PersonID = uC_BasicAppInfo2.PersonID;
            Applicant = uC_BasicAppInfo2.Applicant;
            AppDate = uC_BasicAppInfo2.AppDate;
            LastStatusUpdate = uC_BasicAppInfo2.LastStatusUpdate;    
        }

        public void LoadInfo(int ID)
        {
            uC_BasicAppInfo2.FindAppInfo(ID);
            uC_D_L_AppInfo2.FindAppInfo(ID);
            LoadPropertiesInfo();
        }
    }
}
