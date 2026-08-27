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
    public partial class frmDetain_ReleaseLicenses : Form
    {
        int _LicenseID = 0;
        public frmDetain_ReleaseLicenses()
        {
            InitializeComponent();
        }

        public frmDetain_ReleaseLicenses(int LicenseID)
        {
            InitializeComponent();
            this._LicenseID = LicenseID;
        }

        public enDetainMode DetainMode { get; set;}
        public bool IsSaved { get; private set; }

        clsLicenses _LicenseInfo;



        private bool IsDetain()
        {
            return clsDetainedLicenses.IsLicenseDetained(_LicenseInfo.LicenseID);
        }

        private void InitializeDetainForm()
        {
            laTitle.Text = "Detain License";
            uC_DetainInfo1.InitializeDetainInfo();
            btSave.Text = "Detain";
            btSave.Image = Resources.Detaine_24x24;
        }

        private void InitializeReleaseForm()
        {
            this.Text = laTitle.Text = "Release License";
            btSave.Text = "Release";
            btSave.Image = Resources.Release_24x24;
            uC_DetainInfo1.InitializeReleaseInfo();
        }

        private bool AddReleaseApplication(ref int AppID)
        {
            clsApplications NewApp = new clsApplications();

            NewApp.PersonID = _LicenseInfo.DriverInfo.PersonID;
            NewApp.ApplicationDate = DateTime.Now;
            NewApp.ApplicationTypeID = (int)enApplicationTypeID.ReleaseDetainedDrivingLicsense;
            NewApp.ApplicationStatus = ((int)enApplicationStatus.New);
            NewApp.LastStatusDate = NewApp.ApplicationDate;
            NewApp.PaidFees = clsApplicationTypes.GetAppTypeFees(enApplicationTypeID.ReleaseDetainedDrivingLicsense);
            NewApp.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (NewApp.Save())
            {
                AppID = NewApp.ApplicationID;
                return true;
            }
            else
                return false;
        }

        private bool AddDetainLicense()
        {
            clsDetainedLicenses DetainedLicense = new clsDetainedLicenses();

            DetainedLicense.LicenseID = _LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;

            if (uC_DetainInfo1.GetFineFees() > -1)
                DetainedLicense.FineFees = uC_DetainInfo1.FineFees;
            else
                return false;

            DetainedLicense.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (DetainedLicense.AddDetainLicense())
            {
                uC_DetainInfo1.SetDetainID(DetainedLicense.DetainID);
                return true;
            }
            else
                return false;
        }

        private bool ReleaseLicense()
        {
            int AppID = 0;

            if (!AddReleaseApplication(ref AppID))
                return false;

            clsDetainedLicenses DetainedLicense = new clsDetainedLicenses();

            DetainedLicense.LicenseID = _LicenseID;
            DetainedLicense.ReleaseDate = DateTime.Now;
            DetainedLicense.ReleasedByUserID = frmLogin.CurrentUser.UserID;
            DetainedLicense.ReleaseApplicationID = AppID;

            if (DetainedLicense.ReleaseLicense())
            {
                uC_DetainInfo1.SetApplicationID(AppID);
                return true;
            }
            else
                return false;
        }

        private bool SaveDetain_or_ReleaseInfo()
        {
            if (DetainMode == enDetainMode.Detain)
                return AddDetainLicense();
            else
                return ReleaseLicense();
        }

        private bool Confirmation()
        {
            if (MessageBox.Show("Are You Sure to Issue This License", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                return true;

            else
                return false;
        }

        private void frmDetain_ReleaseLicenses_Load(object sender, EventArgs e)
        {
            if(_LicenseID == 0)
            {
              if (DetainMode == enDetainMode.Detain)
                InitializeDetainForm();
              else
                InitializeReleaseForm();
            }
            else
            {
                InitializeReleaseForm();
                uC_FindLicenseWithDriverInfo1.Pre_Search(_LicenseID);
            }

            IsSaved = false;
        }

        private void uC_FindLicenseWithDriverInfo1_LicenseFound(DVLD_BLL.clsLicenses obj)
        {
            _LicenseInfo = obj;

            if (_LicenseInfo != null)
            {
                _LicenseID = _LicenseInfo.LicenseID;
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;

                uC_DetainInfo1.SetLicenseID(_LicenseInfo.LicenseID);

                if(DetainMode == enDetainMode.Detain)
                {
                    if (IsDetain())
                    {
                        MessageBox.Show("License Already Detain Please Choose in Other One", "License Already Detain", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        btSave.Enabled = false;
                        return;
                    }
                }

                else                   
                {
                    if (!IsDetain())
                    {
                        MessageBox.Show("License Is`t Detain Please Choose in Other One", "License Not Detain", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        uC_DetainInfo1.SetDetainID(0);
                        uC_DetainInfo1.SetFineFees(0);
                        btSave.Enabled = false;
                        return;
                    }
                    else
                    {
                        uC_DetainInfo1.SetDetainID(clsDetainedLicenses.GetDetainID(_LicenseID));
                        uC_DetainInfo1.SetFineFees(clsDetainedLicenses.GetDetainFees(_LicenseID));
                    }

                }

                btSave.Enabled = true;
            }
            else
            {
                uC_DetainInfo1.SetLicenseID(0);
                llShowLicenseHistory.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                btSave.Enabled = false;
            }

        }

        private void btSave_Click(object sender, EventArgs e)
        {

            if (!Confirmation())
                return;

            if(SaveDetain_or_ReleaseInfo())
            {
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                uC_FindLicenseWithDriverInfo1.Enabled = false;
                uC_DetainInfo1.Enabled = false;
                btSave.Enabled = false;
                IsSaved = true;
            }
            else
            {
                MessageBox.Show("Faild to Saved Data", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicensesHistory LicensesHistory = new frmLicensesHistory(_LicenseInfo.DriverInfo.PersonID);

            LicensesHistory.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (_LicenseID > 0)
            {
                frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(_LicenseID);
                LicenseInfo.ShowDialog();
            }
            else
                MessageBox.Show("Falid to Find License", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
