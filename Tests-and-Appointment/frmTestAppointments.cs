using DVLD.Properties;
using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmTestAppointments : Form
    {
        enum enTestTypes { VisionTest = 1, WrittenTest, StreetTest }
        int LocalAppID = 0;
        int _PassedTests = 0;

        public bool NeedToRefreshData = false;

        public frmTestAppointments(int ID, int PassedTests)
        {
            InitializeComponent();
            LocalAppID = ID;
            _PassedTests = PassedTests + 1;
        }

        private void LoadTestAppointmentsData()
        {
            dgvTestAppointments.DataSource = clsTestAppointments.GetTestAppointments(LocalAppID, _PassedTests);
            laRecords.Text = dgvTestAppointments.RowCount.ToString();
        }

        private void InitializeVisionTestInfo()
        {
            pBTestImage.Image = Resources.eye_test;
            laTestTitle.Text = "Vision Test Appointments";
            this.Text = "Vision Test Appointments";
        }

        private void InitializeWrittenTestInfo()
        {
            pBTestImage.Image = Resources.Details;
            laTestTitle.Text = "Written Test Appointments";
            this.Text = "Written Test Appointments";
        }

        private void InitializeStreetTestInfo()
        {
            pBTestImage.Image = Resources.Car;
            laTestTitle.Text = "Street Test Appointments";
            this.Text = "Street Test Appointments";
        }

        private void InitializeTestInfo()
        {
            enTestTypes TestType = (enTestTypes)_PassedTests;

            switch (TestType)
            {
                case enTestTypes.VisionTest:
                    InitializeVisionTestInfo();
                    break;

                case enTestTypes.WrittenTest:
                    InitializeWrittenTestInfo();
                    break;

                case enTestTypes.StreetTest:
                    InitializeStreetTestInfo();
                    break;

            }

            LoadTestAppointmentsData();
        }

        private bool HasACompleteAppOrActiveAppointment()
        {
            if(clsTestAppointments.HasActiveAppointment(LocalAppID, _PassedTests))
            {
                MessageBox.Show("This Person Already Have an Active Appointment For This Test", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            if(clsTests.HasCompletedTheTest(LocalAppID, _PassedTests))
            {
                MessageBox.Show("You Cant Add Appointment For This Person\n Because he Have Completed This Test.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                pBAddAppointment.Enabled = false;
                return true;
            }

            return false;
        }

        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            InitializeTestInfo();
            uC_FullAppInfo1.LoadInfo(LocalAppID);
        }

        private void pBAddAppointment_Click(object sender, EventArgs e)
        {
            if (HasACompleteAppOrActiveAppointment())
                return;

            frmEditAddAppointment newAppointment = new frmEditAddAppointment(LocalAppID, _PassedTests);

            newAppointment.ShowDialog();

            if (newAppointment.IsSaved)
                LoadTestAppointmentsData();
        }

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((bool)dgvTestAppointments.CurrentRow.Cells["IsLocked"].Value ==  true)
            {
                MessageBox.Show("You Cant Edit This Appointment Becuse it`s Locked", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmEditAddAppointment EditAppointment = new frmEditAddAppointment((int)dgvTestAppointments.CurrentRow.Cells[0].Value,
                LocalAppID, _PassedTests);

            EditAppointment.ShowDialog();

            if (EditAppointment.IsSaved)
                LoadTestAppointmentsData();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((bool)dgvTestAppointments.CurrentRow.Cells["IsLocked"].Value == true)
            {
                MessageBox.Show("You Cant Edit This Appointment Becuse it`s Locked", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmTakeTest takeTest = new frmTakeTest((int)dgvTestAppointments.CurrentRow.Cells[0].Value,
                LocalAppID, _PassedTests);

            takeTest.ShowDialog();

            if (takeTest.IsSaved)
            {
                LoadTestAppointmentsData();
                NeedToRefreshData = true;
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(dgvTestAppointments.RowCount <= 0)
                e.Cancel = true;
        }
    }
}
