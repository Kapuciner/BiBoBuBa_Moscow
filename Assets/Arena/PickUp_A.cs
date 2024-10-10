using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_A : MonoBehaviour
{
    private int randomSkill;
    [SerializeField] private Sprite[] abilitiesUI;
    private bool alreadyPickedUp = false;

    [SerializeField] private float spawnInterval = 10;
    private void Start()
    {
        Reroll();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = abilitiesUI[randomSkill];
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !alreadyPickedUp)
        {
            if (other.gameObject.GetComponent<ArenaPlayerManager>().Skill[0] == "" || other.gameObject.GetComponent<ArenaPlayerManager>().Skill[1] == "")
            {
                alreadyPickedUp = true;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                this.gameObject.GetComponent<Collider>().enabled = false;
                switch (randomSkill)
                {
                    case 0:
                        other.gameObject.GetComponent<ArenaPlayerManager>().GotNewAbility("Fire", abilitiesUI[randomSkill]);
                        break;
                    case 1:
                        other.gameObject.GetComponent<ArenaPlayerManager>().GotNewAbility("Water", abilitiesUI[randomSkill]);
                        other.gameObject.GetComponent<ArenaPlayerManager>().ExtinguishFire(); //тушится огонь при подборе воды
                        break;
                    case 2:
                        other.gameObject.GetComponent<ArenaPlayerManager>().GotNewAbility("Air", abilitiesUI[randomSkill]);
                        break;
                    case 3:
                        other.gameObject.GetComponent<ArenaPlayerManager>().GotNewAbility("Earth", abilitiesUI[randomSkill]);
                        break;
                }

                Invoke("SpawnNew", spawnInterval);
            }
        }
    }

    void SpawnNew()
    {
        Reroll();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = abilitiesUI[randomSkill];
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<Collider>().enabled = true;
        alreadyPickedUp = false;

    }
    void Reroll()
    {
        randomSkill = Random.Range(0, abilitiesUI.Length);
    }
}
