using DVLD.Properties;
using DVLD_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class UC_ShowUserDetails : UserControl
    {
        public UC_ShowUserDetails()
        {
            InitializeComponent();
        }

        private void IsActive_Yes_or_No(bool IsActive)
        {
            if (IsActive == true)
            {
                laIsActive.Text = "Yes";
                pBIsActive.Image = Resources.activities;
            }
                
            else
            {
                laIsActive.Text = "No";
                pBIsActive.Image = Resources.InActive;
            }
        }

        public void LoadUserInfo(clsUser User)
        {
            if(User != null)
            {
                laUserID.Text = User.UserID.ToString();
                laUserName.Text = User.UserName;
                IsActive_Yes_or_No(User.IsActive);
                
            }
        }
    }
}
