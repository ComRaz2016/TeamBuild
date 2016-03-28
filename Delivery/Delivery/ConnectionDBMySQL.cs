using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Delivery
{
    class ConnectionDBMySQL
    {
        public MySqlConnection ConnectionToMySQL;
        public ConnectionDBMySQL()
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
        }

        public MySqlConnection getConnection()
        {
            return ConnectionToMySQL;
        }
    }
}
