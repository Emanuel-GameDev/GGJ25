using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class DeviceManager : MonoBehaviour
{
    [Header("Players")]
    protected PlayerInputManager _playerInputManager;
    [SerializeField] protected List<PlayerInput> _players;
    public List<PlayerInput> Players => _players;

    [SerializeField] protected List<PlayerInputData> _playersData = new List<PlayerInputData>();
    public List<PlayerInputData> PlayersData => _playersData;

    [Header("Prefab References")]
    
    [Tooltip("Prefab to use for new players")]
    [SerializeField] protected GameObject _promptPrefab;

    [Header("Actions")]
    
    protected UnityAction<PlayerInput> onPlayerRemoved; 

    protected virtual void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerPrefab = _promptPrefab;
    }

    protected virtual void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;

        //_playerInputManager.onPlayerLeft += RemovePlayerOnPlayerLeft;

        //InputSystem.onDeviceChange += RemovePlayerOnDeviceRemoved;
    }

    protected virtual void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
        //_playerInputManager.onPlayerLeft -= RemovePlayer;

        //InputSystem.onDeviceChange -= RemovePlayerOnDeviceRemoved;
    }

    protected virtual void AddPlayer(PlayerInput playerToAdd)
    {
        _players.Add(playerToAdd);
    }

    protected virtual void RemovePlayer(PlayerInput playerToRemove)
    {
        Debug.Log("RemovePLayer");
        if (_players.Contains(playerToRemove))
        {
            //_players.Remove(playerToRemove);

            onPlayerRemoved?.Invoke(playerToRemove);
        }
    }

    protected virtual void RemovePlayerOnPlayerLeft(PlayerInput playerToRemove)
    {
        Debug.Log("RemovePlayerOnPlayerLeft");

        if (_players.Contains(playerToRemove))
        {
            _players.Remove(playerToRemove);

            onPlayerRemoved?.Invoke(playerToRemove);
        }
    }

    protected virtual void RemovePlayerOnDeviceRemoved(InputDevice device, InputDeviceChange change)
    {
        //Non {InputDeviceChange.Disconnected} perché altrimenti non trova il device
        //nella lista dei Player (_players)
        if (change == InputDeviceChange.Removed)
        {
            PlayerInput playerToRemove = null;
            foreach (var player in _players)
            {
                if (player.devices.Contains(device))
                {
                    playerToRemove = player;
                    break;
                }
            }

            if (playerToRemove != null)
            {
                RemovePlayer(playerToRemove);
            }
        }
    }

    public void CopyPlayerInputManagerData(List<PlayerInputData> players)
    {
        _playersData = players;

        JoinPlayers(players);
    }

    protected virtual void JoinPlayers(List<PlayerInputData> players)
    {
        players.ForEach(player =>
        {
            _players.Add(_playerInputManager.JoinPlayer(player.playerIndex,
                                            -1,
                                            player.currentControlScheme,
                                            player.device));
        });
    }
}

[Serializable]
public struct PlayerInputData
{
    public int playerIndex;
    public string currentControlScheme;
    public InputDevice device;

    public PlayerInputData(int playerIndex, string currentControlScheme, InputDevice device)
    {
        this.playerIndex = playerIndex;
        this.currentControlScheme = currentControlScheme;
        this.device = device;
    }

    public static PlayerInputData BuildPlayerData(PlayerInput playerInput)
    {
        return new PlayerInputData( playerInput.playerIndex, 
                                    playerInput.currentControlScheme, 
                                    playerInput.devices[0]);
    }

    public static PlayerInputData BuildPlayerData(PlayerInputData playerInputData)
    {
        return new PlayerInputData( playerInputData.playerIndex, 
                                    playerInputData.currentControlScheme, 
                                    playerInputData.device);
    }

    public static PlayerInputData Clone(PlayerInput playerInput)
    {
        return BuildPlayerData(playerInput);
    }

    public static List<PlayerInputData> Clone(List<PlayerInputData> players)
    {
        return players.Select(player => BuildPlayerData(player)).ToList();
    }
}