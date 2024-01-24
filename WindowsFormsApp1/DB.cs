﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class DB
    {
        MySqlConnection connection = new MySqlConnection("server = localhost; port = 8889; username = root; password = root; database = restobase"); 


        public void openConnection()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
                connection.Open(); 
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
