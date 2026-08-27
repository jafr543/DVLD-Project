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
    public partial class frmEditTestType : Form
    {
        private int _ID = 0;
        private clsTestTypes _TestType;

        public bool IsSaved = false;
        public frmEditTestType(int ID)
        {
            InitializeComponent();
            _ID = ID;
        }

        private void FindTestType()
        {
            _TestType = clsTestTypes.Find(_ID);
            if (_TestType != null)
              LoadTestTypeInfo();
        }

        private void LoadTestTypeInfo()
        {
                laID.Text = _TestType.ID.ToString();
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = Convert.ToInt32(_TestType.Fees).ToString();
        }

        private bool ValidateTitle()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorProvider1.SetError(txtTitle, "Title is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtTitle, "");

            return true;
        }

        private bool ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "Description is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtDescription, "");

            return true;
        }

        private bool ValidateFees()
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                errorProvider1.SetError(txtFees, "Fees is Empty!");
                return false;
            }
            else
                errorProvider1.SetError(txtFees, "");

            return true;
        }

        private bool ValidateTestTypeInfo()
        {
            bool IsValid = true;

            if (!ValidateTitle())
                IsValid = false;

            if(!ValidateDescription())
                IsValid = false;

            if (!ValidateFees())
                IsValid = false;

            return IsValid;
        }

        private void SaveTestType()
        {
            if (!ValidateTestTypeInfo())
            {
                MessageBox.Show("Please Enter Valid Valuse!", "Invalid Valuse!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestType.Title = txtTitle.Text.Trim();
            _TestType.Description = txtDescription.Text.Trim();
            _TestType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if (_TestType.Save())
            {
                MessageBox.Show("Test Update Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Failed to Save Test Update", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            FindTestType();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveTestType();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            ValidateTitle();
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            ValidateDescription();
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            ValidateFees();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
