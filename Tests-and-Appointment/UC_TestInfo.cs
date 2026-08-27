using DVLD.Properties;
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
    public partial class UC_TestInfo : UserControl
    {
        public enum enMode { Schedule, TakeTest}
        public UC_TestInfo()
        {
            InitializeComponent();
        }

        public int localAppID { private get; set; }
        public string D_Class { private get; set; }
        public string FullName {private get; set; }
        public int Trial { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Fees { get; set; }

        public void SetImageAndTitle(Image image, string Title)
        {
            pBTestImage.Image = image;
            laTestTitle.Text = Title;
        }

        public void SetMode(int ModeNumber)
        {
            enMode Mode = (enMode)ModeNumber;

            if(Mode == enMode.Schedule)
            {
                dtpAppointmentDate.Visible = true;
                laDate.Visible = false;
            }
            else
            {
                laDate.Visible = true;
                dtpAppointmentDate.Visible = false;                
            }
        }

        public void SetAppointmentMinDate(DateTime AppointmentDate)
        {
            dtpAppointmentDate.MinDate = AppointmentDate;
        }

        public void SetAppointmentDate(DateTime AppointmentDate)
        {
            dtpAppointmentDate.Value = AppointmentDate;
        }


        public void InitialzeInfo()
        {
            laAppID.Text = localAppID.ToString();
            laClass.Text = D_Class;
            laName.Text = FullName;
            laTrial.Text = Trial.ToString();
            laFees.Text = Fees.ToString();
            AppointmentDate = dtpAppointmentDate.Value;

            if (laDate.Visible == true)
                laDate.Text = AppointmentDate.ToShortDateString();
        }

        private void UC_TestInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void dtpAppointmentDate_ValueChanged(object sender, EventArgs e)
        {
            AppointmentDate = (DateTime)dtpAppointmentDate.Value;
        }
    }
}
