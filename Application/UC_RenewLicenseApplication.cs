using DVLD_BLL;
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
    public partial class UC_RenewLicenseApplication : UserControl
    {
        public UC_RenewLicenseApplication()
        {
            InitializeComponent();
        }

        public decimal ApplicationFees { get; private set; }
        public string Notes 
        {
            get
            {
                return txtNotes.Text;
            }
        }

        public void LoadBasicInfo()
        {
            laIssueDate.Text = laApplicationDate.Text = DateTime.Now.ToShortDateString();

            ApplicationFees = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.RenewDrivingLicenseService);
            laApplicationFees.Text = ApplicationFees.ToString();
            laCreatedByUserID.Text = frmLogin.CurrentUser.UserName;
        }

        public void SetExpirationDate(DateTime ExpirationDate)
        {
            laExpirationDate.Text = ExpirationDate.ToShortDateString();
        }

        public void SetNewApp_and_LicenseIDs(int AppID, int NewLicenseID)
        {
            laR_L_ApplicationID.Text = AppID.ToString();
            laR_L_ID.Text = NewLicenseID.ToString();
        }

        public void LoadOldLicenseInfo(clsLicenses OldLicense)
        {
            laLicenseFees.Text = OldLicense.LicenseClassInfo.ClassFees.ToString();

            txtNotes.Text = OldLicense.Notes;

            laTotalFees.Text = (ApplicationFees + OldLicense.LicenseClassInfo.ClassFees).ToString();
        }
    }
}
