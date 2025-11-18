using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private static int musicLevel = 5;
    private const int MAX_MUSIC_LEVEL = 10;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        // if global instance is already available and global instance is same with the new object,
        // then destroy the new object and use the existing one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }

    private void PausedUI_OnMusicLevelUpdate(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private float GetNormalizedMusicLevel()
    {
        return (float)musicLevel / MAX_MUSIC_LEVEL;
    }

    public int GetMusicLevel()
    {
        return musicLevel;
    }

    public void IncrementMusicLevel()
    {
        musicLevel = (musicLevel + 1) % MAX_MUSIC_LEVEL;
        musicAudioSource.volume = GetNormalizedMusicLevel();
    }
}
