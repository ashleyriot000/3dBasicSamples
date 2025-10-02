using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetTranslator : MonoBehaviour
{
  public LayerMask mask;
  public bool canWarp = false;
  public float moveSpeed = 1f;
  public float rotateSpeed = 90f;

  private Vector3 _targetPos;
  private Quaternion _targetRot;

  private void Start()
  {
    _targetPos = transform.position;
  }

  public void OnAttack()
  {
    Debug.Log("Attack");
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    if(Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
    {
      _targetPos = hit.point;
      _targetRot = Quaternion.LookRotation((_targetPos - transform.position).normalized);
      if (!canWarp)
        return;

      transform.rotation = _targetRot;
      transform.position = _targetPos;
      //transform.SetPositionAndRotation(_targetPos, _targetRot);
    }
  }

  private void Update()
  {
    if(canWarp)
      return;

    transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotateSpeed * Time.deltaTime);
  }
}
