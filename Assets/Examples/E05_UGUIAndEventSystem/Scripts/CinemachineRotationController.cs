using Unity.Cinemachine;
using UnityEngine;

public class CinemachineRotationController : MonoBehaviour
{
  public float rotateSpeed = 1f;

  private CinemachineOrbitalFollow _follow;

  void Start()
  {
    _follow = GetComponent<CinemachineOrbitalFollow>();
  }

  void Update()
  { 
    _follow.HorizontalAxis.Value = _follow.HorizontalAxis.Value + rotateSpeed * Time.deltaTime;
  }
}
