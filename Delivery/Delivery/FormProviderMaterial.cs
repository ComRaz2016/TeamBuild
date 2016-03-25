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

        String lastNameFirm = null;
        String lastAdressFirm = null;
        String lastTelefoneFirm = null;

        String lastNameMaterial = null;

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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Material". При необходимости она может быть перемещена или удалена.
            this.materialTableAdapter.Fill(this.testDataSet.Material);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.provider_material". При необходимости она может быть перемещена или удалена.
            this.provider_materialTableAdapter.Fill(this.testDataSet.provider_material);
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

        // Изменение поставщика
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

        // Заполнение первичных ключей поставщиков для добавления
        public void insertProviderAddTable()
        {
            int count = dataGridView4.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String providerPk = null;
                providerPk = dataGridView4.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name_firm`  FROM `Provider`  WHERE `pk_provider`  = '" + providerPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameFirm = null;
                while (dataReader.Read())
                {
                    nameFirm = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView4.Rows[i].Cells[0].Value = nameFirm;
            }
            dataGridView4.Update();
        }

        // Заполнение первичных ключей материалов для добавления
        public void insertMaterialAddTable()
        {
            int count = dataGridView4.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String materialPk = null;
                materialPk = dataGridView4.Rows[i].Cells[2].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name`  FROM `Material`  WHERE `pk_material`  = '" + materialPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameMaterial = null;
                while (dataReader.Read())
                {
                    nameMaterial = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView4.Rows[i].Cells[1].Value = nameMaterial;
            }
            dataGridView4.Update();
        }

        // Заполнение первичных ключей поставщиков в таблице для изменения
        public void insertProviderChangeTable()
        {
            int count = dataGridView5.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String providerPk = null;
                providerPk = dataGridView5.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name_firm`  FROM `Provider`  WHERE `pk_provider`  = '" + providerPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameFirm = null;
                while (dataReader.Read())
                {
                    nameFirm = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView5.Rows[i].Cells[0].Value = nameFirm;
            }
            dataGridView5.Update();
        }

        // Заполнение первичных ключей материалов в таблице для изменения
        public void insertMaterialChangeTable()
        {
            int count = dataGridView5.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String materialPk = null;
                materialPk = dataGridView5.Rows[i].Cells[2].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name`  FROM `Material`  WHERE `pk_material`  = '" + materialPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameMaterial = null;
                while (dataReader.Read())
                {
                    nameMaterial = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView5.Rows[i].Cells[1].Value = nameMaterial;
            }
            dataGridView5.Update();
        }

        // Заполнение первичных ключей поставщиков в таблице для удаления
        public void insertProviderDeleteTable()
        {
            int count = dataGridView6.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String providerPk = null;
                providerPk = dataGridView6.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name_firm`  FROM `Provider`  WHERE `pk_provider`  = '" + providerPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameFirm = null;
                while (dataReader.Read())
                {
                    nameFirm = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView6.Rows[i].Cells[0].Value = nameFirm;
            }
            dataGridView6.Update();
        }

        // Заполнение первичных ключей материалов в таблице для удаления
        public void insertMaterialDeleteTable()
        {
            int count = dataGridView6.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String materialPk = null;
                materialPk = dataGridView6.Rows[i].Cells[2].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `name`  FROM `Material`  WHERE `pk_material`  = '" + materialPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String nameMaterial = null;
                while (dataReader.Read())
                {
                    nameMaterial = dataReader[0].ToString();
                }
                dataReader.Close();
                dataGridView6.Rows[i].Cells[1].Value = nameMaterial;
            }
            dataGridView6.Update();
        }

        // Заполнение первичных ключей материалов и поставщиков в таблице для добавления
        private void dataGridView4_Paint(object sender, PaintEventArgs e)
        {
            insertProviderAddTable();
            insertMaterialAddTable();
        }

        // Заполнение первичных ключей материалов и поставщиков в таблице для удаления 
        private void dataGridView6_Paint(object sender, PaintEventArgs e)
        {
            insertProviderDeleteTable();
            insertMaterialDeleteTable();
        }

        // Заполнение первичных ключей материалов и поставщиков в таблице для изменения
        private void dataGridView5_Paint(object sender, PaintEventArgs e)
        {
            insertProviderChangeTable();
            insertMaterialChangeTable();
        }

        private void FormProviderMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionToMySQL.Close();
        }

        // Добавление названия материала
        private void buttonMaterialAdd_Click(object sender, EventArgs e)
        {
            String nameMaterial = textBoxMaterialAdd.Text;
            
            if (nameMaterial.Trim() == "")
            {
                MessageBox.Show("Необходимо заполнить поле 'Название материала'.");
            }
            else
            {
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `pk_material`  FROM `Material`  WHERE `name`  = '" + nameMaterial + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String materialPk = null;
                while (dataReader.Read())
                {
                    materialPk = dataReader[0].ToString();
                }
                dataReader.Close();

                if (materialPk == null)
                {
                    msc.CommandText = "INSERT INTO `Material` (`name`) VALUES ('" + nameMaterial + "')";
                    msc.Connection = ConnectionToMySQL;
                    msc.ExecuteNonQuery();

                    textBoxMaterialAdd.Clear();

                    this.materialTableAdapter.Fill(this.testDataSet.Material);

                    MessageBox.Show("Добавление записи успешно произведено.");
                }
                else
                {
                    textBoxMaterialAdd.Clear();
                    MessageBox.Show("Запись не добавлена, так как материал с таким названием существует.");
                }
            }
        }

        private void buttonMaterialChange_Click(object sender, EventArgs e)
        {

        }

        private void buttonMaterialDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewMaterialChange(object sender, DataGridViewCellEventArgs e)
        {
            lastNameMaterial = dataGridView8[0, e.RowIndex].Value.ToString();

            textBoxMaterialChange.Text = lastNameMaterial;

            buttonMaterialChange.Enabled = true;
        }

        private void dataGridViewMaterialDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxMaterialDelete.Text = dataGridView9[0, e.RowIndex].Value.ToString();

            buttonMaterialDelete.Enabled = true;
        }
    }
}
