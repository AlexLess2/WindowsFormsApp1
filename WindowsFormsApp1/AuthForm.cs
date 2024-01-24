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

namespace WindowsFormsApp1
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        

        private void Enterbtn_Click(object sender, EventArgs e)
        {
            String loginUser = loginField.Text;
            String passUser = passField.Text;

            DB db = new DB();
            db.openConnection();
            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `logintable` WHERE `login` = @uL AND `pass` = @uP ", db.getConnection());
            
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь Авторизован");
                this.Hide();
                MainForm mainform = new MainForm(); 
                mainform.Show();
                if (loginUser == "Admin")
                {
                    AddForm addform = new AddForm();
                    addform.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный Логин или Пароль");
            }
            db.closeConnection();
        } 

        
    }
}
