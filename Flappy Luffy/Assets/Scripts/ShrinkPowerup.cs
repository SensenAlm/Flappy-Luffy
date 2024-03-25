using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class ShrinkPowerup : MonoBehaviour
{
    [SerializeField] private float shrinkFactor = 0.95f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collider2D playerCollider = other.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                // shrink player
                Vector3 newSize = playerCollider.bounds.size * shrinkFactor;
                playerCollider.transform.localScale = newSize;

                Destroy(gameObject);

                // database
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
                        cmnd.CommandText = "UPDATE powerupTable SET qty=1 WHERE powerup='Shrink'";
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
        }
    }
}
