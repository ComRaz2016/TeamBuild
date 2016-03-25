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
    public partial class FormProviderMaterial : Form
    {
        MySqlConnection ConnectionToMySQL;

        String checkNameFirm = null;
        String checkAdressFirm = null;
        String checkTelefoneFirm = null;

        public FormProviderMaterial()
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

        private void FormProviderMaterial_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Provider". При необходимости она может быть перемещена или удалена.
            this.providerTableAdapter.Fill(this.testDataSet.Provider);

        }

        private void buttonProviderDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewProviderAdd(object sender, DataGridViewCellEventArgs e)
        {
            // Не используется
        }

        private void dataGridViewProviderChange(object sender, DataGridViewCellEventArgs e)
        {
            checkNameFirm = dataGridView2[0, e.RowIndex].Value.ToString();
            checkAdressFirm = dataGridView2[1, e.RowIndex].Value.ToString();
            checkTelefoneFirm = dataGridView2[2, e.RowIndex].Value.ToString();

            textBoxNameFirmChange.Text = checkNameFirm;
            textBoxAdressFirmChange.Text = checkAdressFirm;
            textBoxTelefoneFirmChange.Text = checkTelefoneFirm;
            buttonProviderChange.Enabled = true;
        }

        private void dataGridViewProviderDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNameFirmDelete.Text = dataGridView3[0, e.RowIndex].Value.ToString();
            textBoxAdressFirmDelete.Text = dataGridView3[1, e.RowIndex].Value.ToString();
            textBoxTelefoneFirmDelete.Text = dataGridView3[2, e.RowIndex].Value.ToString();

            buttonProviderDelete.Enabled = true;
        }

        // Добавление поставщика материалов
        private void buttonProviderAdd_Click(object sender, EventArgs e)
        {
            String nameFirm = textBoxNameFirmAdd.Text;
            String adressFirm = textBoxAdressFirmAdd.Text;
            String telefoneFirm = textBoxTelefoneFirmAdd.Text;
            if (nameFirm.Trim() == "")
            {
                MessageBox.Show("Необходимо заполнить поле 'Название фирмы'.");
            }
            else
            {
                if (telefoneFirm.Trim() == "")
                {
                    MessageBox.Show("Необходимо заполнить поле 'Телефонный номер фирмы'.");
                }
                else
                {
                    if (adressFirm.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Адрес расположения фирмы'.");
                    }
                    else
                    {
                        MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "INSERT INTO `Provider` (`name_firm`,`adress_firm`,`tel_number_firm`) VALUES ('" + nameFirm + "', '" + adressFirm + "' , '" + telefoneFirm + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                        textBoxNameFirmAdd.Clear();
                        textBoxAdressFirmAdd.Clear();
                        textBoxTelefoneFirmAdd.Clear();
                        this.providerTableAdapter.Fill(this.testDataSet.Provider);
                        //this.vodTableAdapter.Fill(this.drivers.vod);
                        //checkNull();
                        buttonProviderAdd.Enabled = false;
                    }
                }
            }
        }

        private void buttonProviderDelete_Click_1(object sender, EventArgs e)
        {
            String nameFirm = textBoxNameFirmDelete.Text;
            String adressFirm = textBoxAdressFirmDelete.Text;
            String telefoneFirm = textBoxTelefoneFirmDelete.Text;

            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "DELETE FROM `Provider` WHERE `name_firm` = '" + nameFirm + "' AND `tel_number_firm` = '" + telefoneFirm + "'";
            msc.Connection = ConnectionToMySQL;
            msc.ExecuteNonQuery();
            textBoxNameFirmDelete.Clear();
            textBoxAdressFirmDelete.Clear();
            textBoxTelefoneFirmDelete.Clear();
            this.providerTableAdapter.Fill(this.testDataSet.Provider);
            //checkNull();
            buttonProviderDelete.Enabled = false;
        }
    }
}
