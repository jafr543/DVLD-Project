using DVLD_BLL;
using DVLD_Utilities;
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
    public partial class frmManage_I_L_Applications : Form
    {
        public frmManage_I_L_Applications()
        {
            InitializeComponent();
        }
        enum enFilters
        {
            None
           , I_LicenseID
           , ApplicationID
           , DriverID
           , L_LicenseID
        }

        string SelectedColumn = string.Empty;
        DataTable dtInterNationalLicenses = clsInterNationalLicenses.GetAllI_Licenses();

        private void LoadApplicationsData()
        {
            dgvI_L_Applications.DataSource = dtInterNationalLicenses;

            laRecords.Text = dgvI_L_Applications.RowCount.ToString();
        }

        private void InitializeFilter()
        {
            enFilters Filter = (enFilters)cBFilters.SelectedIndex;

            switch (Filter)
            {
                case enFilters.None:
                    dtInterNationalLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = false;
                    SelectedColumn = string.Empty;
                    break;

                case enFilters.I_LicenseID:
                    dtInterNationalLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "Int.License ID";
                    break;

                case enFilters.ApplicationID:
                    dtInterNationalLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "ApplicationID";
                    break;

                case enFilters.DriverID:
                    dtInterNationalLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "DriverID";
                    break;

                case enFilters.L_LicenseID:
                    dtInterNationalLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "L.License ID";
                    break;
            }
        }

        private void ApplyFilter()
        {
            string Filtertext = txtFilter.Text.Trim();

            int.TryParse(Filtertext, out int ID);

            dtInterNationalLicenses.DefaultView.RowFilter = $"[{SelectedColumn}] = {ID}";

            laRecords.Text = dgvI_L_Applications.RowCount.ToString();
        }

        private void frmManage_I_L_Applications_Load(object sender, EventArgs e)
        {
            LoadApplicationsData();
            cBFilters.SelectedIndex = 0;
        }

        private void cBFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = string.Empty;
            InitializeFilter();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if(txtFilter.Text.Length == 0)
            {
                dtInterNationalLicenses.DefaultView.RowFilter = " ";
                laRecords.Text = dgvI_L_Applications.RowCount.ToString();
                return;
            }
            ApplyFilter();
        }

        private void pBAddNew_Click(object sender, EventArgs e)
        {
            frm_I_L_Application I_L_Application = new frm_I_L_Application();

            I_L_Application.ShowDialog();

            if (I_L_Application.IsSaved)
                LoadApplicationsData();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsPeople.FindByAppID((int)dgvI_L_Applications.CurrentRow.Cells[1].Value);


            if (PersonID != 0)
            {            
                frmPersonDetails PersonDetails = new frmPersonDetails(PersonID);
                PersonDetails.ShowDialog();
            }
            else
                MessageBox.Show("Person Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShow_Int_LicenseInfo Int_LicenseInfo = new frmShow_Int_LicenseInfo((int)dgvI_L_Applications
                                                            .CurrentRow.Cells[0].Value);

            Int_LicenseInfo.ShowDialog();
        }

        private void showLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsPeople.FindByAppID((int)dgvI_L_Applications.CurrentRow.Cells[1].Value);

            if(PersonID != 0)
            {
               frmLicensesHistory LicensesHistory = new frmLicensesHistory(PersonID);
                LicensesHistory.ShowDialog();
            }
            else
                MessageBox.Show("Person Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvI_L_Applications.RowCount, dgvI_L_Applications.CurrentRow))
                e.Cancel = true;
        }
    }
}
