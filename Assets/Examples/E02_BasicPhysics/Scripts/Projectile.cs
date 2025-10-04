using UnityEngine;

public class Projectile : MonoBehaviour
{
  public float launchPower = 1000f;
  private Rigidbody _rb;

  void Awake()
  {
    _rb = GetComponent<Rigidbody>();
  }

  public void Launch(Vector3 launchDirection)
  {
    //리지드바디에 일회성의 물리적인 힘을 주어 움직이게 만듬. 예)총알 등
    _rb.AddForce(launchDirection * launchPower);
  }
}
