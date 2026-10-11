using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = -15f;

    private CharacterController _characterController;
    private float _verticalVelocity;
    private Vector2 _moveInput;
    private bool _jumpTriggered;
    private bool _sprintHeld;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public bool IsGrounded => _characterController.isGrounded;
    public Vector2 MoveInput => _moveInput;
    public bool SprintHeld => _sprintHeld;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    public void Move(Vector3 direction, float speed)
    {
        _characterController.Move(direction * (speed * Time.deltaTime));
    }

    public void Jump()
    {
        _verticalVelocity = _jumpForce;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        _sprintHeld = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            _jumpTriggered = true;
    }

    public bool ConsumeJump()
    {
        if (!_jumpTriggered) return false;
        _jumpTriggered = false;
        return true;
    }
}
