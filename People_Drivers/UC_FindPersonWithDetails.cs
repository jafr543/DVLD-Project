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
    public partial class UC_FindPersonWithDetails : UserControl
    {
        public UC_FindPersonWithDetails()
        {
            InitializeComponent();
        }

        public clsPeople SelectedPerson {  get; private set; }

        public event Action<clsPeople> PersonFound;

        protected virtual void PersonisFound(clsPeople Person)
        {
            Action<clsPeople> handler = PersonFound;
            if(handler != null)
            {
                handler(Person);
            }
        }

        private void uC_FindPerson1_SearchCompleted(DVLD_BLL.clsPeople obj)
        {
            if(obj != null)
            {
                uC_ShowPersonDetails1.LoadFoundedPerson(obj);
                SelectedPerson = obj;

                if(PersonFound != null)
                   PersonisFound(obj);
                
            }
            else
            {
                uC_ShowPersonDetails1.ResetPersonInfo();
                SelectedPerson = null;

                if (PersonFound != null)
                    PersonisFound(null);
            }
        }
    }
}
