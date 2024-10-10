using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Image = UnityEngine.UI.Image;

public class DeviceSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _connectedDeviceName;
    [SerializeField] private int _playerIndex;
    [SerializeField] private Image _image;

    private DeviceConnectManager _connectManager;

    [Header("Sprites")] [SerializeField] private Sprite _kbgp;
    [SerializeField] private Sprite _kb;
    [SerializeField] private Sprite _gp;
    [SerializeField] private Image _devicesImage;
    public InputDevice Device;
    public bool Selected = false;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _connectManager = FindObjectOfType<DeviceConnectManager>();
    }

    private void Update()
    {
        if (Selected)
        {
            _image.color = Color.green;

        }
        else _image.color = Color.red;
        _connectedDeviceName.text = Device?.name;
    }

    public void Connect(InputDevice device)
    {
        Device = device;
        Selected = true;
        _connectManager.ConnectedCount++;
        HideSelectionSprite();
    }

    public void Disconnect(InputDevice device)
    {
        Selected = false;
        Device = null;
        _connectManager.ConnectedCount--;
        ShowSelectionSprite();
        Destroy(PlayerInput.FindFirstPairedToDevice(device));
    }

    public int GetIndex()
    {
        return _playerIndex;
    }

    public void ChangeSelectionText()
    {
        
    }

    public void SetSelectionSprite(Sprite sprite)
    {
        _devicesImage.sprite = sprite;
    }

    public void HideSelectionSprite()
    {
        _devicesImage.gameObject.SetActive(false);
    }

    public void ShowSelectionSprite()
    {
        _devicesImage.gameObject.SetActive(true);
    }
}
