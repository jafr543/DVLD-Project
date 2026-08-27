using DVLD.Properties;
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
    public partial class frmTakeTest : Form
    {
        int _LocalAppID = 0;
        int _TestType = 0;
        int _TestAppointmentID = 0;
        int _PersonID = 0;

        public bool IsSaved = false;

        clsTestAppointments _TestInfo;

        public frmTakeTest(int testAppointmentID, int localAppID, int testType)
        {
            InitializeComponent();
            _TestAppointmentID = testAppointmentID;
            _LocalAppID = localAppID;
            _TestType = testType;
        }

        private void InitializeTestInfo()
        {
            string Title = "Scheduled Test";
            switch (_TestType)
            {
                case 1:
                    uC_TestInfo1.SetImageAndTitle(Resources.eye_test, Title);
                    break;

                case 2:
                    uC_TestInfo1.SetImageAndTitle(Resources.Details, Title);
                    break;

                case 3:
                    uC_TestInfo1.SetImageAndTitle(Resources.Car, Title);
                    break;

            }
        }

        private void LoadTestInfo()
        {
            clsBasicLoaclApplicationsInfo_View AppInfo = clsBasicLoaclApplicationsInfo_View.FindClassAndName(_LocalAppID);
             _TestInfo = clsTestAppointments.Find(_TestAppointmentID);

            if (AppInfo != null)
            {
                _PersonID = AppInfo.PersonID;
                uC_TestInfo1.SetMode(1);
                uC_TestInfo1.localAppID = _LocalAppID;
                uC_TestInfo1.D_Class = AppInfo.ClassName;
                uC_TestInfo1.FullName = AppInfo.Applicant;
                uC_TestInfo1.AppointmentDate = _TestInfo.AppointmentDate;
                uC_TestInfo1.Fees = clsTestTypes.GetTestTypeFees(_TestType);
                uC_TestInfo1.Trial = clsTestAppointments.TrialsNumber(_LocalAppID, _TestType);
                InitializeTestInfo();
            }

        }

        private bool SaveNewTest(bool TestResult)
        {
            clsTests NewTest = new clsTests();

            NewTest.TestAppointmentID = _TestAppointmentID;
            NewTest.TestResult = TestResult;

            if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                NewTest.Notes = txtNotes.Text.Trim();
            else
                NewTest.Notes = null;

            NewTest.CreatedByUserID = frmLogin.CurrentUser.UserID;

            return NewTest.Save();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            LoadTestInfo();
            uC_TestInfo1.InitialzeInfo();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change\n the Pass/Fail results after you save.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
          
            if(rBPass.Checked)
            {
                if(SaveNewTest(true))
                {
                    MessageBox.Show("Test Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }
                else
                    MessageBox.Show("Test Not Saved!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (SaveNewTest(false))
                {
                    MessageBox.Show("Test Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Test Not Saved!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsSaved = true;

            clsTestAppointments.CloseTheAppointment(_TestAppointmentID);

            if (uC_TestInfo1.Trial > 0)
                clsApplications.Complete(_TestInfo.RetakeTestAppID);
            
            this.Close();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
