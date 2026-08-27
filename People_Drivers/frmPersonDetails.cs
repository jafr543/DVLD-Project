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
    public partial class frmPersonDetails : Form
    {
        public bool isSaved = false;

        public frmPersonDetails(int ID)
        {
            InitializeComponent();

            uC_ShowPersonDetails1.FindPerson(ID);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uC_ShowPersonDetails1_UpdatedRequest(bool obj)
        {
            isSaved = obj;
        }
    }
}
