using System;
using System.Data;
using MySql.Data.MySqlClient;

public static class DatabaseHelper
{
    private static string connectionString =
    "Server=localhost;Database=bems_db;Uid=root;Pwd=root;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
    {
        DataTable dt = new DataTable();
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
        }
        return dt;
    }

    public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
