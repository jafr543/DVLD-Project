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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }


        private void LoadApplicationsTypes()
        {
            
            dgvApplicationsTypes.DataSource = clsApplicationTypes.GetallApplicationTypes();

            if(dgvApplicationsTypes.ColumnCount > 0 )
            {
                if (dgvApplicationsTypes.Columns.Contains("ID"))
                    dgvApplicationsTypes.Columns["ID"].FillWeight = 60;

                if (dgvApplicationsTypes.Columns.Contains("Title"))
                    dgvApplicationsTypes.Columns["Title"].FillWeight = 260;

                if (dgvApplicationsTypes.Columns.Contains("Fees"))
                    dgvApplicationsTypes.Columns["Fees"].FillWeight = 60;

                laRecords.Text = dgvApplicationsTypes.Rows.Count.ToString();
            }
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            LoadApplicationsTypes();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType editApplicationType = new frmEditApplicationType
                ((int)dgvApplicationsTypes.CurrentRow.Cells[0].Value);

            editApplicationType.ShowDialog();

            if(editApplicationType.IsSaved)
            {
                LoadApplicationsTypes();
            }
        }
    }
}
