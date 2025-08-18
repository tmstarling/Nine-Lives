using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip gameMusic;

    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioMixer audioMixer;


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
        PlayGameMusic();
    }

    private void PlayMenuMusic()
    {
        if (audioSource.clip == menuMusic) return;
        SwitchTrack(menuMusic);
    }

    private void PlayGameMusic()
    {
        if (audioSource.clip == gameMusic) return;
        SwitchTrack(gameMusic);
    }

    private void SwitchTrack(AudioClip newClip)
    {
        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void SetMusicVolume(float value)
    {
		// unity uses dB so this is how ya work with that
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}