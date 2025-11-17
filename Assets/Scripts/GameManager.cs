using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int currentGameLevel = 1;
    private static int finalScore = 0;

    public static void ResetStaticData()
    {
        currentGameLevel = 1;
        finalScore = 0;
    }

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private int score;
    private float time;
    private bool isForceApplied = false;
    private bool gameStartInitialZoom = false;
    private float lerpSpeed = 2f;

    private bool isUpdateTrackingTarget = true;
    private bool isLanderFocused = false;
    private GameLevel spawnedGameLevel;

    public const int SCORE_PER_COIN = 1000;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnAfterForce += Lander_OnAfterForce;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;

        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;

        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Lander_OnAfterForce(object sender, System.EventArgs e)
    {
        isForceApplied = true;

        if (!isLanderFocused)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            isLanderFocused = true;
            isUpdateTrackingTarget = false;
        }
    }

    private void Update()
    {
        if (isForceApplied)
        {
            time += Time.deltaTime;
        }

        // update orthographic size to a value in the beginning of the game after the camera move ends
        if (gameStartInitialZoom)
        {
            float initialOrthographicSize = 35f;
            gameStartInitialZoom = UpdateCinemachineOrthographicSize(initialOrthographicSize);
        }

        if (isLanderFocused)
        {
            float orthographicSizeLander = 13f;
            isLanderFocused = UpdateCinemachineOrthographicSize(orthographicSizeLander);
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        Debug.Log("gameLevel: " + gameLevel);
        spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCinemachineSplineCartTransform();

        spawnedGameLevel.onCameraReachDestination += GameLevel_onCameraReachDestination;
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.GetGameLevel() == currentGameLevel)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void GameLevel_onCameraReachDestination(object sender, System.EventArgs e)
    {
        if (isUpdateTrackingTarget)
        {
            cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraZoomOutStartTransform();
            gameStartInitialZoom = true;
            isUpdateTrackingTarget = false;
        }
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(SCORE_PER_COIN);
    }

    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        isForceApplied = false;
        if (e.landingType == Lander.LandingType.Success)
        {
            AddScore(e.landingScore);
        }
        else
        {
            score = 0;
        }
    }

    private bool UpdateCinemachineOrthographicSize(float newSize)
    {
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(
            cinemachineCamera.Lens.OrthographicSize,
            newSize,
            Time.deltaTime * lerpSpeed
        );

        // Stop when close enough
        if (Mathf.Abs(cinemachineCamera.Lens.OrthographicSize - newSize) < 0.01f)
        {
            cinemachineCamera.Lens.OrthographicSize = newSize;
            return false;
        }

        return true;
    }

    private void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTotalScore()
    {
        return finalScore;
    }

    public float GetTime()
    {
        return time;
    }

    public int GetLevel()
    {
        return currentGameLevel;
    }

    public void GoToNextLevel()
    {
        currentGameLevel++;
        finalScore += score;

        if (GetGameLevel() == null)
        {
            // No more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            // There are still more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void RetryThisLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
}
