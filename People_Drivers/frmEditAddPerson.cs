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
    public partial class frmEditAddPerson : Form
    {
        private int _ID = -1;

        public bool IsSaved {  get; set; }

        public frmEditAddPerson(int ID)
        {
            InitializeComponent();

            _ID = ID;
        }

        private void UpdateFormTitle()
        {
            if(_ID != -1)
            {
                laTitle.Text = "Update Person";
                laPersonID.Text = _ID.ToString();
            }

        }

        private void frmEditAddPerson_Load(object sender, EventArgs e)
        {
            uC_EditAddPepole1.InitializeMode(_ID);
            UpdateFormTitle();
        }

        private void uC_EditAddPepole1_CloseRequest(bool obj)
        {
            IsSaved = obj;
            this.Close();
        }
    }
}
