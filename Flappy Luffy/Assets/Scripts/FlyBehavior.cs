using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1.5f;
    [SerializeField] private float _rotationSpeed = 10f;

    public bool shieldBool = false;
    private Rigidbody2D _rb;
    public GameObject shieldObject;

    private void Start()
    {
        // initalization of rigid body and shield object
        _rb = GetComponent<Rigidbody2D>();
        shieldObject = GameObject.FindWithTag("Shield");
        shieldObject.GetComponent<SpriteRenderer>().enabled = false;
        shieldObject.SetActive(false);
    }

    private void Update()
    {   // input for luffy to fly
        if (Input.GetMouseButtonDown(0))
        {
            _rb.velocity = Vector2.up * _velocity;
        }
    }
    private void FixedUpdate() // rotation animation
    {
        transform.rotation = Quaternion.Euler(0, 0, _rb.velocity.y * _rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other) // game over
    {
        if (shieldBool == true) // shield is active
        {
            DeactivateShield();
            PipeSpawner.instance._shieldSpawned = false;
        }
        else    // shield is not active
        {
            Score.instance.FinalScore();
            GameManager.instance.GameOver();
        }
    }

    public void ActivateShield()
    {
        if (!shieldBool)
        {
            // show shield
            shieldBool = true;
            shieldObject.GetComponent<SpriteRenderer>().enabled = true;
            shieldObject.SetActive(true);
            // database
            IDbConnection dbcon = DatabaseManager.GetConnection();
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            string query = "SELECT count(*) FROM powerupTable WHERE powerup='Shield'";
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();

            if (reader.Read())
            {
                int count = reader.GetInt32(0);
                if (count > 0)
                {
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "UPDATE powerupTable SET qty=1 WHERE powerup='Shield'";
                    cmnd.ExecuteNonQuery();
                }
                else
                {
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "INSERT INTO powerupTable (powerup, qty) VALUES ('Shield', 1)";
                    cmnd.ExecuteNonQuery();
                }

                IDbCommand cmnd_read1 = dbcon.CreateCommand();
                string query1 = "SELECT qty FROM powerupTable WHERE powerup='Shield'";
                cmnd_read1.CommandText = query1;
                IDataReader reader1 = cmnd_read1.ExecuteReader();

                while (reader1.Read())
                {
                    Debug.Log("Shield Value: " + reader1.GetInt32(0));
                }
            }

        }
    }
    public void DeactivateShield()
    {
        if (shieldBool)
        {
            // hide shield
            shieldBool = false;
            shieldObject.GetComponent<SpriteRenderer>().enabled = false;
            shieldObject.SetActive(false);
            // database
            IDbConnection dbcon = DatabaseManager.GetConnection();
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            string query = "SELECT count(*) FROM powerupTable WHERE powerup='Shield'";
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();

            if (reader.Read())
            {
                int count = reader.GetInt32(0);
                if (count > 0)
                {
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "UPDATE powerupTable SET qty=0 WHERE powerup='Shield'";
                    cmnd.ExecuteNonQuery();
                }
                else
                {
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "INSERT INTO powerupTable (powerup, qty) VALUES ('Shield', 0)";
                    cmnd.ExecuteNonQuery();
                }

                IDbCommand cmnd_read1 = dbcon.CreateCommand();
                string query1 = "SELECT qty FROM powerupTable WHERE powerup='Shield'";
                cmnd_read1.CommandText = query1;
                IDataReader reader1 = cmnd_read1.ExecuteReader();

                while (reader1.Read())
                {
                    Debug.Log("Shield Value: " + reader1.GetInt32(0));
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShieldPowerUp")) // trigger for shield object
        {
            ActivateShield();
            Destroy(other.gameObject);
        }
    }
}
