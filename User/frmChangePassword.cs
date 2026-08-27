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
    public partial class frmChangePassword : Form
    {
        private int _ID;

        private clsUser _SelectedUser;

        public frmChangePassword(int ID)
        {
            InitializeComponent();

            _ID = ID;
        }

        private void FindUser()
        {
            _SelectedUser = clsUser.Find(_ID);
        }

        private void LoadUser()
        {
            uC_ShowUserDetails1.LoadUserInfo(_SelectedUser);
        }

        #region Validation
        private bool ValidateCurrentPassword()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "Password is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtCurrentPassword, "");


            if(txtCurrentPassword.Text != _SelectedUser.Password)
            {
                errorProvider1.SetError(txtCurrentPassword, "Wrong Password!");
                return false;
            }
            else
                errorProvider1.SetError(txtCurrentPassword, "");
        

            return true;
        }

        private bool ValidateNewtPassword()
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, "New Password is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtNewPassword, "");

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


            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPassword, "Wrong Password");
                return false;
            }
            else
                errorProvider1.SetError(txtConfirmPassword, "");

            return true;

        }

        private bool ValidateInputs()
        {
            bool IsValid = true;

            if (!ValidateCurrentPassword())
                IsValid = false;

            if (!ValidatePasswordConfirm())
                IsValid = false;

            return IsValid;
        }

        #endregion

        private void SaveNewPassword()
        {
            if(_SelectedUser != null)
            {
                _SelectedUser.Password = txtNewPassword.Text.Trim();

                if (_SelectedUser.Save())
                {
                    MessageBox.Show("New Password Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                  
                    this.Close();
                }

                else
                {
                    MessageBox.Show("Failed to Save New Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            FindUser();
            LoadUser();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidateCurrentPassword();
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidateNewtPassword();
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidatePasswordConfirm();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please Enter Valid Valuse!", "Invalid Valuse!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveNewPassword();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
