using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PointsBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public float value;
    
    public void SetMaxValue(float v)
    {
        slider.maxValue = v;
    }
    public void SetValue(float v)
    {
        slider.value = v;
    }
}
