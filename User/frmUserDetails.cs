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
    public partial class frmUserDetails : Form
    {
        private clsPeople _Person;
        private clsUser _User;
        private int _ID;

        public frmUserDetails(int UserID)
        {
            InitializeComponent();

            _ID = UserID;
        }

        private void FindUserandPerson()
        {
            _User = clsUser.Find(_ID);

            if (_User != null)
            {
                _Person = _User.PersonInfo;
            }
            else
            {
                MessageBox.Show("User or Person Are Not Found", "Faild Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadPersonInfo()
        {
            uC_ShowPersonDetails1.LoadFoundedPerson(_Person);
        }

        private void LoadUserInfo()
        {
            uC_ShowUserDetails1.LoadUserInfo(_User);
        }

        private void LoadInfo()
        {
            if(_User != null && _Person != null)
            {
                LoadPersonInfo();
                LoadUserInfo();
            }
            
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            FindUserandPerson();
            LoadInfo();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
