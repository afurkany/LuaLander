using UnityEngine;

public class GameObject : MonoBehaviour
{
    private int score;

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        const int coinPickupScore = 500;
        AddScore(coinPickupScore);
    }

    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        AddScore(e.landingScore);
    }

    private void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }
}
