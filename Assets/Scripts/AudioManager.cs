using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonBase<AudioManager>
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 100;
    }

    [Header("Sound Effects")]
    public Sound[] sounds;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Header("Main Menu Background Music")]
    public AudioClip mainMenuMusic;
    [Range(0f, 1f)]
    public float mainMenuMusicVolume = 1f;

    private AudioSource musicSource;
    private readonly Dictionary<string, Sound> soundDict = new();
    private AudioSource audioSource;

    // Store the current position when music is paused
    private float musicPauseTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // Cache sounds
        foreach (var s in sounds)
        {
            if (!soundDict.ContainsKey(s.name))
                soundDict.Add(s.name, s);
        }
    }

    // Play Sounds Effect By Name (For SFX)
    public void PlaySFX(string soundName)
    {
        if (soundDict.TryGetValue(soundName, out Sound s))
        {
            audioSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning($"⚠️ SFX '{soundName}' not found in AudioManager.");
        }
    }

    #region Background Music
    public void PlayMusic()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("⚠️ No background music assigned.");
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;

        // If the music is paused, resume it from the stored position
        if (musicPauseTime > 0f)
        {
            musicSource.time = musicPauseTime;
        }

        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicPauseTime = 0f;  // Reset music pause time
    }

    public void PlayMusicFromBeginning()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("⚠️ No background music assigned.");
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.time = 0f;  // Start from the beginning
        musicSource.Play();
    }
    #endregion

    #region Music Transitions
    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutMusicRoutine(duration));
    }

    private IEnumerator FadeOutMusicRoutine(float duration)
    {
        float startVolume = musicSource.volume;
        musicPauseTime = musicSource.time;  // Store the current time before pausing

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicSource.Pause();  // Pause the music
        musicSource.volume = startVolume;  // Reset volume for next time
    }
    #endregion

    #region Main Menu Music
    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic == null)
        {
            Debug.LogWarning("⚠️ No background music assigned for main menu.");
            return;
        }

        musicSource.clip = mainMenuMusic;
        musicSource.volume = mainMenuMusicVolume;
        musicSource.Play();
    }

    public void StopMainMenuMusic()
    {
        musicSource.Stop();
        musicPauseTime = 0f;  // Reset pause time for main menu music
    }
    #endregion
}
