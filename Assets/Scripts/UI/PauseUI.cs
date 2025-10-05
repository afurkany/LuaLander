using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundLevelButton;
    [SerializeField] private TextMeshProUGUI soundLevelTextMesh;
    [SerializeField] private Button musicLevelButton;
    [SerializeField] private TextMeshProUGUI musicLevelTextMesh;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnpauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });

        soundLevelButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.IncrementSoundLevel();
            UpdateSoundLevelTextMesh();
        });

        musicLevelButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.IncrementMusicLevel();
            UpdateMusicLevelTextMesh();
        });
    }

    // We should access the external reference from Start
    // For the internal references we can use Awake
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateSoundLevelTextMesh();
        UpdateMusicLevelTextMesh();
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateSoundLevelTextMesh()
    {
        soundLevelTextMesh.text = "SOUND" + " " + SoundManager.Instance.GetSoundLevel();
    }

    private void UpdateMusicLevelTextMesh()
    {
        musicLevelTextMesh.text = "MUSIC" + " " + MusicManager.Instance.GetMusicLevel();
    }

    private void Show()
    {
        // select the button when UI is open to select it with gamepad
        resumeButton.Select();

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}