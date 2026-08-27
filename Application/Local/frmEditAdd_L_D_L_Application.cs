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
    public partial class frmEditAdd_L_D_L_Application : Form
    {
        int _LocalAppID = 0;
        string _ClassName = string.Empty;

        clsPeople Person;

        int PersonID = 0;
        clsLDL_Application newApplication;
        public bool IsSaved = false;

        public frmEditAdd_L_D_L_Application()
        {
            InitializeComponent();
        }

        public frmEditAdd_L_D_L_Application(int LocalApp, string className)
        {
            InitializeComponent();
            _LocalAppID = LocalApp;
            _ClassName = className;
            PersonID = clsPeople.FindByLocalAppID(_LocalAppID);
        }

        private void LoadApplicationInfo()
        {
            clsApplications AppInfo = clsApplications.FindByLocalAppID(_LocalAppID);

            if (AppInfo != null)
            {
                cBLicenseClasses.SelectedIndex = cBLicenseClasses.FindStringExact(_ClassName);
                laID.Text = AppInfo.ApplicationID.ToString();
                laApplicationDate.Text = AppInfo.ApplicationDate.ToShortDateString();
                laFees.Text = AppInfo.PaidFees.ToString();
                laCreatedByUser.Text = clsUser.Find(AppInfo.CreatedByUserID).UserName;
            
                btSave.Enabled = true;
            }

        }

        private void LoadLicenseClassesNamesInfo()
        {
            cBLicenseClasses.DataSource = clsLicenseClasses.GetallLicenseClassesNames();

            cBLicenseClasses.DisplayMember = "ClassName";
            cBLicenseClasses.ValueMember = "LicenseClassID";
        }

        private void LoadDateFeesUserInfo()
        {
            laApplicationDate.Text = DateTime.Now.ToShortDateString();
            laFees.Text = clsApplicationTypes.GetAppTypeFees((enApplicationTypeID.NewLocalDrivingLicenseService)).ToString();
            laCreatedByUser.Text = frmLogin.CurrentUser.UserName;
        }

        private void AddNewApplication()
        {
            newApplication = new clsLDL_Application();
            newApplication.PersonID = Person.PersonID;
            newApplication.CreatedByUserID = frmLogin.CurrentUser.UserID;
            newApplication.ApplicationTypeID = (int)enApplicationTypeID.NewLocalDrivingLicenseService;
            newApplication.ApplicationStatus = (int)enApplicationStatus.New;
            newApplication.PaidFees = clsApplicationTypes.Find(newApplication.ApplicationTypeID).Fees;
            newApplication.LicenseClassID = (int)cBLicenseClasses.SelectedValue;
        }

        private void SaveNewApplication()
        {
            if(clsLDL_Application.HasActiveOrCompleteApplication(Person.PersonID, (int)cBLicenseClasses.SelectedValue))
            {
                MessageBox.Show("this Person Have Already Active or Completed Application on This LicenseClass", "Not Allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            AddNewApplication();

            if(newApplication.Save())
            {
                MessageBox.Show("Application Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                laID.Text = newApplication.ApplicationID.ToString();
                btSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Faild to Add Application", "Falid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUpdatedApplication()
        {
            if (clsLDL_Application.HasActiveOrCompleteApplication(PersonID, (int)cBLicenseClasses.SelectedValue))
            {
                MessageBox.Show("this Person Have Already Active or Completed Application on This LicenseClass", "Not Allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (clsLDL_Application.Update(_LocalAppID, (int)cBLicenseClasses.SelectedValue))
            {
                MessageBox.Show("Application Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
            }
            else
            {
                MessageBox.Show("Faild to Update this Application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        #region Events

        private void uC_FindPersonWithDetails2_PersonFound(clsPeople obj)
        {
            if (obj != null)
            {
                Person = obj;
                btNext.Enabled = true;
            }
            else
            {
                Person = null;
                btNext.Enabled = false;
                btSave.Enabled = false;
            }
        }

        private void frmN_L_D_Application_Load(object sender, EventArgs e)
        {
            LoadLicenseClassesNamesInfo();

            if(_LocalAppID != 0)
            {
                LoadApplicationInfo();
                tabControl1.SelectedTab = tbApplicationInfo;
            }
            else
            {
                LoadDateFeesUserInfo();
            }
            
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if(_LocalAppID == 0)
               SaveNewApplication();
            else
                SaveUpdatedApplication();
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tbApplicationInfo;
            btSave.Enabled = true;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage == tbApplicationInfo && Person == null && _LocalAppID == 0)
            {
                e.Cancel = true;
            }
            if(e.TabPage == tbPersonInfo && _LocalAppID != 0)
            {
                e.Cancel = true;
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
