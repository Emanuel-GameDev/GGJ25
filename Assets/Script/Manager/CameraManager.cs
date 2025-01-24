using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup _targetGroup;
    
    private void Start()
    {
        if (_targetGroup == null) return;

        List<PlayerInput> players = ControllerPlayersManager.Instance.Players;

        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("Nessun player trovato nella lista dei giocatori!");
            return;
        }

        foreach (PlayerInput p in players)
        {
            _targetGroup.AddMember(p.gameObject.transform, 1f, 1f);
        }
    }
}
