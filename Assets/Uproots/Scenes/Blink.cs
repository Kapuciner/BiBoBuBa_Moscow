using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Image image;

    public float speed;
    public float power;

    public Color color;

    public AnimationCurve curve;
    private void Update()
    {
        float a;
        //float a = (Mathf.Sin(Time.timeSinceLevelLoad * speed) * power + 1);
        // image.color = Color.Lerp(Color.white, color, a);

        a = curve.Evaluate(Time.timeSinceLevelLoad * speed);        
        image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        
        
    }
}
