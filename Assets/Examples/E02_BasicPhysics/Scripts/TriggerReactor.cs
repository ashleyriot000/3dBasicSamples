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

  //콜라이더 안으로 들어오는 순간 콜백됨
  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Something entered");
    //레이어를 확인해 트리거 허용된 레이어인지 검사
    if (((1 << other.gameObject.layer) & allowLayer.value) == 0)
      return;
    _material.color = enterColor;
  }

  //콜라이더 밖으로 나가는 순간 콜백됨
  private void OnTriggerExit(Collider other)
  {
    Debug.Log("Something exited");

    //레이어를 확인해 트리거 허용된 레이어인지 검사
    if (((1 << other.gameObject.layer) & allowLayer.value) == 0)
      return;

    _material.color = initColor;    
  }

}
