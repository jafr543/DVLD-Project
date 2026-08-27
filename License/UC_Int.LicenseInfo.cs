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
    public partial class UC_Int_LicenseInfo : UserControl
    {
        public UC_Int_LicenseInfo()
        {
            InitializeComponent();
        }

        clsInternationalDriverLicensesInfo_View _LicenseInfo;

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

        private void SetProfileImage(string ImagePath)
        {
            if (!string.IsNullOrWhiteSpace(ImagePath))
                pBProfileImage.ImageLocation = ImagePath;
            else
                pBProfileImage.Image = Resources.DefultProfileImage;
        }

        private void LoadLicenseInfo()
        {
            laName.Text = _LicenseInfo.FullName;
            laInt_LicenseID.Text = _LicenseInfo.InternationalLicenseID.ToString();
            laLicenseID.Text = _LicenseInfo.LicenseID.ToString();
            laNationalNo.Text = _LicenseInfo.NationalNo;
            laGender.Text = _LicenseInfo.Gender;
            SetGenderImage();
            laIssueDate.Text = _LicenseInfo.IssueDate.ToShortDateString();
            laApplicationID.Text = _LicenseInfo.AppID.ToString();
            SetActivationInfo(_LicenseInfo.IsActive);
            laDateOfBirth.Text = _LicenseInfo.DateOfBirth.ToShortDateString();
            laDriverID.Text = _LicenseInfo.DriverID.ToString();
            laExpirationDate.Text = _LicenseInfo.ExpirationDate.ToShortDateString();
            SetProfileImage(_LicenseInfo.ImagePath);
        }

        public void FindLicense(int LicenseID)
        {
            _LicenseInfo = clsInternationalDriverLicensesInfo_View.FindByInt_LicenseID(LicenseID);

            if (_LicenseInfo != null)
                LoadLicenseInfo();
        }

        public void LoadFoundLicense(clsInternationalDriverLicensesInfo_View LicensesInfo)
        {
            _LicenseInfo = LicensesInfo;
            LoadLicenseInfo();
        }

        public void ResetLicenseInfo()
        {
            laName.Text = "???";
            laInt_LicenseID.Text = "???";
            laLicenseID.Text = "???";
            laNationalNo.Text = "???";
            laGender.Text = "???";
            laIssueDate.Text = "???";
            laApplicationID.Text = "???";
            laIsActive.Text = "???";
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
