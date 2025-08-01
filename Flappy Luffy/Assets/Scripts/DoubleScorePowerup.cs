using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScorePowerup : MonoBehaviour
{
    public int duration = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoubleScoreCoroutine());
        }
    }

    IEnumerator DoubleScoreCoroutine()
    {
        Score.instance.ActivateDoubleScore();

        yield return new WaitForSeconds(duration);

        Score.instance.DeactivateDoubleScore();
    }
}
