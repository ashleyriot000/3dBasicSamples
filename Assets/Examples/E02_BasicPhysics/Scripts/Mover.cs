using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
  public bool canRotate = false;
  public float moveSpeed = 1.0f;
  public float rotateSpeed = 90f;

  private Rigidbody _rb;
  private Vector2 _direction = Vector2.zero;

  public void OnMove(InputValue value)
  {
    _direction = value.Get<Vector2>();
  }

  void Start()
  {
    //지속적으로 리지드바디를 사용해야 하기 때문에 미리 캐싱
    _rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    if (canRotate)
    {
      //새로운 위치 계산
      Vector3 newPos = (_direction.y * moveSpeed * Time.fixedDeltaTime * transform.forward) + _rb.position;
      //새로운 방향 계산
      Quaternion newRot = _rb.rotation * Quaternion.Euler(0f, _direction.x * rotateSpeed * Time.fixedDeltaTime, 0f);
      //리지드바디로 새로운 위치와 방향으로 이동시킴
      //_rb.MoveRotation(newRot);
      //_rb.MovePosition(newPos);
      _rb.Move(newPos, newRot);
    }
    else
    {
      //리지드바디로 새로운 위치으로 이동시킴
      _rb.MovePosition(moveSpeed * Time.fixedDeltaTime * new Vector3(_direction.x, 0f, _direction.y) + _rb.position);
    }
  }
}
