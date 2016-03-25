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
    public partial class FormProviderMaterial : Form
    {
        public FormProviderMaterial()
        {
            InitializeComponent();
        }

        private void FormProviderMaterial_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Provider". При необходимости она может быть перемещена или удалена.
            this.providerTableAdapter.Fill(this.testDataSet.Provider);

        }

        private void buttonProviderDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
