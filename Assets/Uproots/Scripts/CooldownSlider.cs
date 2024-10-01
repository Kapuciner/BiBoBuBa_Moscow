using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownSlider : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient Gradient;
    public float value;

    public void SetMaxValue(float v)
    {
        slider.maxValue = v;
    }
    public void SetValue(float v)
    {
        slider.value = v;
        fill.color = Gradient.Evaluate(slider.normalizedValue);
    }
}
