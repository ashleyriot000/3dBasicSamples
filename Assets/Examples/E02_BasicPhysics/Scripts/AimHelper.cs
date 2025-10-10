using UnityEngine;
using UnityEngine.InputSystem;

public class AimHelper : MonoBehaviour
{
  public float sensitivity = 1f;
  
  private float _pitch = 0f;
  private float _yaw = 0f;
  private Vector2 _delta = Vector2.zero;
  
  void Start()
  {
    Vector3 euler = transform.rotation.eulerAngles;
    _pitch = euler.x;
    _yaw = euler.y;

    //마우스 커서 잠금.
    Cursor.lockState = CursorLockMode.Locked;
    //마우스 커서 숨기기
    Cursor.visible = false;
  }

  void Update()
  {
    //매프레임 감지된 마우스 포인터 이동값으로 카메라 회전 방향을 계산.
    _pitch -= _delta.y * sensitivity * Time.deltaTime;
    _pitch = Mathf.Clamp(_pitch, -90f, 90f);
    _yaw += _delta.x * sensitivity * Time.deltaTime;
    transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
  }

  public void OnLook(InputValue inputValue)
  {
    _delta = inputValue.Get<Vector2>();
  }
}
