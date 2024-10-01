using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBar : MonoBehaviour
{
    public BuffCountdown prefab;
    
    public void Add(Sprite sprite, float time)
    {
        BuffCountdown buffCountdown = Instantiate(prefab, transform);
        buffCountdown.Set(sprite, time);
    }
}
