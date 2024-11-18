using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CanvaReadyControl : MonoBehaviour
{
    public string chosenCharacter = null;
    public int playerID;
    [SerializeField] private PlayerReadyPotentia[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
        playerSprites = FindObjectsByType<PlayerReadyPotentia>(FindObjectsSortMode.None);
        playerSprites = playerSprites.OrderBy(sprite => sprite.name).ToArray();
    }

    public void OnReady(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chosenCharacter = playerSprites[playerID].chosenCharacter;
            playerSprites[playerID].ChangeReadyState();
        }
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerSprites[playerID].ChangeCharacter();
        }
    }
}
