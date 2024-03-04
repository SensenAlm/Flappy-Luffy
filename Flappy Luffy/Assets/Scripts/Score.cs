using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    public IDbConnection dbcon;
    public IDbCommand dbcmd;
    private int _score;
    public int _baseScore = 1;
    public GameObject doubleScoreObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _currentScoreText.text = _score.ToString();
        doubleScoreObject = GameObject.FindWithTag("DoubleScore");
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = false;
        doubleScoreObject.SetActive(false);
        SqliteSetup();
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
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

    public void UpdateScore()
    {
        _score = _score + _baseScore;
        _currentScoreText.text = _score.ToString();
        UpdateHighScore();
    }

    public void FinalScore()
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO scoreTable (score) VALUES (" + _score + ")";
        cmnd.ExecuteNonQuery();
    }

    void SqliteSetup()
    {

        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "scoreDB";

        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS scoreTable (id INTEGER PRIMARY KEY, score INTEGER )";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
    }

    public void ActivateDoubleScore()
    {
        _baseScore = 2;
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = true;
        doubleScoreObject.SetActive(true);
    }

    public void DeactivateDoubleScore()
    {
        _baseScore = 1;
        PipeSpawner.instance._doubleSpawned = false;
        doubleScoreObject.GetComponent<SpriteRenderer>().enabled = false;
        doubleScoreObject.SetActive(false);
    }

}
