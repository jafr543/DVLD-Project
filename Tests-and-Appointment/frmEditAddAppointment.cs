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
    public partial class frmEditAddAppointment : Form
    {
        int _LocalAppID = 0;
        int _TestType = -1;
        int _TestAppointmentID = 0;
        int _PersonID = 0;
        int RetakeAppID = 0;
        clsTestAppointments _TestInfo;

        public bool IsSaved = false;

        public frmEditAddAppointment(int LocalAppID, int TestType)
        {
            InitializeComponent();

            _LocalAppID = LocalAppID;
            _TestType = TestType;
        }

        public frmEditAddAppointment(int TestAppointmentID, int LocalAppID, int TestType)
        {
            InitializeComponent();

            _TestAppointmentID = TestAppointmentID;
            _LocalAppID = LocalAppID;
            _TestType = TestType;
        }

        private void IsRetakeApp(int Trials)
        {
            uC_TestInfo1.Trial = Trials;

            if (Trials > 0)
            {
                gBRetakeInfo.Enabled = true;
                int RFees = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.RetakeTest);
                laRFees.Text = RFees.ToString();
                laTotalFees.Text = (RFees + uC_TestInfo1.Fees).ToString();
            }
            else
                gBRetakeInfo.Enabled = false;
        }

        private void InitializeTestInfo()
        {

            if(uC_TestInfo1.Trial > 0 && _TestAppointmentID == 0)
            {
                string Title = "Schedule Retake Test";

                switch (_TestType)
                {
                    case 1:
                        this.Text = Title;
                        uC_TestInfo1.SetImageAndTitle(Resources.eye_test, Title);
                        break;

                    case 2:
                        this.Text = Title;
                        uC_TestInfo1.SetImageAndTitle(Resources.Details, Title);
                        break;

                    case 3:
                        this.Text = Title;
                        uC_TestInfo1.SetImageAndTitle(Resources.Car, Title);
                        break;

                }

                return;
            }

            if (_TestAppointmentID == 0)
            {
                switch (_TestType)
                {
                    case 1:
                        this.Text = "Schedule Vision Test";
                        uC_TestInfo1.SetImageAndTitle(Resources.eye_test, "Schedule Vision Test");
                        break;

                    case 2:
                        this.Text = "Schedule Written Test";
                        uC_TestInfo1.SetImageAndTitle(Resources.Details, "Schedule Written Test");
                        break;

                    case 3:
                        this.Text = "Schedule Street Test";
                        uC_TestInfo1.SetImageAndTitle(Resources.Car, "Schedule Street Test");
                        break;

                }


            }

            else
            {
                switch (_TestType)
                {
                    case 1:
                        this.Text = "Update Vision Test Appointment";
                        uC_TestInfo1.SetImageAndTitle(Resources.eye_test, "Update Vision Test Appointment");
                        break;

                    case 2:
                        this.Text = "Update Written Test Appointment";
                        uC_TestInfo1.SetImageAndTitle(Resources.Details, "Update Written Test Appointment");
                        break;

                    case 3:
                        this.Text = "Update Street Test Appointment";
                        uC_TestInfo1.SetImageAndTitle(Resources.Car, "Update Street Test Appointment");
                        break;
                }
            }
        }

        private void LoadAppInfo()
        {
            clsBasicLoaclApplicationsInfo_View AppInfo = clsBasicLoaclApplicationsInfo_View.FindClassAndName(_LocalAppID);

            if(_TestAppointmentID != 0)
                _TestInfo = clsTestAppointments.Find(_TestAppointmentID);

            if (AppInfo != null)
            {
                 _PersonID = AppInfo.PersonID;
                 uC_TestInfo1.localAppID = _LocalAppID;
                 uC_TestInfo1.D_Class = AppInfo.ClassName;
                 uC_TestInfo1.FullName = AppInfo.Applicant;
                 uC_TestInfo1.Fees = clsTestTypes.GetTestTypeFees(_TestType);
                  IsRetakeApp(clsTestAppointments.TrialsNumber(_LocalAppID, _TestType));
                 InitializeTestInfo();
            }
            else
            {
                MessageBox.Show("Application Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btSave.Enabled = false;
            }

        }

        private bool SaveNewAppointment()
        {
            clsTestAppointments NewAppointment = new clsTestAppointments();

            NewAppointment.TestTypeID = _TestType;
            NewAppointment.LocalAppID = _LocalAppID;
            NewAppointment.AppointmentDate = uC_TestInfo1.AppointmentDate.Date;

            if (gBRetakeInfo.Enabled == true)
                NewAppointment.PaidFees = Convert.ToDecimal(laTotalFees.Text);
            else
                NewAppointment.PaidFees = (decimal)uC_TestInfo1.Fees;

            NewAppointment.CreatedByUserID = frmLogin.CurrentUser.UserID;
            NewAppointment.IsLocked = false;

            if (RetakeAppID != 0)
                NewAppointment.RetakeTestAppID = RetakeAppID;
            else
                NewAppointment.RetakeTestAppID = 0;

            return NewAppointment.Save();
        }

        private bool SaveUpdatedAppointment()
        {
            if (_TestInfo != null)
            {
                _TestInfo.AppointmentDate = uC_TestInfo1.AppointmentDate.Date;
                return _TestInfo.Save();
            }
            else
                return false;
        }

        private bool SaveRetakeApplication()
        {
            clsApplications RetakeApp = new clsApplications();

            RetakeApp.PersonID = _PersonID;
            RetakeApp.CreatedByUserID = frmLogin.CurrentUser.UserID;
            RetakeApp.ApplicationTypeID = (int)enApplicationTypeID.RetakeTest;
            RetakeApp.ApplicationStatus = (int)enApplicationStatus.New;
            RetakeApp.PaidFees = clsApplicationTypes.Find(RetakeApp.ApplicationTypeID).Fees;
            RetakeApp.ApplicationDate = DateTime.Now;

            if (RetakeApp.Save())
            {
                RetakeAppID = RetakeApp.ApplicationID;
                return true;
            }
            else
                return false;
        }

        private void frmAddNewAppointment_Load(object sender, EventArgs e)
        {
            LoadAppInfo();

            if (_TestAppointmentID != 0)
            {  
                if(_TestInfo.AppointmentDate < DateTime.Today)
                {
                    uC_TestInfo1.SetAppointmentMinDate(_TestInfo.AppointmentDate);
                }
                else
                {
                    uC_TestInfo1.SetAppointmentMinDate(DateTime.Today);
                    uC_TestInfo1.SetAppointmentDate(_TestInfo.AppointmentDate);
                }
            }
             else
               uC_TestInfo1.SetAppointmentMinDate(DateTime.Today);

            uC_TestInfo1.InitialzeInfo();
        }

        private void btSave_Click(object sender, EventArgs e)
        {

            if (_TestAppointmentID != 0)
            {
                if (SaveUpdatedAppointment())
                {
                    MessageBox.Show("Appointment Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsSaved = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Faild to Update Appointment", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
            }


            if (uC_TestInfo1.Trial > 0)
            {
                if (!SaveRetakeApplication())
                {
                    MessageBox.Show("Faild to Create Retake Application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (SaveNewAppointment())
            {
                MessageBox.Show("Appointment Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Faild to Add Appointment", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
