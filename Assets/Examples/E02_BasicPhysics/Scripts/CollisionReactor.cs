using UnityEngine;

public class CollisionReactor : MonoBehaviour
{
  private Material _material;

  public Color targetColor = Color.white;  
  private int _hitCount;

  void Start()
  {
    //매터리얼의 속성을 지속적으로 변경하기 위해 캐싱
    _material = GetComponent<MeshRenderer>().material;
    _hitCount = 1;
    _material.SetColor("_EmissionColor", (targetColor / 20f) * _hitCount);
  }

  //충돌하는 순간 콜백됨.
  private void OnCollisionEnter(Collision collision)
  {
    Color current = (targetColor / 20f) * ++_hitCount;
    _material.SetColor("_EmissionColor", current);
  }
}
