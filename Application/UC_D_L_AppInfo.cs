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
    public partial class UC_D_L_AppInfo : UserControl
    {
        
        public UC_D_L_AppInfo()
        {
            InitializeComponent();
        }

        public int LocalAppID { get; set; }
        public string ClassName { get; set; }
        public int PassedTests { get; set; }

        public void FindAppInfo(int ID)
        {
            string ClassName = "";
            int PassedTests = 0;

            if (clsLDL_Application.Find(ID, ref ClassName, ref PassedTests))
            {
                LoadAppInfo(ID, ClassName, PassedTests);
            }
            else
            {
                MessageBox.Show("Application Not Found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Visible = false;
            }         
        }

        private void LoadAppInfo(int ID,string ClassName, int PassedTests)
        {
            laID.Text = ID.ToString();
            laAppClass.Text = ClassName;
            laTests.Text = PassedTests.ToString() + "/3";

            this.LocalAppID = ID;
            this.ClassName = ClassName;
            this.PassedTests = PassedTests;

            if (clsLicenses.IsExistLicenseByLocalAppID(LocalAppID))
            {
                llShowLicenseInfo.Visible = true;
            }
            else
                llShowLicenseInfo.Visible = false;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int LicenseID = clsLicenses.FindLicenseIDByLocalAppID(LocalAppID);

            if (LicenseID > 0)
            {
                frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(LicenseID);
                LicenseInfo.ShowDialog();
            }
            else
                MessageBox.Show("Falid to Find License", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
