using DVLD.Properties;
using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_ShowPersonDetails : UserControl
    {
        public UC_ShowPersonDetails()
        {
            InitializeComponent();
        }

        private clsPeople Person;

        public event Action<bool> UpdatedRequest;

        protected virtual void PersonisUpdated(bool IsUpdated)
        {
            Action<bool> handler = UpdatedRequest;
            if (handler != null)
            {
                handler(IsUpdated);
            }
        }

        public void LoadFoundedPerson(clsPeople FoundedPerson)
        {
            Person = FoundedPerson;
            LoadPersonInfo();
        }

        public void ResetPersonInfo()
        {
            laPersonID.Text = "???";
            laName.Text = "???";
            laNationalNo.Text = "???";
            laGender.Text = "???";
            pBGender.Image = Resources.man;
            laEmail.Text = "???";
            laAddress.Text = "???";
            laDateOfBirth.Text = "???";
            laPhone.Text = "???";
            laCountry.Text = "???";

            pBPersonImage.ImageLocation = null;
            pBPersonImage.Image = Resources.DefultProfileImage;

            llEditPerson.Visible = false;
        }

        public bool FindPerson(int ID)
        {
            Person = clsPeople.Find(ID);

            if(Person != null)
            {
                LoadPersonInfo();
                return true;
            }

             return false;
        }

        private void UpdateGenderImage()
        {

            if (Person.Gendor == 0)
            {
                pBGender.Image = Resources.man;
            }
            else
                pBGender.Image = Resources.woman;
        }

        private void LoadPersonInfo()
        {
            if(Person != null)
            {
                laPersonID.Text = Person.PersonID.ToString();
                laName.Text = Person.FullName;
                laNationalNo.Text = Person.NationalNo;
                laGender.Text = Person.GenderText;
                UpdateGenderImage();
                laEmail.Text = Person.Email;
                laAddress.Text = Person.Address;
                laDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
                laPhone.Text = Person.Phone;
                laCountry.Text = Person.CountryName;

                if(!string.IsNullOrWhiteSpace(Person.ImagePath))
                {
                    pBPersonImage.ImageLocation = Person.ImagePath;
                }
                else
                {
                    pBPersonImage.ImageLocation = null;
                    pBPersonImage.Image = Resources.DefultProfileImage;
                }

                llEditPerson.Visible = true;
            }
        }

        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Person == null)
            {
                MessageBox.Show("Person Not Exist!", "Edit Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmEditAddPerson frmEdit = new frmEditAddPerson(Person.PersonID);
            frmEdit.ShowDialog();

            if(frmEdit.IsSaved)
            {
                FindPerson(Person.PersonID);

                if (UpdatedRequest != null)
                    PersonisUpdated(frmEdit.IsSaved);
            }
        }
    }
}
