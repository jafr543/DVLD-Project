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
    public partial class UC_DriverLicenses : UserControl
    {
        bool _IsLocalLicensesLoaded = false;
        bool _IsInternationalLicensesLoaded = false;

        private int _PersonID = 0;

        public UC_DriverLicenses()
        {
            InitializeComponent();
        }

       public void LoadLicensesInfo(int PersonID)
        {
            _PersonID = PersonID;
            tabControl1.SelectedTab = tbLocal;

            LoadLocalLicenses();
            _IsLocalLicensesLoaded = true;
        }

       private void LoadLocalLicenses()
       {
            if(_PersonID > 0)
            {
                dgvLocalLicenses.DataSource = clsLicenses.GetPersonLicenses(_PersonID);

                if (dgvLocalLicenses.RowCount != 0)
                {
                    laLocalRecords.Text = dgvLocalLicenses.RowCount.ToString();
                    laLocalRecords.ForeColor = Color.Black;
                }
                else
                {
                    laLocalRecords.Text = "No Records";
                    laLocalRecords.ForeColor = Color.Red;
                }
            }
       }

       private void LoadInternationalLicenses()
       {
            if(_PersonID > 0)
            {
                dgvInternationalLicenses.DataSource = clsInterNationalLicenses.GetPersonInterationalLicenses(_PersonID);

                if (dgvInternationalLicenses.RowCount != 0)
                {
                    laInternationalRecords.Text = dgvInternationalLicenses.RowCount.ToString();
                    laInternationalRecords.ForeColor = Color.Black;
                }
                else
                {
                    laInternationalRecords.Text = "No Records";
                    laInternationalRecords.ForeColor = Color.Red;
                }

            }
       }

       private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage == tbLocal && !_IsLocalLicensesLoaded)
            {
                LoadLocalLicenses();
                _IsLocalLicensesLoaded = true;
            }

            if(e.TabPage == tbInternational && !_IsInternationalLicensesLoaded)
            {
                LoadInternationalLicenses();
                _IsInternationalLicensesLoaded= true;
            }
        }

       private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
       {
           
           if(tabControl1.SelectedTab == tbLocal)
           {
               frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);

               LicenseInfo.ShowDialog();
           }
           else
           {
               frmShow_Int_LicenseInfo Int_LicenseInfo = new frmShow_Int_LicenseInfo((int)dgvInternationalLicenses.
                   CurrentRow.Cells[0].Value);

                Int_LicenseInfo.ShowDialog();
           }
           
       }
    }
}
