using UnityEngine;
using UnityEngine.InputSystem;

public class AimHelper : MonoBehaviour
{
  //마우스 감도
  public float sensitivity = 1f;
  
  private float _pitch = 0f;
  private float _yaw = 0f;
  private Vector2 _delta = Vector2.zero;
  
  void Start()
  {
    Vector3 euler = transform.rotation.eulerAngles;
    _pitch = euler.x;
    _yaw = euler.y;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  void Update()
  {
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
