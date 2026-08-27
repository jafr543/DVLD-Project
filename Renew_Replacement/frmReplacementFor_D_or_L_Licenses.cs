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
    public partial class frmReplacementFor_D_or_L_Licenses : Form
    {
        public frmReplacementFor_D_or_L_Licenses()
        {
            InitializeComponent();
        }

        clsLicenses _LicenseInfo;

        int _NewLicenseID = 0;
        decimal DamagedFees = 0;
        decimal LostFees = 0;


        private int SetApplicationType()
        {
            if (rBDamagedLicense.Checked)
                return ((int)enApplicationTypeID.Replacement_ForA_DamagedDrivingLicense);
            else
               return ((int)enApplicationTypeID.Replacement_ForA_LostDrivingLicense);
        }

        private void SetApplicationFees()
        {
            
            if(rBDamagedLicense.Checked)
            {
                if(DamagedFees == 0)
                {
                   DamagedFees = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.Replacement_ForA_DamagedDrivingLicense);
                }
                uC_ReplacmentLicenseApplication1.SetApplicationFees(DamagedFees);
            }

            else
            {
                if(LostFees == 0)
                {
                   LostFees = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.Replacement_ForA_LostDrivingLicense);
                }

                uC_ReplacmentLicenseApplication1.SetApplicationFees(LostFees);
            }
        }

        private bool AddReplacementApplication(ref int AppID)
        {
            clsApplications NewApp = new clsApplications();

            NewApp.PersonID = _LicenseInfo.DriverInfo.PersonID;
            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = SetApplicationType();
            NewApp.ApplicationStatus = ((int)enApplicationStatus.New);
            NewApp.LastStatusDate = NewApp.ApplicationDate;
            NewApp.PaidFees = uC_ReplacmentLicenseApplication1.ApplicationFees;
            NewApp.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewApp.Save())
            {
                AppID = NewApp.ApplicationID;
                return true;
            }
            else
                return false;
        }

        private bool ReplaceLicense()
        {
            int AppID = 0;

            if (!AddReplacementApplication(ref AppID))
                return false;

            clsLicenses NewLicense = new clsLicenses();

            NewLicense.AppID = AppID;
            NewLicense.DriverID = _LicenseInfo.DriverID;
            NewLicense.LicenseClass = _LicenseInfo.LicenseClass;
            NewLicense.IssueDate = _LicenseInfo.IssueDate;
            NewLicense.ExpirationDate = _LicenseInfo.ExpirationDate;
            NewLicense.Notes = _LicenseInfo.Notes;
            NewLicense.PaidFees = 0;
            NewLicense.IsActive = true;

            if (rBDamagedLicense.Checked)
                NewLicense.IssueReason = enIssueReason.DamagedReplacement;
            else
                NewLicense.IssueReason = enIssueReason.LostReplacement;

            NewLicense.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewLicense.Save())
            {
                _NewLicenseID = NewLicense.LicenseID;
                uC_ReplacmentLicenseApplication1.SetApplicationID_ReplacedLicenseID(AppID, _NewLicenseID);
                clsApplications.Complete(AppID);
                clsLicenses.DeactivateLicense(_LicenseInfo.LicenseID);
                return true;
            }

            else
                return false;
        }

        private bool Confirmation()
        {
            if (MessageBox.Show("Are You Sure to Issue This License", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                return true;

            else
                return false;
        }

        private void uC_FindLicenseWithDriverInfo1_LicenseFound(DVLD_BLL.clsLicenses obj)
        {
            if (obj != null)
            {
                _LicenseInfo = obj;
                uC_ReplacmentLicenseApplication1.SetOldLicenseID(obj.LicenseID);

                if (!_LicenseInfo.IsActive)
                {
                    MessageBox.Show($"Selected License Not Active,\nPlease Choose another one", "Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btIssueReplacement.Enabled = false;
                    llShowNewLicenseInfo.Enabled = false;
                }
                else
                {
                    btIssueReplacement.Enabled = true;
                    llShowNewLicenseInfo.Enabled = false;
                }

                llShowLicenseHistory.Enabled = true;
            }

            else
              llShowLicenseHistory.Enabled = false;
        }

        private void ReplacementFor_CheckedChanged(object sender, EventArgs e)
        {
            SetApplicationFees();
            if (rBDamagedLicense.Checked)
                laTitle.Text = "Replacement For Damaged License";
            else
                laTitle.Text = "Replacement For Lost License";

            this.Text = laTitle.Text;
            
        }

        private void frmReplacementFor_D_or_L_Licenses_Load(object sender, EventArgs e)
        {
            uC_ReplacmentLicenseApplication1.LoadBasicInfo();
            SetApplicationFees();
        }

        private void btIssueReplacement_Click(object sender, EventArgs e)
        {

            if (!Confirmation())
                return;

            if (ReplaceLicense())
            {
                MessageBox.Show("Replace License Done Successfully", "License Replaced",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                uC_FindLicenseWithDriverInfo1.Enabled = false;
                btIssueReplacement.Enabled = false;
                gBReplacementFor.Enabled = false;
                llShowNewLicenseInfo.Enabled = true;
            }

            else
                MessageBox.Show("Faild to Replace This License", "Replace Faild",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicensesHistory LicensesHistory = new frmLicensesHistory(_LicenseInfo.DriverInfo.PersonID);

            LicensesHistory.ShowDialog();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(_NewLicenseID);

            LicenseInfo.ShowDialog();
        }

    }
}
