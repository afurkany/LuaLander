using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }

    private Rigidbody2D landerRigidBody2D;
    private float maxLandingScore = 100.0f;
    private int actualLandingScore;
    private float maxTotalFuelAmount = 10.0f;
    private float totalFuelAmount = 10.0f;
    private bool isLanderStopped = false;
    private const float maxLandingRotationLimit = 0.9f;
    private const float maxSuccessfulLandingVelocityLimit = 3.5f;

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;

    public event EventHandler<OnLanderLandEventArgs> OnLanderLand;
    public class OnLanderLandEventArgs : EventArgs
    {
        public int landingScore;
    }

    private void Awake()
    {
        Instance = this;
        landerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (totalFuelAmount <= 0f)
        {
            return;
        }

        if (
            Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed
        )
        {
            ConsumeFuel();
            Debug.Log("Remained fuel: " + totalFuelAmount);
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            const float thrustForce = 15.0f;
            landerRigidBody2D.AddForce(thrustForce * transform.up * Time.deltaTime, ForceMode2D.Impulse);

            // set event for lander visual
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            const float rotationForce = 1.0f;
            landerRigidBody2D.AddTorque(rotationForce * Time.deltaTime, ForceMode2D.Impulse);

            // set event for lander visual
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            const float rotationForce = -1.0f;
            landerRigidBody2D.AddTorque(rotationForce * Time.deltaTime, ForceMode2D.Impulse);

            // set event for lander visual
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float maxLandingScoreToGet = maxLandingScore;

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
        if (dotProductOnLanding < maxLandingRotationLimit)
        {
            Debug.Log("Landing angle is too steep!");

            isLanderStopped = true;
            maxLandingScoreToGet = 0f;
            Debug.Log("Landing Score: " + maxLandingScoreToGet);
            return;
        }

        actualLandingScore = CalculateLandingScore(
            maxLandingScoreToGet,
            collision2D.relativeVelocity.magnitude,
            dotProductOnLanding,
            landingPad.GetScoreMultiplier()
        );
    }

    private void OnCollisionStay2D(Collision2D collision2D)
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
            Debug.Log("Landing Score: " + actualLandingScore);
            OnLanderLand?.Invoke(this, new OnLanderLandEventArgs { landingScore = actualLandingScore });
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float AddFuelAmount = 5.0f;
            totalFuelAmount += AddFuelAmount;
            totalFuelAmount = Mathf.Min(totalFuelAmount, maxTotalFuelAmount);
            fuelPickup.SelfDestroy();
        }
        else if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.SelfDestroy();
        }
    }

    private int CalculateLandingScore(
        float maxLandingScoreToGet,
        float landingSpeed,
        float landingDotProduct,
        int scoreMultiplier
    )
    {
        // calculate landing angle score
        const float landingAngleScorePunishmentConstant = 10.0f;
        float landingAnglePunishmentRatio = 1.0f - landingAngleScorePunishmentConstant * (1.0f - landingDotProduct);
        maxLandingScoreToGet *= landingAnglePunishmentRatio;

        // calculate landing speed score
        const float landingSpeedScorePunishmentConstant = 5f;
        float landingSpeedScorePunishment = landingSpeed * landingSpeedScorePunishmentConstant;
        maxLandingScoreToGet -= landingSpeedScorePunishment;

        // update the score with multiplier
        maxLandingScoreToGet *= scoreMultiplier;

        // return max rounded landing score or zero
        return Mathf.RoundToInt(Mathf.Max(maxLandingScoreToGet, 0));
    }

    private void ConsumeFuel()
    {
        const float fuelConsumptionConstant = 1.0f;
        totalFuelAmount -= fuelConsumptionConstant * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return landerRigidBody2D.linearVelocityX * 10.0f;
    }

    public float GetSpeedY()
    {
        return landerRigidBody2D.linearVelocityY * 10.0f;
    }

    public float GetFuelAmount()
    {
        return totalFuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return totalFuelAmount / maxTotalFuelAmount;
    }
}