using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    private int _score;
    public int _baseScore = 1;
    public GameObject doubleScoreObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DatabaseManager.InitializeDatabase(); // initialization of database
    }

    private void Start()
    {
        _currentScoreText.text = _score.ToString();
        doubleScoreObject = GameObject.FindWithTag("DoubleScore");
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = false;
        doubleScoreObject.SetActive(false);
        UpdateHighScore();
        //ResetScore();
    }

    private void UpdateHighScore()
    {
        IDbConnection dbcon = DatabaseManager.GetConnection();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT max(score) FROM scoreTable";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            if (int.Parse(reader[0].ToString()) < _score)
            {
                _highScoreText.text = _score.ToString();
            }
            else
            {
                _highScoreText.text = reader[0].ToString();
            }
        }
    }
    public void ResetScore()
    {
        IDbConnection dbcon = DatabaseManager.GetConnection();
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DELETE FROM scoreTable";
        cmnd.ExecuteNonQuery();
    }
    public void UpdateScore()
    {
        _score = _score + _baseScore;
        _currentScoreText.text = _score.ToString();
        UpdateHighScore();
        if (_score == 1)
        {
            UpdateHighScore();
        }
    }

    public void FinalScore()
    {
        IDbConnection dbcon = DatabaseManager.GetConnection();
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO scoreTable (score) VALUES (" + _score + ")";
        cmnd.ExecuteNonQuery();
    }

    public void ActivateDoubleScore()
    {
        _baseScore = 2;
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = true;
        doubleScoreObject.SetActive(true);

        // database
        IDbConnection dbcon = DatabaseManager.GetConnection();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT count(*) FROM powerupTable WHERE powerup='DoubleScore'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        if (reader.Read())
        {
            int count = reader.GetInt32(0);
            if (count > 0)
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "UPDATE powerupTable SET qty=1 WHERE powerup='DoubleScore'";
                cmnd.ExecuteNonQuery();
            }
            else
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "INSERT INTO powerupTable (powerup, qty) VALUES ('DoubleScore', 1)";
                cmnd.ExecuteNonQuery();
            }

            // update powerup
            IDbCommand cmnd_read1 = dbcon.CreateCommand();
            string query1 = "SELECT qty FROM powerupTable WHERE powerup='DoubleScore'";
            cmnd_read1.CommandText = query1;
            IDataReader reader1 = cmnd_read1.ExecuteReader();

            while (reader1.Read())
            {
                Debug.Log("DoubleScore Value: " + reader1.GetInt32(0));
            }
        }
    }



    public void DeactivateDoubleScore()
    {
        _baseScore = 1;
        PipeSpawner.instance._doubleSpawned = false;
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = false;
        doubleScoreObject.SetActive(false);

        // database
        IDbConnection dbcon = DatabaseManager.GetConnection();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT count(*) FROM powerupTable WHERE powerup='DoubleScore'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        if (reader.Read())
        {
            int count = reader.GetInt32(0);
            if (count > 0)
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "UPDATE powerupTable SET qty=0 WHERE powerup='DoubleScore'";
                cmnd.ExecuteNonQuery();
            }
            else
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = "INSERT INTO powerupTable (powerup, qty) VALUES ('DoubleScore', 0)";
                cmnd.ExecuteNonQuery();
            }

            // update powerup value
            IDbCommand cmnd_read1 = dbcon.CreateCommand();
            string query1 = "SELECT qty FROM powerupTable WHERE powerup='DoubleScore'";
            cmnd_read1.CommandText = query1;
            IDataReader reader1 = cmnd_read1.ExecuteReader();

            while (reader1.Read())
            {
                Debug.Log("DoubleScore Value: " + reader1.GetInt32(0));
            }
        }
    }


}
