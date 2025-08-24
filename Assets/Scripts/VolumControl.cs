using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumControl : MonoBehaviour
{
    [SerializeField] string _volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] float _multiplier = 30f;
    [SerializeField] Toggle _muteToggle;
    private bool _disableToggleEvent;

    private void Awake()
    {
        //Debug.Log("[VolumControl] Awake: Initializing volume system");
        //Debug.Log($"[VolumControl] Slider defaulted to max: {_slider.value}");
        //Debug.Log("[VolumControl] Mute toggle set to ON");

        _slider.value = _slider.maxValue;
        SetVolume(_slider.value);
        _muteToggle.isOn = true;
        _slider.onValueChanged.AddListener(SetVolume);
        _muteToggle.onValueChanged.AddListener(Mute);
    }

    private void Mute(bool enableSound)
    {
        if (_disableToggleEvent)
            return;

        if (enableSound)
        {
            _slider.value = _slider.maxValue;
        }
        else
        {
            _slider.value = _slider.minValue;
        }
    }
    public void ApplySavedVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(_volumeParameter, _slider.maxValue);
        _slider.value = savedVolume;
        ApplyVolume(savedVolume);
    }

    private void ApplyVolume(float value)
    {
        float safeValue = Mathf.Clamp(value, 0.0001f, 1f);
        float dbValue = Mathf.Log10(safeValue) * _multiplier;
        _mixer.SetFloat(_volumeParameter, dbValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
    }

    private void SetVolume(float value)
    {
        _mixer.SetFloat(_volumeParameter, Mathf.Log10(value) * _multiplier);
        _disableToggleEvent = true;
        _muteToggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(_volumeParameter, _slider.maxValue);

        _slider.value = savedVolume;
        SetVolume(savedVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
