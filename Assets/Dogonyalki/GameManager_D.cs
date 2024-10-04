using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_D : MonoBehaviour
{
    [SerializeField] GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver() {
        winScreen.SetActive(true);
    }
}
