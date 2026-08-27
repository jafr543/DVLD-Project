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
    public partial class frmAddEditUser : Form
    {
        public frmAddEditUser()
        {
            InitializeComponent();
        }

        public frmAddEditUser(int UserID)
        {
            InitializeComponent();

            _SelectedUser = clsUser.Find(UserID);
        }

        public bool IsSaved { get; private set; }

        private clsPeople _SelectedPerson;
        private clsUser _SelectedUser;

        #region Validation

        private bool ValidateUserName()
        {

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "User Name is Empty!");
                return false;
            }

            else
                errorProvider1.SetError(txtUserName, "");

            if (_SelectedUser != null)
            {
                if (clsUser.IsUserNameExist(txtUserName.Text, _SelectedUser.UserID))
                {
                    errorProvider1.SetError(txtUserName, "User Name Already in Use!");
                    return false;

                }
                else
                    errorProvider1.SetError(txtUserName, "");
            }
            else
            {
                if (clsUser.IsUserNameExist(txtUserName.Text))
                {
                    errorProvider1.SetError(txtUserName, "User Name Already in Use!");
                    return false;

                }
                else
                    errorProvider1.SetError(txtUserName, "");
            }

            return true;

        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtPassword, "");

            return true;
        }

        private bool ValidatePasswordConfirm()
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtConfirmPassword, "");

         
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Wrong Password");
                return false;
            }
            else
                errorProvider1.SetError(txtConfirmPassword, "");

            return true;

        }

        private bool ValidateUserInfo()
        {
            bool IsValid = true;

            if (!ValidateUserName())
                IsValid = false;

            if(!ValidatePassword())
                IsValid = false;
            
            if(!ValidatePasswordConfirm())
                IsValid = false;

            return IsValid;
        }

        #endregion

        private void LoadUser()
        {
            if(_SelectedUser != null)
            {
                laUserID.Text = _SelectedUser.UserID.ToString();
                txtUserName.Text = _SelectedUser.UserName;
                txtPassword.Text = _SelectedUser.Password;
                txtConfirmPassword.Text = _SelectedUser.Password;
                cBIsActive.Checked = _SelectedUser.IsActive;
            }
        }

        private void SavenewUser()
        {
            if(!ValidateUserInfo())
            {
                MessageBox.Show("Some fileds are Not Valide!", "Invalid Valuse!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsUser NewUser = new clsUser();

            NewUser.PersonInfo = _SelectedPerson;
            NewUser.UserName = txtUserName.Text.Trim();
            NewUser.Password = txtPassword.Text.Trim();
            NewUser.IsActive = cBIsActive.Checked;

            if(NewUser.Save())
            {
                MessageBox.Show("New User Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Failed to Save New User", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUser()
        {
            if (!ValidateUserInfo())
            {
                MessageBox.Show("Some fileds are Not Valide!", "Invalid Values!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            _SelectedUser.UserName = txtUserName.Text;
            _SelectedUser.Password = txtPassword.Text;
            _SelectedUser.IsActive = cBIsActive.Checked;

            if (_SelectedUser.Save())
            {
                MessageBox.Show("User Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Failed to Updated User", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Events

        private void uC_FindPersonWithDetails1_PersonFound(clsPeople obj)
        {
            if (obj != null)
            {
                if (clsUser.IsUserExistByPersonID(obj.PersonID))
                {
                    MessageBox.Show("this User already Exist!", "User Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _SelectedPerson = null;
                    btNext.Enabled = false;
                    btSave.Enabled = false;
                    return;
                }

                _SelectedPerson = obj;
                btNext.Enabled = true;
            }
            else
            {
                _SelectedPerson = null;
                btNext.Enabled = false;
                btSave.Enabled = false;

            }
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            if(_SelectedUser != null)
            {
                tabControl1.SelectedTab = tbLoginInfo;
                laTitle.Text = "Update User";
                this.Text = "Edit User";
                btSave.Enabled = true;
                LoadUser();

            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage == tbLoginInfo)
            {
                if(_SelectedUser == null && _SelectedPerson == null)
                e.Cancel = true;
            }
            
            if(e.TabPage == tbPersonInfo && _SelectedUser != null)
            {
                e.Cancel = true;
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tbLoginInfo;
            btSave.Enabled = true;
        }

        private void btSave_Click(object sender, EventArgs e)
        {

            if (_SelectedPerson != null)
                SavenewUser();
            else
                UpdateUser();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            IsSaved = false;
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ValidatePassword();
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            ValidatePasswordConfirm();
        }

        #endregion


    }
}
