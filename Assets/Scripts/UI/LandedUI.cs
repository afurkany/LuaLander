using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI labelTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Image background;
    [SerializeField] private Button nextButton;

    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }

    private void Start()
    {
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;

        Hide();
    }

    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            nextButtonClickAction = GameManager.Instance.GoToNextLevel;
            nextButtonTextMesh.text = "CONTINUE";
            titleTextMesh.text = "<color=#00ff00>SUCCESSFUL LANDING!</color>";
            background.fillAmount = 1.0f;
            labelTextMesh.text =
                "Landing Speed" + "\n" +
                "Landing Angle" + "\n" +
                "Landing Multiplier" + "\n" +
                "Landing Score";

            statsTextMesh.text =
                RoundWithPrecision(e.landingSpeed, 1) + "\n" +
                RoundWithPrecision(e.landingAngle, 2) + "\n" +
                "x" + e.landingMultiplier + "\n" +
                e.landingScore;
        }
        else
        {
            nextButtonClickAction = GameManager.Instance.RetryThisLevel;
            nextButtonTextMesh.text = "RESTART";
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            background.fillAmount = 0.3f;
            labelTextMesh.text = e.crashReason;
            statsTextMesh.text = "";
        }
        Show();
    }

    private float RoundWithPrecision(float value, float precision)
    {
        precision = Mathf.Pow(10, precision);
        return Mathf.Round(value * precision) / precision;
    }

    private void Show()
    {
        // select the button when UI is open to select it with gamepad
        nextButton.Select();

        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
