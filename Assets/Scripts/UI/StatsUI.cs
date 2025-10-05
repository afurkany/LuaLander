using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject SpeedLeftArrowGameObject;
    [SerializeField] private GameObject SpeedRightArrowGameObject;
    [SerializeField] private GameObject SpeedUpArrowGameObject;
    [SerializeField] private GameObject SpeedDownArrowGameObject;
    [SerializeField] private Image fuelImage;

    private void Update()
    {
        DisplaySpeedDirectionArrowX();
        UpdateStatsTextMesh();
        DisplayFuelAmount();
    }

    private void UpdateStatsTextMesh()
    {
        statsTextMesh.text =
            GameManager.Instance.GetLevel() + "\n" +
            GameManager.Instance.GetScore() + "\n" +
            Mathf.RoundToInt(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.RoundToInt(Lander.Instance.GetSpeedX())) + "\n" +
            Mathf.Abs(Mathf.RoundToInt(Lander.Instance.GetSpeedY())) + "\n";
    }

    private void DisplaySpeedDirectionArrowX()
    {
        SpeedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0);
        SpeedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0);
        SpeedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        SpeedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
    }

    private void DisplayFuelAmount()
    {
        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
}
