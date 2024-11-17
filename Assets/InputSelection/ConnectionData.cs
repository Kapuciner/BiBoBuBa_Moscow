using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ConnectionData", menuName = "ScriptableObjects/ConnectionData", order = 2)]
public class ConnectionData : ScriptableObject
{
    public struct PlayerControlData
    {
        public int playerID;
        public InputDevice Device;
        public bool _isConnected;
    }

    public PlayerControlData p1_controlData;
    public PlayerControlData p2_controlData;
    public PlayerControlData p3_controlData;
    public PlayerControlData p4_controlData;

    public void SetData(int playerIndex, InputDevice device)
    {
        switch (playerIndex)
        {
            case 0:
            {
                p1_controlData.Device = device;
                p1_controlData.playerID = 0;
                p1_controlData._isConnected = true;
                break;
            }
            case 1:
            {
                p2_controlData.Device = device;
                p2_controlData.playerID = 1;  
                p2_controlData._isConnected = true;
                break;
            }
            case 2:
            {
                p3_controlData.Device = device;
                p3_controlData.playerID = 2;
                p3_controlData._isConnected = true;
                break;
            }
            case 3:
            {
                p4_controlData.Device = device;
                p4_controlData.playerID = 3;
                p4_controlData._isConnected = true;
                break;
            }
        }
    }

    public List<PlayerControlData> ConnectedPlayersData()
    {
        var list = new List<PlayerControlData>();
        if (p1_controlData._isConnected)
        {
            list.Add(p1_controlData);
        }
        if (p2_controlData._isConnected)
        {
            list.Add(p2_controlData);
        }
        if (p3_controlData._isConnected)
        {
            list.Add(p3_controlData);
        }
        if (p4_controlData._isConnected)
        {
            list.Add(p4_controlData);
        }

        return list;
    }

    public int GetPlayerCount()
    {
        return ConnectedPlayersData().Count;
    }

    public void Clear()
    {
        ClearPlayerData(p1_controlData);
        ClearPlayerData(p2_controlData);
        ClearPlayerData(p3_controlData);
        ClearPlayerData(p4_controlData);
        
    }

    private void ClearPlayerData(PlayerControlData playerControlData)
    {
        p1_controlData = new PlayerControlData();
        p2_controlData = new PlayerControlData();
        p3_controlData = new PlayerControlData();
        p4_controlData = new PlayerControlData();
        return;
        playerControlData.Device = null;
        playerControlData.playerID = 0;
        playerControlData._isConnected = false;
    }
}
