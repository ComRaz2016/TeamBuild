using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delivery
{
    public partial class FormDriverTS : Form
    {
        public FormDriverTS()
        {
            InitializeComponent();
        }

        private void FormDriverTS_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Car". При необходимости она может быть перемещена или удалена.
            this.carTableAdapter.Fill(this.testDataSet.Car);

        }
    }
}
