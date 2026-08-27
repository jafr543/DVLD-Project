using DVLD_BLL;
using DVLD_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmDrivers : Form
    {
        public frmDrivers()
        {
            InitializeComponent();
        }

        enum enFilters
        {
             None
            ,DriverID
            ,PersonID
            ,NationalNo
            ,FullName
        }

        string SelectedColumn = string.Empty;  

        DataTable dtDrivers = clsDrivers.GetAllDrivers();

        private void LoadDrivers()
        {
            dgvDrivers.DataSource = dtDrivers;

            if(dgvDrivers.ColumnCount > 0)
            {

                if (dgvDrivers.Columns.Contains("Full Name"))
                    dgvDrivers.Columns["Full Name"].FillWeight = 200;
            }
            laRecords.Text = dgvDrivers.RowCount.ToString();
        }

        private void InitializeFilter()
        {
            enFilters Filter = (enFilters)cBFilters.SelectedIndex;

            switch(Filter)
            {
                    case enFilters.None:
                    dtDrivers.DefaultView.RowFilter = " ";
                    txtFilter.Visible = false;
                    SelectedColumn = string.Empty;
                    break;

                    case enFilters.DriverID:
                    dtDrivers.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "DriverID";
                    break;

                    case enFilters.PersonID:
                    dtDrivers.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "PersonID";
                    break;

                    case enFilters.NationalNo:
                    dtDrivers.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "NationalNo";
                    break;

                    case enFilters.FullName:
                    dtDrivers.DefaultView.RowFilter = " ";
                    txtFilter.Visible = true;
                    SelectedColumn = "FullName";
                    break;
            }
        }

        private void ApplyFilter()
        {
            string Filtertext = txtFilter.Text.Trim();

            if (SelectedColumn == "DriverID")
            {
                if(int.TryParse(Filtertext , out int ID))
                {
                    dtDrivers.DefaultView.RowFilter = $"DriverID = {ID}";
                }
                return;
            }

            if (SelectedColumn == "PersonID")
            {
                if (int.TryParse(Filtertext, out int ID))
                {
                    dtDrivers.DefaultView.RowFilter = $"PersonID = {ID}";
                }
                return;
            }

            dtDrivers.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{Filtertext}%'";
        }

        private void frmDrivers_Load(object sender, EventArgs e)
        {
            LoadDrivers();
            cBFilters.SelectedIndex = 0;
        }

        private void cBFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeFilter();
            txtFilter.Text = string.Empty;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if(txtFilter.Text == string.Empty)
            {
                dtDrivers.DefaultView.RowFilter = " ";
                return;
            }
            ApplyFilter();
            laRecords.Text = dgvDrivers.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(SelectedColumn == "DriverID" || SelectedColumn == "PersonID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails Person = new frmPersonDetails((int)dgvDrivers.CurrentRow.Cells[1].Value);

            Person.ShowDialog();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_I_L_Application newApplication = new frm_I_L_Application();

            newApplication.ShowDialog();
        }

        private void showLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicensesHistory LicensesHistory = new frmLicensesHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);

            LicensesHistory.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvDrivers.RowCount, dgvDrivers.CurrentRow))
                e.Cancel = true;
        }
    }
}
