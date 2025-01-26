using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player, IPauseable
{
    private InputActionAsset _inputAsset;
    private InputActionMap _playerMap;
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _throwAction;
    public InputAction ThrowAction => _throwAction;

    private Vector2 _moveValue;
    public Vector2 MoveValue => _moveValue;
    private Vector2 _rotateValue;

    [SerializeField] private float _speed;
    [SerializeField] private float deadZone = 0.2f;

    public string PlayerID = "";
    public bool isShooting = false;
    public bool isMoving = false; // Flag to check if the player is moving

    private bool isfacingRight = true;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    void Awake()
    {
        _inputAsset = GetComponent<PlayerInput>().actions;
        _playerMap = _inputAsset.FindActionMap("Player");
        _moveAction = _playerMap.FindAction("Move");
        _rotateAction = _playerMap.FindAction("Rotate");
        _throwAction = _playerMap.FindAction("Throw");

        

        sight = transform.GetChild(1).gameObject;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
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

    void FixedUpdate()
    {
        Vector2 newPosition = new Vector2(_moveValue.x, _moveValue.y) * _speed * Time.deltaTime;
        transform.Translate(newPosition);

        if (newPosition.x < 0f && isfacingRight)
            Flip();
        if (newPosition.x > 0f && !isfacingRight)
            Flip();
    }

    private void Movement(InputAction.CallbackContext context)
    {
        _moveValue = context.ReadValue<Vector2>();

        // Update isMoving based on the magnitude of movement input
        if (_moveValue.magnitude >  deadZone) // If the input is beyond the dead zone, the player is moving
        {
            isMoving = true;
            animator.SetBool("isMoving", isMoving);
            
        }
        else
        {
            isMoving = false; // If there is no movement input, the player is not moving
            
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        isfacingRight = !isfacingRight;
    }

    #region rotation

    private void Rotate(InputAction.CallbackContext context)
    {
        _rotateValue = context.ReadValue<Vector2>();

        if (_rotateValue != Vector2.zero)
            isShooting = true;

        switch ((_rotateValue.x, _rotateValue.y))
        {
            case (-1f, 0f): // Left
                ApplyRotation(90f);
                break;
            case (1f, 0f): // Right
                ApplyRotation(-90f);
                break;
            case (0f, 1f): // Up
                ApplyRotation(0);
                break;
            case (0f, -1f): // Down
                ApplyRotation(180f);
                break;
            case (-1f, 1f): // Up-Left
                ApplyRotation(45f);
                break;
            case (1f, 1f): // Up-Right
                ApplyRotation(-45f);
                break;
            case (-1f, -1f): // Down-Left
                ApplyRotation(135f);
                break;
            case (1f, -1f): // Down-Right
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

    public void Pause()
    {
        _moveValue = Vector2.zero;
        _moveAction.Disable();
        _rotateAction.Disable();
        _throwAction.Disable();

        GetComponent<PlayerStats>().Pause();
    }

    public void Unpause()
    {
        _moveAction.Enable();
        _rotateAction.Enable();
        _throwAction.Enable();

        GetComponent<PlayerStats>().Unpause();
    }

    #endregion

    public void RiseSpeed(float amount)
    {
        _speed += _speed * amount / 100f;
    }
}
