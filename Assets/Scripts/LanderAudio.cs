using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] AudioSource thrusterAudioSource;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;

        SoundManager.Instance.OnSoundChange += SoundManager_OnSoundChange;

        thrusterAudioSource.Pause();
        thrusterAudioSource.volume = SoundManager.Instance.GetNormalizedSoundLevel();
    }

    private void SoundManager_OnSoundChange(object sender, System.EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetNormalizedSoundLevel();
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Pause();
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }
}
