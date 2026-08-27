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
    public partial class frmLicensesHistory : Form
    {
        int _PersonID = 0;

        public frmLicensesHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmLicensesHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != 0)
            {
                uC_ShowPersonDetails1.FindPerson(_PersonID);
                uC_DriverLicenses1.LoadLicensesInfo(_PersonID);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
