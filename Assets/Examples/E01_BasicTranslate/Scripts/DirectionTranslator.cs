using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionTranslator : MonoBehaviour
{
  public bool canRotate = false;
  public float movingSpeed = 1f;
  public float rotateSpeed = 60f;

  private Vector2 _movingDirection;


  public void OnMove(InputValue value)
  {
    _movingDirection = value.Get<Vector2>();
  }

  private void Update()
  {
    if (canRotate)
    {
      transform.Rotate(_movingDirection.x * rotateSpeed * Time.deltaTime * Vector3.up);
      transform.Translate(_movingDirection.y * movingSpeed * Time.deltaTime * Vector3.forward);
    }
    else
    {
      //transform.position = movingSpeed * Time.deltaTime * new Vector3(_movingDirection.x, 0f, _movingDirection.y) + transform.position;
      transform.Translate(movingSpeed * Time.deltaTime * new Vector3(_movingDirection.x, 0f, _movingDirection.y), Space.World);
    }
  }
}
