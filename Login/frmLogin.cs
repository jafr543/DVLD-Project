using DVLD_BLL;
using DVLD_Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public static clsUser CurrentUser;

        private bool ValidateUserNameAndPassword()
        {
            bool isValid = true;

            if(string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "User Name is Empty!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
            }

            if(string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is Empty!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }

            return isValid;
        }

        private void FindUser()
        {
            if(!ValidateUserNameAndPassword())
            {
                MessageBox.Show("User Name or Password are Empty!", "Empty Values!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CurrentUser = clsUser.Find(txtUserName.Text, txtPassword.Text);

            if(CurrentUser != null)
            {
                if(CurrentUser.IsActive)
                {
                   RememberUser();
                    this.Hide();
                   frmMainForm frmMainForm = new frmMainForm();
                   frmMainForm.ShowDialog();
                    this.Visible = true;

                    if (!cBRememberMe.Checked)
                    {
                      txtUserName.Clear();
                      txtPassword.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Your account is inactive. Please Contact the administrator", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  
            }
            else
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void RememberUser()
        {
            if (cBRememberMe.Checked)
            {
                clsRememberMe.SaveUserNameAndPassword(txtUserName.Text + "|" + txtPassword.Text);
            }
            else
            {
                clsRememberMe.DeleteUserLoginRecord();
            }
        }

        private void LoadUserLoginInfo()
        {
            string UserName;
            string Password;

            if(clsRememberMe.Load(out UserName, out Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            LoadUserLoginInfo();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            FindUser();
        }

        private void pBClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btLogin_MouseEnter(object sender, EventArgs e)
        {
            btLogin.ForeColor = Color.White;
        }

        private void btLogin_MouseLeave(object sender, EventArgs e)
        {
            btLogin.ForeColor = Color.Cyan;
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btLogin.PerformClick();

                e.SuppressKeyPress = true;
            }
        }
    }
}
