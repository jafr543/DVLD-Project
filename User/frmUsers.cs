using DVLD_BLL;
using DVLD_Utilities;
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
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        private DataTable dtUsers;
        private string _SelectedColumn = string.Empty;

        enum enFilter
        {
            None,
            UserID,
            PersonID,
            FullName,
            UserName,
            IsActive
        }
        enum enActivesFilter
        {
            All,
            Actives,
            InActives
        }

        private void InitializeFilter()
        {
            enFilter Filter = (enFilter)cBFilters.SelectedIndex;

            switch(Filter)
            {
                    case enFilter.None:
                    txtFilter.Visible = false;
                    cBActivesFilter.Visible = false;
                    _SelectedColumn = string.Empty;
                    dtUsers.DefaultView.RowFilter = "";
                    break;

                    case enFilter.UserID:
                    txtFilter.Visible = true;
                    cBActivesFilter.Visible = false;
                    _SelectedColumn = "UserID";
                    break;

                    case enFilter.PersonID:
                    txtFilter.Visible = true;
                    cBActivesFilter.Visible = false;
                    _SelectedColumn = "PersonID";
                    break;

                    case enFilter.FullName:
                    txtFilter.Visible = true;
                    cBActivesFilter.Visible = false;
                    _SelectedColumn = "FullName";
                    break;

                    case enFilter.UserName:
                    txtFilter.Visible = true;
                    cBActivesFilter.Visible = false;
                    _SelectedColumn = "UserName";
                    break;

                    case enFilter.IsActive:
                    txtFilter.Visible = false;
                    cBActivesFilter.Visible = true;
                    _SelectedColumn = "IsActive";
                    break;
            }
        }

        private void InitializeActivesFilter()
        {
            enActivesFilter activesFilter = (enActivesFilter)cBActivesFilter.SelectedIndex;

            switch(activesFilter)
            {
                    case enActivesFilter.All:
                    dtUsers.DefaultView.RowFilter = "";
                    break;

                    case enActivesFilter.Actives:
                    dtUsers.DefaultView.RowFilter = "IsActive = true";
                    break;

                    case enActivesFilter.InActives:
                    dtUsers.DefaultView.RowFilter = "IsActive = false";
                    break;
            }
        }

        private void ApplyFilter()
        {
            if(string.IsNullOrWhiteSpace(_SelectedColumn) ||string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dtUsers.DefaultView.RowFilter = "";
                return;
            }

            string Filtertext = txtFilter.Text.Trim();
            
            if( _SelectedColumn == "UserID")
            {
                if (int.TryParse(txtFilter.Text, out int ID))
                {
                    dtUsers.DefaultView.RowFilter = $"UserID = {ID}";
                }
            }

            if (_SelectedColumn == "PersonID")
            {
                if (int.TryParse(txtFilter.Text, out int ID))
                {
                    dtUsers.DefaultView.RowFilter = $"PersonID = {ID}";
                }
            }

            if(_SelectedColumn == "FullName")
            {
                dtUsers.DefaultView.RowFilter = $"FullName Like '{Filtertext}%'";
            }

            if(_SelectedColumn == "UserName")
            {
                dtUsers.DefaultView.RowFilter = $"UserName Like '{Filtertext}%'";
            }

        }

        private void LoadUsers()
        {
            dtUsers = clsUser.GetallUsers();
            dgvUsers.DataSource = dtUsers;
            
            if(dgvUsers.RowCount > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].FillWeight = 110;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].FillWeight = 110;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].FillWeight = 350;

                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].FillWeight = 120;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].FillWeight = 110;

                 laRecords.Text = dgvUsers.RowCount.ToString();
            }
        }

        private void DeleteUser(int UserID)
        {
            if(frmLogin.CurrentUser.UserID != UserID)
            {
                 if(clsUser.Delete(UserID))
                 {
                     MessageBox.Show("User Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     LoadUsers();
                 }
                 else
                 {
                     MessageBox.Show("User Deleted Faild Because its Conacted to Other Data", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
            }

            else
            {             
               MessageBox.Show("You cannot delete the currently logged-in user.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            LoadUsers();
            cBFilters.SelectedIndex = 0;
            cBActivesFilter.SelectedIndex = 0;
        }

        private void cBFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeFilter();
            txtFilter.Text = string.Empty;
            laRecords.Text = dgvUsers.RowCount.ToString();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
            laRecords.Text = dgvUsers.RowCount.ToString();
        }

        private void cBActivesFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeActivesFilter();
            laRecords.Text = dgvUsers.RowCount.ToString();
        }

        private void pBaddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser AddEditUser = new frmAddEditUser();

            AddEditUser.ShowDialog();
            if(AddEditUser.IsSaved)
            {
                LoadUsers();
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails User = new frmUserDetails((int)dgvUsers.CurrentRow.Cells[0].Value);

            User.ShowDialog();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser newUser = new frmAddEditUser();
            newUser.ShowDialog();

            if (newUser.IsSaved)
                LoadUsers();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddEditUser EditUser = new frmAddEditUser((int)dgvUsers.CurrentRow.Cells[0].Value);

            EditUser.ShowDialog();

            if(EditUser.IsSaved)
            {
                LoadUsers();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
           DialogResult Result = MessageBox.Show("this User will be Deleted are Sure to Continue?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            
            if(Result == DialogResult.OK)
            {
                DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            }
        }

        private void chengPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);

            changePassword.ShowDialog();
        }

        private void cMSEditAddDeleteUser_Opening(object sender, CancelEventArgs e)
        {
            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvUsers.RowCount, dgvUsers.CurrentRow))
                e.Cancel = true;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(_SelectedColumn == "UserID" ||  _SelectedColumn == "PersonID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }
    }
}
