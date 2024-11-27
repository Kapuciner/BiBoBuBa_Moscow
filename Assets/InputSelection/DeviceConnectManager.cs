using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeviceConnectManager : MonoBehaviour
{
   [SerializeField] private List<DeviceSelection> _deviceSelections;
   
    private bool _kbConnected1 = false;
    private bool _kbConnected2 = false;

    private DeviceSelection _kbSelection1;
    private DeviceSelection _kbSelection2;

    public int ConnectedCount;

    private Coroutine _readyRoutine;

    [SerializeField] private ConnectionData _connectionData;
    [SerializeField] private TextMeshProUGUI GameStartingText;

    [SerializeField] private GameObject DeviceSelectionUI;

    [SerializeField] private float ReadyTime;
    
    private void Awake()
    {
        Time.timeScale = 1;
        //_deviceSelections = FindObjectsOfType<DeviceSelection>().ToList();
        ConnectedCount = 0;
        GameStartingText.gameObject.SetActive(false);
        DeviceSelectionUI.SetActive(true);
        if (MenuManager.alreadyChosenControl)
        {
            GetComponent<PlayerInputManager>().DisableJoining();

            ConnectPlayers();
            DeviceSelectionUI.SetActive(false);
            var inputs = FindObjectsOfType<PlayerInput>();
            foreach (var input in inputs)
            {
                if (input.isActiveAndEnabled)
                {
                    input.SwitchCurrentActionMap("Lobby");
                }
            }
            var mngr = GetComponent<PlayerInputManager>();
        
            FindObjectOfType<LobbyManager>().OnStarted();
            
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MenuManager.alreadyChosenControl = false;
            SceneManager.LoadScene(0);
        }
        
        if (ConnectedCount >= 2 && _readyRoutine == null)
        {
            StartReadyCountdown();
        }
    }

    private void StartReadyCountdown()
    {
        _readyRoutine = StartCoroutine(ReadyRoutine());
    }

    private IEnumerator ReadyRoutine()
    {
        GameStartingText.gameObject.SetActive(true);
        float elapsed = 0;
        float time = ReadyTime;
        int prev_count = ConnectedCount;
        while (elapsed < time)
        {
            //заново таймер когда люди заходят/выходят
            if (ConnectedCount != prev_count)
            {
                prev_count = ConnectedCount;
                elapsed = 0;
            }
            if (ConnectedCount == 4)
            {
                GetComponent<PlayerInputManager>().DisableJoining();
            }
            else
            {
                GetComponent<PlayerInputManager>().EnableJoining();
            }
            GameStartingText.text = "Начало через " + (time-elapsed).ToString("F1") + "с";
            if (ConnectedCount < 2)
            {
                _readyRoutine = null;
                GameStartingText.gameObject.SetActive(false);
                GetComponent<PlayerInputManager>().EnableJoining();
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GameStartingText.gameObject.SetActive(false);
        GetComponent<PlayerInputManager>().DisableJoining();
        StartGame();
    }

    private void StartGame()
    {

        SaveControlData();
        DeviceSelectionUI.SetActive(false);
        var inputs = FindObjectsOfType<PlayerInput>();
        foreach (var input in inputs)
        {
            if (input.isActiveAndEnabled)
            {
                input.SwitchCurrentActionMap("Lobby");
            }
        }
        var mngr = GetComponent<PlayerInputManager>();
        
        FindObjectOfType<LobbyManager>().OnStarted();
        MenuManager.alreadyChosenControl = true;
    }
    public void ConnectPlayers()
    {
        bool keyboardJoined = false;
        foreach (var player in _connectionData.ConnectedPlayersData())
        {
            if (player.Device is Keyboard)
            {
                if (keyboardJoined)
                {
                    continue;
                }
                keyboardJoined = true;
            }
            GetComponent<PlayerInputManager>().
                JoinPlayer(player.playerID, -1,
                    null, player.Device);
        }
    }
    private void SaveControlData()
    {
        var inputs = FindObjectsOfType<PlayerInput>();
        List<PlayerInput> inputs_to_destroy = new List<PlayerInput>();
        
        foreach (var input in inputs)
        {
            bool selected = false;
            foreach (var device in _deviceSelections)
            {
                if (input.devices[0] == device.Device)
                {
                    selected = true;
                    _connectionData.SetData(device.GetIndex(), device.Device);
                }
            }

            if (!selected)
            {
                inputs_to_destroy.Add(input);
            }
            
        }

        foreach (var dInput in inputs_to_destroy)
        {
            Destroy(dInput);
        }
    }
    public void OnGamepadPress(InputDevice device)
    {
        TryConnectDevice(device);
    }

    public void OnKeyboardPress1(InputDevice device)
    {
        if (!_kbConnected1 && GetEmptySelection() != null)
        {
            DeviceSelection selection = GetEmptySelection();
            selection.Connect(device);
            _kbConnected1 = true;
            _kbSelection1 = selection;
            return;
        }
        _kbSelection1.Disconnect(device);
        _kbConnected1 = false;
    }
    public void OnKeyboardPress2(InputDevice device)
    {
        if (!_kbConnected2 && GetEmptySelection() != null)
        {
            DeviceSelection selection = GetEmptySelection();
            selection.Connect(device);
            _kbConnected2 = true;
            _kbSelection2 = selection;
            return;
        }
        _kbSelection2?.Disconnect(device);
        _kbConnected2 = false;
    }

    private void TryConnectDevice(InputDevice device)
    {
        if (IsSelected(device))
        {
            GetDeviceAssignment(device)?.Disconnect(device);
            return;
        }
        GetEmptySelection()?.Connect(device);
    }
    

    public DeviceSelection GetEmptySelection()
    {
        for (int i = 0; i < _deviceSelections.Count; i++)
        {
            if (_deviceSelections[i].Selected == false)
            {
                return _deviceSelections[i];
            } 
        }

        return null;
    }

    public bool IsSelected(InputDevice device)
    {
        foreach (var selection in _deviceSelections)
        {
            if (selection.Device == device)
            {
                return true;
            }
        }
        return false;
    }

    public DeviceSelection GetDeviceAssignment(InputDevice device)
    {
        foreach (var selection in _deviceSelections)
        {
            if (selection.Device == device)
            {
                return selection;
            }
        }
        return null;
    }

    public int GetPlayerCount()
    {
        return _connectionData.GetPlayerCount();
    }

    public void OnDeviceDisconnect(DeviceSelection disconnectedSelection)
    {
        int index = disconnectedSelection.GetIndex();
        if (IsSelectionAGap(disconnectedSelection))
        {
            for (int i = index + 1; i < _deviceSelections.Count; i++)
            {
                if (_deviceSelections[i].Selected)
                {
                    if (_kbSelection1 == _deviceSelections[i])
                    {
                        _kbSelection1 = disconnectedSelection;
                    }
                    if (_kbSelection2 == _deviceSelections[i])
                    {
                        _kbSelection2 = disconnectedSelection;
                    }
                    disconnectedSelection.Connect(_deviceSelections[i].Device);
                    _deviceSelections[i].DisconnectButLeaveDevice();
                    return;
                }
            } 
        }
    }

    private bool IsSelectionAGap(DeviceSelection selection)
    {
        int index = selection.GetIndex();

        bool right = false;
        for (int i = index + 1; i < _deviceSelections.Count; i++)
        {
            if (_deviceSelections[i].Selected)
            {
                right = true;
            }
        }

        if (right && selection.Selected == false)
        {
            return true;
        }

        return false;
    }
    
}
