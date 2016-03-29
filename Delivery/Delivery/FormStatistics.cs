using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Delivery
{
    public partial class FormStatistics : Form
    {
        MySqlConnection ConnectionToMySQL;
        Form mainForm;

        List<Double> materialTonnComplete = new List<Double>();
        List<Double> materialTonnCancel = new List<Double>();
        List<Double> materialTonnActive = new List<Double>();
        List<Double> materialTonnInactive = new List<Double>();

        public FormStatistics(MySqlConnection connection, Form form)
        {
            ConnectionToMySQL = connection;
            mainForm = form;
            InitializeComponent();
        }

        private void FormStatistics_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Material". При необходимости она может быть перемещена или удалена.
            this.materialTableAdapter.Fill(this.testDataSet.Material);

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            ConnectionToMySQL.Close();
            mainForm.Show();
            this.Close();
        }

        private void FormStatistics_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionToMySQL.Close();
            mainForm.Show();
        }

        public String getStatusPk(String statusDesc)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_status FROM `order_status` WHERE `name_status`  = '" + statusDesc + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String statusPk = null;
            while (dataReader.Read())
            {
                statusPk = dataReader[0].ToString();
            }
            dataReader.Close();
            return statusPk;
        }

        public String getMesurePk(String measureDesc)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_measure FROM `Measure` WHERE `Nazv`  = '" + measureDesc + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String measurePk = null;
            while (dataReader.Read())
            {
                measurePk = dataReader[0].ToString();
            }
            dataReader.Close();
            return measurePk;
        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date > dateTimePickerOn.Value.Date)
            {
                MessageBox.Show("Задан неверный период времени.");
            }
            else
            {
                List<String> material = new List<String>();
                

                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT *  FROM `Material`";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();

                materialTonnComplete.Clear();
                materialTonnCancel.Clear();
                materialTonnActive.Clear();
                materialTonnInactive.Clear();

                while (dataReader.Read())
                {
                    material.Add(dataReader[0].ToString());
                    materialTonnComplete.Add(0);
                    materialTonnCancel.Add(0);
                    materialTonnActive.Add(0);
                    materialTonnInactive.Add(0);
                }
                dataReader.Close();

                String date = null;
                String statusPk = null;
                String completeStatus = getStatusPk("Complete");
                String cancelStatus = getStatusPk("Cancel");
                String activeStatus = getStatusPk("Active");
                String inactiveStatus = getStatusPk("Inactive");

                String bulkMeasure = getMesurePk("Bulk");
                String bagMeasure = getMesurePk("Bag");

                int count = 0;
                int countActiveOrder = 0;
                int countInactiveOrder = 0;
                int countCompleteOrder = 0;
                int countCancelOrder = 0;

                int allProfit = 0;

                msc.CommandText = "SELECT *  FROM `Order`";
                msc.Connection = ConnectionToMySQL;
                dataReader = msc.ExecuteReader();

                while (dataReader.Read())
                {  
                    date = dataReader[3].ToString();
                    String []dateSplit = date.Split('.',' ');
                    DateTime dateOrder = new DateTime(Convert.ToInt32(dateSplit[2]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[0]));
                    if (dateOrder.Date >= dateTimePickerFrom.Value.Date && dateOrder.Date <= dateTimePickerOn.Value.Date)
                    {
                        count++;

                        statusPk = dataReader[12].ToString();
                        String materialPk = dataReader[13].ToString();
                        int index = material.IndexOf(materialPk);
                        String volume = dataReader[2].ToString();
                        String measureOrder = dataReader[14].ToString();
                        if (statusPk == completeStatus)
                        {
                            countCompleteOrder++;

                            String costOrder = dataReader[11].ToString();
                            int cost = Convert.ToInt32(costOrder);
                            allProfit += (cost / 115) * 15;

                            double tonn = materialTonnComplete[index];
                            if (measureOrder == bagMeasure)
                            {
                                tonn += Convert.ToDouble(volume) * 0.05;
                            }
                            else
                            {
                                tonn += Convert.ToDouble(volume);
                            }
                            materialTonnComplete.RemoveAt(index);
                            materialTonnComplete.Insert(index,tonn);
                        }
                        if (statusPk == cancelStatus)
                        {
                            countCancelOrder++;

                            double tonn = materialTonnCancel[index];
                            if (measureOrder == bagMeasure)
                            {
                                tonn += Convert.ToDouble(volume) * 0.05;
                            }
                            else
                            {
                                tonn += Convert.ToDouble(volume);
                            }
                            materialTonnCancel.RemoveAt(index);
                            materialTonnCancel.Insert(index, tonn);
                        }
                        if (statusPk == activeStatus)
                        {
                            countActiveOrder++;

                            double tonn = materialTonnActive[index];
                            if (measureOrder == bagMeasure)
                            {
                                tonn += Convert.ToDouble(volume) * 0.05;
                            }
                            else
                            {
                                tonn += Convert.ToDouble(volume);
                            }
                            materialTonnActive.RemoveAt(index);
                            materialTonnActive.Insert(index, tonn);
                        }
                        if (statusPk == inactiveStatus)
                        {
                            countInactiveOrder++;
                            
                            double tonn = materialTonnInactive[index];
                            if (measureOrder == bagMeasure)
                            {
                                tonn += Convert.ToDouble(volume) * 0.05;
                            }
                            else
                            {
                                tonn += Convert.ToDouble(volume);
                            }
                            materialTonnInactive.RemoveAt(index);
                            materialTonnInactive.Insert(index, tonn);
                        }
                    }
                }
                dataReader.Close();

                int i = 0;
                foreach (double tonn in materialTonnComplete)
                {
                    dataGridView1[1, i].Value = tonn.ToString();
                    i++;
                }

                textBoxAllProfit.Text = allProfit.ToString();

                labelAllOrder.Text = count.ToString();
                labelActiveOrder.Text = countActiveOrder.ToString();
                labelInactive.Text = countInactiveOrder.ToString();
                labelCompleteOrder.Text = countCompleteOrder.ToString();
                labelCancelOrder.Text = countCancelOrder.ToString();

                MessageBox.Show("Отчет составлен. "+ count.ToString());
            }
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            foreach (double tonn in materialTonnComplete)
            {
                dataGridView1[1, i].Value = tonn.ToString();
                i++;
            }
        }

        private void dataGridView2_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            foreach (double tonn in materialTonnCancel)
            {
                dataGridView2[1, i].Value = tonn.ToString();
                i++;
            }
        }

        private void dataGridView3_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            foreach (double tonn in materialTonnActive)
            {
                dataGridView3[1, i].Value = tonn.ToString();
                i++;
            }
        }

        private void dataGridView4_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            foreach (double tonn in materialTonnInactive)
            {
                dataGridView4[1, i].Value = tonn.ToString();
                i++;
            }
        }
    }
}
