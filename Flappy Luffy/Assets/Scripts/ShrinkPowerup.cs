using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPowerup : MonoBehaviour
{
    [SerializeField] private float shrinkFactor = 1.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collider2D playerCollider = other.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Vector3 newSize = playerCollider.bounds.size * shrinkFactor;
                playerCollider.transform.localScale = newSize;

                Destroy(gameObject);
            }
        }
    }
}
