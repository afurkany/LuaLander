using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;
    [SerializeField] private AudioClip crashAudioClip;

    private static int soundLevel = 5;
    private const int MAX_SOUND_LEVEL = 10;

    public event EventHandler OnSoundChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnLanderLand += Lander_OnLanderLand;
    }

    private void Lander_OnLanderLand(object sender, Lander.OnLanderLandEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position, GetNormalizedSoundLevel());
        }
        else
        {
            AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, GetNormalizedSoundLevel());
        }
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position, GetNormalizedSoundLevel());
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position, GetNormalizedSoundLevel());
    }

    public float GetNormalizedSoundLevel()
    {
        return ((float)soundLevel) / MAX_SOUND_LEVEL;
    }

    public int GetSoundLevel()
    {
        return soundLevel;
    }

    public void IncrementSoundLevel()
    {
        soundLevel = (soundLevel + 1) % MAX_SOUND_LEVEL;
        OnSoundChange?.Invoke(this, EventArgs.Empty);
    }
}