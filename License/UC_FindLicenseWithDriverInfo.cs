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
    public partial class UC_FindLicenseWithDriverInfo : UserControl
    {
        public UC_FindLicenseWithDriverInfo()
        {
            InitializeComponent();
        }


        public clsLicenses LicenseInfo { get; private set;}

        public event Action<clsLicenses> LicenseFound;

        protected virtual void SendLicenseInfo(clsLicenses LicenseInfo)
        {
            Action<clsLicenses> handler = LicenseFound;

            if(handler != null)
            {
                handler(LicenseInfo);
            }
        }

        private void uC_FindLicense1_SearchCompleted(DVLD_BLL.clsLicenses obj)
        {
            if(obj != null)
            {
                uC_LicenseInfo1.LoadFoundLicense(obj);
                LicenseInfo = obj;
                if(LicenseFound != null)
                    SendLicenseInfo(obj);
            }
            
            else
            {
                uC_LicenseInfo1.ResetLicenseInfo();
                LicenseInfo = null;
                if (LicenseFound != null)
                    SendLicenseInfo(null);
            }
        }

        public void Pre_Search(int LicenseID)
        {
            if(LicenseID != 0)
            {
                uC_FindLicense1.Pre_Search(LicenseID);
                uC_FindLicense1.Enabled = false;
            }
        }
    }
}
