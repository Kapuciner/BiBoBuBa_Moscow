using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPicture;
    [SerializeField] private GameObject choseDeviceWindow;
    [SerializeField] private GameObject deviceConnectManager;
    static private bool alreadyChosenControl = false;
    [SerializeField] private ConnectionData _connectionData;
    void Start()
    {
        Physics.defaultMaxDepenetrationVelocity = 20;
        GameManagerArena.winCounts = new List<int>() { 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (mainPicture.activeSelf)
                mainPicture.SetActive(false);
        }
    }
}
