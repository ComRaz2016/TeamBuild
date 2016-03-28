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

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date > dateTimePickerOn.Value.Date)
            {
                MessageBox.Show("Задан неверный период времени.");
            }
            else
            {
                String date = null;
                String statusPk = null;
                String completeStatus = getStatusPk("Complete");
                String cancelStatus = getStatusPk("Cancel");
                String activeStatus = getStatusPk("Active");
                String inactiveStatus = getStatusPk("Inactive");

                int count = 0;
                int countActiveOrder = 0;
                int countInactiveOrder = 0;
                int countCompleteOrder = 0;
                int countCancelOrder = 0;

                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT *  FROM `Order`";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();

                while (dataReader.Read())
                {  
                    date = dataReader[3].ToString();
                    String []dateSplit = date.Split('.',' ');
                    DateTime dateOrder = new DateTime(Convert.ToInt32(dateSplit[2]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[0]));
                    if (dateOrder.Date >= dateTimePickerFrom.Value.Date && dateOrder.Date <= dateTimePickerOn.Value.Date)
                    {
                        count++;

                        statusPk = dataReader[12].ToString();
                        if (statusPk == completeStatus)
                        {
                            countCompleteOrder++;
                        }
                        if (statusPk == cancelStatus)
                        {
                            countCancelOrder++;
                        }
                        if (statusPk == activeStatus)
                        {
                            countActiveOrder++;
                        }
                        if (statusPk == inactiveStatus)
                        {
                            countInactiveOrder++;
                        }
                    }
                }
                dataReader.Close();

                labelAllOrder.Text = count.ToString();
                labelActiveOrder.Text = countActiveOrder.ToString();
                labelInactive.Text = countInactiveOrder.ToString();
                labelCompleteOrder.Text = countCompleteOrder.ToString();
                labelCancelOrder.Text = countCancelOrder.ToString();

                MessageBox.Show("Отчет составлен. "+ count.ToString());
            }
        }
    }
}
