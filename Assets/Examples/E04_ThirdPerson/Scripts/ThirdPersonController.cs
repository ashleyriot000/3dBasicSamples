using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
  public float cameraSensitivity = 1f;
  public Transform cameraTarget;
  public float walkingSpeed = 2.5f;
  public float sprintingSpeed = 5.4f;
  public float gravity = 9.8f;
  public float accelerate = 5f;
  public float rotationSpeed = 360f;

  private CharacterController _charController;
  private Animator _anim;
  private float _pitch = 0f;
  private float _yaw = 0f;

  private Vector2 _delta = Vector2.zero;
  private Vector2 _dir = Vector2.zero;
  private bool _isSprinting = false;

  private Vector3 _lastForward;
  private float _currentSpeed;

  public void OnMove(InputValue value)
  {
    _dir = value.Get<Vector2>();
  }

  public void OnLook(InputValue value)
  {
    _delta = value.Get<Vector2>() * 0.05f;
  }

  public void OnSprint(InputValue value)
  {
    _isSprinting = value.isPressed;
  }

  private void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    if(cameraTarget == null)
      cameraTarget = transform.Find("CameraTarget");

    _charController = GetComponent<CharacterController>();
    _anim = GetComponent<Animator>();
  }

  private void Update()
  {
    ControlCharacter();
    ControlAnimation();
    ControlCamera();
  }

  private void ControlCamera()
  {
    _pitch -= _delta.y * cameraSensitivity;
    _pitch = Mathf.Clamp(_pitch, -30f, 70f);
    _yaw += _delta.x * cameraSensitivity;
    cameraTarget.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
  }


  private void ControlCharacter()
  {
    if(_dir != Vector2.zero)
    {
      Quaternion camRotation = Quaternion.Euler(0f, cameraTarget.eulerAngles.y, 0f);
      _lastForward = (camRotation * Vector3.forward) * _dir.y
        + (camRotation * Vector3.right) * _dir.x;

      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_lastForward.normalized, Vector3.up), rotationSpeed * Time.deltaTime);
    }    
    
    float targetSpeed = _isSprinting ? sprintingSpeed : walkingSpeed;
    if (_dir == Vector2.zero)
      targetSpeed = 0f;

    if (_currentSpeed > targetSpeed + 0.1f || _currentSpeed < targetSpeed - 0.1f)
      _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * accelerate);
    else
      _currentSpeed = targetSpeed;

      _charController.Move((_currentSpeed * Time.deltaTime * _lastForward) + (Vector3.down * gravity));
  }

  private void ControlAnimation()
  {
    _anim.SetFloat("Speed", _currentSpeed);
  }
}
