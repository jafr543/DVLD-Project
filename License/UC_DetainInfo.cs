using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_DetainInfo : UserControl
    {
        public UC_DetainInfo()
        {
            InitializeComponent();
        }

        public decimal FineFees {  get; private set;}

        public decimal GetFineFees()
        {
            if (string.IsNullOrWhiteSpace(txtFineFees.Text))
                return 0;

            if ((decimal.TryParse(txtFineFees.Text, out decimal Fees)))
            {
                FineFees = Fees;
                return Fees;
            }
                

            return 0;    
        }

        public void InitializeDetainInfo()
        {
            laDetainDate.Text = DateTime.Now.ToShortDateString();
            laCreatedByUserName.Text = frmLogin.CurrentUser.UserName;
            plApplicationInfo.Visible = false;
            plTotalFees.Visible = false;
        }

        public void InitializeReleaseInfo()
        {
            laDetainDate.Text = DateTime.Now.ToShortDateString();
            laCreatedByUserName.Text = frmLogin.CurrentUser.UserName;
            laApplicationFees.Text = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.ReleaseDetainedDrivingLicsense).ToString();
            laCreatedByUserName.Text = frmLogin.CurrentUser.UserName;
            plApplicationInfo.Visible = true;
            plTotalFees.Visible = true;
        }

        public void SetDetainID(int DetainID)
        {
            if(DetainID != 0)
            laDetainID.Text = DetainID.ToString();
            else
                laDetainID.Text = "???";
        }

        public void SetLicenseID(int LicenseID)
        {
            if (LicenseID != 0)
            {
                laLicenseID.Text = LicenseID.ToString();
                txtFineFees.Enabled = true;
            }
            else
            {
                laLicenseID.Text = "???";
                txtFineFees.Text = "0.00";
                txtFineFees.Enabled = false;
            }
            
        }

        public void SetApplicationID(int ApplicationID)
        {
            laApplicationID.Text = ApplicationID.ToString();
        }

        public void SetFineFees(decimal Fees)
        {
            if (Fees != 0)
            {
                txtFineFees.Text = Fees.ToString();
                FineFees = Fees;
                txtFineFees.Visible = false;
                laFineFees.Visible = true;
                laFineFees.Text = FineFees.ToString();

                    laTotalFees.Text = (FineFees + Convert.ToDecimal(laApplicationFees.Text)).ToString();
            }
            else
            {
                txtFineFees.Visible = false;
                laFineFees.Visible = true;
                laFineFees.Text = "???";
                if (plTotalFees.Visible == true)
                    laTotalFees.Text = "???";
            }


        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
