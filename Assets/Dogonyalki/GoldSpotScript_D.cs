using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
public class GoldSpotScript_D : MonoBehaviour
{
    //[SerializeField] actionTimerCircle timer;
    // public TMP_Text timerText;
    public GameObject[] stonePile;

    public GameObject goldPiece;

    GoldPieceScript goldPieceScript;

    public int timeToWork;

    float timeRemaining; 

    public int workLeft;

    // Start is called before the first frame update
    void Start()
    {
        goldPieceScript = goldPiece.GetComponent<GoldPieceScript>();
        workLeft = stonePile.Length;
        //timer.StartTimer();
    }

    public int GetWorkLeft() {
        return workLeft;
    } 

    public void DoWork() {
        if (workLeft > 0) {
            stonePile[^workLeft].SetActive(false);
            workLeft -= 1;
            if(workLeft == 0){
                print("SET GOLD");
                goldPieceScript.SetGoldAvailable();
            }
        }
    }
}
