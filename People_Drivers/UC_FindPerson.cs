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
    public partial class UC_FindPerson : UserControl
    {
        public UC_FindPerson()
        {
            InitializeComponent();
        }

        enum enFilter
        {
            PersonID,
            NationalNo
        }

        clsPeople Person;

        public event Action<clsPeople> SearchCompleted;

        protected virtual void SendPerson(clsPeople Person)
        {
            Action<clsPeople> handler = SearchCompleted;

            if(handler != null)
            {
                handler(Person);
            }
        }

        private bool ValidateInputs()
        {
            if(string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                errorProvider1.SetError(txtSearch, "Empty Inputs!");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtSearch, "");
                return true;
            }
        }

        private void FindByPersonID()
        {
            if (int.TryParse(txtSearch.Text, out int ID))
            {
                    Person = clsPeople.Find(ID);

                if(Person != null)
                {
                    if (SearchCompleted != null)
                        SendPerson(Person);
                }
                else
                {
                    MessageBox.Show("Person Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (SearchCompleted != null)
                        SendPerson(null);
                }
            }
        }

        private void FindByNationalNo()
        {
            Person = clsPeople.Find(txtSearch.Text);

            if(Person != null)
            {
                if (SearchCompleted != null)
                    SendPerson(Person);
            }
            else
            {
                MessageBox.Show("Person Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (SearchCompleted != null)
                    SendPerson(null);
            }
        }

        private void InitializeFilter()
        {
            if(enFilter.PersonID == (enFilter)cBFilter.SelectedIndex)
            {
                FindByPersonID();
            }
            
            if(enFilter.NationalNo == (enFilter)cBFilter.SelectedIndex)
            {
                FindByNationalNo();
            }
        }

        private void pBFindPerson_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("You Cant Search with Empty Inputs!", "Empty Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InitializeFilter();
        }

        private void pBAddPerson_Click(object sender, EventArgs e)
        {
            frmEditAddPerson frmAdd = new frmEditAddPerson(-1);

            frmAdd.ShowDialog();
        }

        private void UC_FindPerson_Load(object sender, EventArgs e)
        {
            cBFilter.SelectedIndex = 0;
            txtSearch.Focus();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (!ValidateInputs())
                {
                    MessageBox.Show("You Cant Search with Empty Inputs!", "Empty Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                InitializeFilter();

            }
            

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((enFilter)cBFilter.SelectedIndex == enFilter.PersonID)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }
    }
}
