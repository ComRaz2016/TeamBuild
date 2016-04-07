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
            buttonEdit1.Enabled = false;
            setDataGridActivOrder();
            setDataGridOtherOrder();
            setDataGridCompleteOrder();
            setDataGridRawOrder();
        }

        private void FormOrders_Load(object sender, EventArgs e)
        {
            
        }



        //метод хаполняет таблицу активными заказами
        private void setDataGridActivOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE date_time LIKE " + '"' + DateTime.Today.ToString("dd.MM.yyyy") + "%" + '"' + " OR pk_status = 2 OR pk_status = 3 OR pk_status = 4 OR pk_status = 5";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                dataGridView1.Rows[i].Cells[1].Value = getStringStatus(dataReader[12].ToString());          //статус заказа
                dataGridView1.Rows[i].Cells[1].Style.BackColor = getColorCell(dataReader[12].ToString());
                dataGridView1.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                dataGridView1.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(dataReader[3].ToString().IndexOf(" "));      //время
                dataGridView1.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                dataGridView1.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                dataGridView1.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
                i++;
            }
            dataReader.Close();
            inserMaterial(dataGridView1);
        }

        //кусок бд в проге))
        private String getStringStatus(String pk)
        {
            switch (pk)
            {
                case "1":
                    return "Ожидает доставки";
                case "2":
                    return "Едет на загрузку";
                case "3":
                    return "Загружается";
                case "4":
                    return "Осуществлят доставку";
                case "5":
                    return "Доставка выполнена";
            }
            return "";
        }
        
        private Color getColorCell(String pk)
        {
            if (pk == "5")
                return Color.Green;
            return Color.White;
        }

        //метод хаполняет таблицу обработанных заказов
        private void setDataGridCompleteOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE pk_status = 6" ;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells[0].Value = dataReader[1].ToString();
                dataGridView3.Rows[i].Cells[1].Value = dataReader[4].ToString();
                dataGridView3.Rows[i].Cells[2].Value = dataReader[3].ToString().Substring(0,dataReader[3].ToString().IndexOf(" "));      //время
                dataGridView3.Rows[i].Cells[3].Value = dataReader[11].ToString();
                i++;
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
            int i = 0;
            while (dataReader.Read())
            {
                if (isGoodDate(dataReader[3].ToString()))
                {
                    dataGridView2.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                    dataGridView2.Rows[i].Cells[1].Value = "Ожидает доставки";          //статус заказа
                    dataGridView2.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                    dataGridView2.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(0,dataReader[3].ToString().IndexOf(" "));      //дата
                    dataGridView2.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                    dataGridView2.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                    dataGridView2.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
                    i++;
                }
                else
                {
                    badOrders.Add(dataReader[0].ToString());    //запоминаем пк плохих заказов
                }
            }
            dataReader.Close();
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
                msc.CommandText = "UPDATE `Order`  SET `pk_status` = 7 WHERE `pk_order` = '" + bad + "'";
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
            foreach (DataGridViewRow row in table.Rows)
            {
                try
                {
                    row.Cells[6].Value = materials.Find((Tuple<string, string> t) => t.Item1 == row.Cells[6].Value.ToString()).Item2;    //вот чему учит Scala
                }catch(Exception e)
                {}
            }
        }

        private void setDataGridRawOrder()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE pk_status = 7";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                dataGridView4.Rows[i].Cells[0].Value = dataReader[1].ToString();    //номер заказа
                dataGridView4.Rows[i].Cells[2].Value = dataReader[4].ToString();    //адрес
                dataGridView4.Rows[i].Cells[3].Value = dataReader[3].ToString().Substring(0, dataReader[3].ToString().IndexOf(" "));      //дата
                dataGridView4.Rows[i].Cells[4].Value = dataReader[11].ToString();      //стоимость
                dataGridView4.Rows[i].Cells[5].Value = dataReader[2].ToString();        //объем закза
                dataGridView4.Rows[i].Cells[6].Value = dataReader[13].ToString();       //материал
                i++;
            }
            dataReader.Close();
            //dataGridView4.Rows.RemoveAt(dataGridView4.Rows.Count - 1);
            inserMaterial(dataGridView4);
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
            //setDataGridActivOrder();
        }

        private void FormOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionToMySQL.Close();
            mainForm.Show();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //setDataGridActivOrder();
        }

        //по идеи никогда не вызывается))) 
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ( e.RowIndex >=0 && e.RowIndex < dataGridView1.NewRowIndex)
            {
                if (e.ColumnIndex == 1)
                {
                    DialogResult rez =  MessageBox.Show("Вы уверены что хотите сменить стутус заказа?", "Статус заказа.", MessageBoxButtons.OKCancel);
                    if (rez == DialogResult.OK)
                    {
                        String status = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        int pkStatus = 0;
                        switch (status)
                        {
                            case "Ожидает доставки":
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Едет на загрузку";
                                pkStatus = 2;
                                break;
                            case "Едет на загрузку":
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Загружается";
                                pkStatus = 3;
                                break;
                            case "Загружается":
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Осуществлят доставку";
                                pkStatus = 4;
                                break;
                            case "Осуществлят доставку":
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Доставка выполнена";
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                                pkStatus = 5;
                                break;
                        }
                        if (pkStatus != 0)
                        {
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "UPDATE `Order`  SET `pk_status` = + " + pkStatus + " WHERE `nomer` = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();
                        }
                    }
                    
                }
                String st = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (st == "Ожидает доставки" || st == "Едет на загрузку")
                {
                    buttonEdit1.Enabled = true;
                }
                else
                {
                    buttonEdit1.Enabled = false;
                }
                
            }
        }

        private void buttonEdit1_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.SelectedRows[0].IsNewRow)
            {
                Form editForm = new OrderEdit(ConnectionToMySQL, this, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                this.Visible = false;
                editForm.Show();
            }
            
           //dataGridView1.SelectedRows[0].Cells;
        }

       
    }
}
