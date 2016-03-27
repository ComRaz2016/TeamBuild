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

        String lastTSDriverFIO = null;
        String lastTSDriverNumber = null;
        String lastDriverTSMark = null;
        String lastDriverTSNumber = null;

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
        // Добавление водителя
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

        // Изменение водителя
        private void buttonDriverChange_Click(object sender, EventArgs e)
        {
            String driverFIO = textBoxDriverFIOChange.Text.Trim();
            String driverNumber = textBoxDriverNumberChange.Text.Trim();
            String driverTelefone = textBoxDriverTelefoneChange.Text.Trim();
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
                        msc.CommandText = "SELECT `pk_driver`  FROM `Driver`  WHERE `nomber_driver`  = '" + driverNumber + "' AND `fio_driver`  = '" + driverFIO + "'";
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
                            msc.CommandText = "UPDATE `Driver`  SET `fio_driver` = '" + driverFIO + "', `nomber_driver` = '" + driverNumber + "' , `tel_number_driver` = '" + driverTelefone + "' WHERE `fio_driver` = '" + lastDriverFIO + "' AND `nomber_driver` = '" + lastDriverNumber + "' AND `tel_number_driver` = '" + lastDriverTelefone + "'";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            textBoxDriverFIOChange.Clear();
                            textBoxDriverTelefoneChange.Clear();
                            textBoxDriverNumberChange.Clear();

                            lastDriverFIO = null;
                            lastDriverTelefone = null;
                            lastDriverNumber = null;

                            this.driverTableAdapter.Fill(this.testDataSet.Driver);

                            MessageBox.Show("Изменение записи успешно произведено.");

                            buttonDriverChange.Enabled = false;
                        }
                        else
                        {
                            lastDriverFIO = null;
                            lastDriverTelefone = null;
                            lastDriverNumber = null;

                            textBoxDriverFIOChange.Clear();
                            textBoxDriverTelefoneChange.Clear();
                            textBoxDriverNumberChange.Clear();

                            MessageBox.Show("Запись не изменена, так как водитель с таким удостоверением существует.");

                            buttonDriverChange.Enabled = false;
                        }
                    }
                }
            }
        }

        // Удаление водителя
        private void buttonDriverDelete_Click(object sender, EventArgs e)
        {
            String driverFIO = textBoxDriverFIODelete.Text.Trim();
            String driverNumber = textBoxDriverNumberDelete.Text.Trim();
            String driverTelefone = textBoxDriverTelefoneDelete.Text.Trim();
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
                        msc.CommandText = "SELECT `pk_driver`  FROM `Driver`  WHERE `nomber_driver`  = '" + driverNumber + "' AND `fio_driver` = '" + driverFIO + "'";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        String driverPk = null;
                        while (dataReader.Read())
                        {
                            driverPk = dataReader[0].ToString();
                        }
                        dataReader.Close();

                        msc.CommandText = "SELECT `pk_car`  FROM `Car`  WHERE `pk_driver`  = '" + driverPk + "'";
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
                            msc.CommandText = "DELETE FROM `Driver` WHERE `fio_driver` = '" + driverFIO + "' AND `nomber_driver` = '" + driverNumber + "'";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();

                            textBoxDriverFIODelete.Clear();
                            textBoxDriverNumberDelete.Clear();
                            textBoxDriverTelefoneDelete.Clear();

                            this.driverTableAdapter.Fill(this.testDataSet.Driver);

                            MessageBox.Show("Удаление записи успешно произведено.");

                            buttonDriverDelete.Enabled = false;
                        }
                        else
                        {
                            textBoxDriverFIODelete.Clear();
                            textBoxDriverNumberDelete.Clear();
                            textBoxDriverTelefoneDelete.Clear();

                            MessageBox.Show("Удаление водителя невозможно, так как он предоставляет ТС для доставки материалов.");

                            buttonDriverDelete.Enabled = false;
                        }
                    }
                }
            }
        }

        // Выбор водителя для изменения
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

        // Выбор водителя для удаления
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

        // Заполнение первичных ключей водителей
        public void insertDriverAddTable()
        {
            int count = dataGridView3.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String driverPk = null;
                driverPk = dataGridView3.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `fio_driver`, `nomber_driver`  FROM `Driver`  WHERE `pk_driver`  = '" + driverPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String driverFIO = null;
                String driverNumber = null;
                while (dataReader.Read())
                {
                    driverFIO = dataReader[0].ToString();
                    driverNumber = dataReader[1].ToString();
                }
                dataReader.Close();
                dataGridView3.Rows[i].Cells[1].Value = driverFIO;
                dataGridView3.Rows[i].Cells[2].Value = driverNumber;
            }
            dataGridView3.Update();
        }

        // Заполнение первичных ключей водителей
        public void insertDriverChangeTable()
        {
            int count = dataGridView2.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String driverPk = null;
                driverPk = dataGridView2.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `fio_driver`, `nomber_driver`  FROM `Driver`  WHERE `pk_driver`  = '" + driverPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String driverFIO = null;
                String driverNumber = null;
                while (dataReader.Read())
                {
                    driverFIO = dataReader[0].ToString();
                    driverNumber = dataReader[1].ToString();
                }
                dataReader.Close();
                dataGridView2.Rows[i].Cells[1].Value = driverFIO;
                dataGridView2.Rows[i].Cells[2].Value = driverNumber;
            }
            dataGridView2.Update();
        }

        // Заполнение первичных ключей водителей
        public void insertDriverDeleteTable()
        {
            int count = dataGridView1.RowCount - 1;
            for (int i = 0; i < count; i++)
            {
                String driverPk = null;
                driverPk = dataGridView1.Rows[i].Cells[4].Value.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT `fio_driver`, `nomber_driver`  FROM `Driver`  WHERE `pk_driver`  = '" + driverPk + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String driverFIO = null;
                String driverNumber = null;
                while (dataReader.Read())
                {
                    driverFIO = dataReader[0].ToString();
                    driverNumber = dataReader[1].ToString();
                }
                dataReader.Close();
                dataGridView1.Rows[i].Cells[1].Value = driverFIO;
                dataGridView1.Rows[i].Cells[2].Value = driverNumber;
            }
            dataGridView1.Update();
        }

        // Заполнение первичных ключей водителей в таблице для добавления
        private void dataGridView3_Paint(object sender, PaintEventArgs e)
        {
            insertDriverAddTable();
        }

        // Заполнение первичных ключей водителей в таблице для изменения
        private void dataGridView2_Paint(object sender, PaintEventArgs e)
        {
            insertDriverChangeTable();
        }

        // Заполнение первичных ключей водителей в таблице для удаления
        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            insertDriverDeleteTable();
        }

        private void comboBoxDriverNumberAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxDriverFIOAdd.SelectedIndex = comboBoxDriverNumberAdd.SelectedIndex;
        }

        private void comboBoxDriverNumberAdd_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBoxDriverNumberAdd.SelectedIndex = comboBoxDriverFIOAdd.SelectedIndex;
        }

        // Выбор ТС для изменения
        private void dataGridViewTSChange(object sender, DataGridViewCellEventArgs e)
        {
            lastTSDriverFIO = dataGridView2[1, e.RowIndex].Value.ToString();
            lastTSDriverNumber = dataGridView2[2, e.RowIndex].Value.ToString();
            lastDriverTSMark = dataGridView2[0, e.RowIndex].Value.ToString();
            lastDriverTSNumber = dataGridView2[3, e.RowIndex].Value.ToString();

            comboBoxTSDriverFIOChange.Text = dataGridView2[1, e.RowIndex].Value.ToString();
            comboBoxTSDriverNumberChange.Text = dataGridView2[2, e.RowIndex].Value.ToString();
            textBoxTSMarkChange.Text = dataGridView2[0, e.RowIndex].Value.ToString();
            textBoxTSNumberChange.Text = dataGridView2[3, e.RowIndex].Value.ToString();
            String tsBag = dataGridView2[5, e.RowIndex].Value.ToString();
            if (tsBag == "1")
            {
                checkBoxTSBagChange.Checked = true;
            }
            else
            {
                checkBoxTSBagChange.Checked = false;
            }
            String tsBulk = dataGridView2[6, e.RowIndex].Value.ToString();
            if (tsBulk == "1")
            {
                checkBoxTSBulkChange.Checked = true;
            }
            else
            {
                checkBoxTSBulkChange.Checked = false;
            }
            textBoxTSTonnChange.Text = dataGridView2[7, e.RowIndex].Value.ToString();
            textBoxTSZone1Change.Text = dataGridView2[8, e.RowIndex].Value.ToString();
            textBoxTSZone2Change.Text = dataGridView2[9, e.RowIndex].Value.ToString();
            textBoxTSZone3Change.Text = dataGridView2[10, e.RowIndex].Value.ToString();
            textBoxTSDopKmChange.Text = dataGridView2[11, e.RowIndex].Value.ToString();

            buttonTSChange.Enabled = true;
        }

        // Выбор ТС для удаления
        private void dataGridViewTSDelete(object sender, DataGridViewCellEventArgs e)
        {
            comboBoxTSDriverFIODelete.Text = dataGridView1[1, e.RowIndex].Value.ToString();
            comboBoxTSDriverNumberDelete.Text = dataGridView1[2, e.RowIndex].Value.ToString();
            textBoxTSMarkDelete.Text = dataGridView1[0, e.RowIndex].Value.ToString();
            textBoxTSNumberDelete.Text = dataGridView1[3, e.RowIndex].Value.ToString();
            String tsBag = dataGridView1[5, e.RowIndex].Value.ToString();
            if (tsBag == "1")
            {
                checkBoxTSBagDelete.Checked = true;
            }
            else
            {
                checkBoxTSBagDelete.Checked = false;
            }
            String tsBulk = dataGridView1[6, e.RowIndex].Value.ToString();
            if (tsBulk == "1")
            {
                checkBoxTSBulkDelete.Checked = true;
            }
            else
            {
                checkBoxTSBulkDelete.Checked = false;
            }
            textBoxTSTonnDelete.Text = dataGridView1[7, e.RowIndex].Value.ToString();
            textBoxTSZone1Delete.Text = dataGridView1[8, e.RowIndex].Value.ToString();
            textBoxTSZone2Delete.Text = dataGridView1[9, e.RowIndex].Value.ToString();
            textBoxTSZone3Delete.Text = dataGridView1[10, e.RowIndex].Value.ToString();
            textBoxTSDopKmDelete.Text = dataGridView1[11, e.RowIndex].Value.ToString();

            buttonTSDelete.Enabled = true;
        }

        // Добавление ТС
        private void buttonTSAdd_Click(object sender, EventArgs e)
        {
            String driverFIO = comboBoxDriverFIOAdd.Text.Trim();
            String driverNumber = comboBoxDriverNumberAdd.Text.Trim();
            String tsMark = textBoxTSMarkAdd.Text.Trim();
            String tsNumber = textBoxTSNumberAdd.Text.Trim();
            String tsBag = null;
            if (checkBoxTSBagAdd.Checked == true)
            {
                tsBag = "1";
            }
            else
            {
                tsBag = "0";
            }
            String tsBulk = null;
            if (checkBoxTSBulkAdd.Checked == true)
            {
                tsBulk = "1";
            }
            else
            {
                tsBulk = "0";
            }
            String tsTonn = textBoxTSTonnAdd.Text.Trim();
            String tsZone1 = textBoxTSZone1Add.Text.Trim();
            String tsZone2 = textBoxTSZone2Add.Text.Trim();
            String tsZone3 = textBoxTSZone3Add.Text.Trim();
            String tsDopKm = textBoxTSDopKmAdd.Text.Trim();

            if (driverFIO.Trim() == "")
            {
                MessageBox.Show("Необходимо выбрать водителя в 'Водитель'.");
            }
            else
            {
                if (driverNumber.Trim() == "")
                {
                    MessageBox.Show("Необходимо выбрать водительское удостоверение в 'Водительское удостоверение'.");
                }
                else
                {
                    if (tsMark.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Марка ТС'.");
                    }
                    else
                    {
                        if (tsNumber.Trim() == "")
                        {
                            MessageBox.Show("Необходимо заполнить поле 'Регистрационный номер'.");
                        }
                        else
                        {
                            if (checkBoxTSBagAdd.Checked == false && checkBoxTSBulkAdd.Checked == false)
                            {
                                MessageBox.Show("Необходимо выбрать виды доставок 'Насыпь или мешок'.");
                            }
                            else
                            {
                                if (tsTonn.Trim() == "")
                                {
                                    MessageBox.Show("Необходимо заполнить поле 'Тоннаж'.");
                                }
                                else
                                {
                                    if (tsZone1.Trim() == "")
                                    {
                                        MessageBox.Show("Необходимо заполнить поле 'Зона 1'.");
                                    }
                                    else
                                    {
                                        if (tsZone2.Trim() == "")
                                        {
                                            MessageBox.Show("Необходимо заполнить поле 'Зона 2'.");
                                        }
                                        else
                                        {
                                            if (tsZone3.Trim() == "")
                                            {
                                                MessageBox.Show("Необходимо заполнить поле 'Зона 3'.");
                                            }
                                            else
                                            {
                                                if (tsDopKm.Trim() == "")
                                                {
                                                    MessageBox.Show("Необходимо заполнить поле 'Доп. км'.");
                                                }
                                                else
                                                {
                                                    MySqlCommand msc = new MySqlCommand();
                                                    msc.CommandText = "SELECT `pk_car`  FROM `Car`  WHERE `regist_number`  = '" + tsNumber + "'";
                                                    msc.Connection = ConnectionToMySQL;
                                                    MySqlDataReader dataReader = msc.ExecuteReader();
                                                    String carPk = null;
                                                    while (dataReader.Read())
                                                    {
                                                        carPk = dataReader[0].ToString();
                                                    }
                                                    dataReader.Close();

                                                    if (carPk == null)
                                                    {
                                                        msc.CommandText = "SELECT `pk_driver`  FROM `Driver`  WHERE `nomber_driver`  = '" + driverNumber + "'";
                                                        msc.Connection = ConnectionToMySQL;
                                                        dataReader = msc.ExecuteReader();
                                                        String driverPk = null;
                                                        while (dataReader.Read())
                                                        {
                                                            driverPk = dataReader[0].ToString();
                                                        }
                                                        dataReader.Close();

                                                        msc.CommandText = "INSERT INTO `Car` (`mark_car`,`regist_number`,`delivery_bag`, `delivery_bulk`, `tonnage`, `Costfistzone`, `Costsecondzone`, `Costthirdzone`, `Costdopkm`, `pk_driver`) VALUES ('" + tsMark + "', '" + tsNumber + "' , '" + tsBag + "', '" + tsBulk + "' , '" + tsTonn + "', '" + tsZone1 + "' , '" + tsZone2 + "', '" + tsZone3 + "' , '" + tsDopKm + "', '" + driverPk + "')";
                                                        msc.Connection = ConnectionToMySQL;
                                                        msc.ExecuteNonQuery();

                                                        textBoxTSDopKmAdd.Clear();
                                                        textBoxTSZone1Add.Clear();
                                                        textBoxTSZone2Add.Clear();
                                                        textBoxTSZone3Add.Clear();
                                                        textBoxTSTonnAdd.Clear();
                                                        textBoxTSMarkAdd.Clear();
                                                        textBoxTSNumberAdd.Clear();

                                                        this.carTableAdapter.Fill(this.testDataSet.Car);

                                                        MessageBox.Show("Добавление записи успешно произведено.");
                                                    }
                                                    else
                                                    {
                                                        textBoxTSDopKmAdd.Clear();
                                                        textBoxTSZone1Add.Clear();
                                                        textBoxTSZone2Add.Clear();
                                                        textBoxTSZone3Add.Clear();
                                                        textBoxTSTonnAdd.Clear();
                                                        textBoxTSMarkAdd.Clear();
                                                        textBoxTSNumberAdd.Clear();

                                                        MessageBox.Show("Запись не добавлена, так как ТС с таким регистрационным номером существует.");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void tabPage8_Leave(object sender, EventArgs e)
        {
            textBoxTSMarkDelete.Clear();
            textBoxTSNumberDelete.Clear();
            checkBoxTSBagDelete.Checked = false;
            checkBoxTSBulkDelete.Checked = false;

            textBoxTSTonnDelete.Clear();
            textBoxTSZone1Delete.Clear();
            textBoxTSZone2Delete.Clear();
            textBoxTSZone3Delete.Clear();
            textBoxTSDopKmDelete.Clear();

            buttonTSDelete.Enabled = false;
        }

        private void tabPage7_Leave(object sender, EventArgs e)
        {
            textBoxTSMarkChange.Clear();
            textBoxTSNumberChange.Clear();
            checkBoxTSBagChange.Checked = false;
            checkBoxTSBulkChange.Checked = false;

            textBoxTSTonnChange.Clear();
            textBoxTSZone1Change.Clear();
            textBoxTSZone2Change.Clear();
            textBoxTSZone3Change.Clear();
            textBoxTSDopKmChange.Clear();

            buttonTSChange.Enabled = false;
        }

        private void tabPage6_Leave(object sender, EventArgs e)
        {
            textBoxTSMarkAdd.Clear();
            textBoxTSNumberAdd.Clear();
            checkBoxTSBagAdd.Checked = false;
            checkBoxTSBulkAdd.Checked = false;

            textBoxTSTonnAdd.Clear();
            textBoxTSZone1Add.Clear();
            textBoxTSZone2Add.Clear();
            textBoxTSZone3Add.Clear();
            textBoxTSDopKmAdd.Clear();
        }

        // Удаление ТС
        private void buttonTSDelete_Click(object sender, EventArgs e)
        {
            String driverFIO = comboBoxTSDriverFIODelete.Text.Trim();
            String driverNumber = comboBoxTSDriverNumberDelete.Text.Trim();
            String tsMark = textBoxTSMarkDelete.Text.Trim();
            String tsNumber = textBoxTSNumberDelete.Text.Trim();
            String tsBag = null;
            if (checkBoxTSBagDelete.Checked == true)
            {
                tsBag = "1";
            }
            else
            {
                tsBag = "0";
            }
            String tsBulk = null;
            if (checkBoxTSBulkDelete.Checked == true)
            {
                tsBulk = "1";
            }
            else
            {
                tsBulk = "0";
            }
            String tsTonn = textBoxTSTonnDelete.Text.Trim();
            String tsZone1 = textBoxTSZone1Delete.Text.Trim();
            String tsZone2 = textBoxTSZone2Delete.Text.Trim();
            String tsZone3 = textBoxTSZone3Delete.Text.Trim();
            String tsDopKm = textBoxTSDopKmDelete.Text.Trim();

            if (driverFIO.Trim() == "")
            {
                MessageBox.Show("Необходимо выбрать водителя в 'Водитель'.");
            }
            else
            {
                if (driverNumber.Trim() == "")
                {
                    MessageBox.Show("Необходимо выбрать водительское удостоверение в 'Водительское удостоверение'.");
                }
                else
                {
                    if (tsMark.Trim() == "")
                    {
                        MessageBox.Show("Необходимо заполнить поле 'Марка ТС'.");
                    }
                    else
                    {
                        if (tsNumber.Trim() == "")
                        {
                            MessageBox.Show("Необходимо заполнить поле 'Регистрационный номер'.");
                        }
                        else
                        {
                            if (checkBoxTSBagDelete.Checked == false && checkBoxTSBulkDelete.Checked == false)
                            {
                                MessageBox.Show("Необходимо выбрать виды доставок 'Насыпь или мешок'.");
                            }
                            else
                            {
                                if (tsTonn.Trim() == "")
                                {
                                    MessageBox.Show("Необходимо заполнить поле 'Тоннаж'.");
                                }
                                else
                                {
                                    if (tsZone1.Trim() == "")
                                    {
                                        MessageBox.Show("Необходимо заполнить поле 'Зона 1'.");
                                    }
                                    else
                                    {
                                        if (tsZone2.Trim() == "")
                                        {
                                            MessageBox.Show("Необходимо заполнить поле 'Зона 2'.");
                                        }
                                        else
                                        {
                                            if (tsZone3.Trim() == "")
                                            {
                                                MessageBox.Show("Необходимо заполнить поле 'Зона 3'.");
                                            }
                                            else
                                            {
                                                if (tsDopKm.Trim() == "")
                                                {
                                                    MessageBox.Show("Необходимо заполнить поле 'Доп. км'.");
                                                }
                                                else
                                                {
                                                    MySqlCommand msc = new MySqlCommand();
                                                    msc.CommandText = "SELECT `pk_driver`  FROM `Driver`  WHERE `nomber_driver`  = '" + driverNumber + "' AND `fio_driver` = '" + driverFIO + "'";
                                                    msc.Connection = ConnectionToMySQL;
                                                    MySqlDataReader dataReader = msc.ExecuteReader();
                                                    String driverPk = null;
                                                    while (dataReader.Read())
                                                    {
                                                        driverPk = dataReader[0].ToString();
                                                    }
                                                    dataReader.Close();

                                                    msc.CommandText = "DELETE FROM `Car` WHERE `pk_driver` = '" + driverPk + "' AND `mark_car` = '" + tsMark + "' AND `regist_number` = '" + tsNumber + "'";
                                                    msc.Connection = ConnectionToMySQL;
                                                    msc.ExecuteNonQuery();

                                                    MessageBox.Show("Удаление записи успешно произведено.");

                                                    this.carTableAdapter.Fill(this.testDataSet.Car);

                                                    buttonDriverDelete.Enabled = false;
                                                    // Проверка на то, что ТС не производит доставку,с активным, либо неактивным статусом
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
