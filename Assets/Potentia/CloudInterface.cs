using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudInterface : MonoBehaviour
{
    public int currentEnergy = 5;
    int currentAbilityIndex = 0;
    public GameObject[] frames;
    public CloudScript clousScript;
    public AudioSource nope;

    [SerializeField] private Sprite cloudEnergyFull;
    [SerializeField] private Sprite cloudEnergyEmpty;
    [SerializeField] private Image[] cloudEmptyEnergy;

    private void Start()
    {
        nope = GetComponent<AudioSource>();
        UpdateInterface();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!useAbility()) {
                nope.Play();
            };
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetMouseButtonDown(1))
        {
            currentAbilityIndex++;
            if (currentAbilityIndex == 5)
                currentAbilityIndex = 0;
            UpdateInterface();
        }

        //if (scroll > 0f)
        //{
        //    currentAbilityIndex++;
        //    if (currentAbilityIndex == 5)
        //        currentAbilityIndex = 0;
        //    UpdateInterface();

        //}
        //else if (scroll < 0f)
        //{
        //    currentAbilityIndex--;
        //    if (currentAbilityIndex == -1)
        //        currentAbilityIndex = 4;
        //    UpdateInterface();
        //}
    }

    public bool TryAddEnergy()
    {
        bool returnVal;
        if (currentEnergy < 5) {
            currentEnergy++;
            returnVal = true;
        }
        else returnVal = false;
        UpdateInterface();
        return returnVal;
        
    }

    bool useAbility()
    {
        if (currentAbilityIndex == 0 && currentEnergy >= 1)
        {
            clousScript.Ability1();
            currentEnergy--;
            UpdateInterface();
            return true;
        }
        if (currentAbilityIndex == 1 && currentEnergy >= 2)
        {
            currentEnergy -= 2;
            UpdateInterface();
            clousScript.Ability4();
            return true;
        }
        if (currentAbilityIndex == 2 && currentEnergy >= 3)
        {
            currentEnergy -= 3;
            UpdateInterface();
            clousScript.Ability2();
            return true;
        }
        if (currentAbilityIndex == 3 && currentEnergy >= 4)
        {
            currentEnergy -= 4;
            UpdateInterface();
            clousScript.Ability5();
            return true;
        }
        if (currentAbilityIndex == 4 && currentEnergy >= 5)
        {
            currentEnergy -= 5;
            UpdateInterface();
            clousScript.Ability3();
            return true;
        }
        return false;
    }

    void UpdateInterface()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == currentAbilityIndex)
            {
                frames[i].GetComponent<RectTransform>().localScale = new Vector3(0.52f, 0.52f, 1);
                frames[i].GetComponent<Image>().color = new Color32(0xDE, 0x00, 0xA0, 0xFF);
            }
            else
            {
                frames[i].GetComponent<RectTransform>().localScale = new Vector3(0.5038357f, 0.4571707f, 1);
                frames[i].GetComponent<Image>().color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            }
        }         

        for (int i = 0; i < 5; i++)
        {
            if (i < currentEnergy)
            {
                cloudEmptyEnergy[i].sprite = cloudEnergyFull;
            }
            else
            {
                cloudEmptyEnergy[i].sprite = cloudEnergyEmpty;
            }
        }
        Debug.Log($"current ability is {currentAbilityIndex}");
    }
}