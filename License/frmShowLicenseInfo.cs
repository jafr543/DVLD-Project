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
    public partial class frmShowLicenseInfo : Form
    {
        int _LicenseID = 0;

        public frmShowLicenseInfo(int LicenseID = 0)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            if(_LicenseID != 0)
                uC_LicenseInfo1.LoadLicenseInfo(_LicenseID);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
