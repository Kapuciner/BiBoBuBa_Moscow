using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveZone_D : MonoBehaviour
{
    public GameObject goldPiece;
    public GameObject doorObj;

    public int workLeft;

    GoldPieceScript goldPieceScript;
    SlideDoor_D slideDoorScript;
    // Start is called before the first frame update
    void Start()
    {
        goldPieceScript = goldPiece.GetComponent<GoldPieceScript>();
        slideDoorScript = doorObj.GetComponent<SlideDoor_D>();
        workLeft = slideDoorScript.workNeeded;
    }

    public int GetWorkLeft() {
        return workLeft;
    } 

    public void DoWork() {
        if (workLeft > 0) {
            slideDoorScript.SlideDoorOneStep();
            workLeft = slideDoorScript.workNeeded;
            if(workLeft == 0){
                goldPieceScript.SetGoldAvailable();
            }
        }
    }
}
