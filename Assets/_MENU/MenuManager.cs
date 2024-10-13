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
        //if (_connectionData.GetPlayerCount() > 0)
        //{
        //    alreadyChosenControl = true;
        //    mainPicture.SetActive(false);
        //    choseDeviceWindow.SetActive(false);
        //    deviceConnectManager.SetActive(false);
        //}
        Physics.defaultMaxDepenetrationVelocity = 20;
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
