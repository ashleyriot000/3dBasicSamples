using System;
using UnityEngine;

public class TriggerReactor : MonoBehaviour
{
  public LayerMask allowLayer;
  public Color initColor;
  public Color enterColor;

  private Material _material;


  private void Start()
  {
    _material = GetComponent<MeshRenderer>().material;
    _material.color = initColor;
  }

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Something entered");
    if (((1 << other.gameObject.layer) & allowLayer.value) == 0)
      return;
    _material.color = enterColor;
  }

  private void OnTriggerExit(Collider other)
  {
    Debug.Log("Something exited");
    if (((1 << other.gameObject.layer) & allowLayer.value) == 0)
      return;

    _material.color = initColor;    
  }

}
