using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{
    private int randomLootId = 999;
    private List<string> abilitiesNames;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Reroll();
            while (collision.gameObject.GetComponent<ArenaPlayerManager>().canDoAbilities[randomLootId] != 0)
            {
                Reroll();
            }

            collision.gameObject.GetComponent<ArenaPlayerManager>().GotNewAbility(abilitiesNames[randomLootId]);

            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        abilitiesNames = new List<string> { "dash1", "dash2", "dash3", "dash4", "dash5", "dash6", "dash7", "dash8" };

    }
    void Reroll()
    {
        randomLootId = Random.Range(0, abilitiesNames.Count);
    }
}
