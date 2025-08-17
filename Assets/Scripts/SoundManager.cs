using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip gameMusic;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioMixer musicMixer;

    [SerializeField] public AudioSource click;
    [SerializeField] public AudioSource hover;


    // boom
    private void Awake()
    {
		// there can only be one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
		// ONLY ONE
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
		// plays menu music and binds scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMenuMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		// idk if we have a main menu scene yet but yeah, it's main menu
		// switches track on context
        /*if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameMusic();
        }*/
        if (gameObject != null) PlayGameMusic();
    }

    private void PlayMenuMusic()
    {
        if (musicSource.clip == menuMusic) return;
        SwitchTrack(menuMusic);
    }

    private void PlayGameMusic()
    {
        if (musicSource.clip == gameMusic) return;
        SwitchTrack(gameMusic);
    }

    private void SwitchTrack(AudioClip newClip)
    {
        musicSource.clip = newClip;
        musicSource.Play();
    }

    public void SetMusicVolume(float value)
    {
        // unity uses dB so this is how ya work with that
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}