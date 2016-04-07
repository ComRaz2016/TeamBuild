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

    public partial class OrderEdit : Form
    {

        MySqlConnection ConnectionToMySQL;
        Form mainForm;

        double oneWorkerSale = 300; //Цена одного грузчика

        double materialCost = 0;    //Общая стоимость материалов
        double workerCost = 0;      //Общая стоимость грузчиков
        double truckCost = 0;       //Общая стоимость машин

        double tonnCost = 0;    //Цена за тонну
        double bagCost = 0;     //Цена за мешок

        bool checkMaterial = false;
        bool error = false;

        List<String> trucks = new List<String>();   // Машины, подходящие для доставки
        List<String> trucksKey = new List<String>();    // Первичные ключи машин, подходящих для доставки
        List<String> trucksDriver = new List<String>();    // водители машин

        List<String> oldPoly = new List<string>();
        List<int> pk_instr = new List<int>();
        List<int> pk_cars = new List<int>();

        // Получение номера заказа
        public String getOrderNumber()
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT number_order  FROM order_number  WHERE pk_order_number  = 1";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String orderNumber = null;
            while (dataReader.Read())
            {
                orderNumber = dataReader[0].ToString();
            }
            dataReader.Close();
            textBox10.Text = orderNumber;
            return orderNumber;
        }

        // Увеличение номера заказа на единицу, если заказ оформлен
        public void increaseOrderNumber(String orderNumber)
        {
            String nextNumber = Convert.ToString(Convert.ToInt32(orderNumber) + 1);
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "UPDATE order_number  SET number_order = '" + nextNumber + "' WHERE pk_order_number = 1";
            msc.Connection = ConnectionToMySQL;
            msc.ExecuteNonQuery();
        }

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
        //получаем стоимость доставки в машины с пк car в зону zone
        public int getTruckCoastZone(String car, int zone)
        {
            MySqlCommand msc = new MySqlCommand();
            String query;
            switch (zone)
            {
                case 1:
                    query = "Costfistzone";
                    break;
                case 2:
                    query = "Costsecondzone";
                    break;
                case 3:
                    query = "Costthirdzone";
                    break;
                default:
                    query = null;
                    break;
            }
            if (query != null)
            {
                int rez = 0;
                msc.CommandText = "SELECT " + query + " FROM Car  WHERE " + car + " = pk_car";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                while (dataReader.Read())
                {
                    rez = Convert.ToInt32(dataReader[0].ToString());
                }
                dataReader.Close();
                return rez;
            }
            else
            {
                return -1;
            }

        }

        //стоимость доп км машины car за dop километров
        public int getCostDopKm(String car, int dop)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT Costdopkm FROM Car  WHERE " + car + " = pk_car";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            int costDopKm = 0;
            while (dataReader.Read())
            {
                costDopKm = Convert.ToInt32(dataReader[0].ToString());
            }
            dataReader.Close();
            return costDopKm * dop;

        }

        //получает стоимоть автомобиля по ключу car, с учетом выбранной зоны в combobox5 и если зона "Зона 3+", то так же учитывается
        //стоимость доп километров, а так же учитывается количество рейсов kol
        public double getTruckCoas(String car, int kol)
        {
            double rez = 0;
            switch (comboBox5.Text)
            {
                case "Зона 1":
                    rez += getTruckCoastZone(car, 1);
                    break;
                case "Зона 2":
                    rez += getTruckCoastZone(car, 2);
                    break;
                case "Зона 3":
                case "Зона 3+":
                    rez += getTruckCoastZone(car, 3);
                    break;
            }
            if (comboBox5.Text.Equals("Зона 3+"))
                rez += getCostDopKm(car, Convert.ToInt32(numericUpDown6.Value));
            rez *= kol;
            return rez;
        }

        public void calculationTruckCost()
        {
            truckCost = 0;
            if (comboBox3.SelectedItem != null)
            {
                truckCost += getTruckCoas(trucksKey[trucks.IndexOf(comboBox3.Text)], Convert.ToInt32(textBox2.Text));
            }
            if (panel4.Visible == true && comboBox4.SelectedItem != null)
            {
                truckCost += getTruckCoas(trucksKey[trucks.IndexOf(comboBox4.Text)], Convert.ToInt32(textBox3.Text));
            }
        }

        // Рассчет количества рейсов для первой
        public void resultTonnageFirstTruck()
        {
            //
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
            }
            else
            {
                textBox2.Text = "0";
            }
            calculationTruckCost();
            //
        }

        // Рассчет количства рейсов для второй машины
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
            else
            {
                textBox3.Text = "0";
            }
            calculationTruckCost();
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
                    else
                    {
                        textBox2.Text = "0";
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
                    else
                    {
                        textBox3.Text = "0";
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
                    else
                    {
                        textBox2.Text = "0";
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
                    else
                    {
                        textBox3.Text = "0";
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
                    else
                    {
                        textBox2.Text = "0";
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
                    else
                    {
                        textBox2.Text = "0";
                }
            }
        }
            calculationTruckCost();
        }

        // Заполнение combobox доступными машинами
        public void resultTrucks()
        {
            if (trucks.Count == 0)
            {
                MessageBox.Show("В данный момент нет свободных машин, которые удолитворяют требованиям.", "Нет машин, удолитворяющих требованиям");
                error = true;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                radioButton4.Checked = false;
                radioButton3.Checked = true;
                error = false;
                resultCar();
                return;
            }
            if (radioButton4.Checked == true)
            {
                IEnumerable<String> rez;
                rez = trucksDriver.Union(trucksDriver);
                if (rez.Count() <= 1)
                {
                     MessageBox.Show("В данный момент нет двух свободных водителей с машинами, которые удолитворяют требованиям.", "Нет машин, удолитворяющих требованиям");
                    error = true;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    radioButton4.Checked = false;
                    radioButton3.Checked = true;
                    error = false;
                    return;
                }
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                comboBox4.Items.Clear();
                comboBox4.Text = "";
                foreach (String truck in trucks)
                {
                    comboBox3.Items.Add(truck);
                }
                if (comboBox3.Items.Count != 0)
                    comboBox3.SelectedIndex = 0;
                foreach (String truck in trucks)
                {
                    int i = trucks.IndexOf(truck);
                    int j = trucks.IndexOf(comboBox3.SelectedItem.ToString());
                    if (trucksDriver[i] != 
                        trucksDriver[j])
                        comboBox4.Items.Add(truck);
                }
                if (comboBox4.Items.Count != 0)
                    comboBox4.SelectedIndex = 0;
                
                List<String> listForDelete = new List<String>();
                foreach (String truck in comboBox3.Items)
                {
                    int i = trucks.IndexOf(truck.ToString());
                    if (comboBox4.SelectedItem != null)
                    {
                        int j = trucks.IndexOf(comboBox4.SelectedItem.ToString());
                        if (trucksDriver[i] == trucksDriver[j])
                        {
                            listForDelete.Add(truck);
                        }
                    }
                }
                foreach(var deleteElem in listForDelete)
                {
                    comboBox3.Items.Remove(deleteElem);
                }
                /*for (int i = 0; i < trucks.Count; i++)
                {
                    comboBox3.Items.Add(trucks.ElementAt(i));
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
                    }*/
                           
                //}
                }
            else
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                foreach (String truck in trucks)
                {
                    comboBox3.Items.Add(truck);
                }
                if (comboBox3.Items.Count != 0)
                    comboBox3.SelectedIndex = 0;
            }
        }

        // Расчет стоимости заказа
        public void resultCost()
        {
            double result = 0;
            result += materialCost;
            result += workerCost;
            calculationTruckCost();
            result += truckCost;
            double firmProcent = 0.15;
            textBox8.Text = Convert.ToString((int)result*(1+ firmProcent)); //мы делаем надбавку вообщето, а не просто отбираем у всех ценников по чуть-чуть
            textBox9.Text = Convert.ToString((int)(result * firmProcent)); //это чему равны наши 15%
        }

        //возвращает массив ключей автомобилей, которые могут перевозить query
        public List<String> instructionCars(String query)
        {
            List<String> cars = new List<String>();
            MySqlCommand msc = new MySqlCommand();
            String q = null;
            if (query == "bag")
                q = "delivery_bag";
            if (query == "bulk")
                q = "delivery_bulk";
            msc.CommandText = "SELECT pk_car  FROM Car  WHERE " + q + " = 1";
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


        //массив пк для требований
        public List<String> getCarInstructionsPk(String car)
        {
            List<String> instructions = new List<String>();
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_instruction  FROM instruction_car  WHERE pk_car  = '" + car + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                instructions.Add(dataReader[0].ToString());
                //MessageBox.Show(dataReader[0].ToString());
            }
            dataReader.Close();
            return instructions;
        }

        //название требования по пк
        public String getInstructionsName(String pk_instr)
        {
            String rezult = null;
            MySqlCommand msc = new MySqlCommand();
            MySqlDataReader dataReader;
            msc.CommandText = "SELECT desc_instruction  FROM instruction  WHERE pk_instruction  = '" + pk_instr + "'";
            msc.Connection = ConnectionToMySQL;
            dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                rezult = dataReader[0].ToString();
                //MessageBox.Show(dataReader[0].ToString());
            }
            dataReader.Close();
            return rezult;
        }

        //Получить строку для вывода машины по ключу
        public String getOutputStringForCar(String pk_car)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT mark_car,regist_number, tonnage  FROM Car  WHERE pk_car  = '" + pk_car + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
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
            return carName + "(" + regNumber + ") " + tonnage + "т";
        }

        public String getPkDriver(String car)
        {
            String rezult = null;
            MySqlCommand msc = new MySqlCommand();
            MySqlDataReader dataReader;
            msc.CommandText = "SELECT pk_driver  FROM Car  WHERE pk_car  = '" + car + "'";
            msc.Connection = ConnectionToMySQL;
            dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                rezult = dataReader[0].ToString();
                //MessageBox.Show(dataReader[0].ToString());
            }
            dataReader.Close();
            return rezult;
        }

        //возвращает картеж, в котором первый элемент - это строка для вывода, второй - пк выведенных машин, третий - пк водилы.
        //машины должны поддерживать требование inst, пк которых передаются в параметре cars
        public List<Tuple<String, String, String>> instructionCars(List<String> cars, String inst)
        {
            List<Tuple<String, String, String>> rezult = new List<Tuple<String, String, String>>();
            foreach (String car in cars)
            {
                List<String> instructions = getCarInstructionsPk(car);
                foreach (String instruction in instructions)
                {
                    String instructionName = getInstructionsName(instruction);
                    if (instructionName == inst)
                    {
                        String truck = getOutputStringForCar(car);
                        String driver = getPkDriver(car);
                        rezult.Add(new Tuple<String, String, String>(truck, car, driver));
                    }
                }

            }
            return rezult;
        }


        //возвращает картеж, в котором первый элемент - это строка для вывода, второй - пк выведенных машин.
        //машины, пк которых передаются в параметре cars
        public List<Tuple<String,String, String>> allCars(List<String> cars)
        {
            List<Tuple<String, String, String>> rezult = new List<Tuple<String, String, String>>();
            MySqlCommand msc = new MySqlCommand();
            foreach(var car in cars)
            {
                String truck = getOutputStringForCar(car);
                String driver = getPkDriver(car);
                rezult.Add(new Tuple<String, String,String>(truck, car, driver));
            }
            return rezult;
        }


        public void resultCar()
        {
            comboBox3.Items.Clear();
            trucks.Clear();
            trucksKey.Clear();
            trucksDriver.Clear();
            //trucksTonnage.Clear();
            List<String> cars = new List<String>();
            bool bulk = false;  //заказ на груз насыпью
            bool bag = false;   //заказ на груз в мешках
            List<Tuple<String, String>> rezult;

            bool compact = false;  // требование на малогабаритное ТС
            bool tipper = false;   // требование на самосвал
            bool onboard = false;  // требование на бортовой автомобиль
            bool selfloader = false;  // требование на самопогрузчик автомобиль

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
            if (checkBox4.Checked == true)
            {
                onboard = true;
            }
            if (checkBox5.Checked == true)
            {
                selfloader = true;
            }
            if (bag)
            {
                cars = instructionCars("bag");
            }
            else
            {
                cars = instructionCars("bulk");
            }
            List<Tuple<String, String, String>> rezultCompact = new List<Tuple<string, string, string>>();
            List<Tuple<String, String, String>> rezultTipper = new List<Tuple<string, string, string>>();
            List<Tuple<String, String, String>> rezultOnboard = new List<Tuple<string, string, string>>();
            List<Tuple<String, String, String>> rezultSelfloader = new List<Tuple<string, string, string>>();
            IEnumerable<Tuple<String, String, String>> rez;
            if (compact)
            {
                rezultCompact = instructionCars(cars,"Compact");
            }
            if (tipper)
            {
                rezultTipper = instructionCars(cars, "Tipper");
            }
            if (onboard)
            {
                rezultOnboard = instructionCars(cars, "Onboard");
            }
            if (selfloader)
            {
                rezultSelfloader = instructionCars(cars, "Selfloader");
            }
            trucks.Clear();
            trucksKey.Clear();
           
            if (!compact && !tipper && !onboard && !selfloader)
            {
                rez = allCars(cars);   
            }
            else
            {
                rez = rezultCompact.Union(rezultOnboard.Union(rezultSelfloader.Union(rezultTipper)));
                if (compact)
                    rez = rez.Intersect(rezultCompact);
                if (onboard)
                    rez = rez.Intersect(rezultOnboard);
                if (tipper)
                    rez = rez.Intersect(rezultTipper);
                if (selfloader)
                    rez = rez.Intersect(rezultSelfloader);

            }
            foreach (var car in rez)
            {
                trucks.Add(car.Item1);
                trucksKey.Add(car.Item2);
                trucksDriver.Add(car.Item3);

            }
            resultTrucks();
            return;       
        }


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
            //comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 0;
        }

        public OrderEdit(MySqlConnection connection,Form form, String num_order)
        {
            ConnectionToMySQL = connection;
            mainForm = form;
            InitializeComponent();
            comboBox5.SelectedIndex = 0;
            //Установка минимальной датой сегодняшнюю дату
            // dateTimePicker1.MinDate = dateTimePicker1.Value.Date;
            //getOrderNumber();
            insertMaterial();
            loadFullForm(num_order);
            /*resultCost();
            resultCar();*/
            //insertMaterial();
        }

        private void loadFullForm(String num_order)
        {
            textBox10.Text = num_order;
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT * FROM `Order` WHERE nomer = " + num_order;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                oldPoly.Add(dataReader[0].ToString());
                oldPoly.Add(dataReader[1].ToString());
                oldPoly.Add(dataReader[2].ToString());
                oldPoly.Add(dataReader[3].ToString());
                oldPoly.Add(dataReader[4].ToString());
                oldPoly.Add(dataReader[5].ToString());
                oldPoly.Add(dataReader[6].ToString());
                oldPoly.Add(dataReader[7].ToString());
                oldPoly.Add(dataReader[8].ToString());
                oldPoly.Add(dataReader[9].ToString());
                oldPoly.Add(dataReader[10].ToString());
                oldPoly.Add(dataReader[11].ToString());
                oldPoly.Add(dataReader[12].ToString());
                oldPoly.Add(dataReader[13].ToString());
                oldPoly.Add(dataReader[14].ToString());
            }
            dataReader.Close();
            if (oldPoly[14] == "1")
            {
                tabPage1.Select();  //насыпь
                comboBox1.SelectedItem = getMaterial(oldPoly[13]);      //материал в доставке
                numericUpDown1.Value = Decimal.Parse(oldPoly[2].ToString());        //сколько
            }
            setInstruction(oldPoly[0]);         //требования к авто в заказе
            setCars(oldPoly[0]);                //заполняем авто в заказе
            if (oldPoly[10].ToString() != "0")  //рабы
            {
                checkBox1.Checked = true;
                numericUpDown3.Value = Decimal.Parse(oldPoly[10].ToString());
            }
            textBox4.Text = oldPoly[5].ToString();
            textBox5.Text = oldPoly[6].ToString();
            textBox6.Text = oldPoly[4].ToString();
            comboBox5.SelectedItem = comboBox5.Items[Int32.Parse(oldPoly[8].ToString()) - 1];
            if (oldPoly[8].ToString() == "4")
            {
                numericUpDown6.Value = Decimal.Parse(oldPoly[9].ToString());
            }
            dateTimePicker1.Value =  DateTime.Parse(oldPoly[3].ToString());
            textBox7.Text = oldPoly[7].ToString();
        }
        
        private void setCars(String pk)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_car FROM `order_car` WHERE pk_order = " + pk;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            while (dataReader.Read())
            {
                pk_cars.Add(Int32.Parse(dataReader[0].ToString()));
            }
            dataReader.Close();
            if (pk_cars.Count == 2)
            {
                radioButton4.Checked = true;
                comboBox3.Text = getOutputStringForCar(pk_cars[0].ToString());
                comboBox4.Text = getOutputStringForCar(pk_cars[1].ToString());
                numericUpDown4.Value = getValumeCar(pk_cars[0].ToString(), pk);
            }
            else
            {
                comboBox3.Text = getOutputStringForCar(pk_cars[0].ToString());
            }
                

            
        }

        private decimal getValumeCar(string pk_car, string pk_order)
        {

            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT volume_car FROM `order_car` WHERE pk_car = " + pk_car + " AND pk_order = " + pk_order + ";";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String rez = "";
            while (dataReader.Read())
            {
               rez = dataReader[0].ToString();
            }
            dataReader.Close();
            return Decimal.Parse(rez);

        }

        private void setInstruction(String pk)
        {
             MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_instruction FROM `order_instruction` WHERE pk_order = " + pk;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
           
            while (dataReader.Read())
            {
                pk_instr.Add(Int32.Parse(dataReader[0].ToString()));
            }
            dataReader.Close();
            foreach(int pk1 in pk_instr)
            {
                switch (pk1)
                {
                    case 1:
                        checkBox2.Checked = true;
                        break;
                    case 2:
                        checkBox3.Checked = true;
                        break;
                    case 3:
                        checkBox4.Checked = true;
                        break;
                    case 4:
                        checkBox5.Checked = true;
                        break;
                }
            }
        }



        private string getMaterial(String pk)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT name FROM `Material` WHERE pk_material = " + pk;
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String rez = "";
            while (dataReader.Read())
            {
                rez = dataReader[0].ToString();
            }
            dataReader.Close();
            return rez;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                this.Height = 705;
                panel5.Location = new Point(14, 409);
                panel4.Visible = true;
                numericUpDown4.Enabled = true;
                label7.Enabled = true;
                numericUpDown4.Visible = true;
                label7.Visible = true;
                //
                resultCar();
                //
                //
                resultTonnage();
                //
                resultCost();
            }
            
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = false;
            label7.Enabled = false;
            numericUpDown4.Visible = false;
            label7.Visible = false;
            panel4.Visible = false;
            this.Height = 625;
            panel5.Location = new Point(14, 328);
            //
            resultCar();
            //
            resultCost();
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
            //ConnectionToMySQL.Close();
            mainForm.Show();
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
               // resultCost();
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
            resultCost();
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
            if (!error)
            {
                //
                resultCar();
                //
                //
                resultTonnage();
                //
                resultCost();
            }
            

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!error)
            {
                //
                resultCar();
                //
                //
                resultTonnage();
                //
                resultCost();
            }

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
                if (!String.Equals(comboBox4.SelectedItem, truck) &&
                     !String.Equals(trucksDriver[trucks.IndexOf(truck)], trucksDriver[trucks.IndexOf(comboBox4.SelectedItem.ToString())]))
                    comboBox3.Items.Add(truck);
            }
            comboBox3.SelectedItem = currentItems;
            resultCost();
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var currentItems = comboBox4.SelectedItem;
            comboBox4.Items.Clear();
            comboBox4.Text = "";
            foreach (String truck in trucks)
            {
                if (!String.Equals(comboBox3.SelectedItem.ToString(), truck) &&
                    !String.Equals(trucksDriver[trucks.IndexOf(truck)], trucksDriver[trucks.IndexOf(comboBox3.SelectedItem.ToString())]))
                    comboBox4.Items.Add(truck);
            }
            comboBox4.SelectedItem = currentItems;
            resultCost();
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
            resultCost();
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
            resultCost();
        }


        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!error)
            {
                //
                resultCar();
                //
                //
                resultTonnage();
                //
                resultCost();
            }

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!error)
            {
                //
                resultCar();
                //
                //
                resultTonnage();
                //
                resultCost();
            }
        }

        // Получение первичного ключа единиц измерения
        public String getMeasurePk()
        {
            // Насыпь
            if (tabControl1.SelectedTab == tabPage1)
            {
                String measure = "Bulk";
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_measure  FROM Measure  WHERE Nazv  = '" + measure + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String measurePk = null;
                while (dataReader.Read())
                {
                    measurePk = dataReader[0].ToString();
                    //MessageBox.Show(measurePk);
                }
                dataReader.Close();
                return measurePk;
            }
            // Мешок
            else
            {
                String measure = "Bag";
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_measure  FROM Measure  WHERE Nazv  = '" + measure + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String measurePk = null;
                while (dataReader.Read())
                {
                    measurePk = dataReader[0].ToString();
                    //MessageBox.Show(measurePk);
                }
                dataReader.Close();
                return measurePk;
            }
        }

        // Получение первичного ключа товара на доставку
        public String getPkStatus()
        {
            
                        String status = "Wait";
                        MySqlCommand msc = new MySqlCommand();
                        msc.CommandText = "SELECT pk_status  FROM order_status  WHERE name_status  = '" + status + "'";
                        msc.Connection = ConnectionToMySQL;
                        MySqlDataReader dataReader = msc.ExecuteReader();
                        String statusPk = null;
                        while (dataReader.Read())
                        {
                            statusPk = dataReader[0].ToString();
                            //MessageBox.Show(materialPk);
                        }
                        dataReader.Close();
                        return statusPk;
                   
            //int differenceMonth = dateTimePicker1.Value.Month.CompareTo(DateTime.Now.Month);
            //if (difference == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    MessageBox.Show("Необходимо изменить время заказа. Минимальное время доставки - 1 час", "Ошибка во времени");
            //    return null;

            //}
            //if (dayOrder == dayNow)
            //{
            //    String status = "Active";
            //    MySqlCommand msc = new MySqlCommand();
            //    msc.CommandText = "SELECT pk_status  FROM order_status  WHERE name_status  = '" + status + "'";
            //    msc.Connection = ConnectionToMySQL;
            //    MySqlDataReader dataReader = msc.ExecuteReader();
            //    String statusPk = null;
            //    while (dataReader.Read())
            //    {
            //        statusPk = dataReader[0].ToString();
            //        //MessageBox.Show(materialPk);
            //    }
            //    dataReader.Close();
            //    return statusPk;
            //}
            //else
            //{
            //    String status = "Inactive";
            //    MySqlCommand msc = new MySqlCommand();
            //    msc.CommandText = "SELECT pk_status  FROM order_status  WHERE name_status  = '" + status + "'";
            //    msc.Connection = ConnectionToMySQL;
            //    MySqlDataReader dataReader = msc.ExecuteReader();
            //    String statusPk = null;
            //    while (dataReader.Read())
            //    {
            //        statusPk = dataReader[0].ToString();
            //        //MessageBox.Show(materialPk);
            //    }
            //    dataReader.Close();
            //    return statusPk;
            //}
        }

        // Получение первичного ключа товара на доставку
        public String getPkMaterial()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                String material = comboBox1.SelectedItem.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_material  FROM Material  WHERE name  = '" + material + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String materialPk = null;
                while (dataReader.Read())
                {
                    materialPk = dataReader[0].ToString();
                    //MessageBox.Show(materialPk);
                }
                dataReader.Close();
                return materialPk;
            }
            else
            {
                String material = comboBox2.SelectedItem.ToString();
                MySqlCommand msc = new MySqlCommand();
                msc.CommandText = "SELECT pk_material  FROM Material  WHERE name  = '" + material + "'";
                msc.Connection = ConnectionToMySQL;
                MySqlDataReader dataReader = msc.ExecuteReader();
                String materialPk = null;
                while (dataReader.Read())
                {
                    materialPk = dataReader[0].ToString();
                    //MessageBox.Show(materialPk);
                }
                dataReader.Close();
                return materialPk;
            }
        }

        // Проверка адреса доставки, ФИО, телефона
        public bool checkAdressFIOTelefone()
        {
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Необходимо ввести Заказчика", "Пустое поле");
                return false;
            }
            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Необходимо ввести Номер телефона", "Пустое поле");
                return false;
            }
            if (textBox6.Text.Trim() == "")
            {
                MessageBox.Show("Необходимо ввести Адрес доставки", "Пустое поле");
                return false;
            }
            return true;
        }

        // Подсчет количества грузчиков
        public String getCountWorkers()
        {
            if (checkBox1.Checked == true)
            {

                return numericUpDown3.Value.ToString();
            }
            else
            {
                return "0";
            }
        }

        // Получение объема заказа
        public String getVolumeOrder()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                return numericUpDown1.Text.Substring(0, numericUpDown1.Text.IndexOf(',')) + "." + numericUpDown1.Text.Substring(numericUpDown1.Text.IndexOf(',') + 1);
            }
            else
            {
                return numericUpDown2.Text.Substring(0, numericUpDown2.Text.IndexOf(',')) + "." + numericUpDown2.Text.Substring(numericUpDown2.Text.IndexOf(',') + 1);
            }
        }

        // Проверка выбора зоны доставки
        public bool checkNumberZone()
        {
            if (comboBox5.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать Зону доставки", "Пустое поле");
                return false;
            }
            else
            {
                return true;
            }
        }

        // Получение зоны доставки
        public String getNumberZone()
        {
            int numberZone = comboBox5.SelectedIndex + 1;
            return numberZone.ToString();
        }

        // Получение дополнительных километров
        public String getExtendedKm()
        {
            if (comboBox5.SelectedIndex == 3)
            {
                return numericUpDown6.Value.ToString();
            }
            else
            {
                return "0";
            }
        }

        public String getDataTime()
        {
            String dateTime = dateTimePicker1.Value.ToString("dd/MM/yyyy HH:mm");
            return dateTime;
        }

        //Получение даты и времени доставки
        public bool checkDataTime()
        {
            int difference = dateTimePicker1.Value.CompareTo(DateTime.Now.AddHours(1));
            //int dateOrder = Convert.ToInt32(dateTimePicker1.Value.ToString("dd/MM/yyyy HH:mm:ss"));//.ToString("dd"));
            //int dateNow = Convert.ToInt32(DateTime.Now.AddHours(1));
            if (difference>=0 )
            {
                return true;
            }
            else
            {
                MessageBox.Show("Необходимо изменить время заказа. Минимальное время доставки - 1 час", "Ошибка во времени");
                return false;

            }
            /* if (dayOrder == dayNow)
             {
                 int hourOrder = Convert.ToInt32(dateTimePicker1.Value.ToString("HH"));
                 int minuteOrder = Convert.ToInt32(dateTimePicker1.Value.ToString("mm"));

                 DateTime nowTime = DateTime.Now.AddHours(1);
                 int hourNow = nowTime.Hour;
                 int minuteNow = nowTime.Minute;
                 if (hourNow > hourOrder)
                 {
                     MessageBox.Show("Необходимо изменить время заказа. Минимальное время доставки - 1 час", "Ошибка в часах");
                     return false;
                 }
                 if (hourNow == hourOrder)
                 {
                     if (minuteNow > minuteOrder)
                     {
                         MessageBox.Show("Необходимо изменить время заказа. Минимальное время доставки - 1 час", "Ошибка в минутах");
                         return false;
                     }
                 }
                 return true;
             }
             else
             {
                 return true;
             }*/

        }

        public bool checkTrucks()
        {
            if (comboBox3.Items.Count == 0)
            {
                MessageBox.Show("Отсутствуют машины, которые могут осуществить доставку","Отсутствие машин");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Получение первичного ключа материала
            String materialPk = getPkMaterial();

            //Получение первичного ключа единиц измерения
            String measurePk = getMeasurePk();

            //Получение номеpa заказа
            String numberOrder = getOrderNumber();

            // Получение объема заказа
            String volumeOrder = getVolumeOrder();

            // Подсчет количества грузчиков
            String countWorkers = getCountWorkers();

            // Стоимость доставки
            String costOrder = textBox8.Text;

            if (checkTrucks())
            {
                if (checkNumberZone())
                {
                    // Получение зоны доставки
                    String numberZone = getNumberZone();

                    // Если Зона 3+, то получить дополнительные км
                    String extendedKm = getExtendedKm();

                    if (checkDataTime())
                    {
                        String pkStatusOrder = getPkStatus();

                        //Получение даты и времени доставки
                        String dataTimeOrder = getDataTime();

                        // Проверка номера телефона, адреса доставки и заказчика
                        if (checkAdressFIOTelefone())
                        {
                            String adressOrder = textBox6.Text;
                            String telefoneOrder = textBox5.Text;
                            String clientOrder = textBox4.Text;
                            String commentOrder = null;
                            if (textBox7.Text.Trim() == "")
                            {
                                commentOrder = null;
                            }
                            else
                            {
                                commentOrder = textBox7.Text;
                            }

                            //
                            MySqlCommand msc = new MySqlCommand();
                            msc.CommandText = "INSERT INTO `Order` (`nomer`, `volume`, `date_time`, `adress`, `contact`, `number_contact`, `comment`, `Numberzone`, `Exstendway`, `worker`, `cost_order`, `pk_status`, `pk_material`, `pk_measure`) VALUES ('" + numberOrder + "', '" + volumeOrder + "', '" + dataTimeOrder + "', '" + adressOrder + "', '" + clientOrder + "', '" + telefoneOrder + "', '" + commentOrder + "', '" + numberZone + "', '" + extendedKm + "', '" + countWorkers + "', '" + costOrder + "', '" + pkStatusOrder + "', '" + materialPk + "', '" + measurePk + "')";
                            msc.Connection = ConnectionToMySQL;
                            msc.ExecuteNonQuery();
                            //Добавление машин в расшивочную таблицу
                            createCarsOrder(numberOrder);
                            createInstructionsOrder(numberOrder);
                            // Увеличение номера заказа
                            increaseOrderNumber(getOrderNumber());
                            MessageBox.Show("Заказ успешно оформлен! Спасибо, что выбрали нас!", "Заказ оформлен");
                            ConnectionToMySQL.Close();
                            mainForm.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        //Добавление машин в расшивочную таблицу
        public void createCarsOrder(String numberOrder)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_order FROM `Order` WHERE `nomer`  = '" + numberOrder + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String orderPk = null;
            while (dataReader.Read())
            {
                orderPk = dataReader[0].ToString();
            }
            dataReader.Close();
            if (orderPk == null)
            {
                MessageBox.Show("Ошибка при добавлении заказа");
            }
            else
            {
                String keyFirstTruck = null;
                String keySecondTruck = null;

                String countTripFirstTruck = null;
                String countTripSecondTruck = null;


                String volumeFistTruck = null;
                String volumeSecondTruck = null;

                if (radioButton4.Checked == true)
                {
                    if (comboBox3.SelectedItem != null)
                    {
                        countTripFirstTruck = textBox2.Text;
                        keyFirstTruck = trucksKey[trucks.IndexOf(comboBox3.SelectedItem.ToString())];
                        volumeFistTruck = numericUpDown4.Text.Substring(0, numericUpDown4.Text.IndexOf(',')) + "." + numericUpDown4.Text.Substring(numericUpDown4.Text.IndexOf(',') + 1);
                        msc.CommandText = "INSERT INTO `order_car` (`pk_car`, `pk_order`, `count_trip`, `volume_car`) VALUES ('" + keyFirstTruck + "', '" + orderPk + "', '" + countTripFirstTruck + "', '" + volumeFistTruck + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                    if (comboBox4.SelectedItem != null)
                    {
                        countTripSecondTruck = textBox3.Text;
                        keySecondTruck = trucksKey[trucks.IndexOf(comboBox4.SelectedItem.ToString())];
                        volumeSecondTruck = numericUpDown5.Text.Substring(0, numericUpDown5.Text.IndexOf(',')) + "." + numericUpDown5.Text.Substring(numericUpDown5.Text.IndexOf(',') + 1);
                        msc.CommandText = "INSERT INTO `order_car` (`pk_car`, `pk_order`, `count_trip`, `volume_car`) VALUES ('" + keySecondTruck + "', '" + orderPk + "', '" + countTripSecondTruck + "', '" + volumeSecondTruck + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
                else
                {
                    if (comboBox3.SelectedItem != null)
                    {
                        countTripFirstTruck = textBox2.Text;
                        keyFirstTruck = trucksKey[trucks.IndexOf(comboBox3.SelectedItem.ToString())];
                        volumeFistTruck = numericUpDown4.Text.Substring(0, numericUpDown4.Text.IndexOf(',')) + "." + numericUpDown4.Text.Substring(numericUpDown4.Text.IndexOf(',') + 1);
                        msc.CommandText = "INSERT INTO `order_car` (`pk_car`, `pk_order`, `count_trip`, `volume_car`) VALUES ('" + keyFirstTruck + "', '" + orderPk + "', '" + countTripFirstTruck + "', '" + volumeFistTruck + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
            }

        }

        public void createInstructionsOrder(String numberOrder)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_order FROM `Order` WHERE `nomer`  = '" + numberOrder + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String orderPk = null;
            while (dataReader.Read())
            {
                orderPk = dataReader[0].ToString();
            }
            dataReader.Close();
            if (orderPk == null)
            {
                MessageBox.Show("Ошибка при добавлении заказа");
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    String instructionPk = getInstructionPk("Compact");
                    if (instructionPk != null)
                    {
                        msc.CommandText = "INSERT INTO `order_instruction` (`pk_instruction`, `pk_order`) VALUES ('" + instructionPk + "', '" + orderPk + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
                if (checkBox3.Checked == true)
                {
                    String instructionPk = getInstructionPk("Tipper");
                    if (instructionPk != null)
                    {
                        msc.CommandText = "INSERT INTO `order_instruction` (`pk_instruction`, `pk_order`) VALUES ('" + instructionPk + "', '" + orderPk + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
                if (checkBox4.Checked == true)
                {
                    String instructionPk = getInstructionPk("Onboard");
                    if (instructionPk != null)
                    {
                        msc.CommandText = "INSERT INTO `order_instruction` (`pk_instruction`, `pk_order`) VALUES ('" + instructionPk + "', '" + orderPk + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
                if (checkBox5.Checked == true)
                {
                    String instructionPk = getInstructionPk("Selfloader");
                    if (instructionPk != null)
                    {
                        msc.CommandText = "INSERT INTO `order_instruction` (`pk_instruction`, `pk_order`) VALUES ('" + instructionPk + "', '" + orderPk + "')";
                        msc.Connection = ConnectionToMySQL;
                        msc.ExecuteNonQuery();
                    }
                }
            }
        }

        public String getInstructionPk(String instructionDesc)
        {
            MySqlCommand msc = new MySqlCommand();
            msc.CommandText = "SELECT pk_instruction FROM `instruction` WHERE `desc_instruction`  = '" + instructionDesc + "'";
            msc.Connection = ConnectionToMySQL;
            MySqlDataReader dataReader = msc.ExecuteReader();
            String instructionPk = null;
            while (dataReader.Read())
            {
                instructionPk = dataReader[0].ToString();
            }
            dataReader.Close();
            return instructionPk;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            resultCost();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            //ConnectionToMySQL.Close();
            this.Close();
        }
    }
}
