using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    private InputActionAsset _inputAsset;
    private InputActionMap _playerMap;
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _throwAction;
    public InputAction ThrowAction => _throwAction;

    private float2 _moveValue;
    public float2 MoveValue => _moveValue;
    private Vector2 _rotateValue;    

    [SerializeField] private float _speed;
    [SerializeField] private float deadZone = 0.2f;
    
    void Awake()
    {
        _inputAsset = GetComponent<PlayerInput>().actions;
        _playerMap = _inputAsset.FindActionMap("Player");
        _moveAction = _playerMap.FindAction("Move");
        _rotateAction = _playerMap.FindAction("Rotate");
        _throwAction = _playerMap.FindAction("Throw");

        sight = transform.GetChild(1).gameObject;
    }

    void OnEnable()
    {
        _moveAction.performed += Movement;
        _moveAction.Enable();

        _rotateAction.performed += Rotate;
        _rotateAction.canceled += Rotate;
        _rotateAction.Enable(); 
    }

    void OnDisable()
    {
        _moveAction.performed -= Movement;
        _moveAction.Disable();

        _rotateAction.performed -= Rotate;
        _rotateAction.canceled -= Rotate;
        _rotateAction.Disable();
    }

    void Update()
    {
        transform.Translate(new Vector2(_moveValue.x, _moveValue.y) * _speed * Time.deltaTime);
    }

    private void Movement(InputAction.CallbackContext context) => _moveValue = context.ReadValue<Vector2>();

    #region rotation

    private void Rotate(InputAction.CallbackContext context)
    {
        _rotateValue = context.ReadValue<Vector2>();
        //Debug.Log("_rotate value: " + _rotateValue);

        if (_rotateValue != Vector2.zero)
            isShooting = true;

        switch ((_rotateValue.x, _rotateValue.y))
        {
            case (-1f, 0f): // Sinistra
                ApplyRotation(90f);
                break;
            case (1f, 0f): // Destra
                ApplyRotation(-90f);
                break;
            case (0f, 1f): // Su
                ApplyRotation(0);
                break;
            case (0f, -1f): // Gi�
                ApplyRotation(180f);
                break;
            case (-1f, 1f): // Alto-Sinistra
                ApplyRotation(45f);
                break;
            case (1f, 1f): // Alto-Destra
                ApplyRotation(-45f);
                break;
            case (-1f, -1f): // Basso-Sinistra
                ApplyRotation(145f);
                break;
            case (1f, -1f): // Basso-Destra
                ApplyRotation(225f);
                break;
            default:
                isShooting = false;

                break;
        }
    }

    private void ApplyRotation(float angle)
    {
        sight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion
}
