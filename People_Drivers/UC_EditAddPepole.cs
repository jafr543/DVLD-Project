using DVLD.Properties;
using DVLD_BLL;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_EditAddPepole : UserControl
    {
        enum enMode { Update, AddNew }
        enMode Mode = enMode.AddNew;

        private int _ID = -1;
        
        clsPeople Person;


        private string _newFolderPath = @"C:\DVLD\People_Images";
        private string _SelectedFilePath = string.Empty;
        private string _CurrentImagePath = string.Empty;
        private string _ImageExtension = string.Empty;

        public event Action<bool> CloseRequest;

        protected virtual void _CloseRequest(bool IsSaved)
        {
            Action<bool> handler = CloseRequest;
            if (handler != null)
            {
                handler(IsSaved);
            }
        }

        public UC_EditAddPepole()
        {
            InitializeComponent();
            Directory.CreateDirectory(_newFolderPath);
        }

        private void LoadPersonInfo()
        {
            txtNationalNo.Text = Person.NationalNo;
            txtFirstName.Text = Person.FirstName;
            txtSecondName.Text = Person.SecondName;
            txtThirdName.Text = Person.ThirdName;
            txtLastName.Text = Person.LastName;
            dtpDateofBirth.Value = Person.DateOfBirth;
            cBCountry.SelectedValue = Person.NationalityCountryID;
            if (Person.Gendor == 0)
                rBMale.Checked = true;
            else
                rBFemale.Checked = true;

            txtAddress.Text = Person.Address;
            txtPhone.Text = Person.Phone;

            if (Person.Email != null || Person.Phone != string.Empty)
                txtEmail.Text = Person.Email;

            if (!string.IsNullOrEmpty(Person.ImagePath) || !string.IsNullOrWhiteSpace(Person.ImagePath))
            {
                pBProfileImage.ImageLocation = Person.ImagePath;
                LLRemoveImage.Visible = true;
            }
        }

        private void FillPersonObject()
        {
            if (Mode == enMode.AddNew)
            {
                Person = new clsPeople();
            }
            else
            {
                Person = clsPeople.Find(_ID);
            }

        }

        private void LoadCountries()
        {
            DataTable dtCountries = clsPeople.CountriesList();

            cBCountry.DataSource = dtCountries;

            cBCountry.DisplayMember = "CountryName";
            cBCountry.ValueMember = "CountryID";

            cBCountry.SelectedIndex = cBCountry.FindStringExact("Iraq");
        }

        public void InitializeMode(int ID)
        {
            _ID = ID;
            if (_ID <= 0)
                Mode = enMode.AddNew;
            else
                Mode = enMode.Update;

            FillPersonObject();

            if (Mode == enMode.Update)
            {
                LoadPersonInfo();
            }
        }

        private void UC_EditAddPepole_Load(object sender, EventArgs e)
        {
            LoadCountries();
            dtpDateofBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateofBirth.Value = dtpDateofBirth.MaxDate;
            dtpDateofBirth.MinDate = DateTime.Now.AddYears(-100);
        }

        #region Validation

        private bool ValidateField(TextBox txt, string FieldName)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt, $"{FieldName} is required");
                return false;
            }

            errorProvider1.SetError(txt, "");
            return true;
        }

        private bool ValidateNationalNo()
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text) || string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "The National No Name is Required!");
                return false;
            }
            if (clsPeople.isNationalNoExist(txtNationalNo.Text, _ID))
            {
                errorProvider1.SetError(txtNationalNo, "The National No already in use!");
                return false;
            }

            
            errorProvider1.SetError(txtNationalNo, "");
            return true;
        }

        private bool ValidateEmail()
        {
            if (!clsPeople.isValidEmail(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Wrong Email Format!");
                return false;
            }
            
                errorProvider1.SetError(txtEmail, "");
                return true;
            
        }

        private bool ValidateDateOfBirth()
        {
            if (dtpDateofBirth.Value.Date > DateTime.Today.AddYears(-18))
            {
                errorProvider1.SetError(dtpDateofBirth, "Age Must be 18 or Older");
                return false;
            }

                errorProvider1.SetError(dtpDateofBirth, "");
                return true;
            
        }

        private int ValidateGender()
        {
            if(rBMale.Checked)
            {
                return 0;
            }

            return 1;
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider1.SetError(txtFirstName, "The First Name is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtFirstName, "");
            }

            if (string.IsNullOrEmpty(txtSecondName.Text) || string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                errorProvider1.SetError(txtSecondName, "The Second Name is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtSecondName, "");
            }

            if (string.IsNullOrEmpty(txtThirdName.Text) || string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                errorProvider1.SetError(txtThirdName, "The Third Name is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtThirdName, "");
            }

            if (string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider1.SetError(txtLastName, "The Last Name is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtLastName, "");
            }

            if (string.IsNullOrEmpty(txtNationalNo.Text) || string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "The National No Name is Required!");
                isValid = false;
            }

            else if (clsPeople.isNationalNoExist(txtNationalNo.Text, _ID))
            {
                errorProvider1.SetError(txtNationalNo, "The National No already in use!");
                isValid = false;
            }

            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }

            if (dtpDateofBirth.Value.Date > DateTime.Today.AddYears(-18))
            {
                errorProvider1.SetError(dtpDateofBirth, "Age Must be 18 or Older");
                isValid = false;
            }

            else
            {
                errorProvider1.SetError(dtpDateofBirth, "");
            }

            if (string.IsNullOrEmpty(txtPhone.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "The Phone Number is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }

            if (!clsPeople.isValidEmail(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Wrong Email Format!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }

            if (string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                errorProvider1.SetError(txtAddress, "The Address is Required!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtAddress, "");
            }

            return isValid;

        }

        #endregion

        #region ImageManager
        private void LoadPicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pBProfileImage.ImageLocation = openFileDialog.FileName;
                _SelectedFilePath = openFileDialog.FileName;
                _ImageExtension = Path.GetExtension(openFileDialog.FileName);
                LLRemoveImage.Visible = true;
            }

        }

        private void RemovePicture()
        {
            pBProfileImage.Image = Resources.DefultProfileImage;
            pBProfileImage.ImageLocation = null;
            _CurrentImagePath = Person.ImagePath;
            _SelectedFilePath = "";
            Person.ImagePath = "";
        }

        private string CreateImagePath()
        {
            string _newFileName = Guid.NewGuid().ToString();
            return Path.Combine(_newFolderPath, _newFileName + _ImageExtension);
        }

        private bool SaveImage()
        {

            if (Person.ImagePath == pBProfileImage.ImageLocation)
                return true;


            if (!string.IsNullOrWhiteSpace(_SelectedFilePath))
            {
                _CurrentImagePath = Person.ImagePath;
                string newPath = CreateImagePath();

                try
                {
                    File.Copy(_SelectedFilePath, newPath);
                    Person.ImagePath = newPath;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }


            if (!string.IsNullOrWhiteSpace(_CurrentImagePath))
            {
                try
                {
                    File.Delete(_CurrentImagePath);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return true;

        }

        #endregion

        private void ShowSuccessMessage()
        {
            if (Mode == enMode.AddNew)
            {
                MessageBox.Show("Person Added SuccessFully", "Add Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Person Updated SuccessFully", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowFailedMessage()
        {
            if (Mode == enMode.AddNew)
            {
                MessageBox.Show("Person Added Filed", "Added Filed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Person Updated Filed", "Updated Filed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePersonInfo()
        {
            if(!ValidateInputs())
            {
                MessageBox.Show("Please Make sure to Full all the Fields with Valid Values!", "Error Wrong Values!", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
                return;
            }

            Person.NationalNo = txtNationalNo.Text.Trim();
            Person.FirstName = txtFirstName.Text.Trim();
            Person.SecondName = txtSecondName.Text.Trim();
            Person.ThirdName = txtThirdName.Text.Trim();
            Person.LastName = txtLastName.Text.Trim();
            Person.DateOfBirth = dtpDateofBirth.Value;
            Person.NationalityCountryID = (int)cBCountry.SelectedValue;
            Person.Gendor = ValidateGender();
            Person.Address = txtAddress.Text.Trim();
            Person.Phone = txtPhone.Text.Trim();

            if(txtEmail.Text != null)
                Person.Email = txtEmail.Text.Trim();
            else
                Person.Email = "";

            if(!SaveImage())
            {
                ShowFailedMessage();
                return;
            }

            if (Person.Save())
            {
                ShowSuccessMessage();

                if (CloseRequest != null)
                    _CloseRequest(true);
            }
                
            
            else
                ShowFailedMessage();                
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SavePersonInfo();
        }

        #region Events
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtFirstName, "First Name");
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtSecondName, "Second Name");
        }

        private void txtThirdName_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtThirdName, "Third Name");
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtLastName, "Last Name");
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtPhone, "Phone Number");
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            ValidateField(txtAddress, "Address");
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtEmail.Text))
            ValidateEmail();
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            ValidateNationalNo();
        }

        private void dtpDateofBirth_Validating(object sender, CancelEventArgs e)
        {
            ValidateDateOfBirth();
        }

        private void LLSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadPicture();
        }

        private void LLRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RemovePicture();
            LLRemoveImage.Visible = false;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            if (CloseRequest != null)
                _CloseRequest(false);
        }

        #endregion
    }
}
