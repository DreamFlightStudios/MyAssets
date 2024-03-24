using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("MovementSettings")]
    [SerializeField] private int _speed;
    [SerializeField] private int _gravity;

    [Header("CameraSettings")]
    [SerializeField] private int _sensivity;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Vector2 _minMaxRotationX;

    private PlayerInput _playerInput;
    private CharacterController _characterController;

    private Vector3 _rotationDiraction;
    private Vector3 _movementDirection;

    private void Awake() => _playerInput = new PlayerInput();

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        Vector2 input = _playerInput.Player.LookInput.ReadValue<Vector2>();

        _rotationDiraction += new Vector3(-input.y, input.x, 0) * Time.deltaTime * _sensivity;
        _rotationDiraction.x = Mathf.Clamp(_rotationDiraction.x, _minMaxRotationX.x, _minMaxRotationX.y);

        transform.rotation = Quaternion.Euler(0, _rotationDiraction.y, 0);
        _camera.transform.rotation = Quaternion.Euler(_rotationDiraction);
    }

    private void Move()
    {
        Vector2 input = _playerInput.Player.MovementInput.ReadValue<Vector2>();
        _movementDirection = (input.x * transform.right + input.y * transform.forward) * _speed * Time.deltaTime;

        if (!_characterController.isGrounded)
        {
            _movementDirection.y -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_movementDirection);
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();
}
