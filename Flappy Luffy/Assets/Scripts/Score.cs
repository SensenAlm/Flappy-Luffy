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
    public bool doubleScoreActive = false;
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
            _highScoreText.text = reader[0].ToString();
        }
    }

    public void UpdateScore()
    {
        _score++;
        _currentScoreText.text = _score.ToString();
        UpdateHighScore();
    }

    public void FinalScore()
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO scoreTable (score) VALUES (" + _score + ")";
        cmnd.ExecuteNonQuery();
    }

    public void ResetScore()
    {
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "DROP TABLE scoreTable";
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
        doubleScoreActive = true;
        AddScore(_baseScore);
    }

    public void DeactivateDoubleScore()
    {
        doubleScoreActive = false;
        PipeSpawner.instance._doubleSpawned = false;
        Debug.Log("ended na");
    }

    public void AddScore(int points)
    {
        _score++;
    }
}
