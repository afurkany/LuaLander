using UnityEngine;

public class GameManagerVisual : MonoBehaviour
{
    [SerializeField] private ScorePopup scorePopupPrefab;

    private void Start()
    {
        Lander.Instance.OnCoinPickup += LanderOnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
    }

    private void Lander_OnFuelPickup(object sender, Lander.OnFuelPickupEventArgs e)
    {
        Instantiate(scorePopupPrefab, e.position, Quaternion.identity).SetText("+FUEL");
    }

    private void LanderOnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        Instantiate(scorePopupPrefab, e.position, Quaternion.identity).SetText("+" + GameManager.SCORE_PER_COIN);
    }
}
