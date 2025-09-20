using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidBody2D;
    private float landingScore = 10.0f;
    private bool isLanderStopped = false;

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
        float maxLandingScoreToGet = landingScore;

        // Check if the landing is on landing pad
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the Terrain!");

            isLanderStopped = true;
            maxLandingScoreToGet = 0f;
            Debug.Log("Landing Score: " + maxLandingScoreToGet);
            return;
        }

        // Define a max landing speed for successful landing
        const float maxSuccessfulLandingVelocityLimit = 3.5f;
        if (collision2D.relativeVelocity.magnitude > maxSuccessfulLandingVelocityLimit)
        {
            Debug.Log("Landing is too fast!");

            isLanderStopped = true;
            maxLandingScoreToGet = 0f;
            Debug.Log("Landing Score: " + maxLandingScoreToGet);
            return;
        }

        // Define a max langding angle for successful landing
        float dotProductOnLanding = Vector2.Dot(Vector2.up, transform.up);
        const float maxLandingRotationLimit = 0.9f;
        if (dotProductOnLanding < maxLandingRotationLimit)
        {
            Debug.Log("Landing angle is too steep!");

            isLanderStopped = true;
            maxLandingScoreToGet = 0f;
            Debug.Log("Landing Score: " + maxLandingScoreToGet);
            return;
        }

        landingScore = CalculateLandingScore(
            maxLandingScoreToGet,
            collision2D.relativeVelocity.magnitude,
            dotProductOnLanding
        );
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // print the calculated score just the first time when the lander is fully stopped
        if (
            !isLanderStopped &&
            (landerRigidBody2D.angularVelocity < 1e-3) &&
            (landerRigidBody2D.linearVelocity.magnitude < 1e-3)
        )
        {
            isLanderStopped = true;
            Debug.Log("Landing is successful!");
            Debug.Log("Landing Score: " + landingScore);
        }
    }

    /// <summary>
    /// Calculates the max landing score to get when the lander is fully stopped.
    /// </summary>
    private float CalculateLandingScore(float maxLandingScoreToGet, float landingSpeed, float landingDotProduct)
    {
        // calculate landing angle score
        const float landingAngleScorePunishmentConstant = 10.0f;
        float landingAngleScorePunishment = Mathf.Abs(1.0f - landingDotProduct) * Mathf.Pow(landingAngleScorePunishmentConstant, 2);
        maxLandingScoreToGet -= landingAngleScorePunishment;

        // calculate landing speed score
        const float landingSpeedScorePunishmentConstant = 0.5f;
        float landingSpeedScorePunishment = landingSpeed * landingSpeedScorePunishmentConstant;
        maxLandingScoreToGet -= landingSpeedScorePunishment;

        // return max rounded landing score or zero
        return Mathf.Round(Mathf.Max(maxLandingScoreToGet, 0) * 100.0f) * 0.01f;
    }
}
