using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private float time;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;
    }

    private void Update()
    {
        time += Time.deltaTime;
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

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }
}
