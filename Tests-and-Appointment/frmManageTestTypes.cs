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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }


        private void LoadTestTypes()
        {

            dgvTestTypes.DataSource = clsTestTypes.GetallTestTypes();

            if(dgvTestTypes.ColumnCount > 0 )
            {
                if (dgvTestTypes.Columns.Contains("ID"))
                    dgvTestTypes.Columns["ID"].FillWeight = 40;

                if (dgvTestTypes.Columns.Contains("Title"))
                    dgvTestTypes.Columns["Title"].FillWeight = 80;

                if (dgvTestTypes.Columns.Contains("Description"))
                    dgvTestTypes.Columns["Description"].FillWeight = 260;

                if (dgvTestTypes.Columns.Contains("Fees"))
                    dgvTestTypes.Columns["Fees"].FillWeight = 40;
            }

            laRecords.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            LoadTestTypes();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType editTestType = new frmEditTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value);

            editTestType.ShowDialog();

            if(editTestType.IsSaved)
            {
                LoadTestTypes();
            }
        }
    }
}
