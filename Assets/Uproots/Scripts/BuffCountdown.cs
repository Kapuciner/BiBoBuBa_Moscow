using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffCountdown : MonoBehaviour
{
    private Sprite _sprite;
    private float _time;
    public TextMeshProUGUI text;
    public Image image;
    
    public void Set(Sprite sprite, float time)
    {
        _sprite = sprite;
        _time = time;

        image.sprite = _sprite;

    }

    private void Update()
    {
        text.text = Mathf.RoundToInt(_time).ToString();
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
