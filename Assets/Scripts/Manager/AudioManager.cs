using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Singleton")]
    public static AudioManager Instance;

    [Header("Audio Sources Settings")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _gameplayMusic;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 0.8f;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _slideSound;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 1f;

    private bool _isFading = false;

    void Awake() // <- gestisce l'implementazione del Singleton
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMainMenuMusic(); // <- inizia con la musica del main menu
    }

    #region Initialization
    private void InitializeAudioSources() // <- se non sono assegnate nell'inspector, crea le AudioSource automaticamente
    {
        if (_musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
        }

        if (_sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            _sfxSource = sfxObj.AddComponent<AudioSource>();
            _sfxSource.loop = false;
            _sfxSource.playOnAwake = false;
        }

        UpdateVolumes();
    }
    #endregion
    private void UpdateVolumes()
    {
        if (_musicSource != null) _musicSource.volume = _musicVolume;

        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }
    public void PlayMainMenuMusic()
    {
        if (_mainMenuMusic != null && !_isFading)
        {
            if (_musicSource.clip == _mainMenuMusic && _musicSource.isPlaying) return;

            FadeToNewMusic(_mainMenuMusic);
        }
    }

    public void PlayGameplayMusic()
    {
        if (_gameplayMusic != null && !_isFading)
        {
            if (_musicSource.clip == _gameplayMusic && _musicSource.isPlaying) return;

            FadeToNewMusic(_gameplayMusic);
        }
    }

    public void StopMusic()
    {
        if (_musicSource.isPlaying) StartCoroutine(FadeOut(_musicSource, _fadeDuration));
    }

    private void FadeToNewMusic(AudioClip newClip)
    {
        if (_musicSource.isPlaying) StartCoroutine(FadeToNewClip(newClip));

        else
        {
            _musicSource.clip = newClip;
            _musicSource.Play();
            StartCoroutine(FadeIn(_musicSource, _fadeDuration));
        }
    }

    public void PlayJumpSound()
    {
        if (_jumpSound != null) _sfxSource.PlayOneShot(_jumpSound);
    }

    #region Fade Coroutines
    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        _isFading = true;
        float startVolume = 0f;
        audioSource.volume = startVolume;

        while (audioSource.volume < _musicVolume)
        {
            audioSource.volume += _musicVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = _musicVolume;
        _isFading = false;
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        _isFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        _isFading = false;
    }

    private System.Collections.IEnumerator FadeToNewClip(AudioClip newClip)
    {
        _isFading = true;

        yield return StartCoroutine(FadeOut(_musicSource, _fadeDuration * 0.5f));

        _musicSource.clip = newClip;
        _musicSource.Play();

        yield return StartCoroutine(FadeIn(_musicSource, _fadeDuration * 0.5f));

        _isFading = false;
    }
    #endregion

    #region Scene Management
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) // <- gestisce automaticamente il cambio di musica in base alla scena
    {
        switch (scene.name.ToLower())
        {
            case "mainmenu":
            case "menu":
                {
                    PlayMainMenuMusic();
                    break;
                }
            case "game":
            case "gameplay":
            case "level":
                {
                    PlayGameplayMusic();
                    break;
                }
        }
    }
    #endregion
}
