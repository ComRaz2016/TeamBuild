using MySql.Data.MySqlClient;
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
    public partial class FormOrders : Form
    {
        private MySqlConnection ConnectionToMySQL;
        private Form mainForm;

        public FormOrders(MySqlConnection connection, Form form)
        {
            ConnectionToMySQL = connection;
            mainForm = form;
            InitializeComponent();
        }

        private void FormOrders_Load(object sender, EventArgs e)
        {
            tabPageActive.Select();
            setDataGridActivOrder();
        }



        //метод хаполняет таблицу активными заказами
        private void setDataGridActivOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE date_time LIKE " + '"' + DateTime.Today.ToString("dd.MM.yyyy") + "%" + '"';
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                int i = dataGridView1.NewRowIndex;
                dataGridView1.Rows[i].Cells[0].Value = dataReader[1].ToString();
                dataGridView1.Rows[i].Cells[1].Value = "Ожидает доставки";
                dataGridView1.Rows[i].Cells[2].Value = dataReader[4].ToString();
                dataGridView1.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(dataReader[3].ToString().IndexOf(" "));
                dataGridView1.Rows[i].Cells[4].Value = dataReader[11].ToString();
                dataGridView1.Rows[i].Cells[5].Value = dataReader[2].ToString();
                dataGridView1.Rows[i].Cells[6].Value = dataReader[14].ToString();
            }
            dataReader.Close();
            /*msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE date_time LIKE " + '"' + DateTime.Today.AddDays(1).ToString("dd.MM.yyyy") + "%" + '"';
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();*/
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            setDataGridActivOrder();
        }

        private void FormOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionToMySQL.Close();
            mainForm.Show();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            setDataGridActivOrder();
        }
    }
}
