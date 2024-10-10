using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpArena : MonoBehaviour //для рандомного спавна в области. использовать вместе с LootboxSpawnerArea.cs и Lootbox.cs
{
    private int randomSkill;
    int timer = -1;
    [SerializeField] int maxTimeLife = 15;
    [SerializeField] int startFlashTime = 12;
    [SerializeField] private Sprite[] abilitiesUI;

    [SerializeField] float checkDistance = 1.5f; //менять в инспекторе
    [SerializeField] float moveSpeed = 0.5f;
    private bool isMovingDown = false;

    [SerializeField] LayerMask groundLayer;
    private bool alreadyPickedUp = false;
    private void Start()
    {
        StartCoroutine(LifeTimer());
        randomSkill = Random.Range(0, abilitiesUI.Length);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = abilitiesUI[randomSkill];
    }

    private void Update()
    {
        CheckSpaceBelow();
    }


    void CheckSpaceBelow() // по идее должен фиксить баг, когда пикапы разбиваются об голову игрока и находятся выше положенного
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            float distanceToGround = hit.distance;

            if (distanceToGround > checkDistance)
            {
                isMovingDown = true;
                MoveDown();
            }
            else
            {
                isMovingDown = false;
            }
        }

    }

    void MoveDown()
    {
        if (isMovingDown)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !alreadyPickedUp)
        {
            if (other.gameObject.GetComponent<ArenaPlayerManager>().Skill[0] == "" || other.gameObject.GetComponent<ArenaPlayerManager>().Skill[1] == "")
            {
                alreadyPickedUp = true;
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
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator LifeTimer()
    {
        while (timer < maxTimeLife)
        {
            if (timer == startFlashTime) // starts flash
                StartCoroutine(Flash());
            yield return new WaitForSeconds(1);  
            timer++;                             
        }

        Destroy(this.gameObject);
    }

    IEnumerator Flash()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.material.color;

        while (true)
        {
            Color transparentColor = originalColor;
            transparentColor.a = 0.0f; 

            renderer.material.color = transparentColor;


            yield return new WaitForSeconds(0.15f);


            renderer.material.color = originalColor;

            yield return new WaitForSeconds(0.4f);
        }
    }
}
