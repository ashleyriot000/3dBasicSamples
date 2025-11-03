using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
  [Header("Cinemachine")]
  public float cameraSensitivity = 1f;
  public Transform cameraTarget;

  [Header("Movement")]
  public float walkingSpeed = 2.5f;
  public float sprintingSpeed = 5.4f;
  public float gravity = 9.8f;
  public float accelerate = 5f;
  public float rotationSpeed = 360f;
  public float jumpHeight = 2f;
  
  [Header("Ground Check")]
  public bool _isGround = true;
  public float groundedRadius = 0.3f;
  public float groundedOffset = 0.2f;
  public LayerMask groundLayer;

  private CharacterController _charController;
  private Animator _anim;
  private float _pitch = 0f;
  private float _yaw = 0f;
  private Vector2 _delta = Vector2.zero;
  private Vector2 _dir = Vector2.zero;
  private bool _isSprinting = false;
  private Vector3 _lastForward;
  private float _currentSpeed; 
  private float _verticalVelocity = 0f;

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

  public void OnJump()
  {
    if (!_isGround)
      return;
    /*
    물리학을 기반으로 특정 높이(JumpHeight)에 도달하는 데 필요한 초기 수직 속도(verticalVelocity)를
    계산하는 공식. 이 공식은 운동 에너지(Kinetic Energy)와 위치 에너지의 전환 원리, 즉 에너지 보존 
    법칙을 기반으로 합니다.
    📐 사용된 공식 및 원리 설명코드에 사용된 공식은 다음과 같습니다.

    v_f^2 = v_i^2 + 2ad

    유니티의 중력 값은 보통 아래 방향을 나타내기 위해 음수(-9.81)로 설정됩니다. 
    따라서 Sqrt(-2 x Gravity x JumpHeight) 공식에서, 
    Gravity가 음수이기 때문에 -2를 곱해주면 전체 값이 양수가 됩니다. 
    Mathf.Sqrt는 음수의 제곱근을 계산할 수 없기 때문에 이 부호 변경은 필수적입니다.
    */

    //점프로 원하는 높이까지 도달하는 데 필요한 초기 수직 속도값을 구하기
    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    _anim.SetTrigger("Jump");
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
    GroundCheck();
    ControlCharacter();
    ControlAnimation();
    ControlCamera();
  }

  private void ControlCharacter()
  {
    if (_dir != Vector2.zero)
    {
      Quaternion camRotation = Quaternion.Euler(0f, cameraTarget.eulerAngles.y, 0f);
      _lastForward = (camRotation * Vector3.forward) * _dir.y
        + (camRotation * Vector3.right) * _dir.x;

      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_lastForward.normalized, Vector3.up), rotationSpeed * Time.deltaTime);
    }


    if (!_isGround)
      _verticalVelocity += gravity * Time.deltaTime;

    float targetSpeed = _isSprinting ? sprintingSpeed : walkingSpeed;
    if (_dir == Vector2.zero)
      targetSpeed = 0f;

    if (_currentSpeed > targetSpeed + 0.1f || _currentSpeed < targetSpeed - 0.1f)
      _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * accelerate);
    else
      _currentSpeed = targetSpeed;


    _charController.Move((_currentSpeed * Time.deltaTime * _lastForward) + (_verticalVelocity * Time.deltaTime * Vector3.up));
  }

  private void ControlAnimation()
  {
    _anim.SetFloat("Speed", _currentSpeed / sprintingSpeed);
  }

  private void GroundCheck()
  {
    Vector3 gPos = transform.position;
    gPos.y += groundedOffset;
    _isGround = Physics.CheckSphere(gPos, groundedRadius, groundLayer, QueryTriggerInteraction.Ignore);
    _anim.SetBool("IsGround", _isGround);
  }

  private void ControlCamera()
  {
    _pitch -= _delta.y * cameraSensitivity;
    _pitch = Mathf.Clamp(_pitch, -30f, 70f);
    _yaw += _delta.x * cameraSensitivity;
    cameraTarget.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
  }  

  //하이어라키에서 게임오브젝트가 선택되어 있는 동안 호출.
  private void OnDrawGizmosSelected()
  {
    Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
    Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

    if (_isGround) Gizmos.color = transparentGreen;
    else Gizmos.color = transparentRed;

    Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z),
    groundedRadius);
  }
}
