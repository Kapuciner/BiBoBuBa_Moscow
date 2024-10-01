using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaPlayerManager : MonoBehaviour
{
    private Vector3 move;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private float speed;

    private Rigidbody _rb;
    private SpriteRenderer _sr;

    private string Skill1 = "";
    private string Skill2 = "";

    [SerializeField] private Sprite[] abilitiesUI;
    [SerializeField] private Image[] abilityImage; //первый или второй скилл
    [SerializeField] private TMP_Text[] skillName;
    [SerializeField] private TMP_Text[] skillKey;
    [SerializeField] private TMP_Text[] cooldownTXT;
    private int cooldown;
    private int abilityNumber = 0;

    public List<int> canDoAbilities; //leave empty

    void Start()
    {
        canDoAbilities = new List<int> { 0, 0, 0, 0, 0, 0, 0 , 0};

        _rb = GetComponent<Rigidbody>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Handle_Input();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
    }

    void Handle_Input()
    {
        move = Vector3.zero;
        if (Input.GetKey(up))
        {
            move += new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(down))
        {
            move -= new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(right))
        {
            move += new Vector3(1f, 0f, -1f);
            _sr.flipX = false;
        }
        if (Input.GetKey(left))
        {
            move -= new Vector3(1f, 0f, -1f);
            _sr.flipX = true;
        }

        move.Normalize();
    }

    public void GotNewAbility(string name)
    {
        switch (name)
        {
            case "dash1":
                abilityImage[abilityNumber].sprite = abilitiesUI[0];
                skillName[abilityNumber].text = "Dash1";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 5;
                break;
            case "dash2":
                abilityImage[abilityNumber].sprite = abilitiesUI[1];
                skillName[abilityNumber].text = "Dash2";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 4;
                break;
            case "dash3":
                abilityImage[abilityNumber].sprite = abilitiesUI[2];
                skillName[abilityNumber].text = "Dash3";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 6;
                break;
            case "dash4":
                abilityImage[abilityNumber].sprite = abilitiesUI[3];
                skillName[abilityNumber].text = "Dash4";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 2;
                break;
            case "dash5":
                abilityImage[abilityNumber].sprite = abilitiesUI[4];
                skillName[abilityNumber].text = "Dash5";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 1;
                break;
            case "dash6":
                abilityImage[abilityNumber].sprite = abilitiesUI[5];
                skillName[abilityNumber].text = "Dash6";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 5;
                break;
            case "dash7":
                abilityImage[abilityNumber].sprite = abilitiesUI[6];
                skillName[abilityNumber].text = "Dash7";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 8;
                break;
            case "dash8":
                abilityImage[abilityNumber].sprite = abilitiesUI[7];
                skillName[abilityNumber].text = "Dash8";
                skillKey[abilityNumber].text = "Shift";
                cooldown = 2;
                break;
        }
    }
}
