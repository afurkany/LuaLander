using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : MonoBehaviour
{
    [SerializeField] private ScorePopup scorePopupPrefab;

    // DO NOT FORGET TO ADD "Cinemachine Impulse Listener" into CinemachineCamera
    [SerializeField] private CinemachineImpulseSource crashCinemachineImpulseSource;
    [SerializeField] private CinemachineImpulseSource pickupCinemachineImpulseSource;

    private const float CRASH_IMPULSE_MAGNITUDE = 10f;
    private const float PICKUP_IMPULSE_MAGNITUDE = 0.2f;

    private void Start()
    {
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;
        Lander.Instance.OnCoinPickup += LanderOnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
    }

    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        if (e.landingType != Lander.LandingType.Success)
        {
            crashCinemachineImpulseSource.GenerateImpulse(CRASH_IMPULSE_MAGNITUDE);
        }
    }

    private void Lander_OnFuelPickup(object sender, Lander.OnFuelPickupEventArgs e)
    {
        Instantiate(scorePopupPrefab, e.position, Quaternion.identity).SetText("+FUEL");
        pickupCinemachineImpulseSource.GenerateImpulse(PICKUP_IMPULSE_MAGNITUDE);
    }

    private void LanderOnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        Instantiate(scorePopupPrefab, e.position, Quaternion.identity).SetText("+" + GameManager.SCORE_PER_COIN);
        pickupCinemachineImpulseSource.GenerateImpulse(PICKUP_IMPULSE_MAGNITUDE);
    }
}
