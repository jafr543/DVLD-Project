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
    public partial class UC_BasicAppInfo : UserControl
    {
        public UC_BasicAppInfo()
        {
            InitializeComponent();
        }

        #region Properties
        public int AppID { get; set; }
        public string Status { get; set; }
        public decimal Fees { get; set; }
        public string AppType { get; set; }
        public int PersonID { get; set; }
        public string Applicant { get; set; }
        public DateTime AppDate { get; set; }
        public DateTime LastStatusUpdate { get; set; }

        #endregion

        clsBasicLoaclApplicationsInfo_View _BasicAppInfo;

        public void FindAppInfo(int ID)
        {
            clsBasicLoaclApplicationsInfo_View BasicAppInfo = clsBasicLoaclApplicationsInfo_View.Find(ID);
            if (BasicAppInfo != null)
            {
                _BasicAppInfo = BasicAppInfo;
                LoadAppInfo();
            }
            
        }

        private void LoadAppInfo()
        {
            laID.Text = _BasicAppInfo.AppID.ToString();
            laStatus.Text = _BasicAppInfo.Status;
            laFees.Text = _BasicAppInfo.Fees.ToString();
            laType.Text = _BasicAppInfo.AppType;
            laApplicant.Text = _BasicAppInfo.Applicant;
            laDate.Text = _BasicAppInfo.AppDate.ToShortDateString();
            laLastStatusUpdate.Text = _BasicAppInfo.LastStatusUpdate.ToShortDateString();
            laCreatedBy.Text = _BasicAppInfo.UserName;

            this.AppID = _BasicAppInfo.AppID;
            this.Status = _BasicAppInfo.Status;
            this.Fees = _BasicAppInfo.Fees;
            this.AppType = _BasicAppInfo.AppType;
            this.PersonID = _BasicAppInfo.PersonID;
            this.Applicant = _BasicAppInfo.Applicant;
            this.AppDate = _BasicAppInfo.AppDate;
            this.LastStatusUpdate = _BasicAppInfo.LastStatusUpdate;
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails PersonDetails = new frmPersonDetails(_BasicAppInfo.PersonID);

                PersonDetails.ShowDialog();
                      
        }
    }
}
