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
        //List<String> trucksTonnage = new List<String>();    // Тоннаж машин, подходящих для доставки

        // Обеспечивает блокировку части формы, которая не отвечает за выбор материала доставки
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

        // Рассчет тоннажа машины
        public double truckTonnage(String truck)
        {
            String[] splitTruck = truck.Split('(', ')');
            String[] tonnage = splitTruck[2].Split('т');
            return Convert.ToDouble(tonnage[0]);
        }

        // Рассчет тоннажа мешков
        public double materialTonnage(double  materialTonn)
        {
            if (materialTonn < 1)
            {
                return 1;
            }
            for (int i = 1; i < 100; i++)
            {

                if (materialTonn >= i && materialTonn < (i + 0.5))
                {
                    double difference =  materialTonn - i;
                    materialTonn = i + difference;
                    break;
                }
                if (materialTonn >= (i + 0.5) && materialTonn <= (i + 1))
                {
                    materialTonn = i + 1;
                    break;
                }
            }
            return materialTonn;
        }

        // Рассчет для первой машины
        public void resultTonnageFirstTruck()
        {
            //
            double materialTonnFirstTruck = Convert.ToDouble(numericUpDown4.Value);
            String selectedTruck = comboBox3.SelectedItem.ToString();
            double truckTonn = truckTonnage(selectedTruck);
            //numericUpDown4.Value = 0;
            if (materialTonnFirstTruck == 0)
            {
                textBox2.Text = "0";
            }
            else
            {
                if (materialTonnFirstTruck <= truckTonn)
                {
                    textBox2.Text = "1";
                }
                else {
                    int countTrip = 0;
                    if ((materialTonnFirstTruck % truckTonn) > 0)
                    {
                        countTrip = (int)(materialTonnFirstTruck / truckTonn) + 1;
                    }
                    else
                    {
                        countTrip = (int)(materialTonnFirstTruck / truckTonn);
                    }
                    textBox2.Text = Convert.ToString(countTrip);
                }
            }
            //
        }

        // Рассчет для второй машины
        public void resultTonnageSecondTruck()
        {
            //
            if (comboBox4.SelectedItem != null)
            {
                double materialTonnSecondTruck = Convert.ToDouble(numericUpDown5.Value);
                String selectedTruck = comboBox4.SelectedItem.ToString();
                double truckTonn = truckTonnage(selectedTruck);
                //numericUpDown4.Value = 0;
                if (materialTonnSecondTruck == 0)
                {
                    textBox3.Text = "0";
                }
                else
                {
                    if (materialTonnSecondTruck <= truckTonn)
                    {
                        textBox3.Text = "1";
                    }
                    else
                    {
                        int countTrip = 0;
                        if ((materialTonnSecondTruck % truckTonn) > 0)
                        {
                            countTrip = (int)(materialTonnSecondTruck / truckTonn) + 1;
                        }
                        else
                        {
                            countTrip = (int)(materialTonnSecondTruck / truckTonn);
                        }
                        textBox3.Text = Convert.ToString(countTrip);
                    }
                }
            }
            //
        }

        // Рассчет количества рейсов для машины
        public void resultTonnage()
        {
            if (radioButton4.Checked == true)
            {
                // Доставка двумя машинами
                if (tabControl1.SelectedTab == tabPage1)
                {
                    double materialTonn = Convert.ToDouble(numericUpDown1.Value);

                    numericUpDown4.Maximum = Convert.ToDecimal(materialTonn);
                    numericUpDown5.Maximum = Convert.ToDecimal(materialTonn);

                    numericUpDown4.Value = Convert.ToDecimal(materialTonn / 2);
                    numericUpDown5.Value = Convert.ToDecimal(materialTonn / 2);
                    // Рассчет рейсов для первой машины
                    double materialTonnFirstTruck = Convert.ToDouble(numericUpDown4.Value);
                    if (comboBox3.SelectedItem != null)
                    {
                        String selectedTruck = comboBox3.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);

                        if (materialTonnFirstTruck == 0)
                        {
                            textBox2.Text = "0";
                        }
                        else
                        {
                            if (materialTonnFirstTruck <= truckTonn)
                            {
                                textBox2.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonnFirstTruck % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonnFirstTruck / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonnFirstTruck / truckTonn);
                                }
                                textBox2.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
                    // Рассчет рейсов для второй машины
                    if (comboBox4.SelectedItem != null)
                    {
                        double materialTonnSecondTruck = Convert.ToDouble(numericUpDown5.Value);
                        String selectedTruck = comboBox4.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);

                        if (materialTonnSecondTruck == 0)
                        {
                            textBox3.Text = "0";
                        }
                        else
                        {
                            if (materialTonnSecondTruck <= truckTonn)
                            {
                                textBox3.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonnSecondTruck % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonnSecondTruck / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonnSecondTruck / truckTonn);
                                }
                                textBox3.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
                    //

                }
                if (tabControl1.SelectedTab == tabPage3)
                {
                    double materialTonn = Convert.ToDouble(numericUpDown2.Value) * 0.05;

                    numericUpDown4.Maximum = Convert.ToDecimal(materialTonn);
                    numericUpDown5.Maximum = Convert.ToDecimal(materialTonn);

                    double halfMaterialTruck = materialTonn / 2;
                    numericUpDown4.Value = Convert.ToDecimal(halfMaterialTruck);
                    numericUpDown5.Value = Convert.ToDecimal(halfMaterialTruck);
                    // Рассчет рейсов для первой машины
                    double materialTonnFirstTruck = Convert.ToDouble(numericUpDown4.Value);
                    if (comboBox3.SelectedItem != null)
                    {
                        String selectedTruck = comboBox3.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);

                        if (materialTonnFirstTruck == 0)
                        {
                            textBox2.Text = "0";
                        }
                        else
                        {
                            if (materialTonnFirstTruck <= truckTonn)
                            {
                                textBox2.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonnFirstTruck % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonnFirstTruck / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonnFirstTruck / truckTonn);
                                }
                                textBox2.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
                    // Рассчет рейсов для второй машины
                    if (comboBox4.SelectedItem != null)
                    {
                        double materialTonnSecondTruck = Convert.ToDouble(numericUpDown5.Value);
                        String selectedTruck = comboBox4.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);

                        if (materialTonnSecondTruck == 0)
                        {
                            textBox3.Text = "0";
                        }
                        else
                        {
                            if (materialTonnSecondTruck <= truckTonn)
                            {
                                textBox3.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonnSecondTruck % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonnSecondTruck / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonnSecondTruck / truckTonn);
                                }
                                textBox3.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
                    //
                }
            }
            else
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                    double materialTonn = Convert.ToDouble(numericUpDown1.Value);
                    if (comboBox3.SelectedItem != null)
                    {
                        String selectedTruck = comboBox3.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);
                        numericUpDown4.Value = 0;
                        if (materialTonn == 0)
                        {
                            textBox2.Text = "0";
                        }
                        else
                        {
                            if (materialTonn <= truckTonn)
                            {
                                textBox2.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonn % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonn / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonn / truckTonn);
                                }
                                textBox2.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
                }
                if (tabControl1.SelectedTab == tabPage3)
                {
                    double materialTonn = Convert.ToDouble(numericUpDown2.Value) * 0.05;
                    materialTonn = materialTonnage(materialTonn);
                    if (comboBox3.SelectedItem != null)
                    {
                        String selectedTruck = comboBox3.SelectedItem.ToString();
                        double truckTonn = truckTonnage(selectedTruck);
                        numericUpDown4.Value = 0;
                        if (materialTonn == 0)
                        {
                            textBox2.Text = "0";
                        }
                        else
                        {
                            if (materialTonn <= truckTonn)
                            {
                                textBox2.Text = "1";
                            }
                            else
                            {
                                int countTrip = 0;
                                if ((materialTonn % truckTonn) > 0)
                                {
                                    countTrip = (int)(materialTonn / truckTonn) + 1;
                                }
                                else
                                {
                                    countTrip = (int)(materialTonn / truckTonn);
                                }
                                textBox2.Text = Convert.ToString(countTrip);
                            }
                        }
                    }
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
            int firmProcent = 15;
            textBox9.Text = Convert.ToString((int)(result / firmProcent));
        }

        //возвращает массив ключей автомобилей, которые могут перевозить насыпь
        public List<String> bulkCars()
        {
            List<String> cars = new List<String>();
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
            return cars;
        }

        //возвращает массив ключей машин, которые могут перевозить мешки
        public List<String> bagCars()
        {
            List<String> cars = new List<String>();
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
            return cars;
        }

        
        public List<Tuple<String,String>> compactCars(List<String> cars)
        {
            List<String> instructions = new List<String>();
            List<Tuple<String, String>> rezult = new List<Tuple<String, String>>();
            MySqlCommand msc = new MySqlCommand();
            foreach (String car in cars)
            {
                instructions.Clear();
                msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                while (dataReader.Read())
                {
                    instructions.Add(dataReader[0].ToString());
                    //MessageBox.Show(dataReader[0].ToString());
                }
                dataReader.Close();
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
                        /*trucks.Add(truck);
                        trucksKey.Add(car);*/
                        rezult.Add(new Tuple<String,String>(truck,car));
                    }
                }

            }
            return rezult;
        }

        public List<Tuple<String, String>> tipperCars(List<String> cars)
        {
            List<String> instructions = new List<String>();
            List<Tuple<String, String>> rezult = new List<Tuple<String, String>>();
            MySqlCommand msc = new MySqlCommand();
            foreach (String car in cars)
            {
                instructions.Clear();
                msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                while (dataReader.Read())
                {
                    instructions.Add(dataReader[0].ToString());
                    //MessageBox.Show(dataReader[0].ToString());
                }
                dataReader.Close();
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
                        /*trucks.Add(truck);
                        trucksKey.Add(car);*/
                        rezult.Add(new Tuple<String, String>(truck, car));
                    }
                }

            }
            return rezult;
        }

        public List<Tuple<String,String>> allCars()
        {
            List<Tuple<String, String>> rezult = new List<Tuple<String, String>>();
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT mark_car,regist_number, tonnage, pk_car  FROM Car  ";
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
                rezult.Add(new Tuple<String, String>(truck, carKey));
            }
            dataReader.Close();
            return rezult;
        }

        public void resultCar()
        {
            comboBox3.Items.Clear();
            trucks.Clear();
            trucksKey.Clear();
            //trucksTonnage.Clear();
            List<String> cars = new List<String>();
            bool bulk = false;  //заказ на груз насыпью
            bool bag = false;   //заказ на груз в мешках
            List<Tuple<String, String>> rezult;

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
            if (bag)
            {
                cars = bagCars();
            }
            else
            {
                cars = bulkCars();
            }
            List<Tuple<String, String>> rezultCompact = null;
            List<Tuple<String, String>> rezultTipper = null;
            if (compact)
            {
                rezultCompact = compactCars(cars);
            }
            if (tipper)
            {
                rezultTipper = tipperCars(cars);
            }
            trucks.Clear();
            trucksKey.Clear();
            if (compact && tipper)
            {
                foreach(var com in rezultCompact)
                {
                    if (rezultTipper.Contains(com))
                    {
                        trucks.Add(com.Item1);
                        trucksKey.Add(com.Item2);
                        
                    }
                }
                resultTrucks();
                return;
            }
            if (compact)
            {
                foreach (var com in rezultCompact)
                {
                    trucks.Add(com.Item1);
                    trucksKey.Add(com.Item2);
                    
                }
                resultTrucks();
                return;
            }
            if (tipper)
            {
                foreach (var tip in rezultTipper)
                {
                    trucks.Add(tip.Item1);
                    trucksKey.Add(tip.Item2);
                }
                resultTrucks();
                return;
            }
            List<Tuple<String, String>> rezultAll = allCars();
            foreach (var car in rezultAll)
            {
                trucks.Add(car.Item1);
                trucksKey.Add(car.Item2);
                
            }
            resultTrucks();
            return;
        }


        /*public void resultCar()
        {
            comboBox3.Items.Clear();
            trucks.Clear();
            trucksKey.Clear();
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
                            //trucksTonnage.Add(tonnage);
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
                                //trucksTonnage.Add(tonnage);
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
                                    //trucksTonnage.Add(tonnage);
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
                                //trucksTonnage.Add(tonnage);
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
                            //trucksTonnage.Add(tonnage);
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
                                //trucksTonnage.Add(tonnage);
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
                                    //trucksTonnage.Add(tonnage);
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
                                //trucksTonnage.Add(tonnage);
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
        }*/

        public void insertMaterial()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT name FROM Material";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            //String car = null;
            List<String> materials = new List<String>();
            while (dataReader.Read())
            {
                materials.Add(dataReader[0].ToString());
                //MessageBox.Show(dataReader[0].ToString());
            }
            dataReader.Close();
            foreach (var material in materials)
            {
                comboBox1.Items.Add(material);
                comboBox2.Items.Add(material);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        public Form3()
        {
            String serverName = "127.0.0.1"; // Адрес сервера (для локальной базы пишите "localhost")
            string userName = "dbadmin"; // Имя пользователя
            string dbName = "Test"; //Имя базы данных
            string port = "6565"; // Порт для подключения
            //string port = "9570"; // Порт для подключения
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
            /*List<Tuple<String, String>> cars = allCars();
            foreach(var car in cars)
            {
                trucks.Add(car.Item1);
                trucksKey.Add(car.Item2);
            }*/
            resultCar();
            resultTrucks();
            insertMaterial();
            //
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.Height = 705;
            panel5.Location = new Point(14, 409);
            panel4.Visible = true;
            numericUpDown4.Enabled = true;
            label7.Enabled = true;
            //
            resultCar();
            //
            //
            resultTonnage();
            //
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = false;
            label7.Enabled = false;
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
                dataReader.Close();
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
            //
            resultTonnage();
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
            //
            resultTonnage();
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
            //
            resultTonnage();
            //
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //
            resultCar();
            //
            //
            resultTonnage();
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
                //
                resultTonnage();
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
            
            if (tabControl1.SelectedTab == tabPage1)
            {
                numericUpDown4.Maximum = numericUpDown1.Value;
                double materialTonn = Convert.ToDouble(numericUpDown1.Value);
                if (Convert.ToDouble(numericUpDown4.Value) <= materialTonn)
                {
                    numericUpDown5.Value = Convert.ToDecimal(materialTonn - Convert.ToDouble(numericUpDown4.Value));
                }
                //
                resultTonnageFirstTruck();
                //
            }
            if (tabControl1.SelectedTab == tabPage3)
            {
                
                double materialTonn = Convert.ToDouble(numericUpDown2.Value) * 0.05;
                numericUpDown4.Maximum = Convert.ToDecimal(materialTonn);
                //materialTonn = materialTonnage(materialTonn);
                
                if (Convert.ToDouble(numericUpDown4.Value) <= materialTonn)
                {
                    numericUpDown5.Value = Convert.ToDecimal(materialTonn - Convert.ToDouble(numericUpDown4.Value));
                }
                //
                resultTonnageFirstTruck();
                //
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            resultTonnage();
            //
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                numericUpDown5.Maximum = numericUpDown1.Value;
                double materialTonn = Convert.ToDouble(numericUpDown1.Value);
                if (Convert.ToDouble(numericUpDown5.Value) <= materialTonn)
                {
                    numericUpDown4.Value = Convert.ToDecimal(materialTonn - Convert.ToDouble(numericUpDown5.Value));
                }
                //
                resultTonnageSecondTruck();
                //
            }
            if (tabControl1.SelectedTab == tabPage3)
            {
                
                double materialTonn = Convert.ToDouble(numericUpDown2.Value) * 0.05;
                numericUpDown4.Maximum = Convert.ToDecimal(materialTonn);
                //materialTonn = materialTonnage(materialTonn);
                if (Convert.ToDouble(numericUpDown5.Value) <= materialTonn)
                {
                    numericUpDown4.Value = Convert.ToDecimal(materialTonn - Convert.ToDouble(numericUpDown5.Value));
                }
                //
                resultTonnageSecondTruck();
                //
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
