using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
            setDataGridOtherOrder();
            setDataGridCompleteOrder();
            setDataGridRawOrder();
        }



        //метод хаполняет таблицу активными заказами
        private void setDataGridActivOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE date_time LIKE " + '"' + DateTime.Today.ToString("dd.MM.yyyy") + "%" + '"' + " AND pk_status = 1";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                int i = dataGridView1.NewRowIndex;
                dataGridView1.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                dataGridView1.Rows[i].Cells[1].Value = "Ожидает доставки";          //статус заказа
                dataGridView1.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                dataGridView1.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(dataReader[3].ToString().IndexOf(" "));      //время
                dataGridView1.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                dataGridView1.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                dataGridView1.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
            }
            dataReader.Close();
            inserMaterial(dataGridView1);
        }

        //метод хаполняет таблицу обработанных заказов
        private void setDataGridCompleteOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE pk_status = 7" ;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                int i = dataGridView3.NewRowIndex;
                dataGridView3.Rows[i].Cells[0].Value = dataReader[1].ToString();
                dataGridView3.Rows[i].Cells[1].Value = dataReader[4].ToString();
                dataGridView3.Rows[i].Cells[2].Value = dataReader[3].ToString().Substring(0,dataReader[3].ToString().IndexOf(" "));      //время
                dataGridView1.Rows[i].Cells[3].Value = dataReader[11].ToString();
            }
            dataReader.Close();
        }

        //метод заполняет таблицу не активных заказов
        private void setDataGridOtherOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE date_time NOT LIKE " + '"' + DateTime.Today.ToString("dd.MM.yyyy") + "%" + '"' + " AND pk_status = 1";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            List<String> badOrders = new List<String>();
            while (dataReader.Read())
            {
                if (isGoodDate(dataReader[3].ToString()))
                {
                    int i = dataGridView2.NewRowIndex;
                    dataGridView2.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                    dataGridView2.Rows[i].Cells[1].Value = "Ожидает доставки";          //статус заказа
                    dataGridView2.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                    dataGridView2.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(0,dataReader[3].ToString().IndexOf(" "));      //дата
                    dataGridView2.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                    dataGridView2.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                    dataGridView2.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
                }
                else
                {
                    badOrders.Add(dataReader[0].ToString());    //запоминаем пк плохих заказов
                    /*int i = dataGridView4.NewRowIndex;
                    dataGridView4.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                    dataGridView4.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                    dataGridView4.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(0, dataReader[3].ToString().IndexOf(" "));      //дата
                    dataGridView4.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                    dataGridView4.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                    dataGridView4.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал*/
                }
            }
            dataReader.Close();
            inserMaterial(dataGridView2);
            updateRawOrder(badOrders);
        }

        //просрочные заказы
        private void updateRawOrder(List<string> badOrders)
        {
            MySqlCommand msc = new MySqlCommand();
            foreach (String bad in badOrders)
            {
                msc.CommandText = "UPDATE `Order`  SET `pk_status` = '8` WHERE `pk_order` = '" + bad + "'";
                msc.Connection = ConnectionToMySQL;
                msc.ExecuteNonQuery();
            }
        }

        //заполнение названий материала
        private void inserMaterial(DataGridView table)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Material`";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            List<Tuple<String, String>> materials = new List<Tuple<string, string>>();
            while (dataReader.Read())
            {
                materials.Add(new Tuple<String, String>(dataReader[0].ToString(), dataReader[1].ToString()));
            }
            dataReader.Close();
            foreach(DataGridViewRow row in table.Rows)
            {
                row.Cells[6].Value = materials.Find((Tuple<string,string> t) => t.Item1 == row.Cells[6].Value.ToString()).Item2;    //вот чему учит Scala
            }
        }

        private void setDataGridRawOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE pk_status = 8";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                int i = dataGridView4.NewRowIndex;
                dataGridView4.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                dataGridView4.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                dataGridView4.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(0, dataReader[3].ToString().IndexOf(" "));      //дата
                dataGridView4.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                dataGridView4.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                dataGridView4.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
            }
            dataReader.Close();
        }

        private bool isGoodDate(String dateOrder)
        {
            DateTime dO = DateTime.Parse(dateOrder);
            if (dO > DateTime.Today)
            {
                return true;
            }
            return false;
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
