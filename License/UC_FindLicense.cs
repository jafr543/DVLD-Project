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
    public partial class UC_FindLicense : UserControl
    {
        public UC_FindLicense()
        {
            InitializeComponent();
        }

        public event Action<clsLicenses> SearchCompleted;

        protected virtual void SendLicenseInfo(clsLicenses LicenseInfo)
        {
            Action<clsLicenses> handler = SearchCompleted;

            if (handler != null)
            {
                handler(LicenseInfo);
            }
        }

        private bool ValidateValues(int LicenseID)
        {
            if(LicenseID == 0)
            {
                errorProvider1.SetError(txtFilter, "Invalid Value!");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtFilter, "");
                return true;
            }
        }

        private void FindLicense()
        {
            int.TryParse(txtFilter.Text.Trim(), out int SearchID);

            if (!ValidateValues(SearchID))
            {
                MessageBox.Show("Please Enter a Valid Value", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (SearchCompleted != null)
                    SendLicenseInfo(null);
                return;
            }

            clsLicenses LicenseInfo = clsLicenses.Find(SearchID);

            if (LicenseInfo != null)
            {
                if (SearchCompleted != null)
                    SendLicenseInfo(LicenseInfo);
            }

            else
            {
                MessageBox.Show("License with ID: " + SearchID + " Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (SearchCompleted != null)
                    SendLicenseInfo(null);
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void pBFindLicense_Click(object sender, EventArgs e)
        {
            FindLicense();
        }

        public void Pre_Search(int LicenseID)
        {
            clsLicenses LicenseInfo = clsLicenses.Find(LicenseID);

            if (LicenseInfo != null)
            {
                txtFilter.Text = LicenseID.ToString();
                if (SearchCompleted != null)
                    SendLicenseInfo(LicenseInfo);
            }

            else
            {

                if (SearchCompleted != null)
                    SendLicenseInfo(null);
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                FindLicense();
            }
        }
    }
}
