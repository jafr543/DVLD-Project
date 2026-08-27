using DVLD_BLL;
using DVLD_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        enum enFilters
        {
            None,
            PersonID,
            NationalNo,
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            Nationality,
            Gendor,
            Phone,
            Email
        }
        private string _SelectedColumn = string.Empty;
        DataTable dtAllPeople;

        public void LoadPeopleData()
        {
            dtAllPeople = clsPeople.PeopleList();
            dgvShowPeople.DataSource = dtAllPeople;

            if (dgvShowPeople.ColumnCount > 0)
            {
                if (dgvShowPeople.Columns.Contains("PersonID"))
                    dgvShowPeople.Columns["PersonID"].FillWeight = 60;

                if (dgvShowPeople.Columns.Contains("Gendor"))
                    dgvShowPeople.Columns["Gendor"].FillWeight = 60;

                if (dgvShowPeople.Columns.Contains("Email"))
                    dgvShowPeople.Columns["Email"].FillWeight = 160;

                laRecords.Text = dtAllPeople.Rows.Count.ToString();
            }
        }

        private void UC_PeopleMange_Load(object sender, EventArgs e)
        {
            LoadPeopleData();
            cBFilter.SelectedIndex = 0;
        }

        private void ShowtxtFilter()
        {
            if (_SelectedColumn == string.Empty)
                txtFilterBy.Visible = false;
            else
                txtFilterBy.Visible = true;
        }

        private void InitializeFilter()
        {
            enFilters Filters = (enFilters)cBFilter.SelectedIndex;

            switch (Filters)
            {
                case enFilters.None:
                    _SelectedColumn = string.Empty;
                    dtAllPeople.DefaultView.RowFilter = "";
                    break;


                case enFilters.PersonID:
                    _SelectedColumn = "PersonID";
                    break;


                case enFilters.NationalNo:
                    _SelectedColumn = "NationalNo";
                    break;


                case enFilters.FirstName:
                    _SelectedColumn = "FirstName";
                    break;


                case enFilters.SecondName:
                    _SelectedColumn = "SecondName";
                    break;


                case enFilters.ThirdName:
                    _SelectedColumn = "ThirdName";
                    break;


                case enFilters.LastName:
                    _SelectedColumn = "LastName";

                    break;

                case enFilters.Nationality:
                    _SelectedColumn = "Nationality";

                    break;


                case enFilters.Gendor:
                    _SelectedColumn = "Gendor";

                    break;


                case enFilters.Phone:
                    _SelectedColumn = "Phone";

                    break;


                case enFilters.Email:
                    _SelectedColumn = "Email";

                    break;
            }

            ShowtxtFilter();

        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_SelectedColumn) || string.IsNullOrWhiteSpace(txtFilterBy.Text.Trim()))
            {
                dtAllPeople.DefaultView.RowFilter = "";
                return;
            }

            string Filtertext = txtFilterBy.Text.Trim();

            if (_SelectedColumn == "PersonID")
            {
                if (int.TryParse(txtFilterBy.Text, out int ID))
                {
                    dtAllPeople.DefaultView.RowFilter = $"PersonID = {ID}";
                }
            }
            else
            {
                dtAllPeople.DefaultView.RowFilter = $"{_SelectedColumn} Like '{Filtertext}%'";               
            }           
        }

        private void DeletePerson(int PersonID)
        {
            if (!clsPeople.isPersonExist(PersonID))
            {
                MessageBox.Show("Person Not Found!", "Found Filed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ImagePath;

            if (clsPeople.Delete(PersonID, out ImagePath))
            {
                LoadPeopleData();
                MessageBox.Show("Person Deleted Successfully!", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!string.IsNullOrWhiteSpace(ImagePath))
                    File.Delete(ImagePath);
            }
            else
            {
                MessageBox.Show("Cannot delete this person because it is linked to other records.", "Delete Filed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            LoadPeopleData();
            cBFilter.SelectedIndex = 0;
        }

        private void cBFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            InitializeFilter();
            txtFilterBy.Text = string.Empty;
        }

        private void txtFilterBy_TextChanged_1(object sender, EventArgs e)
        {
            ApplyFilter();
            if (_SelectedColumn == "PersonID")
            {
                txtFilterBy.Text = System.Text.RegularExpressions.Regex.Replace(txtFilterBy.Text, @"[^\d]", "");
            }
        }

        private void pBAddnewPerson_Click(object sender, EventArgs e)
        {
            frmEditAddPerson addPerson = new frmEditAddPerson(-1);

            addPerson.ShowDialog();
            if (addPerson.IsSaved)
            {
                LoadPeopleData();
            }
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_SelectedColumn == "PersonID" || _SelectedColumn == "Phone")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void ShowDetailsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmPersonDetails personDetails = new frmPersonDetails((int)dgvShowPeople.CurrentRow.Cells[0].Value);

            personDetails.ShowDialog();

            if (personDetails.isSaved)
                LoadPeopleData();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditAddPerson addPerson = new frmEditAddPerson(-1);

            addPerson.ShowDialog();

            if (addPerson.IsSaved)
            {
                LoadPeopleData();
            }
        }

        private void editToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmEditAddPerson EditPerson = new frmEditAddPerson((int)dgvShowPeople.CurrentRow.Cells[0].Value);

            EditPerson.ShowDialog();

            if (EditPerson.IsSaved)
            {
                LoadPeopleData();
            }
        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Person Will Be Deleted!", "Warning",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DeletePerson((int)dgvShowPeople.CurrentRow.Cells[0].Value);
            }
        }

        private void sendEmailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("This Featur Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void phoneCallToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("This Featur Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cMSEditAddDeletePerson_Opening_1(object sender, CancelEventArgs e)
        {
            if (clsDGV_Validation.IsDGVEmpty_Or_SelectedRowNull(dgvShowPeople.RowCount, dgvShowPeople.CurrentRow))
                e.Cancel = true;
        }

        private void btClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
