using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _startMenuCanvas;
    [SerializeField] private GameObject _scoreCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void MainMenu()
    {
        Time.timeScale = 0f; // pause
        _scoreCanvas.SetActive(false);

    }
    public void Play()
    {
        Time.timeScale = 1f; // resume
        _scoreCanvas.SetActive(true);
        _startMenuCanvas.SetActive(false);
    }
    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);

        Time.timeScale = 0f; // pause

        // database for shrink value to set to 0

        IDbConnection dbcon = DatabaseManager.GetConnection();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT count(*) FROM powerupTable WHERE powerup='Shrink'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        if (reader.Read())
        {
            int count = reader.GetInt32(0);
            if (count > 0)
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "UPDATE powerupTable SET qty=0 WHERE powerup='Shrink'";
                cmnd.ExecuteNonQuery();
            }
            else
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "INSERT INTO powerupTable (powerup, qty) VALUES ('Shrink', 1)";
                cmnd.ExecuteNonQuery();
            }

            IDbCommand cmnd_read1 = dbcon.CreateCommand();
            string query1 = "SELECT qty FROM powerupTable WHERE powerup='Shrink'";
            cmnd_read1.CommandText = query1;
            IDataReader reader1 = cmnd_read1.ExecuteReader();

            while (reader1.Read())
            {
                Debug.Log("Shrink Value: " + reader1.GetInt32(0));
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
