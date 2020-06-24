using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Respawner respawner;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Rigidbody head;
    [SerializeField] private float speed;
    [Range(0f, 0.4f)][SerializeField] private float headLeanAmount;

    private void Awake()
    {
        respawner = FindObjectOfType<Respawner>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            respawner.RespawnPlayer();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // Clamp magnitude to use normalized vector where an axis exceeds maxLength. Preferred over
        // using an explicitly normalized vector as it would prevent partial movement with a joystick
        var input = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1f);

        // Calculate speed factoring in mass, so changing mass won't alter the overall speed. Doing this in
        // update over Start() so we can adjust speed throughout the game
        body.AddForce(input * speed * body.mass);

        // Move BB-8's head to the correct position and apply rotation
        head.transform.position = transform.position;
        head.AddTorque(body.angularVelocity);

        // Use a lower torque scale when travelling in a straight direction to allow for increased head rotation
        var movement = Mathf.Abs(1f - Vector3.Dot(head.transform.forward, body.velocity.normalized));
        var torqueScale = Mathf.Clamp(movement - headLeanAmount, 0.2f, 1f);
        ApplyHeadRotation(head, torqueScale);

        // Point BB-8's head in direction of movement
        var angle = Vector3.Dot(head.transform.right, body.velocity.normalized) * (Mathf.Rad2Deg * Time.fixedDeltaTime * 20f);
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        head.rotation *= q;
    }

    private void ApplyHeadRotation(Rigidbody head, float torqueScale)
    {
        var target = Vector3.up;
        var current = head.transform.forward;
        // Axis of rotation
        var x = Vector3.Cross(current, target);
        var theta = Mathf.Asin(x.magnitude);
        // Change in angular velocity
        var w = x.normalized * (theta / Time.fixedDeltaTime * torqueScale);
        // Current rotation in world space
        var q = head.rotation * head.inertiaTensorRotation;
        // Transform to local space
        w = Quaternion.Inverse(q) * w;
        // Calculate torque and convert back to world space
        var T = q * Vector3.Scale(head.inertiaTensor, w);
        head.AddTorque(T, ForceMode.Force);
    }
}
