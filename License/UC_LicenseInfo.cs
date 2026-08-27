using DVLD.Properties;
using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_LicenseInfo : UserControl
    {
        public clsLicenses LicenseInfo { get; private set; }

        public UC_LicenseInfo()
        {
            InitializeComponent();
        }

        private void SetGenderImage()
        {
            if (laGender.Text == "Male")
                pBGender.Image = Resources.man;
            else
                pBGender.Image = Resources.woman;
        }

        private void SetActivationInfo(bool IsActive)
        {
            if (IsActive)
            {
                laIsActive.Text = "Yes";
                pBIsActive.Image = Resources.activities;
            }
            else
            {
                laIsActive.Text = "No";
                pBIsActive.Image = Resources.InActive;
            }
        }

        private void SetDetainInfo(bool IsDetained)
        {
            if (IsDetained)
            {
                laIsDetained.Text = "Yes";
                pBIsDetained.Image = Resources.activities;
            }
            else
            {
                laIsDetained.Text = "No";
                pBIsDetained.Image = Resources.InActive;
            }

        }

        private void SetProfileImage(string ImagePath)
        {
            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                if(File.Exists(ImagePath))
                    pBProfileImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                pBProfileImage.Image = Resources.DefultProfileImage;
        }

        private void LoadInfo()
        {
            if (LicenseInfo != null)
            {
                laClass.Text = LicenseInfo.LicenseClassInfo.ClassName;
                laName.Text = LicenseInfo.DriverInfo.PersonInfo.FullName;
                laLicenseID.Text = LicenseInfo.LicenseID.ToString();
                laNationalNo.Text = LicenseInfo.DriverInfo.PersonInfo.NationalNo;
                laGender.Text = LicenseInfo.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
                SetGenderImage();
                laIssueDate.Text = LicenseInfo.IssueDate.ToShortDateString();
                laIssueReason.Text = LicenseInfo.IssueReasonText;
                laNotes.Text = LicenseInfo.Notes;
                SetActivationInfo(LicenseInfo.IsActive);
                laDateOfBirth.Text = LicenseInfo.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
                laDriverID.Text = LicenseInfo.DriverID.ToString();
                laExpirationDate.Text = LicenseInfo.ExpirationDate.ToShortDateString();
                SetDetainInfo(clsDetainedLicenses.IsLicenseDetained(LicenseInfo.LicenseID));
                SetProfileImage(LicenseInfo.DriverInfo.PersonInfo.ImagePath);
            }

        }

        public void LoadLicenseInfo(int LicenseID)
        {
            LicenseInfo = clsLicenses.Find(LicenseID);
            LoadInfo();
        }

        public void LoadFoundLicense(clsLicenses LicensesInfo)
        {
            LicenseInfo = LicensesInfo;
            LoadInfo();
        }

        public void ResetLicenseInfo()
        {
            laClass.Text = "???";
            laName.Text = "???";
            laLicenseID.Text = "???";
            laNationalNo.Text = "???";
            laGender.Text = "???";
            laIssueDate.Text = "???";
            laIssueReason.Text = "???";
            laNotes.Text = "???";
            laDateOfBirth.Text = "???";
            laDriverID.Text = "???";
            laExpirationDate.Text = "???";
            pBGender.Image = Resources.question_mark;
            pBProfileImage.Image = Resources.DefultProfileImage;
            pBIsActive.Image = Resources.question_mark;
            pBIsDetained.Image = Resources.question_mark;
        }

    }
}
