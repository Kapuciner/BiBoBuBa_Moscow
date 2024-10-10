using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager_D : MonoBehaviour
{
    [SerializeField] GameObject winScreen;

    [SerializeField] Timer_D gameTimer;
    [SerializeField] float gameLengthSecs;
    [SerializeField] PlayerScript_D[] players;

    [SerializeField] Slider player1Slider;
    [SerializeField] TextMeshProUGUI player1RespawnTimer;

    [SerializeField] Slider player2Slider;
    [SerializeField] TextMeshProUGUI player2RespawnTimer;

    // Start is called before the first frame update

    void Awake()
    {
        gameTimer.SetTimer(gameLengthSecs);
    }
    void Start()
    {
        players[0].SetPickaxeState(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGoldToPlayer(int playerNumber) {
        print("GOLD COLLECTED");
        if(playerNumber == 0) player1Slider.value++;
        else player2Slider.value++;
    }

    public void playerRespawn(int playerNumber){
        
        if(playerNumber == 0) StartCoroutine(RespawnCoroutine(player1RespawnTimer, playerNumber));
        else StartCoroutine(RespawnCoroutine(player2RespawnTimer, playerNumber));
        
    }

    IEnumerator RespawnCoroutine(TextMeshProUGUI respawnTimer, int playerIndex){
        float elapsed = 0;
        players[playerIndex].gameObject.SetActive(false);
        while (elapsed < 5f)
        {
            respawnTimer.text = Mathf.RoundToInt(5 - elapsed).ToString();
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        respawnTimer.text = "";
        players[playerIndex].transform.position = new Vector3(3, 1.5f, -2);
        players[playerIndex].gameObject.SetActive(true);
    }

    public void GameOver() {
        winScreen.SetActive(true);
    }
}
