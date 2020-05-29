using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    private int count;
    private int countToWin;
    [SerializeField] Text countText;
    [SerializeField] Text winText;
    //[SerializeField] Transform head;
    [SerializeField] private float speed = 1f;

    [Tooltip("Scale torque by this value")]
    public float TorqueScale = 0.3f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        count = 0;
        countToWin = GameObject.FindGameObjectsWithTag("Pickup").Length;
        UpdateScore(count);
        winText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        body.AddForce(movement * speed);

        if (body.velocity.magnitude > speed)
        {
            body.velocity = body.velocity.normalized * speed;
        }

        // Lock head to body's position and rotation
        //head.LookAt(transform);
        //head.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            UpdateScore(++count);
        }
    }

    private void UpdateScore(int score)
    {
        countText.text = score.ToString();
        if (score == countToWin)
        {
            winText.gameObject.SetActive(true);
        }
    }
}
