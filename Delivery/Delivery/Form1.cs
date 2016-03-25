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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Driver". При необходимости она может быть перемещена или удалена.
            this.driverTableAdapter.Fill(this.testDataSet.Driver);

        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            Form f3 = new Form3();
            f3.Show();
        }

        private void buttonProviderMaterial_Click(object sender, EventArgs e)
        {
            Form fProviderMaterial = new FormProviderMaterial();
            fProviderMaterial.Show();
        }

        private void buttonDriverTS_Click(object sender, EventArgs e)
        {

        }

        private void buttonStatistics_Click(object sender, EventArgs e)
        {

        }

        private void buttonObserverOrder_Click(object sender, EventArgs e)
        {

        }

        private void buttonCheckCost_Click(object sender, EventArgs e)
        {

        }
    }
}
