using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleOnPlayerJoin : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    void Awake()
    {
        _playerInputManager = FindAnyObjectByType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += ToggleThis;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= ToggleThis;
    }

    private void ToggleThis(PlayerInput input)
    {
        this.gameObject.SetActive(false);
    }
}
