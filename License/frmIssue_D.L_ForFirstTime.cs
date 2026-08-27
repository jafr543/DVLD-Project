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
    public partial class frmIssue_DL_ForFirstTime : Form
    {
        int _LocalAppID = 0;
        int _DriverID = 0;

        public bool IsIssue = false;

        public frmIssue_DL_ForFirstTime(int LocalAppID)
        {
            InitializeComponent();

            _LocalAppID = LocalAppID;
        }

        private void LoadAppInfo()
        {
            uC_FullAppInfo1.LoadInfo(_LocalAppID);
        }

        private bool IsPersonADriver()
        {
            _DriverID = clsDrivers.GetDriverID(uC_FullAppInfo1.PersonID);

            return (_DriverID > 0);
        }

        private bool AddNewDriver()
        {
            clsDrivers newDriver = new clsDrivers();

            newDriver.PersonID = uC_FullAppInfo1.PersonID;
            newDriver.CreatedByUserID = frmLogin.CurrentUser.UserID;

            if (newDriver.Save())
            {
                _DriverID = newDriver.DriverID;
                return true;
            }

            else
                return false;
        }

        private bool SaveNewLicense(int DriverID)
        {
            clsLicenses newLicense = new clsLicenses();

            newLicense.AppID = uC_FullAppInfo1.AppID;
            newLicense.DriverID = _DriverID;
            newLicense.LicenseClass = clsLicenseClasses.FindClassIDByClassName(uC_FullAppInfo1.ClassName);
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = newLicense.IssueDate.AddYears
                            (clsLicenseClasses.GetLicenseClassValidityLength(newLicense.LicenseClass));
            newLicense.Notes = txtNotes.Text;
            newLicense.PaidFees = clsLicenseClasses.GetLicenseClassFees(newLicense.LicenseClass);
            newLicense.IsActive = true;
            newLicense.IssueReason = enIssueReason.FirstTime;
            newLicense.CreatedByUserID = frmLogin.CurrentUser.UserID;

            return newLicense.Save();
        }

        private void frmIssue_DL_ForFirstTime_Load(object sender, EventArgs e)
        {
            LoadAppInfo();
        }

        private void btIssue_Click(object sender, EventArgs e)
        {
            if(IsPersonADriver())
            {
                SaveNewLicense(_DriverID);
                clsApplications.Complete(uC_FullAppInfo1.AppID);
                IsIssue = true;
                MessageBox.Show("New License Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                if(AddNewDriver())
                {
                    if(SaveNewLicense(_DriverID))
                    {
                        clsApplications.Complete(uC_FullAppInfo1.AppID);
                        IsIssue = true;
                        MessageBox.Show("New License Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                        MessageBox.Show("Faild to Add New License!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Faild to Add New Driver!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
