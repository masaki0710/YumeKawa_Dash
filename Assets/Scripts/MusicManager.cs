using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource bgmSource;
    private float defaultBgmVolume;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            defaultBgmVolume = bgmSource.volume;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bgmSource != null)
        {
            if (scene.name == "GameScene")
            {
                bgmSource.pitch = 1.0f;
                bgmSource.volume = defaultBgmVolume;
                if (!bgmSource.isPlaying)
                {
                    bgmSource.Play();
                }
            }
            else
            {
                bgmSource.Stop();
            }
        }

        bgmSource.pitch = 1.0f;
        bgmSource.volume = defaultBgmVolume;

    }

    public void StartGameOverEffect(float duration)
    {
        StartCoroutine(FadeOutAndSlowDown(duration));
    }

    private IEnumerator FadeOutAndSlowDown(float duration)
    {
        float currentTime = 0f;
        float startPitch = bgmSource.pitch;
        float startVolume = bgmSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float t = currentTime / duration;
            if (bgmSource != null)
            {
                bgmSource.pitch = Mathf.Lerp(startPitch, 0.5f, t);
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);
            }
            yield return null;
        }

        bgmSource.Stop();
    }

    public void SetPitch(float pitch)
    {
        if (bgmSource != null)
        {
            bgmSource.pitch = pitch;
        }
    }

    public void SetVolumeRatio(float ratio)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = defaultBgmVolume * ratio;
        }
    }

    public void stopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
}
