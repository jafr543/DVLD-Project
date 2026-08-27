using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frm_I_L_Application : Form
    {
        public frm_I_L_Application()
        {
            InitializeComponent();
        }

        public frm_I_L_Application(int LicenseID)
        {
            InitializeComponent();

            uC_FindLicenseWithDriverInfo1.Pre_Search(LicenseID);
        }

        clsLicenses _LicenseInfo;

        public bool IsSaved = false;

        int _Int_LicenseID = 0;

        private bool ValidLicense()
        {
            if(_LicenseInfo.ExpirationDate <= DateTime.Now.Date)
            {
                MessageBox.Show("This License are Invalid Because\nit`s Expired", "Not Valid", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            if(!_LicenseInfo.IsActive)
            {
                MessageBox.Show("This License are Invalid Because\nit`s Inactive", "Inactive Licaense", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool AddNewApplication(ref int AppID)
        {
            clsApplications NewApp = new clsApplications();

            NewApp.PersonID = _LicenseInfo.DriverInfo.PersonID;
            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = ((int)enApplicationTypeID.NewIntemationalLicense);
            NewApp.ApplicationStatus = ((int)enApplicationStatus.New);
            NewApp.LastStatusDate = NewApp.ApplicationDate;
            NewApp.PaidFees = uC_InternationalApp1.PaidFees;
            NewApp.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewApp.Save())
            {
                AppID = NewApp.ApplicationID;
                return true;
            }
            else
                return false;
        }

        private bool AddNewInterNationalLicense()
        {
            int AppID = 0;

            if (!AddNewApplication(ref AppID))
                return false;

            clsInterNationalLicenses NewLicense = new clsInterNationalLicenses();

            NewLicense.AppID = AppID;
            NewLicense.DriverID = _LicenseInfo.DriverID;
            NewLicense.LocalLicenseID = _LicenseInfo.LicenseID;
            NewLicense.IssueDate = uC_InternationalApp1.IssueDate;
            NewLicense.ExpirationDate = uC_InternationalApp1.ExpirationDate;
            NewLicense.IsActive = true;
            NewLicense.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if(NewLicense.Save())
            {
                _Int_LicenseID = NewLicense.InternationalLicenseID;
                uC_InternationalApp1.SetAppIDAndI_LicenseID(AppID, _Int_LicenseID);
                clsApplications.Complete(AppID);
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

        private void frm_I_L_Application_Load(object sender, EventArgs e)
        {
            uC_InternationalApp1.InitializeBasicInfo();
        }

        private void uC_FindLicenseWithDriverInfo1_LicenseFound(DVLD_BLL.clsLicenses obj)
        {
            if (obj != null)
            {
                _LicenseInfo = obj;
                uC_InternationalApp1.SetLocalLicenseID(_LicenseInfo.LicenseID);
                llShowLicensesHistory.Enabled = true;

                if (clsInterNationalLicenses.IsLicenseExist(_LicenseInfo.LicenseID))
                { 
                    MessageBox.Show("Person already have an Active International License", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _Int_LicenseID = clsInterNationalLicenses.GetLicenseID(_LicenseInfo.LicenseID);
                    btIssue.Enabled = false;
                    llShowLicenseInfo.Enabled = true;
                    return;
                }

                if (_LicenseInfo.LicenseClass != (int)enLicensesClassesID.Class3_Ordinarydrivinglicense)
                {
                    MessageBox.Show("You Can Only Issue License With Class 3", "Invalid License", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    btIssue.Enabled = false;
                    llShowLicenseInfo.Enabled = false;
                    return;

                }

                if (!ValidLicense())
                {
                        btIssue.Enabled = false;
                        llShowLicenseInfo.Enabled = false;
                    return;
                }

                    btIssue.Enabled = true;
                     llShowLicenseInfo.Enabled = false;
            }
            else
            {
                _LicenseInfo = null;
                btIssue.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                llShowLicensesHistory.Enabled = false;
                uC_InternationalApp1.SetLocalLicenseID(0);
            }
        }

        private void btIssue_Click(object sender, EventArgs e)
        {
            if (!Confirmation())
                return;

            if(AddNewInterNationalLicense())
            {
                MessageBox.Show("International License Issued Successfully", "License Issued",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                uC_FindLicenseWithDriverInfo1.Enabled = false;
                btIssue.Enabled = false;
                llShowLicenseInfo.Enabled = true;
                IsSaved = true;
            }

            else
                MessageBox.Show("Faild to Issue International License", "Issued Faild",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_LicenseInfo.DriverInfo.PersonID != 0)
            {
               frmLicensesHistory licensesHistory = new frmLicensesHistory(_LicenseInfo.DriverInfo.PersonID);
                licensesHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Search For Person First", "No Person Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_Int_LicenseID != 0)
            {
                frmShow_Int_LicenseInfo Show_Int_LicenseInfo = new frmShow_Int_LicenseInfo(_Int_LicenseID);

                Show_Int_LicenseInfo.ShowDialog();
            }
            else
            {
                MessageBox.Show("License Not Found", "No License Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
