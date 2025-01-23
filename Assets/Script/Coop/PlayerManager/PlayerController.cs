using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActionAsset _inputAsset;
    private InputActionMap _playerMap;
    private InputAction _moveAction;

    [SerializeField] private float2 _moveValue;
    [SerializeField] private float _speed;
    
    void Awake()
    {
        _inputAsset = GetComponent<PlayerInput>().actions;
        _playerMap = _inputAsset.FindActionMap("Player");
        _moveAction = _playerMap.FindAction("Move");
    }

    void OnEnable()
    {
        _moveAction.performed += Movement;
        _moveAction.Enable();
    }

    void OnDisable()
    {
        _moveAction.performed -= Movement;
        _moveAction.Disable();
    }

    void Update()
    {
        transform.Translate(new Vector2(_moveValue.x, _moveValue.y) * _speed * Time.deltaTime);
    }

    private void Movement(InputAction.CallbackContext context) => _moveValue = context.ReadValue<Vector2>();
}
