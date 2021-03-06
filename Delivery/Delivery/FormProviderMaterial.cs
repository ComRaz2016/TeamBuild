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
        Form mainForm;

        String lastNameFirm = null;
        String lastAdressFirm = null;
        String lastTelefoneFirm = null;

        String lastNameMaterial = null;

        String lastBagCost = null;
        String lastTonnCost = null;
        String lastNameFirmMaterial = null;
        String lastMaterialFirm = null;


        public FormProviderMaterial(MySqlConnection connection, Form form)
        {
            ConnectionToMySQL = connection;
            mainForm = form;
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
            String nameFirm = textBoxNameFirmDelete.Text.Trim();
            String adressFirm = textBoxAdressFirmDelete.Text.Trim();
            String telefoneFirm = textBoxTelefoneFirmDelete.Text.Trim();

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

                MessageBox.Show("Удаление поставщика невозможно, так как он предоставляет материалы для доставки.");
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
            String nameFirm = textBoxNameFirmAdd.Text.Trim();
            String adressFirm = textBoxAdressFirmAdd.Text.Trim();
            String telefoneFirm = textBoxTelefoneFirmAdd.Text.Trim();
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
            String nameFirm = textBoxNameFirmChange.Text.Trim();
            String adressFirm = textBoxAdressFirmChange.Text.Trim();
            String telefoneFirm = textBoxTelefoneFirmChange.Text.Trim();

            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT `pk_provider`  FROM `Provider`  WHERE `name_firm`  = '" + lastNameFirm + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String lastProviderPk = null;
            while (dataReader.Read())
            {
                lastProviderPk = dataReader[0].ToString();
            }
            dataReader.Close();

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
                        //MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "SELECT `pk_provider`  FROM `Provider`  WHERE `name_firm`  = '" + nameFirm + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        String providerPk = null;
                        while (dataReader.Read())
                        {
                            providerPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        if (lastProviderPk == providerPk)
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

                            MessageBox.Show("Изменение записи успешно произведено.");

                            buttonProviderChange.Enabled = false;
                        }
                        else
                        {
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

                                MessageBox.Show("Изменение записи успешно произведено.");

                                buttonProviderChange.Enabled = false;
                            }
                            else
                            {
                                lastAdressFirm = null;
                                lastNameFirm = null;
                                lastTelefoneFirm = null;

                                textBoxNameFirmChange.Clear();
                                textBoxAdressFirmChange.Clear();
                                textBoxTelefoneFirmChange.Clear();

                                MessageBox.Show("Запись не изменена, так как фирма с таким названием существует.");

                                buttonProviderChange.Enabled = false;
                            }
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
            mainForm.Show();
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

        // Изменение названия материала
        private void buttonMaterialChange_Click(object sender, EventArgs e)
        {
            String nameMaterial = textBoxMaterialChange.Text.Trim();

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
                    msc.CommandText = "UPDATE `Material`  SET `name` = '" + nameMaterial + "' WHERE `name` = '" + lastNameMaterial + "'";
                    msc.Connection = ConnectionToMySQL;
                    msc.ExecuteNonQuery();
                    textBoxMaterialChange.Clear();

                    lastNameMaterial = null;

                    this.materialTableAdapter.Fill(this.testDataSet.Material);

                    MessageBox.Show("Изменение записи успешно произведено.");

                    buttonMaterialChange.Enabled = false;
                }
                else
                {
                    lastNameMaterial = null;

                    textBoxMaterialChange.Clear();

                    MessageBox.Show("Запись не изменена, так как материал с таким названием существует.");

                    buttonMaterialChange.Enabled = false;
                }
            }
        }

        // Удаление названия материала
        private void buttonMaterialDelete_Click(object sender, EventArgs e)
        {
            String nameMaterial = textBoxMaterialDelete.Text.Trim();

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

            msc.CommandText = "SELECT `pk_provider`  FROM `provider_material`  WHERE `pk_material`  = '" + materialPk + "'";
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
                try
                {
                    msc.CommandText = "DELETE FROM `Material` WHERE `name` = '" + nameMaterial + "'";
                    msc.Connection = ConnectionToMySQL;
                    msc.ExecuteNonQuery();

                    textBoxMaterialDelete.Clear();

                    this.materialTableAdapter.Fill(this.testDataSet.Material);

                    MessageBox.Show("Удаление записи успешно произведено.");
                }
                catch (Exception)
                {
                    textBoxMaterialDelete.Clear();

                    MessageBox.Show("Удаление записи невозможно, так как производится учет доставляемых товаров.");
                }
            }
            else
            {
                textBoxMaterialDelete.Clear();

                MessageBox.Show("Удаление материала невозможно, так как он предоставляет поставщиками для доставки.");
            }
            buttonMaterialDelete.Enabled = false;
        }

        // Выбор названия материала для изменения
        private void dataGridViewMaterialChange(object sender, DataGridViewCellEventArgs e)
        {
            lastNameMaterial = dataGridView8[0, e.RowIndex].Value.ToString();

            textBoxMaterialChange.Text = lastNameMaterial;

            buttonMaterialChange.Enabled = true;
        }

        // Выбор названия материала для удаления
        private void dataGridViewMaterialDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxMaterialDelete.Text = dataGridView9[0, e.RowIndex].Value.ToString();

            buttonMaterialDelete.Enabled = true;
        }

        private void tabPage12_Leave(object sender, EventArgs e)
        {
            textBoxMaterialDelete.Clear();
            buttonMaterialDelete.Enabled = false;
        }

        private void tabPage11_Leave(object sender, EventArgs e)
        {
            textBoxMaterialChange.Clear();
            buttonMaterialChange.Enabled = false;
        }

        private void tabPage10_Leave(object sender, EventArgs e)
        {
            textBoxMaterialAdd.Clear();
        }

        // Добавление материалов поставщиков
        private void buttonMaterialProviderAdd_Click(object sender, EventArgs e)
        {
            String nameFirm = comboBoxProviderAdd.Text.Trim();
            String nameMaterial = comboBoxMaterialAdd.Text.Trim();
            String bagCost = textBoxBagCostAdd.Text.Trim();
            String tonnCost = textBoxTonnCostAdd.Text.Trim();
            if (comboBoxProviderAdd.Items.Count == 0)
            {
                MessageBox.Show("Необходимо выбрать поставщика.");
            }
            else
            {
                if (comboBoxMaterialAdd.Items.Count == 0)
                {
                    MessageBox.Show("Необходимо выбрать материал.");
                }
                else
                {
                    if (bagCost.Trim() == "" && tonnCost.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Цена за тонну' или 'Цена за мешок'.");
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

                        msc.CommandText = "SELECT `pk_material`  FROM `Material`  WHERE `name`  = '" + nameMaterial + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        String materialPk = null;
                        while (dataReader.Read())
                        {
                            materialPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        msc.CommandText = "SELECT `cost_bag`  FROM `provider_material`  WHERE `pk_provider`  = '" + providerPk + "' AND `pk_material`  = '" + materialPk + "'";
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
                            msc.CommandText = "INSERT INTO `provider_material` (`pk_provider`,`pk_material`,`cost_bag`, `cost_tonna`) VALUES ('" + providerPk + "', '" + materialPk + "' , '" + bagCost + "', '" + tonnCost + "')";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            this.provider_materialTableAdapter.Fill(this.testDataSet.provider_material);

                            MessageBox.Show("Добавление записи успешно произведено.");

                            textBoxBagCostAdd.Clear();
                            textBoxTonnCostAdd.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Поставщик уже предоставляет данный материал для доставки.");

                            textBoxBagCostAdd.Clear();
                            textBoxTonnCostAdd.Clear();
                        }
                    }
                }
            }
        }

        // Изменение материалов поставщиков
        private void buttonProviderMaterialChange_Click(object sender, EventArgs e)
        {
            String nameMaterial = comboBoxMaterialChange.Text.Trim();
            String nameFirm = comboBoxProviderChange.Text.Trim();
            String bagCost = textBoxBagCostChange.Text.Trim();
            String tonnCost = textBoxTonnCostChange.Text.Trim();

            if (comboBoxProviderChange.Items.Count == 0)
            {
                MessageBox.Show("Необходимо выбрать поставщика.");
            }
            else
            {
                if (comboBoxMaterialChange.Items.Count == 0)
                {
                    MessageBox.Show("Необходимо выбрать материал.");
                }
                else
                {
                    if (bagCost.Trim() == "" && tonnCost.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Цена за тонну' или 'Цена за мешок'.");
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

                        msc.CommandText = "SELECT `pk_material`  FROM `Material`  WHERE `name`  = '" + nameMaterial + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        String materialPk = null;
                        while (dataReader.Read())
                        {
                            materialPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        msc.CommandText = "UPDATE `provider_material`  SET `cost_bag` = '" + bagCost + "', `cost_tonna` = '" + tonnCost + "' WHERE `pk_provider` = '" + providerPk + "' AND `pk_material` = '" + materialPk + "'";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();

                        textBoxTonnCostChange.Clear();
                        textBoxBagCostChange.Clear();

                        this.provider_materialTableAdapter.Fill(this.testDataSet.provider_material);

                        MessageBox.Show("Изменение записи успешно произведено.");

                        buttonProviderMaterialChange.Enabled = false;
                    }
                }
            }
        }

        // Удаление материалов поставщиков
        private void buttonProviderMaterialDelete_Click(object sender, EventArgs e)
        {
            String nameMaterial = comboBoxMaterialDelete.Text.Trim();
            String nameFirm = comboBoxProviderDelete.Text.Trim();
            String bagCost = textBoxBagCostDelete.Text.Trim();
            String tonnCost = textBoxTonnCostDelete.Text.Trim();

            if (comboBoxProviderDelete.Items.Count == 0)
            {
                MessageBox.Show("Необходимо выбрать поставщика.");
            }
            else
            {
                if (comboBoxMaterialDelete.Items.Count == 0)
                {
                    MessageBox.Show("Необходимо выбрать материал.");
                }
                else
                {
                    if (bagCost.Trim() == "" && tonnCost.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Цена за тонну' или 'Цена за мешок'.");
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

                        msc.CommandText = "SELECT `pk_material`  FROM `Material`  WHERE `name`  = '" + nameMaterial + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        String materialPk = null;
                        while (dataReader.Read())
                        {
                            materialPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        try
                        {
                            msc.CommandText = "DELETE FROM `provider_material` WHERE `pk_material` = '" + materialPk + "' AND `pk_provider` = '" + providerPk + "'";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            textBoxTonnCostDelete.Clear();
                            textBoxBagCostDelete.Clear();

                            this.provider_materialTableAdapter.Fill(this.testDataSet.provider_material);

                            MessageBox.Show("Удаление записи успешно произведено.");

                            buttonProviderMaterialDelete.Enabled = false;
                        }
                        catch (Exception)
                        {
                            textBoxTonnCostDelete.Clear();
                            textBoxBagCostDelete.Clear();

                            this.provider_materialTableAdapter.Fill(this.testDataSet.provider_material);

                            MessageBox.Show("Удаление записи невозможно, так как производится учет доставляемых товаров.");

                            buttonProviderMaterialDelete.Enabled = false;
                        }
                    }
                }
            }
        }

        // Выбор материалов поставщика для изменения
        private void dataGridViewProviderMaterialChange(object sender, DataGridViewCellEventArgs e)
        {
            lastBagCost = dataGridView5[3, e.RowIndex].Value.ToString();
            lastTonnCost = dataGridView5[5, e.RowIndex].Value.ToString();
            lastNameFirmMaterial = dataGridView5[0, e.RowIndex].Value.ToString();
            lastMaterialFirm = dataGridView5[1, e.RowIndex].Value.ToString();

            textBoxBagCostChange.Text = lastBagCost;
            textBoxTonnCostChange.Text = lastTonnCost;
            comboBoxProviderChange.Text = lastNameFirmMaterial;
            comboBoxMaterialChange.Text = lastMaterialFirm;

            buttonProviderMaterialChange.Enabled = true;
        }

        // Выбор материалов поставщика для удаления
        private void dataGridViewProviderMaterialDelete(object sender, DataGridViewCellEventArgs e)
        {
            textBoxBagCostDelete.Text = dataGridView6[3, e.RowIndex].Value.ToString();
            textBoxTonnCostDelete.Text = dataGridView6[5, e.RowIndex].Value.ToString();
            comboBoxProviderDelete.Text = dataGridView6[0, e.RowIndex].Value.ToString();
            comboBoxMaterialDelete.Text = dataGridView6[1, e.RowIndex].Value.ToString();

            buttonProviderMaterialDelete.Enabled = true;
        }

        private void tabPage8_Leave(object sender, EventArgs e)
        {
            textBoxBagCostDelete.Clear();
            textBoxTonnCostDelete.Clear();

            buttonProviderMaterialDelete.Enabled = false;
        }

        private void tabPage7_Leave(object sender, EventArgs e)
        {
            textBoxBagCostChange.Clear();
            textBoxTonnCostChange.Clear();

            buttonProviderMaterialChange.Enabled = false;
        }

        private void tabPage6_Leave(object sender, EventArgs e)
        {
            textBoxBagCostAdd.Clear();
            textBoxTonnCostAdd.Clear();
        }

        private void textBoxNameFirmAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(char.IsLetter(c) || c == '\b' || char.IsDigit(c) || c == ' ');
        }

        private void textBoxTelefoneFirmAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == '+');
        }

        private void textBoxAdressFirmAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || char.IsLetter(c) || c == ',' || c == '.' || c == ' ');
        }

        private void textBoxNameFirmChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(char.IsLetter(c) || c == '\b' || char.IsDigit(c) || c == ' ');
        }

        private void textBoxTelefoneFirmChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == '+');
        }

        private void textBoxAdressFirmChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || char.IsLetter(c) || c == ',' || c == '.' || c == ' ');
        }

        private void textBoxBagCostAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == ',');
        }

        private void textBoxTonnCostAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == ',');
        }

        private void textBoxBagCostChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == ',');
        }

        private void textBoxTonnCostChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(c == '\b' || char.IsDigit(c) || c == ',');
        }

        private void textBoxMaterialAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(char.IsLetter(c) || c == '\b' || char.IsDigit(c) || c == ' ');
        }

        private void textBoxMaterialChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(char.IsLetter(c) || c == '\b' || char.IsDigit(c) || c == ' ');
        }
    }
}
