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
    public partial class frmManageDetainedLicenses : Form
    {
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        DataTable dtDetainedLicenses;
        string SelectedColumn = string.Empty;
        int _PersonID = 0;

        enum enFilters
        {
            None,
            DetainID,
            IsReleased,
            NationalNo,
            FullName,
            ReleaseApplicationID,
        }

        enum enIsReleased
        {
            Yes,
            No    
        }

        private void InitializeFilter()
        {
            enFilters Filter = (enFilters)cBFilters.SelectedIndex;

            switch (Filter)
            {
                case enFilters.None:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = false;
                    cBIsReleased.Visible = false;
                    SelectedColumn = string.Empty;
                    break;

                case enFilters.DetainID:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    cBIsReleased.Visible = false;
                    SelectedColumn = "DetainID";
                    break;

                case enFilters.IsReleased:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = false;
                    cBIsReleased.SelectedIndex = 0;
                    cBIsReleased.Visible = true;
                    SelectedColumn = "IsReleased";
                    break;

                case enFilters.NationalNo:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    cBIsReleased.Visible = false;
                    SelectedColumn = "NationalNo";
                    break;

                case enFilters.FullName:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    cBIsReleased.Visible = false;
                    SelectedColumn = "FullName";
                    break;

                case enFilters.ReleaseApplicationID:
                    dtDetainedLicenses.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    cBIsReleased.Visible = false;
                    SelectedColumn = "ReleaseApplicationID";
                    break;
            }
        }

        private void ApplyFilter()
        {
            string Filtertext = txtFilter.Text.Trim();

            if (SelectedColumn == "DetainID")
            {
                if (int.TryParse(Filtertext, out int ID))
                {
                    dtDetainedLicenses.DefaultView.RowFilter = $"DetainID = {ID}";
                }
                return;
            }

            if (SelectedColumn == "ReleaseApplicationID")
            {
                if (int.TryParse(Filtertext, out int ID))
                {
                    dtDetainedLicenses.DefaultView.RowFilter = $"[Release App.ID] = {ID}";
                }
                return;
            }

            if (SelectedColumn == "NationalNo")
            {
                dtDetainedLicenses.DefaultView.RowFilter = $"NationalNo LIKE '{Filtertext}%'";
            }

            if (SelectedColumn == "FullName")
            {
                dtDetainedLicenses.DefaultView.RowFilter = $"[Full Name] LIKE '{Filtertext}%'";
            }

        }

        private void IsReleaseFilter()
        {
            if ((enIsReleased)cBIsReleased.SelectedIndex == enIsReleased.Yes)
                dtDetainedLicenses.DefaultView.RowFilter = "IsReleased = 1";
            else
                dtDetainedLicenses.DefaultView.RowFilter = "IsReleased = 0";
        }

        private void LoadDetainedLicenses()
        {
            dtDetainedLicenses = clsDetainedLicenses.GetDetainedLicenses();
            dgvDetainedLicenses.DataSource = dtDetainedLicenses;

            if (dgvDetainedLicenses.Columns.Contains("Full Name"))
                dgvDetainedLicenses.Columns["Full Name"].FillWeight = 180;

            laRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void ManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            LoadDetainedLicenses();
            cBFilters.SelectedIndex = 0;
        }

        private void cBFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeFilter();
            txtFilter.Text = string.Empty;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cBIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsReleaseFilter();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (SelectedColumn == "DetainID" || SelectedColumn == "ReleaseApplicationID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void pBDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses DetainLicense = new frmDetain_ReleaseLicenses();

            DetainLicense.DetainMode = enDetainMode.Detain;
            DetainLicense.ShowDialog();

            if (DetainLicense.IsSaved)
                LoadDetainedLicenses();
        }

        private void pBReleaseLicense_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses ReleaseLicense = new frmDetain_ReleaseLicenses();

            ReleaseLicense.DetainMode = enDetainMode.Release;
            ReleaseLicense.ShowDialog();

            if (ReleaseLicense.IsSaved)
                LoadDetainedLicenses();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails Person = new frmPersonDetails(_PersonID);

            Person.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            LicenseInfo.ShowDialog();
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicensesHistory LicenseHistory = new frmLicensesHistory(_PersonID);

            LicenseHistory.ShowDialog();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses ReleaseLicenses = new frmDetain_ReleaseLicenses((int)dgvDetainedLicenses.
                CurrentRow.Cells[1].Value);

            ReleaseLicenses.DetainMode = enDetainMode.Release;
            ReleaseLicenses.ShowDialog();

            if (ReleaseLicenses.IsSaved)
                LoadDetainedLicenses();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvDetainedLicenses.RowCount, dgvDetainedLicenses.CurrentRow))
            {
                e.Cancel = true;
                return;
            }
            
            _PersonID = clsPeople.FindByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            releaseLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicenses.CurrentRow.Cells["IsReleased"].Value;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
