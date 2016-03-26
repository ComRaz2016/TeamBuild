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
    public partial class FormDriverTS : Form
    {
        MySqlConnection ConnectionToMySQL;

        String lastDriverFIO = null;
        String lastDriverTelefone = null;
        String lastDriverNumber = null;

        public FormDriverTS()
        {
            String serverName = "127.0.0.1"; // Адрес сервера (для локальной базы пишите "localhost")
            string userName = "dbadmin"; // Имя пользователя
            string dbName = "Test"; //Имя базы данных
            //string port = "6565"; // Порт для подключения
            string port = "9570"; // Порт для подключения
            string password = "dbadmin"; // Пароль для подключения
            string charset = "utf8";
            String connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + dbName +
                ";port=" + port +
                ";password=" + password +
                ";charset=" + charset + ";";
            ConnectionToMySQL = new MySqlConnection(connStr);
            ConnectionToMySQL.Open();

            InitializeComponent();
        }

        private void FormDriverTS_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Driver". При необходимости она может быть перемещена или удалена.
            this.driverTableAdapter.Fill(this.testDataSet.Driver);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Car". При необходимости она может быть перемещена или удалена.
            this.carTableAdapter.Fill(this.testDataSet.Car);

        }

        private void buttonDriverAdd_Click(object sender, EventArgs e)
        {
            String driverFIO = textBoxDriverFIOAdd.Text.Trim();
            String driverNumber = textBoxDriverNumberAdd.Text.Trim();
            String driverTelefone = textBoxDriverTelefoneAdd.Text.Trim();
            if (driverFIO.Trim() == "")
            {
                MessageBox.Show("Необходимо заполнить поле 'ФИО водителя'.");
            }
            else
            {
                if (driverNumber.Trim() == "")
                {
                    MessageBox.Show("Необходимо заполнить поле 'Номер водительского удостоверения'.");
                }
                else
                {
                    if (driverTelefone.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Номер телефона водителя'.");
                    }
                    else
                    {
                        MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "SELECT `pk_driver`  FROM `Driver`  WHERE `nomber_driver`  = '" + driverNumber + "'";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        String driverPk = null;
                        while (dataReader.Read())
                        {
                            driverPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        if (driverPk == null)
                        {
                            msc.CommandText = "INSERT INTO `Driver` (`fio_driver`,`nomber_driver`,`tel_number_driver`) VALUES ('" + driverFIO + "', '" + driverNumber + "' , '" + driverTelefone + "')";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            textBoxDriverFIOAdd.Clear();
                            textBoxDriverNumberAdd.Clear();
                            textBoxDriverTelefoneAdd.Clear();

                            this.driverTableAdapter.Fill(this.testDataSet.Driver);

                            MessageBox.Show("Добавление записи успешно произведено.");
                        }
                        else
                        {
                            textBoxDriverNumberAdd.Clear();
                            MessageBox.Show("Запись не добавлена, так как водитель с таким номером удостоверения существует.");
                        }
                    }
                }
            }
        }

        private void buttonDriverChange_Click(object sender, EventArgs e)
        {

        }

        private void buttonDriverDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewDriverChange(object sender, DataGridViewCellEventArgs e)
        {
            lastDriverFIO = dataGridView6[0, e.RowIndex].Value.ToString();
            lastDriverNumber = dataGridView6[1, e.RowIndex].Value.ToString();
            lastDriverTelefone = dataGridView6[2, e.RowIndex].Value.ToString();

            textBoxDriverFIOChange.Text = lastDriverFIO;
            textBoxDriverNumberChange.Text = lastDriverNumber;
            textBoxDriverTelefoneChange.Text = lastDriverTelefone;

            buttonDriverChange.Enabled = true;
        }

        private void dataGridViewDriverDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxDriverFIODelete.Text = dataGridView6[0, e.RowIndex].Value.ToString();
            textBoxDriverNumberDelete.Text = dataGridView6[1, e.RowIndex].Value.ToString();
            textBoxDriverTelefoneDelete.Text = dataGridView6[2, e.RowIndex].Value.ToString();
            
            buttonDriverDelete.Enabled = true;
        }

        private void tabPage3_Leave(object sender, EventArgs e)
        {
            textBoxDriverFIOAdd.Clear();
            textBoxDriverNumberAdd.Clear();
            textBoxDriverTelefoneAdd.Clear();
        }

        private void tabPage4_Leave(object sender, EventArgs e)
        {
            textBoxDriverFIOChange.Clear();
            textBoxDriverNumberChange.Clear();
            textBoxDriverTelefoneChange.Clear();

            buttonDriverChange.Enabled = false;
        }

        private void tabPage5_Leave(object sender, EventArgs e)
        {
            textBoxDriverFIODelete.Clear();
            textBoxDriverNumberDelete.Clear();
            textBoxDriverTelefoneDelete.Clear();

            buttonDriverDelete.Enabled = false;
        }
    }
}
