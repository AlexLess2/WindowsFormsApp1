
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    internal class listViewWorking
    {
        string[] mainNames = new string[1];
        int[] mainQuans = new int[1];
        string[] compNames = new string[1];
        int[] compQuans = new int[1];

        public void listViewFiller(int dishCount, string[] chosenDish, System.Windows.Forms.ListView listView1)
        {
            
            
            listView1.BeginUpdate();

            DB db = new DB();
           
            DataTable prod_table = new DataTable();
            DataTable prod_table1 = new DataTable();
            DataTable product_table = new DataTable();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT `product_name`,`quan` FROM `products` ", db.getConnection());
            MySqlCommand command1 = new MySqlCommand("SELECT `product`, `quantity` FROM `ingredient` WHERE `dish` = @uN ", db.getConnection());
            
            command1.Parameters.Add("@uN", MySqlDbType.VarChar).Value = chosenDish[0];

            adapter1.SelectCommand = command1;
            adapter1.Fill(prod_table);

            adapter1.SelectCommand = command;
            adapter1.Fill(product_table);

            int rowCount = new int();
            rowCount = product_table.Rows.Count + 1;

            mainNames = new string[rowCount];
            mainQuans = new int[rowCount];
            string[] bufNames = new string[rowCount];
            int[] bufQuans = new int[rowCount];
            string[] newNames = new string[rowCount];
            int [] newQuans = new int[rowCount];
            compNames = new string[rowCount - 1];
            compQuans = new int[rowCount - 1];

            string[] compNamesUnf = new string[rowCount - 1];
            int[] compQuansUnf = new int[rowCount - 1];
            int i = 0;

            foreach (DataRow row in product_table.Rows)
            {
                compNamesUnf[i] = row.Field<string>("product_name");
                compQuansUnf[i] = row.Field<int>(1);
                i++;
            }
            i = 0;
            foreach (DataRow row in prod_table.Rows)
            {
                mainNames[i] = row.Field<string>("product");
                mainQuans[i] = row.Field<int>(1);
                i++;
            }

            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("Название Продукта", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("кол-во", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Наличие", 100, HorizontalAlignment.Left);

            if (dishCount > 1)
            {
                command1.Parameters.Clear();
                
                for (int j = 1; j < dishCount; j++)
                {
                    command1.Parameters.Clear();

                    command1.Parameters.Add("@uN", MySqlDbType.VarChar).Value = chosenDish[j];
                    adapter1.SelectCommand = command1;
                    prod_table1.Clear();
                    adapter1.Fill(prod_table1);

                    i = 0;
                    foreach (DataRow row in prod_table1.Rows)
                    {
                        bufNames[i] = row.Field<string>("product");
                        bufQuans[i] = row.Field<int>(1);
                        i++;
                    }

                    int mainId = 0;
                    int bufId = 0;
                    int someId = 0;

                    do
                    {

                        bufId = 0;

                        do
                        {

                            if (mainNames[mainId] == bufNames[bufId])
                            {
                                if (!newNames.Contains(bufNames[bufId]))
                                    mainQuans[mainId] += bufQuans[bufId];
                            }

                            else
                            {
                                while (String.IsNullOrEmpty(mainNames[someId]) == false)
                                {
                                    someId++;
                                }

                                if (!mainNames.Contains(bufNames[bufId]))
                                {
                                    mainNames[someId] = bufNames[bufId];
                                    mainQuans[someId] = bufQuans[bufId];
                                    newNames[someId] = bufNames[bufId];
                                    newQuans[someId] = bufQuans[bufId];
                                }


                            }
                            bufId++;

                        }
                        while (String.IsNullOrEmpty(bufNames[bufId]) == false);


                        mainId++;

                    }
                    while (String.IsNullOrEmpty(mainNames[mainId]) == false);
                }
            }


            i = 0;
            while (String.IsNullOrEmpty(mainNames[i]) == false)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Text = mainNames[i];
                lvi.SubItems.Add(mainQuans[i].ToString());

                for (int q = 0; q < compNamesUnf.Length; q++)
                {
                    if (compNamesUnf[q].Equals(mainNames[i]))
                    {
                        lvi.SubItems.Add(compQuansUnf[q].ToString());
                        compQuans[i] = compQuansUnf[q];
                        compNames[i] = compNamesUnf[q];
                    }
                }

                listView1.Items.Add(lvi);
                i++;
            }

            listView1.EndUpdate();
            db.closeConnection();
            
        }

        public void listViewFile()
        {
            
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.txt";
            saveFile1.Filter = "Text files|*.txt";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFile1.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                {
                    int i = 0;
                    while (String.IsNullOrEmpty(mainNames[i]) == false)
                    {
                        sw.WriteLine(mainNames[i] + " нужно: " + mainQuans[i] + " имеется: " + compQuans[i]);
                        i++;
                    }
                    sw.Close();
                }
            }
        }

        public void Updater()
        {

            DB db = new DB();

            db.openConnection();
            MySqlDataAdapter adapter1 = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("UPDATE `products` SET `quan` = @uN WHERE `product_name` = @uL ", db.getConnection());

            int rowCount = mainNames.Length;
            string[] bufNames = new string[rowCount];
            int[] bufQuans = new int[rowCount];
            string[] newNames = new string[rowCount];
            int[] newQuans = new int[rowCount];

            int i = 0;
            try
            {
                while (String.IsNullOrEmpty(mainNames[i]) == false)
                {
                    if (mainQuans[i] > compQuans[i])
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add("@uN", MySqlDbType.Int64).Value = mainQuans[i];
                        command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = compNames[i];
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        db.closeConnection();
                        return;
                    }
                    i++;
                }
           
                command.ExecuteNonQuery();
            }
            catch
            {
                
                MessageBox.Show("Выберете Блюдо");
            }

            db.closeConnection();
        }
    }
}