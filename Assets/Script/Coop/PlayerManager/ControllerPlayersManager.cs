using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPlayersManager : DeviceManager
{
    [Header("Players Spawn Properties")]
    [SerializeField] private List<Transform> _startingPoints;
    [SerializeField] private List<LayerMask> _playerLayerMasks;

    [Header("Debug")]
    private static ControllerPlayersManager _instance;
    public static ControllerPlayersManager Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;

        int i = 0;
        _players.ForEach(player => 
        {
            player.transform.position = _startingPoints[i].position;
            i++;
            Debug.Log((player.name));
        });
    }

    protected override void AddPlayer(PlayerInput player)
    {
        _players.Add(player);

        Transform playerParent = player.transform.parent;
        playerParent.position = _startingPoints[0].position;
    }
}