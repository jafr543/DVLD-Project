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
    public partial class UC_InternationalApp : UserControl
    {
        public UC_InternationalApp()
        {
            InitializeComponent();
        }

        public decimal PaidFees { get; private set; }

        public DateTime IssueDate { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public void InitializeBasicInfo()
        {
            IssueDate = DateTime.Now;
            laApplicationDate.Text = laIssueDate.Text = IssueDate.ToShortDateString();

            ExpirationDate = IssueDate.AddYears(1);
            laExpirationDate.Text = ExpirationDate.ToShortDateString();

            laFees.Text = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.NewIntemationalLicense).ToString();
            PaidFees = Convert.ToDecimal(laFees.Text);
            laCreatedBy.Text = frmLogin.CurrentUser.UserName;
        }

        public void SetLocalLicenseID(int LocalLicenseID)
        {
            if (LocalLicenseID != 0)
                laL_LicenseID.Text = LocalLicenseID.ToString();
            else
                laL_LicenseID.Text = "???";
        }

        public void SetAppIDAndI_LicenseID(int AppID, int I_LicenseID)
        {
            laApplicationID.Text = AppID.ToString();
            laI_LicenseID.Text = I_LicenseID.ToString();
        }

    }
}
