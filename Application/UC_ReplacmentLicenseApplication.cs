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
    public partial class UC_ReplacmentLicenseApplication : UserControl
    {
        public UC_ReplacmentLicenseApplication()
        {
            InitializeComponent();
        }

        public decimal ApplicationFees { get; private set; }


        public void SetApplicationFees(decimal Fees)
        {
            ApplicationFees = Fees;
            laApplicationFees.Text = Fees.ToString();
        }

        public void SetApplicationID_ReplacedLicenseID(int ApplicationID, int ReplacedLicenseID)
        {
            laL_R_ApplicationID.Text = ApplicationID.ToString();
            laReplacedLicenseID.Text = ReplacedLicenseID.ToString();
        }

        public void SetOldLicenseID(int LicenseID)
        {
            laOldLicenseID.Text = LicenseID.ToString();
        }

        public void LoadBasicInfo()
        { 
            laApplicationDate.Text = DateTime.Now.ToShortDateString();
            laCreatedByUserID.Text = frmLogin.CurrentUser.UserName;

        }
    }
}
