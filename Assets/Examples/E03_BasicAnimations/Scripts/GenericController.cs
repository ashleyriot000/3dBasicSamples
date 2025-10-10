using UnityEngine;

public class GenericController : MonoBehaviour
{
  private Animator _anim;

  private void Start()
  {
    //지속적으로 사용하기 위해 캐싱
    _anim = GetComponent<Animator>();
  }
  
  public void OnAttack()
  {
    //애니메이터 컨트롤러 안의 파라메터 업데이트
    _anim.SetTrigger("Grab");
  }
}
