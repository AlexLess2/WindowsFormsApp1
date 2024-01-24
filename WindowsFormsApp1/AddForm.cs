using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WindowsFormsApp1
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String dishName = textBox1.Text;
            int Time=0;
            int Pers=0;
            try
            {
                Time = Convert.ToInt32(textBox2.Text);
                Pers = Convert.ToInt32(textBox2.Text);

            }
            catch
            {
                MessageBox.Show("Неправильное время или количество персон");
            }

            String bufIngr = richTextBox2.Text;
            string[] Ingr = bufIngr.Split('\n');
            string[] ingrName = new string[Ingr.Length];
            int[] ingrCount = new int[Ingr.Length];
            int i=0;
            try
            {
                foreach (string s in Ingr)
                {
                    string[] Ingr2 = s.Split('-');
                    ingrName[i] = Ingr2[0];
                    ingrCount[i] = Convert.ToInt32(Ingr2[1]);
                    i++;
                    
                }
            }
            catch
            {
                MessageBox.Show("Неправильный формат ингридиентов");
                return;
            }
            DB db = new DB();

            db.openConnection();
            
            MySqlCommand command = new MySqlCommand("INSERT INTO `dishes`(`dish_name`, `recept`, `time`, `persons`) VALUES (@uN, @uL, @uK, @uH) ", db.getConnection());
            MySqlCommand command1 = new MySqlCommand("INSERT INTO `ingredient`(`dish`, `product`, `quantity`) VALUES (@N,@uL,@uK)", db.getConnection());
            command.Parameters.Add("@uN", MySqlDbType.VarChar).Value= dishName.ToString();
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = richTextBox1.Text.ToString();
            command.Parameters.Add("@uK", MySqlDbType.Int64).Value = Time;
            command.Parameters.Add("@uH", MySqlDbType.Int64).Value = Pers;

            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Блюдо записано");
            }

            for(i = 0; i < ingrCount.Length; i++)
            {
                command1.Parameters.Clear();
                command1.Parameters.Add("@N", MySqlDbType.VarChar).Value = dishName.ToString();
                command1.Parameters.Add("@uL", MySqlDbType.VarChar).Value = ingrName[i].ToString();
                command1.Parameters.Add("@uK", MySqlDbType.Int64).Value = ingrCount[i];
                command1.ExecuteNonQuery();
                
            }

            db.closeConnection();

        }

        private void label9_Click(object sender, EventArgs e)
        {
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String dishName = textBox4.Text;
            DB db = new DB();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("DELETE FROM `dishes` WHERE `dish_name` = @uN ", db.getConnection());
            MySqlCommand command1 = new MySqlCommand("DELETE FROM `ingredient` WHERE `dish` = @uN ", db.getConnection());
            command.Parameters.Add("@uN", MySqlDbType.VarChar).Value = dishName.ToString();
            command1.Parameters.Add("@uN", MySqlDbType.VarChar).Value = dishName.ToString();
            command.ExecuteNonQuery();
            command1.ExecuteNonQuery();
        }

        private void addProdBtn_Click(object sender, EventArgs e)
        {
            String Name = prodName.Text;
            String Ed = edName.Text;
            int quant = 0;
            try
            {
                quant = Convert.ToInt32(quantity.Text);

            }
            catch
            {
                MessageBox.Show("Неправильно введено количество");
                return;
            }
            DB db = new DB();

            db.openConnection();

            MySqlCommand command = new MySqlCommand("INSERT INTO `products`(`product_name`, `unit`, `quan`) VALUES (@uN, @uL, @uK) ", db.getConnection());
            command.Parameters.Add("@uN", MySqlDbType.VarChar).Value = Name.ToString();
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Ed.ToString();
            command.Parameters.Add("@uK", MySqlDbType.Int64).Value = quant;
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Продукт записан");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String prodName = textBox6.Text;

            DB db = new DB();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("DELETE FROM `products` WHERE `product_name` = @uN ", db.getConnection());
            
            command.Parameters.Add("@uN", MySqlDbType.VarChar).Value = prodName.ToString();
          
            command.ExecuteNonQuery();
           
        }

        private void AddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void AddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            return;
        }

        private void AddForm_Deactivate(object sender, EventArgs e)
        {
            return;
        }
    }
}
