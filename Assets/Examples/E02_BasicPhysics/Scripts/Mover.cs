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
    _rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    if (canRotate)
    {
      Vector3 newPos = (_direction.y * moveSpeed * Time.fixedDeltaTime * transform.forward) + _rb.position;
      Quaternion newRot = _rb.rotation * Quaternion.Euler(0f, _direction.x * rotateSpeed * Time.fixedDeltaTime, 0f);
      _rb.Move(newPos, newRot);
    }
    else
    {
      _rb.MovePosition(moveSpeed * Time.fixedDeltaTime * new Vector3(_direction.x, 0f, _direction.y) + _rb.position);
    }
  }
}
