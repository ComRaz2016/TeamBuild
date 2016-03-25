﻿using System;
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

        String lastNameFirm = null;
        String lastAdressFirm = null;
        String lastTelefoneFirm = null;

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

        // Удаление поставщика
        private void buttonProviderDelete_Click(object sender, EventArgs e)
        {
            String nameFirm = textBoxNameFirmDelete.Text;
            String adressFirm = textBoxAdressFirmDelete.Text;
            String telefoneFirm = textBoxTelefoneFirmDelete.Text;

            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT `pk_provider`  FROM `Provider`  WHERE `name_firm`  = '" + nameFirm + "' AND `tel_number_firm` = '" + telefoneFirm + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String providerPk = null;
            while (dataReader.Read())
            {
                providerPk = dataReader[0].ToString();
            }
            dataReader.Close();

            msc.CommandText = "SELECT `pk_material`  FROM `provider_material`  WHERE `pk_provider`  = '" + providerPk + "'";
            msc.Connection = ConnectionToMySQL;
            dataReader = msc.ExecuteReader();
            String anything = null;
            int count = 0;
            while (dataReader.Read())
            {
                count++;
                anything = dataReader[0].ToString();
            }
            dataReader.Close();
            if (count == 0)
            {
                msc.CommandText = "DELETE FROM `Provider` WHERE `name_firm` = '" + nameFirm + "' AND `tel_number_firm` = '" + telefoneFirm + "'";
                msc.Connection = ConnectionToMySQL;
                msc.ExecuteNonQuery();

                textBoxNameFirmDelete.Clear();
                textBoxAdressFirmDelete.Clear();
                textBoxTelefoneFirmDelete.Clear();

                this.providerTableAdapter.Fill(this.testDataSet.Provider);

                MessageBox.Show("Удаление записи успешно произведено.");
            }
            else
            {
                textBoxNameFirmDelete.Clear();
                textBoxAdressFirmDelete.Clear();
                textBoxTelefoneFirmDelete.Clear();

                MessageBox.Show("Удаление поставщика невозможно, так как он предоставляет материалы для доставки");
            }
            buttonProviderDelete.Enabled = false;
        }

        private void dataGridViewProviderAdd(object sender, DataGridViewCellEventArgs e)
        {
            // Не используется
        }

        // Выбор поставщика для изменения
        private void dataGridViewProviderChange(object sender, DataGridViewCellEventArgs e)
        {
            lastNameFirm = dataGridView2[0, e.RowIndex].Value.ToString();
            lastAdressFirm = dataGridView2[1, e.RowIndex].Value.ToString();
            lastTelefoneFirm = dataGridView2[2, e.RowIndex].Value.ToString();

            textBoxNameFirmChange.Text = lastNameFirm;
            textBoxAdressFirmChange.Text = lastAdressFirm;
            textBoxTelefoneFirmChange.Text = lastTelefoneFirm;
            buttonProviderChange.Enabled = true;
        }

        // Выбор поставщика для удаления
        private void dataGridViewProviderDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxNameFirmDelete.Text = dataGridView3[0, e.RowIndex].Value.ToString();
            textBoxAdressFirmDelete.Text = dataGridView3[1, e.RowIndex].Value.ToString();
            textBoxTelefoneFirmDelete.Text = dataGridView3[2, e.RowIndex].Value.ToString();

            buttonProviderDelete.Enabled = true;
        }

        // Добавление поставщика
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
                        msc.CommandText = "SELECT `pk_provider`  FROM `Provider`  WHERE `name_firm`  = '" + nameFirm + "'";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        String providerPk = null;
                        while (dataReader.Read())
                        {
                            providerPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        if (providerPk == null)
                        {
                            msc.CommandText = "INSERT INTO `Provider` (`name_firm`,`adress_firm`,`tel_number_firm`) VALUES ('" + nameFirm + "', '" + adressFirm + "' , '" + telefoneFirm + "')";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            textBoxNameFirmAdd.Clear();
                            textBoxAdressFirmAdd.Clear();
                            textBoxTelefoneFirmAdd.Clear();

                            this.providerTableAdapter.Fill(this.testDataSet.Provider);

                            MessageBox.Show("Добавление записи успешно произведено.");
                        }
                        else
                        {
                            textBoxNameFirmAdd.Clear();
                            MessageBox.Show("Запись не добавлена, так как фирма с таким названием существует.");
                        }
                    }
                }
            }
        }

        private void tabPage3_Leave(object sender, EventArgs e)
        {
            textBoxNameFirmAdd.Clear();
            textBoxAdressFirmAdd.Clear();
            textBoxTelefoneFirmAdd.Clear();
        }

        private void tabPage4_Leave(object sender, EventArgs e)
        {
            textBoxNameFirmChange.Clear();
            textBoxAdressFirmChange.Clear();
            textBoxTelefoneFirmChange.Clear();

            buttonProviderChange.Enabled = false;
        }

        private void tabPage5_Leave(object sender, EventArgs e)
        {
            textBoxNameFirmDelete.Clear();
            textBoxAdressFirmDelete.Clear();
            textBoxTelefoneFirmDelete.Clear();

            buttonProviderDelete.Enabled = false;
        }

        private void buttonProviderChange_Click(object sender, EventArgs e)
        {
            String nameFirm = textBoxNameFirmChange.Text;
            String adressFirm = textBoxAdressFirmChange.Text;
            String telefoneFirm = textBoxTelefoneFirmChange.Text;

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
                        msc.CommandText = "SELECT `pk_provider`  FROM `Provider`  WHERE `name_firm`  = '" + nameFirm + "'";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        String providerPk = null;
                        while (dataReader.Read())
                        {
                            providerPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        if (providerPk == null)
                        {
                            //MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "UPDATE `Provider`  SET `name_firm` = '" + nameFirm + "', `tel_number_firm` = '" + telefoneFirm + "' , `adress_firm` = '" + adressFirm + "' WHERE `name_firm` = '" + lastNameFirm + "' AND `tel_number_firm` = '" + lastTelefoneFirm + "' AND `adress_firm` = '" + lastAdressFirm + "'";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();
                            textBoxNameFirmChange.Clear();
                            textBoxAdressFirmChange.Clear();
                            textBoxTelefoneFirmChange.Clear();

                            lastAdressFirm = null;
                            lastNameFirm = null;
                            lastTelefoneFirm = null;

                            this.providerTableAdapter.Fill(this.testDataSet.Provider);

                            buttonProviderChange.Enabled = false;

                            MessageBox.Show("Изменение записи успешно произведено.");
                        }
                        else
                        {
                            textBoxNameFirmChange.Clear();
                            MessageBox.Show("Запись не изменена, так как фирма с таким названием существует.");
                        }
                    }
                }
            }
        }
    }
}
