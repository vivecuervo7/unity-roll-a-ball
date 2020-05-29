using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 3f;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, player.position + offset, followSpeed * Time.deltaTime);
    }
}
