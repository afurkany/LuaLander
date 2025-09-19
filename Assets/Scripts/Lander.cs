using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidBody2D;

    private void Awake()
    {
        landerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            const float thrustForce = 15.0f;
            landerRigidBody2D.AddForce(thrustForce * transform.up * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            const float rotationForce = 1.0f;
            landerRigidBody2D.AddTorque(rotationForce * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            const float rotationForce = -1.0f;
            landerRigidBody2D.AddTorque(rotationForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        const float maxSuccessfulLandingVelocityLimit = 3.5f;
        if (collision2D.relativeVelocity.magnitude > maxSuccessfulLandingVelocityLimit)
        {
            Debug.Log("Landing is too fast!");
            return;
        }

        float dotProductOnLanding = Vector2.Dot(Vector2.up, transform.up);
        const float maxLandingRotationLimit = 0.9f;
        if (dotProductOnLanding < maxLandingRotationLimit)
        {
            Debug.Log("Landing is too steep!");
            return;
        }

        Debug.Log("Landing is successful!");
    }
}
