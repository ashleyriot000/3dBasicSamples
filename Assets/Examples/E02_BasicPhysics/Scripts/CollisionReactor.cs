using UnityEngine;

public class CollisionReactor : MonoBehaviour
{
  private MeshRenderer _renderer;
  private Material _material;

  public Color targetColor = Color.white;  
  private int _hitCount;

  void Start()
  {
    _renderer = GetComponent<MeshRenderer>();
    _material = GetComponent<MeshRenderer>().material;
    _hitCount = 1;
    _material.SetColor("_EmissionColor", (targetColor / 20f) * _hitCount);
  }

  private void OnCollisionEnter(Collision collision)
  {
    Color current = (targetColor / 20f) * ++_hitCount;
    _renderer.material.SetColor("_EmissionColor", current);
    _material.SetColor("_EmissionColor", current);
  }
}
