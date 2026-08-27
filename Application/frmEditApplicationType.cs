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
    public partial class frmEditApplicationType : Form
    {
        private int _ID;
        private clsApplicationTypes _ApplicationType;

        public bool IsSaved = false;

        public frmEditApplicationType(int ID)
        {
            InitializeComponent();
            _ID = ID;

            
        }

       private void FindApplicationType()
       {
            _ApplicationType = clsApplicationTypes.Find(_ID);

            if(_ApplicationType != null)
            {
                LoadApplicationType();
            }
       }

        private void LoadApplicationType()
        {       
                laID.Text = _ID.ToString();
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = Convert.ToInt32(_ApplicationType.Fees).ToString();    
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

        private bool ValidateApplicationTypeInfo()
        {
            bool IsValid = true;

            if(!ValidateTitle())
                IsValid = false;

            if(!ValidateFees())
                IsValid = false;

            return IsValid;
        }

        private void SaveApplicationType()
        {
            if(!ValidateApplicationTypeInfo())
            {
                MessageBox.Show("Please Enter Valid Valuse!", "Invalid Valuse!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ApplicationType.Title = txtTitle.Text.Trim();
            _ApplicationType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Appication Update Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsSaved = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Failed to Save Appication Update", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            FindApplicationType();            
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            ValidateTitle();
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            ValidateFees();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveApplicationType();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
