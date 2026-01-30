using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderManager : MonoBehaviour
{
    [Serializable]
    public class SliderBinding
    {
        public string parameterName;
        public Slider slider;
        [Range(0.0001f, 1f)]
        public float defaultValue = 1f;
    }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SliderBinding[] sliderBindings;

    private void OnEnable()
    {
        InitializeSliders();
    }

    private void OnDisable()
    {
        if (sliderBindings == null)
        {
            return;
        }

        foreach (var binding in sliderBindings)
        {
            if (binding?.slider == null)
            {
                continue;
            }

            binding.slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    private void InitializeSliders()
    {
        if (audioMixer == null || sliderBindings == null)
        {
            return;
        }

        foreach (var binding in sliderBindings)
        {
            if (binding?.slider == null || string.IsNullOrWhiteSpace(binding.parameterName))
            {
                continue;
            }

            binding.slider.minValue = 0.0001f;
            binding.slider.maxValue = 1f;
            binding.slider.value = Mathf.Clamp(binding.defaultValue, 0.0001f, 1f);
            binding.slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            binding.slider.onValueChanged.AddListener(OnSliderValueChanged);
            ApplyVolume(binding.parameterName, binding.slider.value);
        }
    }

    private void OnSliderValueChanged(float _)
    {
        if (audioMixer == null || sliderBindings == null)
        {
            return;
        }

        foreach (var binding in sliderBindings)
        {
            if (binding?.slider == null || string.IsNullOrWhiteSpace(binding.parameterName))
            {
                continue;
            }

            ApplyVolume(binding.parameterName, binding.slider.value);
        }
    }

    private void ApplyVolume(string parameterName, float value)
    {
        var db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameterName, db);
    }
}
