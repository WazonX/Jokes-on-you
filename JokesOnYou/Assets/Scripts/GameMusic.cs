using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameMusic : MonoBehaviour
{
    public static GameMusic instance;
    public enum MusicType
    {
        Stopped,
        Default
    }


    /// <summary>
    /// During game interval between playing music scores in seconds
    /// </summary>
    float MusicIntervalInSeconds = 5f;
    float musicTimer = 0f;

    public AudioClip[] DefaultMusicPool;

    [SerializeField()]
    AudioSource PersitentMusic;

    [SerializeField()]
    AudioSource PersistentSoundFX;

    [SerializeField] AudioMixer mixer;
    [SerializeField] internal AudioClip DefaultClick;

    internal MusicType currentMusicType = MusicType.Default;

    bool isMute = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayDefaultTheme();

    }

    public void PlayDefaultTheme()
    {
        if (currentMusicType != MusicType.Default)
        {
            currentMusicType = MusicType.Default;
            PersitentMusic.loop = true;
            PlayNextRandomMusic(DefaultMusicPool);
        }
        Debug.Log("Start playing music");
    }

    AudioClip[] GetCurrentPool()
    {
        switch(currentMusicType)
        {
            default:
                return DefaultMusicPool;
        }
    }

    private void Update()
    {
        if (currentMusicType != MusicType.Stopped && !PersitentMusic.isPlaying)
        {
            if (musicTimer < MusicIntervalInSeconds)
                musicTimer += Time.deltaTime;
            else
                PlayNextRandomMusic(GetCurrentPool());
        }

        if (Input.GetKeyDown(KeyCode.M))
            ToggleMute();
    }

    void ToggleMute()
    {
        isMute = !isMute;
        mixer.SetFloat(MixerParameters.MasterVolume, isMute ? -80 : 0);
    }

    void PlayNextRandomMusic(AudioClip[] musicPool)
    {
        musicTimer = 0;
        AudioClip randomSong;
        switch (currentMusicType)
        {
            default:
                randomSong = musicPool[Random.Range(0, musicPool.Length)];
                break;
        }

        PersitentMusic.clip = randomSong;
        PersitentMusic.Play();
    }

    public void StopMusic()
    {
        currentMusicType = MusicType.Stopped;
        PersitentMusic.Stop();
    }

    /// <summary>
    /// for interface sounds
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        PersistentSoundFX.clip = clip;
        PersistentSoundFX.Play();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //TODO:changed music type based on scene
    }


    public void SetMusicVolume(float musicVolume)
    {
        mixer.SetFloat(MixerParameters.MusicVolume, musicVolume);
        PlayerPrefs.SetFloat(MixerParameters.MusicVolume, musicVolume);
        PlayerPrefs.Save();
    }
    public void SetSfxVolume(float sfxVolume)
    {
        mixer.SetFloat(MixerParameters.SFXVolume, sfxVolume);
        PlayerPrefs.SetFloat(MixerParameters.SFXVolume, sfxVolume);
        PlayerPrefs.Save();
    }
}
