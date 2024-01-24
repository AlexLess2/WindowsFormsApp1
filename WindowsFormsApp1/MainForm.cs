using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        // обьявление объекта listViewWorking для оптимизации 
        listViewWorking filler = new listViewWorking();
        public MainForm()
        {
            InitializeComponent();

            // заполнение чеклиста

            checkedListBox1.BeginUpdate();

            DB db = new DB();
            db.openConnection();
            DataTable dish_table = new DataTable();

            MySqlDataAdapter adapter1 = new MySqlDataAdapter();

            MySqlCommand command1 = new MySqlCommand("SELECT `dish_name` FROM `dishes` ", db.getConnection());

            adapter1.SelectCommand = command1;
            adapter1.Fill(dish_table);

            foreach (DataRow row in dish_table.Rows)
            {
                checkedListBox1.Items.Add(row.Field<string>("dish_name"));
            }
            checkedListBox1.EndUpdate();


            //заполнение комбобокса ингридиентов

            comboBox1.BeginUpdate();

            DataTable prod_table = new DataTable();

            MySqlCommand command = new MySqlCommand("SELECT `product_name` FROM `products` ", db.getConnection());

            adapter1.SelectCommand = command;
            adapter1.Fill(prod_table);

            foreach (DataRow row in prod_table.Rows)
            {
                comboBox1.Items.Add(row.Field<string>("product_name"));
            }
            comboBox1.Items.Add("Без выбора");

            comboBox1.EndUpdate();

            //заполнение комбобокса персон

            comboBox2.BeginUpdate();

            DataTable prod1_table = new DataTable();

            MySqlCommand command2 = new MySqlCommand("SELECT `persons` FROM `dishes` ", db.getConnection());

            adapter1.SelectCommand = command2;
            adapter1.Fill(prod1_table);

            int rowCount = prod1_table.Rows.Count + 1;

            int[] persInt = new int[rowCount];
            int[] persDist = new int[rowCount];

            int i = 0;

            foreach (DataRow row in prod1_table.Rows)
            {
         
                persInt[i] = Convert.ToInt32(row.Field<UInt32>("persons"));
                i++;
            }

            persDist = persInt.Distinct().ToArray();
            Array.Sort(persDist);

            i = 0;

            while (i<persDist.Length)
            {
                if (persDist[i] != 0)
                    comboBox2.Items.Add(persDist[i].ToString());
                i++;
            }

            comboBox2.Items.Add("Без выбора");
            comboBox2.EndUpdate();
            
            db.closeConnection();
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            //Считаем количество выбранных блюд для выделения памяти
            int dishCount = 0;

            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                // The indexChecked variable contains the index of the item.
                MessageBox.Show("Name: " + itemChecked.ToString() + ", is checked.  ");
                dishCount++;
            }

            string[] chosenDish = new string[dishCount];

            //заполняем массив блюдами

            dishCount = 0;
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                chosenDish[dishCount++] = itemChecked.ToString();
            }

            //listViewWorking filler = new listViewWorking();
            if(dishCount!=0)
                filler.listViewFiller(dishCount, chosenDish, listView1);

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string selectedState = comboBox1.SelectedItem.ToString();
           
            checkedListBox1.BeginUpdate();

            checkedListBox1.Items.Clear();
            
            DB db = new DB();
            db.openConnection();

            DataTable dish_table = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            // ФИЛЬТР БЛЮД ЕСЛИ НЕ ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
            if (selectedState != "Без выбора")
            { 
                MySqlCommand command1 = new MySqlCommand("SELECT `dish` FROM `ingredient` WHERE `product` = @uL ", db.getConnection());


                command1.Parameters.Add("@uL", MySqlDbType.VarChar).Value = selectedState;
                adapter1.SelectCommand = command1;
                adapter1.Fill(dish_table);

                foreach (DataRow row in dish_table.Rows)
                {
                    checkedListBox1.Items.Add(row.Field<string>("dish"));
                }

            }
            // ФИЛЬТР БЛЮД ЕСЛИ ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
            else
            {

                MySqlCommand command = new MySqlCommand("SELECT `dish_name` FROM `dishes` ", db.getConnection());

                adapter1.SelectCommand = command;
                adapter1.Fill(dish_table);

                foreach (DataRow row in dish_table.Rows)
                {
                    checkedListBox1.Items.Add(row.Field<string>("dish_name"));
                }

            }
            checkedListBox1.EndUpdate();
            db.closeConnection();
            

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = comboBox2.SelectedItem.ToString();

            checkedListBox1.BeginUpdate();

            checkedListBox1.Items.Clear();

            DB db = new DB();
            db.openConnection();

            DataTable dish_table = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            // ФИЛЬТР БЛЮД ЕСЛИ НЕ ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
            if (selectedState != "Без выбора")
            {
                int selectedInt=Convert.ToInt32(selectedState);

                MySqlCommand command1 = new MySqlCommand("SELECT `dish_name` FROM `dishes` WHERE `persons` = @uL ", db.getConnection());


                command1.Parameters.Add("@uL", MySqlDbType.Int32).Value = selectedInt;
                adapter1.SelectCommand = command1;

                adapter1.Fill(dish_table);

                foreach (DataRow row in dish_table.Rows)
                {
                    checkedListBox1.Items.Add(row.Field<string>("dish_name"));
                }

            }
            // ФИЛЬТР БЛЮД ЕСЛИ  ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
            else
            {

                MySqlCommand command = new MySqlCommand("SELECT `dish_name` FROM `dishes` ", db.getConnection());
    
                adapter1.SelectCommand = command;
                adapter1.Fill(dish_table);

                foreach (DataRow row in dish_table.Rows)
                {
                    checkedListBox1.Items.Add(row.Field<string>("dish_name"));
                }

            }
            
            checkedListBox1.EndUpdate();
            db.closeConnection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedState = textBox1.Text;

                int selectedInt = Convert.ToInt32(selectedState);



                checkedListBox1.BeginUpdate();

                checkedListBox1.Items.Clear();

                DB db = new DB();
                db.openConnection();

                DataTable dish_table = new DataTable();
                MySqlDataAdapter adapter1 = new MySqlDataAdapter();
                // ФИЛЬТР БЛЮД ЕСЛИ НЕ ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
                if (selectedState != "")
                {

                    MySqlCommand command1 = new MySqlCommand("SELECT `dish_name`,`time` FROM `dishes` ", db.getConnection());


                    adapter1.SelectCommand = command1;

                    adapter1.Fill(dish_table);

                    int rowCount = new int();
                    rowCount = dish_table.Rows.Count + 1;
                    string[] mainNames = new string[rowCount];
                    int[] mainTime = new int[rowCount];
                    string[] bufNames = new string[rowCount];
                    int[] bufTime = new int[rowCount];

                    int i = 0;
                    foreach (DataRow row in dish_table.Rows)
                    {
                        if (row.Field<int>(1) <= selectedInt)
                            checkedListBox1.Items.Add(row.Field<string>("dish_name"));
                        i++;
                    }
                }

                // ФИЛЬТР БЛЮД ЕСЛИ  ВЫБРАНА ПОЗИЦИЯ БЕЗ ВЫБОРА
                else
                {

                    MySqlCommand command = new MySqlCommand("SELECT `dish_name` FROM `dishes` ", db.getConnection());

                    adapter1.SelectCommand = command;
                    adapter1.Fill(dish_table);

                    foreach (DataRow row in dish_table.Rows)
                    {
                        checkedListBox1.Items.Add(row.Field<string>("dish_name"));
                    }

                }

                checkedListBox1.EndUpdate();
                db.closeConnection();
            }
            catch
            {
                return;
            }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            filler.listViewFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            filler.Updater();
        }

      

        private void admButton_Click(object sender, EventArgs e)
        {
            String passUser = passbox.Text;
            DB db = new DB();
            db.openConnection();
            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `logintable` WHERE `login` = @uL AND `pass` = @uP ", db.getConnection());

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = "Admin";
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                AddForm addform = new AddForm();
                addform.Show();
            }
            else
            {
                MessageBox.Show("Неверный пароль");
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            return;
        }
       

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            return;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
