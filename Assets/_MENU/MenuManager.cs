using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPicture;
    [SerializeField] private GameObject choseDeviceWindow;
    [SerializeField] private GameObject deviceConnectManager;
    static public bool alreadyChosenControl = false;
    [SerializeField] private ConnectionData _connectionData;

    private void Awake()
    {
        Physics.defaultMaxDepenetrationVelocity = 40;
    }
    void Start()
    {
        Cursor.visible = false;
        GameManagerArena.winCounts = new List<int>() { 0, 0, 0, 0 };
        
        
        
        if (alreadyChosenControl == false)
        {
            _connectionData.Clear();
            print("обнулили контроль при запуске игры");
        }
        
        for (int i = 0; i < _connectionData.ConnectedPlayersData().Count; i++)
        {
            print(i.ToString() + _connectionData.ConnectedPlayersData()[i].Device.ToString());
            print(i.ToString() + _connectionData.ConnectedPlayersData()[i]._isConnected.ToString());
            print(i.ToString() + _connectionData.ConnectedPlayersData()[i].playerID.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alreadyChosenControl)
        {
            if (mainPicture.activeSelf)
                mainPicture.SetActive(false);
        }
        
        if (Input.anyKeyDown)
        {
            if (mainPicture.activeSelf)
                mainPicture.SetActive(false);
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        alreadyChosenControl = false;
    }
}
