using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore();

        // select the button when UI is open to select it with gamepad
        mainMenuButton.Select();
    }
}
