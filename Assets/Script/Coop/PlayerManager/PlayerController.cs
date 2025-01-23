using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActionAsset _inputAsset;
    private InputActionMap _playerMap;
    private InputAction _moveAction;
    private InputAction _rotateAction;

    private float2 _moveValue;
    private Vector2 _rotateValue;    

    [SerializeField] private float _speed;
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private GameObject mirino;
    
    void Awake()
    {
        _inputAsset = GetComponent<PlayerInput>().actions;
        _playerMap = _inputAsset.FindActionMap("Player");
        _moveAction = _playerMap.FindAction("Move");
        _rotateAction = _playerMap.FindAction("Rotate");

        mirino = transform.GetChild(1).gameObject;
    }

    void OnEnable()
    {
        _moveAction.performed += Movement;
        _moveAction.Enable();

        _rotateAction.performed += Rotate;
        _rotateAction.Enable(); 
    }

    void OnDisable()
    {
        _moveAction.performed -= Movement;
        _moveAction.Disable();

        _rotateAction.performed -= Rotate;
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

        //var _rotateValueSafeNormalized = math.normalizesafe(_rotateValue);
        //Debug.Log("Direction " + _rotateValueSafeNormalized);
        Debug.Log("Direction: " + _rotateValue);

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
            case (0f, -1f): // Giù
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
                // Nessuna rotazione se l'input è neutro
                break;
        }
    }

    private Vector2 NormalizeInput(Vector2 input)
    {
        float x = Mathf.Round(input.x); // Arrotonda al numero intero più vicino
        float y = Mathf.Round(input.y); // Arrotonda al numero intero più vicino
        return new Vector2(x, y);
    }

    private void ApplyRotation(float angle)
    {
        mirino.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion
}
