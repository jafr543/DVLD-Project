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
    public partial class frmManage_LDL_Applications : Form
    {
        public frmManage_LDL_Applications()
        {
            InitializeComponent();
        }

        enum enFilter
        {
            None,
            LDLAppID,
            DrivingClass,
            NationalNo,
            FullName,
            Status,
        }

        private string _SelectedColumn = string.Empty;
        private DataTable dtLDL_Applications;

        private void LoadLDL_ApplicationsData()
        {
            dtLDL_Applications = clsLDL_Application.Getall_LDL_Applications();
            dgvLDLApplication.DataSource = dtLDL_Applications;

            if (dgvLDLApplication.Columns.Count > 0)
            {
                if (dgvLDLApplication.Columns.Contains("NationalNo"))
                    dgvLDLApplication.Columns["NationalNo"].FillWeight = 60;

                if (dgvLDLApplication.Columns.Contains("Status"))
                    dgvLDLApplication.Columns["Status"].FillWeight = 60;

                if (dgvLDLApplication.Columns.Contains("PassedTests"))
                    dgvLDLApplication.Columns["PassedTests"].FillWeight = 80;

                if (dgvLDLApplication.Columns.Contains("FullName"))
                    dgvLDLApplication.Columns["FullName"].FillWeight = 180;
            }

            laRecords.Text = dgvLDLApplication.RowCount.ToString();
        }

        private void InitializeFilter()
        {
            enFilter filter = (enFilter)cBFilters.SelectedIndex;

            switch(filter)
            {
                case enFilter.None:
                    dtLDL_Applications.DefaultView.RowFilter = "";
                    txtFilter.Visible = false;
                    cBDrivingClasses.Visible = false;
                    _SelectedColumn = string.Empty;
                    break;

                case enFilter.LDLAppID:
                    cBDrivingClasses.Visible = false;
                    txtFilter.Visible = true;                   
                    _SelectedColumn = "L.D.L.AppID";
                    break;

                case enFilter.DrivingClass:
                    txtFilter.Visible = false;
                    cBDrivingClasses.Visible = true;
                    cBDrivingClasses.SelectedIndex = 0;
                    break;

                case enFilter.NationalNo:
                    cBDrivingClasses.Visible = false;
                    txtFilter.Visible = true;
                    _SelectedColumn = "NationalNo";
                    break;

                case enFilter.FullName:
                    cBDrivingClasses.Visible = false;
                    txtFilter.Visible = true;
                    _SelectedColumn = "FullName";
                    break;

                case enFilter.Status:
                    cBDrivingClasses.Visible = false;
                    txtFilter.Visible = true;
                    _SelectedColumn = "Status";
                    break;
            }
        }

        private void ApplyFilter()
        {
            if(_SelectedColumn == "None")
            {
                dtLDL_Applications.DefaultView.RowFilter = "";
                return;
            }

            string Filter = txtFilter.Text.Trim();
         
            if(_SelectedColumn == "L.D.L.AppID")
            {
                if(int.TryParse(Filter, out int ID))
                {
                    dtLDL_Applications.DefaultView.RowFilter = $"L.D.L.AppID = {ID}";
                }

                return;
            }
            else
            {
                dtLDL_Applications.DefaultView.RowFilter = $"{_SelectedColumn} Like '{Filter}%'";
            }

            laRecords.Text = dgvLDLApplication.Rows.Count.ToString();
        }

        private void ApplyDrivingClassesFilter()
        {
            enLicensesClassesID filter = (enLicensesClassesID)cBDrivingClasses.SelectedIndex + 1;

            switch(filter)
            {
                case enLicensesClassesID.Class1_SmallMotorcycle:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 1 - Small Motorcycle'";
                    break;

                case enLicensesClassesID.Class2_HeavyMotorcycleLicense:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 2 - Heavy Motorcycle License'";
                    break;

                case enLicensesClassesID.Class3_Ordinarydrivinglicense:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 3 - Ordinary driving license'";
                    break;

                case enLicensesClassesID.Class4_Commercial:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 4 - Commercial'";
                    break;

                case enLicensesClassesID.Class5_Agricultural:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 5 - Agricultural'";
                    break;

                case enLicensesClassesID.Class6_Smallandmediumbus:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 6 - Small and medium bus'";
                    break;

                case enLicensesClassesID.Class7_Truckandheavyvehicle:
                    dtLDL_Applications.DefaultView.RowFilter = "DrivingClass LIKE 'Class 7 - Truck and heavy vehicle'";
                    break;
            }

            laRecords.Text = dgvLDLApplication.Rows.Count.ToString();
        }

        private void AllowedTests(int TestTypeID)
        {
            switch(TestTypeID)
            {
                case 0: //Vision Test
                    scheduleVisionTestToolStripMenuItem.Enabled = true;
                    scheduleWerttinTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;

                case 1: //Written Test
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWerttinTestToolStripMenuItem.Enabled = true;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;

                case 2: //Street Test
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWerttinTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = true;
                    break;

                case 3: //Issue Driving License First Time
                    sechduleTestsToolStripMenuItem.Enabled = false;
                    issueDrivingLicenseFisrtTimeToolStripMenuItem.Enabled = true;
                    break;
            }
        }

        private void AllowedOptionsWithNewStatus(int LocalAppID)
        {
            showDetailsToolStripMenuItem.Enabled = true;

            if(clsTestAppointments.HasReservtionAppointment(LocalAppID))
            {
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
            }
            else
            {
                editApplicationToolStripMenuItem.Enabled = true;
                deleteApplicationToolStripMenuItem.Enabled = true;
            }

            cancelToolStripMenuItem.Enabled = true;
            sechduleTestsToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFisrtTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHiToolStripMenuItem.Enabled = true;

        }

        private void AllowedOptionsWithCancelledStatus()
        {
            showDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = false;
            deleteApplicationToolStripMenuItem.Enabled = false;
            cancelToolStripMenuItem.Enabled = false;
            sechduleTestsToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFisrtTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHiToolStripMenuItem.Enabled = true;
        }

        private void AllowedOptionsWithCompletedStatus()
        {
            showDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = false;
            deleteApplicationToolStripMenuItem.Enabled = false;
            cancelToolStripMenuItem.Enabled = false;
            sechduleTestsToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFisrtTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = true;
            showPersonLicenseHiToolStripMenuItem.Enabled = true;
        }

        private void InitializeStatusOptions(string Status, int LocalAppID)
        {
            switch(Status)
            {
                case "New":
                    AllowedOptionsWithNewStatus(LocalAppID);
                    break;

                case "Cancelled":
                    AllowedOptionsWithCancelledStatus();
                    break;

                case "Completed":
                    AllowedOptionsWithCompletedStatus();
                    break;
            }
        }

        private void ShowTest(int LocalAppID, int PassedTests)
        {
            frmTestAppointments Test = new frmTestAppointments(LocalAppID, PassedTests);

            Test.ShowDialog();
            if (Test.NeedToRefreshData)
                LoadLDL_ApplicationsData();
        }

        #region Events
        private void frmManage_L_Load(object sender, EventArgs e)
        {
            LoadLDL_ApplicationsData();
            cBFilters.SelectedIndex = 0;
        }

        private void cBFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeFilter();
            txtFilter.Clear();
        }

        private void cBDrivingClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyDrivingClassesFilter();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_SelectedColumn == "L.D.L.AppID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void pBAddApplication_Click(object sender, EventArgs e)
        {
            frmEditAdd_L_D_L_Application N_L_D_L_Application = new frmEditAdd_L_D_L_Application();

            N_L_D_L_Application.ShowDialog();
            
            if(N_L_D_L_Application.IsSaved)
            {
                LoadLDL_ApplicationsData();
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApplicationInfo AppInfo = new frmApplicationInfo((int)dgvLDLApplication.CurrentRow.Cells[0].Value);

            AppInfo.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditAdd_L_D_L_Application editAdd_L_D_L_Application = new frmEditAdd_L_D_L_Application((int)dgvLDLApplication.CurrentRow.Cells[0].Value,
                (string)dgvLDLApplication.CurrentRow.Cells["DrivingClass"].Value);

            editAdd_L_D_L_Application.ShowDialog();

            if (editAdd_L_D_L_Application.IsSaved)
                LoadLDL_ApplicationsData();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure To Delete This Application?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            if (!clsLDL_Application.Delete((int)dgvLDLApplication.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("Faild To Delete This Application\n Because it`s Connected to Other Data", "Deleted Faild",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    return;
            }
                  
            LoadLDL_ApplicationsData();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are Sure to Cancel This Application", "Cancel", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (!clsApplications.Cancel((int)dgvLDLApplication.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Faild to Cancel This Application", "Cancel Faild", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
                else
                    LoadLDL_ApplicationsData();
            }
        }

        private void issueDrivingLicenseFisrtTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssue_DL_ForFirstTime newDL = new frmIssue_DL_ForFirstTime((int)dgvLDLApplication.CurrentRow.Cells[0].Value);

            newDL.ShowDialog();

            if (newDL.IsIssue)
                LoadLDL_ApplicationsData();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = clsLicenses.FindLicenseIDByLocalAppID((int)dgvLDLApplication.CurrentRow.Cells[0].Value);

            if (LicenseID > 0)
            {
                frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(LicenseID);
                LicenseInfo.ShowDialog();
            }
            else
                MessageBox.Show("Falid to Find License", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void showPersonLicenseHiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicensesHistory DriverLicenses = new frmLicensesHistory(clsPeople.FindByLocalAppID((int)dgvLDLApplication.CurrentRow.Cells[0].Value));
            
            DriverLicenses.ShowDialog();
        }

        private void dgvLDLApplication_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvLDLApplication.ClearSelection();

                dgvLDLApplication.Rows[e.RowIndex].Selected = true;

                dgvLDLApplication.CurrentCell = dgvLDLApplication.Rows[e.RowIndex].Cells[0];
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvLDLApplication.RowCount, dgvLDLApplication.CurrentRow))
                e.Cancel = true;
            else
            {
                InitializeStatusOptions(dgvLDLApplication.CurrentRow.Cells["Status"].Value.ToString(),
                                                      (int)dgvLDLApplication.CurrentRow.Cells[0].Value);
                
                 if(sechduleTestsToolStripMenuItem.Enabled == true)
                 {
                     AllowedTests((int)dgvLDLApplication.CurrentRow.Cells["PassedTests"].Value);
                 }
            }

            
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTest((int)dgvLDLApplication.CurrentRow.Cells[0].Value,
            (int)dgvLDLApplication.CurrentRow.Cells["PassedTests"].Value);
        }

        private void scheduleWerttinTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTest((int)dgvLDLApplication.CurrentRow.Cells[0].Value,
            (int)dgvLDLApplication.CurrentRow.Cells["PassedTests"].Value);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTest((int)dgvLDLApplication.CurrentRow.Cells[0].Value,
            (int)dgvLDLApplication.CurrentRow.Cells["PassedTests"].Value);
        }

        #endregion
    }
}
