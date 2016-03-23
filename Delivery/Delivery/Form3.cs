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

    public partial class Form3 : Form
    {

        MySqlConnection ConnectionToMySQL;

        double oneWorkerSale = 300; //Цена одного грузчика

        double materialCost = 0;    //Общая стоимость материалов
        double workerCost = 0;      //Общая стоимость грузчиков
        double truckCost = 0;       //Общая стоимость машин

        double tonnCost = 0;    //Цена за тонну
        double bagCost = 0;     //Цена за мешок

        bool checkMaterial = false;

        List<String> trucks = new List<String>();   // Машины, подходящие для доставки
        List<String> trucksKey = new List<String>();    // Первичные ключи машин, подходящих для доставки
        List<String> trucksTonnage = new List<String>();    // Тоннаж машин, подходящих для доставки

        public void changeEnabled()
        {
            if (checkMaterial)
            {
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;
                panel4.Enabled = true;
                panel5.Enabled = true;
                label5.Enabled = true;
                checkBox1.Enabled = true;
                if (checkBox1.Checked)
                {
                    numericUpDown3.Enabled = true;
                }
                else
                {
                    numericUpDown3.Enabled = false;
                }
            }
            else
            {
                panel1.Enabled = false;
                panel2.Enabled = false;
                panel3.Enabled = false;
                panel4.Enabled = false;
                panel5.Enabled = false;
                label5.Enabled = false;
                checkBox1.Enabled = false;
                numericUpDown3.Enabled = false;
            }
        }

        public void resultTonnage()
        {
            if (radioButton4.Checked == true)
            {

            }
            else
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                }
            }
        }

        // Заполнение combobox доступными машинами
        public void resultTrucks()
        {
            if (radioButton4.Checked == true)
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                comboBox4.Items.Clear();
                comboBox4.Text = "";
                for (int i = 0; i < trucks.Count; i++)
                {
                    if (i == 0)
                    {
                        comboBox3.Items.Add(trucks.ElementAt(i));
                        comboBox3.SelectedIndex = 0;
                    }
                    else
                    {
                        if (i == 1)
                        {
                            comboBox4.Items.Add(trucks.ElementAt(i));
                            comboBox4.SelectedIndex = 0;
                        }
                        else
                        {
                            comboBox3.Items.Add(trucks.ElementAt(i));
                            comboBox4.Items.Add(trucks.ElementAt(i));
                        }
                    }
                           
                }
            }
            else
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                foreach (String truck in trucks)
                {
                    comboBox3.Items.Add(truck);
                }
                comboBox3.SelectedIndex = 0;
            }
        }

        // Расчет стоимости заказа
        public void resultCost()
        {
            double result = 0;
            result += materialCost;
            result += workerCost;
            result += truckCost;
            textBox8.Text = Convert.ToString(result);
        }

        // Выбор машин, удовлетворяющих требованиям 
        public void resultCar()
        {
            comboBox3.Items.Clear();
            trucks.Clear();
            trucksKey.Clear();
            trucksTonnage.Clear();
            List<String> cars = new List<String>();
            bool bulk = false;  //заказ на груз насыпью
            bool bag = false;   //заказ на груз в мешках

            bool compact = false;  // требование на малогабаритное ТС
            bool tipper = false;   // требование на самосвал

            if (tabControl1.SelectedTab == tabPage1)
            {
                bulk = true;
            }
            else
            {
                bag = true;
            }
            if (checkBox2.Checked == true)
            {
                compact = true;
            }
            if (checkBox3.Checked == true)
            {
                tipper = true;
            }
            if (bulk) //Насыпь
            {
                if (compact && tipper) // Малогабаритный и самосвал
                {
                    MySqlCommand msc = new MySqlCommand();
                    msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bulk  = 1";
                    msc.Connection = ConnectionToMySQL;
                    MySqlDataReader dataReader = msc.ExecuteReader();
                    //String car = null;
                    while (dataReader.Read())
                    {
                        cars.Add(dataReader[0].ToString());
                        //MessageBox.Show(dataReader[0].ToString());
                    }
                    dataReader.Close();
                    foreach (String car in cars)
                    {
                        List<String> instructions = new List<String>();
                        msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        while (dataReader.Read())
                        {
                            instructions.Add(dataReader[0].ToString());
                            //MessageBox.Show(dataReader[0].ToString());
                        }
                        dataReader.Close();

                        bool instructionFirst = false;
                        bool instructionSecond = false;

                        foreach (String instruction in instructions)
                        {
                            msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            String instructionName = null;
                            while (dataReader.Read())
                            {
                                instructionName = dataReader[0].ToString();
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();
                            if (instructionName == "Compact")
                            {
                                instructionFirst = true;
                            }
                            if (instructionName == "Tipper")
                            {
                                instructionSecond = true;
                            }
                        }
                        if (instructionFirst && instructionSecond)
                        {
                            msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            String carName = null;
                            String regNumber = null;
                            String tonnage = null;
                            while (dataReader.Read())
                            {
                                carName = dataReader[0].ToString();
                                regNumber = dataReader[1].ToString();
                                tonnage = dataReader[2].ToString();
                            }
                            dataReader.Close();
                            String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                            //MessageBox.Show(truck);
                            trucks.Add(truck);
                            trucksKey.Add(car);
                            trucksTonnage.Add(tonnage);
                            //comboBox3.Items.Add(truck);
                            //comboBox3.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    if (compact) // Малогабаритный
                    {
                        MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bulk  = 1";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        while (dataReader.Read())
                        {
                            cars.Add(dataReader[0].ToString());
                            //MessageBox.Show(dataReader[0].ToString());
                        }
                        dataReader.Close();
                        foreach (String car in cars)
                        {
                            List<String> instructions = new List<String>();
                            msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            while (dataReader.Read())
                            {
                                instructions.Add(dataReader[0].ToString());
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();

                            bool instructionFirst = false;

                            foreach (String instruction in instructions)
                            {
                                msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                String instructionName = null;
                                while (dataReader.Read())
                                {
                                    instructionName = dataReader[0].ToString();
                                    //MessageBox.Show(dataReader[0].ToString());
                                }
                                dataReader.Close();
                                if (instructionName == "Compact")
                                {
                                    instructionFirst = true;
                                }
                            }
                            if (instructionFirst)
                            {
                                msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                String carName = null;
                                String regNumber = null;
                                String tonnage = null;
                                while (dataReader.Read())
                                {
                                    carName = dataReader[0].ToString();
                                    regNumber = dataReader[1].ToString();
                                    tonnage = dataReader[2].ToString();
                                }
                                dataReader.Close();
                                String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                //MessageBox.Show(truck);
                                trucks.Add(truck);
                                trucksKey.Add(car);
                                trucksTonnage.Add(tonnage);
                                //comboBox3.Items.Add(truck);
                                //comboBox3.SelectedIndex = 0;
                            }
                        }
                    }
                    else 
                    {
                        if (tipper)  // Самосвал
                        {
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bulk  = 1";
                            msc.Connection = ConnectionToMySQL;
                            MySqlDataReader dataReader = msc.ExecuteReader();
                            //String car = null;
                            while (dataReader.Read())
                            {
                                cars.Add(dataReader[0].ToString());
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();
                            foreach (String car in cars)
                            {
                                List<String> instructions = new List<String>();
                                msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                while (dataReader.Read())
                                {
                                    instructions.Add(dataReader[0].ToString());
                                    //MessageBox.Show(dataReader[0].ToString());
                                }
                                dataReader.Close();

                                bool instructionSecond = false;

                                foreach (String instruction in instructions)
                                {
                                    msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                                    msc.Connection = ConnectionToMySQL;
                                    dataReader = msc.ExecuteReader();
                                    String instructionName = null;
                                    while (dataReader.Read())
                                    {
                                        instructionName = dataReader[0].ToString();
                                        //MessageBox.Show(dataReader[0].ToString());
                                    }
                                    dataReader.Close();
                                    if (instructionName == "Tipper")
                                    {
                                        instructionSecond = true;
                                    }
                                }
                                if (instructionSecond)
                                {
                                    msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                                    msc.Connection = ConnectionToMySQL;
                                    dataReader = msc.ExecuteReader();
                                    String carName = null;
                                    String regNumber = null;
                                    String tonnage = null;
                                    while (dataReader.Read())
                                    {
                                        carName = dataReader[0].ToString();
                                        regNumber = dataReader[1].ToString();
                                        tonnage = dataReader[2].ToString();
                                    }
                                    dataReader.Close();
                                    String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                    //MessageBox.Show(truck);
                                    trucks.Add(truck);
                                    trucksKey.Add(car);
                                    trucksTonnage.Add(tonnage);
                                    //comboBox3.Items.Add(truck);
                                    //comboBox3.SelectedIndex = 0;
                                }
                            }
                        }
                        else  // Требований к ТС нет
                        {
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "SELECT mark_car,regist_number, tonnage, pk_car  FROM Car  WHERE delivery_bulk  = 1";
                            msc.Connection = ConnectionToMySQL;
                            MySqlDataReader dataReader = msc.ExecuteReader();
                            String carName = null;
                            String regNumber = null;
                            String tonnage = null;
                            String carKey = null;
                            while (dataReader.Read())
                            {
                                carName = dataReader[0].ToString();
                                regNumber = dataReader[1].ToString();
                                tonnage = dataReader[2].ToString();
                                carKey = dataReader[3].ToString();
                                String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                //MessageBox.Show(truck);
                                trucks.Add(truck);
                                trucksKey.Add(carKey);
                                trucksTonnage.Add(tonnage);
                                //comboBox3.Items.Add(truck);
                            }
                            dataReader.Close();
                            //comboBox3.SelectedIndex = 0;
                        }
                    }
                }
            }
            else //Мешками
            {
                if (compact && tipper) // Малогабаритный и самосвал
                {
                    MySqlCommand msc = new MySqlCommand();
                    msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bag  = 1";
                    msc.Connection = ConnectionToMySQL;
                    MySqlDataReader dataReader = msc.ExecuteReader();
                    //String car = null;
                    while (dataReader.Read())
                    {
                        cars.Add(dataReader[0].ToString());
                        //MessageBox.Show(dataReader[0].ToString());
                    }
                    dataReader.Close();
                    foreach (String car in cars)
                    {
                        List<String> instructions = new List<String>();
                        msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                        msc.Connection = ConnectionToMySQL;
                        dataReader = msc.ExecuteReader();
                        while (dataReader.Read())
                        {
                            instructions.Add(dataReader[0].ToString());
                            //MessageBox.Show(dataReader[0].ToString());
                        }
                        dataReader.Close();

                        bool instructionFirst = false;
                        bool instructionSecond = false;

                        foreach (String instruction in instructions)
                        {
                            msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            String instructionName = null;
                            while (dataReader.Read())
                            {
                                instructionName = dataReader[0].ToString();
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();
                            if (instructionName == "Compact")
                            {
                                instructionFirst = true;
                            }
                            if (instructionName == "Tipper")
                            {
                                instructionSecond = true;
                            }
                        }
                        if (instructionFirst && instructionSecond)
                        {
                            msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            String carName = null;
                            String regNumber = null;
                            String tonnage = null;
                            while (dataReader.Read())
                            {
                                carName = dataReader[0].ToString();
                                regNumber = dataReader[1].ToString();
                                tonnage = dataReader[2].ToString();
                            }
                            dataReader.Close();
                            String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                            //MessageBox.Show(truck);
                            trucks.Add(truck);
                            trucksKey.Add(car);
                            trucksTonnage.Add(tonnage);
                            //comboBox3.Items.Add(truck);
                            //comboBox3.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    if (compact) // Малогабаритный
                    {
                        MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bag  = 1";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        while (dataReader.Read())
                        {
                            cars.Add(dataReader[0].ToString());
                            //MessageBox.Show(dataReader[0].ToString());
                        }
                        dataReader.Close();
                        foreach (String car in cars)
                        {
                            List<String> instructions = new List<String>();
                            msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                            msc.Connection = ConnectionToMySQL;
                            dataReader = msc.ExecuteReader();
                            while (dataReader.Read())
                            {
                                instructions.Add(dataReader[0].ToString());
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();

                            bool instructionFirst = false;

                            foreach (String instruction in instructions)
                            {
                                msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                String instructionName = null;
                                while (dataReader.Read())
                                {
                                    instructionName = dataReader[0].ToString();
                                    //MessageBox.Show(dataReader[0].ToString());
                                }
                                dataReader.Close();
                                if (instructionName == "Compact")
                                {
                                    instructionFirst = true;
                                }
                            }
                            if (instructionFirst)
                            {
                                msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                String carName = null;
                                String regNumber = null;
                                String tonnage = null;
                                while (dataReader.Read())
                                {
                                    carName = dataReader[0].ToString();
                                    regNumber = dataReader[1].ToString();
                                    tonnage = dataReader[2].ToString();
                                }
                                dataReader.Close();
                                String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                //MessageBox.Show(truck);
                                trucks.Add(truck);
                                trucksKey.Add(car);
                                trucksTonnage.Add(tonnage);
                                //comboBox3.Items.Add(truck);
                                //comboBox3.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        if (tipper)  // Самосвал
                        {
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "SELECT pk_car  FROM Car  WHERE delivery_bag  = 1";
                            msc.Connection = ConnectionToMySQL;
                            MySqlDataReader dataReader = msc.ExecuteReader();
                            //String car = null;
                            while (dataReader.Read())
                            {
                                cars.Add(dataReader[0].ToString());
                                //MessageBox.Show(dataReader[0].ToString());
                            }
                            dataReader.Close();
                            foreach (String car in cars)
                            {
                                List<String> instructions = new List<String>();
                                msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                                msc.Connection = ConnectionToMySQL;
                                dataReader = msc.ExecuteReader();
                                while (dataReader.Read())
                                {
                                    instructions.Add(dataReader[0].ToString());
                                    //MessageBox.Show(dataReader[0].ToString());
                                }
                                dataReader.Close();

                                bool instructionSecond = false;

                                foreach (String instruction in instructions)
                                {
                                    msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + instruction + "'";
                                    msc.Connection = ConnectionToMySQL;
                                    dataReader = msc.ExecuteReader();
                                    String instructionName = null;
                                    while (dataReader.Read())
                                    {
                                        instructionName = dataReader[0].ToString();
                                        //MessageBox.Show(dataReader[0].ToString());
                                    }
                                    dataReader.Close();
                                    if (instructionName == "Tipper")
                                    {
                                        instructionSecond = true;
                                    }
                                }
                                if (instructionSecond)
                                {
                                    msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + car + "'";
                                    msc.Connection = ConnectionToMySQL;
                                    dataReader = msc.ExecuteReader();
                                    String carName = null;
                                    String regNumber = null;
                                    String tonnage = null;
                                    while (dataReader.Read())
                                    {
                                        carName = dataReader[0].ToString();
                                        regNumber = dataReader[1].ToString();
                                        tonnage = dataReader[2].ToString();
                                    }
                                    dataReader.Close();
                                    String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                    //MessageBox.Show(truck);
                                    trucks.Add(truck);
                                    trucksKey.Add(car);
                                    trucksTonnage.Add(tonnage);
                                    //comboBox3.Items.Add(truck);
                                    //comboBox3.SelectedIndex = 0;
                                }
                            }
                        }
                        else  // Требований к ТС нет
                        {
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "SELECT mark_car,regist_number, tonnage, pk_car  FROM Car  WHERE delivery_bag  = 1";
                            msc.Connection = ConnectionToMySQL;
                            MySqlDataReader dataReader = msc.ExecuteReader();
                            String carName = null;
                            String regNumber = null;
                            String tonnage = null;
                            String carKey = null;
                            while (dataReader.Read())
                            {
                                carName = dataReader[0].ToString();
                                regNumber = dataReader[1].ToString();
                                tonnage = dataReader[2].ToString();
                                carKey = dataReader[3].ToString();
                                String truck = carName + "(" + regNumber + ") " + tonnage + "т";
                                //MessageBox.Show(truck);
                                //comboBox3.Items.Add(truck);
                                trucks.Add(truck);
                                trucksKey.Add(carKey);
                                trucksTonnage.Add(tonnage);
                            }
                            dataReader.Close();
                            //comboBox3.SelectedIndex = 0;
                        }
                    }
                }
            }
            //
            resultTrucks();
            //
        }

        public Form3()
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
            //
            resultCost();
            //
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.Height = 705;
            panel5.Location = new Point(14, 409);
            panel4.Visible = true;
            //
            resultCar();
            //
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Visible = false;
            this.Height = 625;
            panel5.Location = new Point(14, 328);
            //
            resultCar();
            //
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Material". При необходимости она может быть перемещена или удалена.
            this.materialTableAdapter.Fill(this.testDataSet.Material);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.provider_material". При необходимости она может быть перемещена или удалена.
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String materialName = comboBox1.Text;
            if (materialName != "")
            {
                //MessageBox.Show(materialName);
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_material  FROM Material  WHERE name  = '" + materialName + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String materialNumber = null;
                while (dataReader.Read())
                {
                    materialNumber = dataReader[0].ToString();
                }
                dataReader.Close();
                //MessageBox.Show(materialNumber);
                msc.CommandText = "SELECT cost_tonna  FROM provider_material  WHERE pk_material  = '" + materialNumber + "'";
                msc.Connection = ConnectionToMySQL;
                dataReader = msc.ExecuteReader();
                int count = 0;
                double cost = 0;
                while (dataReader.Read())
                {
                    count++;
                    cost += Convert.ToDouble(dataReader[0].ToString());
                }
                if (count == 0)
                {
                    //
                    checkMaterial = false;
                    changeEnabled();
                    //
                    MessageBox.Show("Товар не найден");
                    label19.Visible = false;
                    label20.Visible = false;
                    numericUpDown1.Value = 1;
                    numericUpDown1.Enabled = false;
                    //
                    materialCost = 0;
                    resultCost();
                    //
                }
                else
                {
                    //
                    checkMaterial = true;
                    changeEnabled();
                    //
                    label19.Visible = true;
                    label20.Visible = true;
                    tonnCost = cost / count;
                    label20.Text = Convert.ToString(tonnCost) + " рублей/тонну";
                    numericUpDown1.Enabled = true;
                    //MessageBox.Show(Convert.ToString(cost / count));
                    //
                    materialCost = Convert.ToDouble(numericUpDown1.Value) * tonnCost;
                    //
                }
                dataReader.Close();
                //
                resultCost();
                //
            }
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionToMySQL.Close();
        }

        //Вкладка "Насыпное"
        private void tabPage1_Leave(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage3)
            {
                label19.Visible = false;
                label20.Visible = false;
                numericUpDown1.Value = 1;
                numericUpDown1.Enabled = false;
                //
                materialCost = 0;
                resultCost();
                //
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String materialName = comboBox2.Text;
            if (materialName != "")
            {
                //MessageBox.Show(materialName);
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_material  FROM Material  WHERE name  = '" + materialName + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String materialNumber = null;
                while (dataReader.Read())
                {
                    materialNumber = dataReader[0].ToString();
                }
                dataReader.Close();
                //MessageBox.Show(materialNumber);
                msc.CommandText = "SELECT cost_bag  FROM provider_material  WHERE pk_material  = '" + materialNumber + "'";
                msc.Connection = ConnectionToMySQL;
                dataReader = msc.ExecuteReader();
                int count = 0;
                double cost = 0;
                while (dataReader.Read())
                {
                    count++;
                    cost += Convert.ToDouble(dataReader[0].ToString());
                }
                if (count == 0)
                {
                    //
                    checkMaterial = false;
                    changeEnabled();
                    //
                    MessageBox.Show("Товар не найден");
                    label21.Visible = false;
                    label22.Visible = false;
                    numericUpDown2.Value = 1;
                    numericUpDown2.Enabled = false;
                    textBox1.Text = Convert.ToString("0.05");
                    //
                    materialCost = 0;
                    resultCost();
                    //
                }
                else
                {
                    //
                    checkMaterial = true;
                    changeEnabled();
                    //
                    label22.Visible = true;
                    label21.Visible = true;
                    bagCost = cost / count;
                    label21.Text = Convert.ToString(bagCost) + " рублей/мешок";
                    numericUpDown2.Enabled = true;
                    materialCost = Convert.ToDouble(numericUpDown2.Value) * bagCost;
                    //MessageBox.Show(Convert.ToString(cost / count));
                }
                dataReader.Close();
                //
                resultCost();
                //
            }
        }

        private void tabPage3_Leave(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                label21.Visible = false;
                label22.Visible = false;
                numericUpDown2.Value = 1;
                numericUpDown2.Enabled = false;
                textBox1.Text = Convert.ToString("0.05");
                //
                materialCost = 0;
                resultCost();
                //
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            double weight = 0.05;
            int countBag = Convert.ToInt32(numericUpDown2.Value);
            textBox1.Text = Convert.ToString(weight * countBag);
            //
            materialCost = countBag * bagCost;
            resultCost();
            //
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 3)
            {
                label12.Visible = true;
                numericUpDown6.Visible = true;
            }
            else
            {
                label12.Visible = false;
                numericUpDown6.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Enabled == true)
            {
                numericUpDown3.Enabled = false;
                //
                workerCost = 0;
                resultCost();
                //
            }
            else
            {
                //
                workerCost = Convert.ToDouble(numericUpDown3.Value) * oneWorkerSale;
                resultCost();
                //
                numericUpDown3.Enabled = true;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //
            materialCost = Convert.ToDouble(numericUpDown1.Value) * tonnCost;
            resultCost();
            //
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            //
            workerCost = Convert.ToDouble(numericUpDown3.Value) * oneWorkerSale;
            resultCost();
            //

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //
            resultCar();
            //
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //
            resultCar();
            //
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            //
            if (numericUpDown2.Enabled == true)
            {
                //
                checkMaterial = true;
                changeEnabled();
                //
                //
                materialCost = Convert.ToDouble(numericUpDown2.Value) * bagCost;
                resultCost();
                //
            }
            else
            {
                //
                checkMaterial = false;
                changeEnabled();
                //
                materialCost = 0;
                resultCost();
                //
                //
            }
            //
            //
            resultCar();
            //
            
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            //
            if (numericUpDown1.Enabled == true)
            {
                //
                checkMaterial = true;
                changeEnabled();
                //
                //
                materialCost = Convert.ToDouble(numericUpDown1.Value) * tonnCost;
                resultCost();
                //
            }
            else
            {
                //
                checkMaterial = false;
                changeEnabled();
                //
                //
                materialCost = Convert.ToDouble(numericUpDown1.Value) * 0;
                resultCost();
                //
            }
            //
            //
            resultCar();
            //
            
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var currentItems = comboBox3.SelectedItem;
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            foreach (String truck in trucks)
            {
                if (!String.Equals(comboBox4.SelectedItem, truck))
                    comboBox3.Items.Add(truck);
            }
            comboBox3.SelectedItem = currentItems;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var currentItems = comboBox4.SelectedItem;
            comboBox4.Items.Clear();
            comboBox4.Text = "";
            foreach (String truck in trucks)
            {
                if (!String.Equals(comboBox3.SelectedItem.ToString(), truck))
                    comboBox4.Items.Add(truck);
            }
            comboBox4.SelectedItem = currentItems;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
