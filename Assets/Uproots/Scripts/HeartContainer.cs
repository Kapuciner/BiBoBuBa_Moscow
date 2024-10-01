using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public Sprite FullSprite;
    public Sprite EmptySprite;

    public Image image;
    public void SetFull(bool isFull)
    {
        image.sprite = isFull ? FullSprite : EmptySprite;
    }
}
