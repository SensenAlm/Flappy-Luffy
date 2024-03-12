using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private static IDbConnection dbConnection;
    private static IDbCommand dbCommand;

    public static void InitializeDatabase()
    {
        if (dbConnection == null)
        {
            // Create database
            string connection = "URI=file:" + Application.persistentDataPath + "/" + "scoreDB";

            // Open connection
            dbConnection = new SqliteConnection(connection);
            dbConnection.Open();

            // Create table
            dbCommand = dbConnection.CreateCommand();
            string q_createTable = "CREATE TABLE IF NOT EXISTS scoreTable (id INTEGER PRIMARY KEY, score INTEGER )";

            dbCommand.CommandText = q_createTable;
            dbCommand.ExecuteReader();

            dbCommand = dbConnection.CreateCommand();
            string q_createPowerup = "CREATE TABLE IF NOT EXISTS powerupTable (powerup CHAR(20), qty INTEGER)";

            dbCommand.CommandText = q_createPowerup;
            dbCommand.ExecuteReader();
        }
    }

    public static IDbConnection GetConnection()
    {
        return dbConnection;
    }

    public static IDbCommand GetCommand()
    {
        return dbCommand;
    }
}
