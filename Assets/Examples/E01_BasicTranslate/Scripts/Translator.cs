using UnityEngine;
using UnityEngine.InputSystem;

public class Translator : MonoBehaviour
{
  private enum TranslateType
  {
    Directional,
    Destination
  }

  [Header("Basic Options"), SerializeField] 
  private TranslateType _type;
  public float moveSpeed = 1.0f;
  public float rotateSpeed = 90f;

  [Space(10f), Header("Directional Options")]
  public bool canRotate = false;

  [Space(10f), Header("Destination Options")]
  public bool useHold = false;
  public LayerMask mask;



  private bool _isPressed = false;
  private Vector3 _destination = Vector2.zero;
  private Quaternion _toward = Quaternion.identity;


  public void OnPick(InputValue value)
  {
    if (_type != TranslateType.Destination)
      return;

    if (_isPressed = value.Get<float>() > 0f)
    {
      FindGround();
    }
  }

  public void OnMove(InputValue value)
  {
    _destination = value.Get<Vector2>();
  }


  private void Update()
  {
    switch (_type)
    {
      case TranslateType.Directional:
        TranslateViaDirectional();
        break;
      case TranslateType.Destination:
        TranslateViaDestination();
        break;
      default:
        break;      
    }
  }

  private void TranslateViaDirectional()
  {
    if (canRotate)
    {
      transform.Rotate(_destination.x * rotateSpeed * Time.deltaTime * Vector3.up);
      transform.Translate(_destination.y * moveSpeed * Time.deltaTime * Vector3.forward);
    }
    else
    {
      //transform.position = movingSpeed * Time.deltaTime * new Vector3(_movingDirection.x, 0f, _movingDirection.y) + transform.position;
      transform.Translate(moveSpeed * Time.deltaTime * new Vector3(_destination.x, 0f, _destination.y), Space.World);
    }
  }

  private void FindGround()
  {
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
    {
      _destination = hit.point;
      Vector3 toward = _destination - transform.position;      
      if (Vector3.SqrMagnitude(toward) > Vector3.kEpsilonNormalSqrt)
        _toward = Quaternion.LookRotation(toward.normalized, Vector3.up);
    }
  }

  private void TranslateViaDestination()
  {
    if(useHold && _isPressed)
      FindGround();

    Vector3 position = Vector3.MoveTowards(transform.position, _destination, moveSpeed * Time.deltaTime);
    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, _toward, rotateSpeed * Time.deltaTime);
    transform.SetPositionAndRotation(position, rotation);
  }
}
