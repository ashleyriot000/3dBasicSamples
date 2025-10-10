using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidController : MonoBehaviour
{
  public float walkingSpeed = 2.5f;
  public float runningSpeed = 5.0f;
  private Animator _anim;

  [SerializeField] private bool _isWalking;
  [SerializeField] private bool _isSprinting;

  void Start()
  {
    //지속적으로 애니메이터를 사용하기 위해 캐싱
    _anim = GetComponent<Animator>();
  }

  private void SetAnimSpeed()
  {
    if(!_isWalking)
      //애니메이터 컨트롤러안의 Speed 파라미터의 값을 업데이트
      _anim.SetFloat("Speed", 0f);
    else
    {
      if (_isSprinting)
        _anim.SetFloat("Speed", runningSpeed);
      else
        _anim.SetFloat("Speed", walkingSpeed);
    }
  }

  public void OnMove(InputValue value)
  {
    _isWalking = value.Get<Vector2>().magnitude > 0.1f;    
    SetAnimSpeed();
  }

  public void OnSprint(InputValue value)
  {
    _isSprinting = value.Get<float>() > 0f;
    SetAnimSpeed();
  }  
}
