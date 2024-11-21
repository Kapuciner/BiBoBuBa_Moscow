using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerReadyPotentia : MonoBehaviour
{
    public string chosenCharacter = null;
    Animator _animator;
    [SerializeField] private Animator _animatorResultScreen;
    bool playerReady = false;
    int type = 0;
    [SerializeField] private int playerID;
    [SerializeField] private TMP_Text ready;
    [SerializeField] private GameManager gm;

    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
        _animator.SetInteger("playerID", playerID);
        _animator.SetInteger("type", type);
        chosenCharacter = null;

        _animatorResultScreen.SetInteger("playerID", playerID);
        _animatorResultScreen.SetInteger("type", type);
    }
    public void ChangeCharacter()
    {
        if (gm.playersChosen)
            return;

        if (chosenCharacter == null)
        {
            chosenCharacter = "mage";
            type = 1;

            _animator.SetInteger("type", type);
            _animatorResultScreen.SetInteger("type", type);

            if (playerID == 1 || playerID == 3)
            {
                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
        else if (chosenCharacter == "mage" && !playerReady)
        {
            chosenCharacter = "cloud";
            type = 2;

            _animator.SetInteger("type", type);
            _animatorResultScreen.SetInteger("type", type);

            this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (chosenCharacter == "cloud" && !playerReady)
        {
            chosenCharacter = "mage";
            type = 1;
            _animator.SetInteger("type", type);
            _animatorResultScreen.SetInteger("type", type);

            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void ChangeReadyState()
    {
        if (chosenCharacter != null)
        {
            playerReady = !playerReady;

            if (playerReady)
            {
                ready.text = "Готов";
                ready.color = Color.green;
                gm.readyCount++;
                if (type == 1)
                    gm.readyMagesCount++;
                if (type == 2)
                    gm.readyCloudsCount++;
            }
            else
            {
                if (type == 1)
                    gm.readyMagesCount--;
                if (type == 2)
                    gm.readyCloudsCount--;
                ready.text = "Не готов";
                ready.color = Color.red;
                gm.readyCount--;
            }
        }
    }
}
