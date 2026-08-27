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
    public partial class frmShow_Int_LicenseInfo : Form
    {
        int _LicenseID = 0;
        public frmShow_Int_LicenseInfo(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }

        private void frmShow_Int_LicenseInfo_Load(object sender, EventArgs e)
        {
            uC_Int_LicenseInfo1.FindLicense(_LicenseID);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
