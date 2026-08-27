using System;
using System.Linq;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmMainForm : Form
    {
        public frmMainForm()
        {
            InitializeComponent();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople frmMP = new frmManagePeople();
            frmMP.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers frmUser = new frmUsers();
            frmUser.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails userDetails = new frmUserDetails(frmLogin.CurrentUser.UserID);

            userDetails.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(frmLogin.CurrentUser.UserID);

            changePassword.ShowDialog();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void manageApplcationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes applicationTypes = new frmManageApplicationTypes();

            applicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes testTypes = new frmManageTestTypes();

            testTypes.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditAdd_L_D_L_Application NLDApplication = new frmEditAdd_L_D_L_Application();

            NLDApplication.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDrivers Drivers = new frmDrivers();

            Drivers.ShowDialog();
        }

        private void intarnationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_I_L_Application i_L_Application = new frm_I_L_Application();

            i_L_Application.ShowDialog();
        }

        private void localDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_LDL_Applications LDL_Applications = new frmManage_LDL_Applications();

            LDL_Applications.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_I_L_Applications I_L_Applications = new frmManage_I_L_Applications();

            I_L_Applications.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalLicense frmRenew = new frmRenewLocalLicense();

            frmRenew.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacementFor_D_or_L_Licenses ReplacementFor = new frmReplacementFor_D_or_L_Licenses();

            ReplacementFor.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses DetainLicense = new frmDetain_ReleaseLicenses();

            DetainLicense.DetainMode = DVLD_BLL.enDetainMode.Detain;
            DetainLicense.ShowDialog();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses DetainLicense = new frmDetain_ReleaseLicenses();

            DetainLicense.DetainMode = DVLD_BLL.enDetainMode.Release;
            DetainLicense.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses detainedLicenses = new frmManageDetainedLicenses();

            detainedLicenses.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetain_ReleaseLicenses DetainLicense = new frmDetain_ReleaseLicenses();

            DetainLicense.DetainMode = DVLD_BLL.enDetainMode.Release;
            DetainLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_LDL_Applications LocalApplication = new frmManage_LDL_Applications();

            LocalApplication.ShowDialog();
        }
    }
}
