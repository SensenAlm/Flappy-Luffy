using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1.5f;
    [SerializeField] private float _rotationSpeed = 10f;

    public bool shieldBool = false;
    private Rigidbody2D _rb;
    public GameObject shieldObject;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        shieldObject = GameObject.FindWithTag("Shield");
        shieldObject.GetComponent<SpriteRenderer>().enabled = false;
        shieldObject.SetActive(false);
    }

    private void Update()
    {
        var input = Input.inputString;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.velocity = Vector2.up * _velocity;
        }
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, _rb.velocity.y * _rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (shieldBool == true)
        {
            DeactivateShield();
            PipeSpawner.instance._shieldSpawned = false;
        }
        else
        {
            Score.instance.FinalScore();
            GameManager.instance.GameOver();
        }
    }

    public void ActivateShield()
    {
        if (!shieldBool)
        {
            shieldBool = true;
            shieldObject.GetComponent<SpriteRenderer>().enabled = true;
            shieldObject.SetActive(true);

        }
    }
    public void DeactivateShield()
    {
        if (shieldBool)
        {
            shieldBool = false;
            shieldObject.GetComponent<SpriteRenderer>().enabled = false;
            shieldObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShieldPowerUp"))
        {
            ActivateShield();
            Destroy(other.gameObject);
        }
    }
}
