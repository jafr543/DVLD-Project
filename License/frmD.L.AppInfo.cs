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
    public partial class frmApplicationInfo : Form
    {
        int _ID = 0;
        public frmApplicationInfo(int ID)
        {
            InitializeComponent();
            _ID = ID;
        }

        private void frmApplicationInfo_Load(object sender, EventArgs e)
        {
            uC_FullAppInfo1.LoadInfo(_ID);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
