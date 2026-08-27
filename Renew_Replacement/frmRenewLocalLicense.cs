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
    public partial class frmRenewLocalLicense : Form
    {
        public frmRenewLocalLicense()
        {
            InitializeComponent();
        }

        clsLicenses _LicenseInfo;

        int _PersonID = 0;
        int _NewLicenseID = 0;
        int _LicenseClassID = 0;

        private bool ValidateLicense()
        {
            if(_LicenseInfo.ExpirationDate >= DateTime.Now.Date)
            {
                MessageBox.Show($"Selected License Not Expiared yet \n it`s will Expire on " + _LicenseInfo.ExpirationDate.ToShortDateString()
                    , "Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(_LicenseInfo.IsActive == false)
            {
                MessageBox.Show($"Selected License Not Active", "Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool AddRenewApplication(ref int AppID)
        {
            clsApplications NewApp = new clsApplications();

            NewApp.PersonID = _PersonID;
            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = ((int)enApplicationTypeID.RenewDrivingLicenseService);
            NewApp.ApplicationStatus = ((int)enApplicationStatus.New);
            NewApp.LastStatusDate = NewApp.ApplicationDate;
            NewApp.PaidFees = uC_RenewLicenseApplication1.ApplicationFees;
            NewApp.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewApp.Save())
            {
                AppID = NewApp.ApplicationID;
                return true;
            }
            else
                return false;
        }

        private bool RenewLicense()
        {
            int AppID = 0;

            if (!AddRenewApplication(ref AppID))
                return false;

            clsLicenses NewLicense = new clsLicenses();

            NewLicense.AppID = AppID;
            NewLicense.DriverID = _LicenseInfo.DriverID;
            NewLicense.LicenseClass = _LicenseInfo.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = NewLicense.IssueDate.AddYears(_LicenseInfo.LicenseClassInfo.DefaultValidityLength);
            uC_RenewLicenseApplication1.SetExpirationDate(NewLicense.ExpirationDate);
            NewLicense.Notes = uC_RenewLicenseApplication1.Notes;
            NewLicense.PaidFees = _LicenseInfo.PaidFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewLicense.Save())
            {
                _NewLicenseID = NewLicense.LicenseID;
                uC_RenewLicenseApplication1.SetNewApp_and_LicenseIDs(AppID, _NewLicenseID);
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
            if(obj != null)
            {
                _LicenseInfo = obj;
                _PersonID = _LicenseInfo.DriverInfo.PersonID;
                _LicenseClassID = _LicenseInfo.LicenseClassInfo.LicenseClassID;

                if (!ValidateLicense())
                {
                    uC_RenewLicenseApplication1.LoadOldLicenseInfo(_LicenseInfo);
                    btRenew.Enabled = false;
                    llShowNewLicenseInfo.Enabled = false;
                    uC_RenewLicenseApplication1.Enabled = false;
                    return;
                }
                else
                {
                   btRenew.Enabled = true;
                   uC_RenewLicenseApplication1.Enabled = true;
                   llShowNewLicenseInfo.Enabled = false;
                }

                
            }
        }

        private void frmRenewLocalLicense_Load(object sender, EventArgs e)
        {
            uC_RenewLicenseApplication1.LoadBasicInfo();
        }

        private void btRenew_Click(object sender, EventArgs e)
        {
            if (!Confirmation())
                return;

            if (RenewLicense())
            {
                MessageBox.Show("Renew License Done Successfully", "License Renewed",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                uC_FindLicenseWithDriverInfo1.Enabled = false;
                btRenew.Enabled = false;
                llShowNewLicenseInfo.Enabled = true;
            }

            else
                MessageBox.Show("Faild to Renew This License", "Renew Faild",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_PersonID != 0)
            {
                frmLicensesHistory licensesHistory = new frmLicensesHistory(_PersonID);
                licensesHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Search For Person First", "No Person Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_NewLicenseID != 0)
            {
                frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(_NewLicenseID);

                LicenseInfo.ShowDialog();
            }
            else
            {
                MessageBox.Show("License Not Found", "No License Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
