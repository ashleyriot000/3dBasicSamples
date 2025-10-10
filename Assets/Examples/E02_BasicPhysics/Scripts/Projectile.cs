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
    _rb.AddForce(launchDirection * launchPower,  ForceMode.Force);
    /*
    ForceMode 세부 설명
    1.ForceMode.Force(지속적, 질량 영향 O)
    작동 방식: 뉴턴의 제2법칙(F= ma)에 따라 작동합니다.AddForce에 전달된 값이 F(힘, 뉴턴)로 간주되어, a = F / m 만큼의 가속도가 물리 프레임 동안 지속적으로 적용됩니다.
    활용: 엔진의 추력처럼 시간에 걸쳐 물체의 속도를 점진적으로 변화시켜야 할 때 사용합니다.질량이 무거울수록 속도가 느리게 변합니다.

    2.ForceMode.Acceleration(지속적, 질량 영향 X)
    작동 방식: AddForce에 전달된 값을 a(가속도)로 간주하여, 질량과 관계없이 모든 Rigidbody에 동일한 속도 변화를 유발합니다.F가 아닌 a를 직접 설정하는 것과 같습니다.
    활용: 캐릭터에게 중력을 구현할 때처럼, 모든 객체가 질량과 무관하게 동일한 가속도를 받아야 할 때 유용합니다.

    3.ForceMode.Impulse(순간적, 질량 영향 O)
    작동 방식: 물리 프레임 단 한 번만 적용되는 순간적인 힘(충격량)을 적용합니다. 이 힘은 운동량 변화(Δp)를 유발하며, Δv = 충격량 / m 만큼 속도를 즉시 변화시킵니다.
    활용: 폭발에 의한 밀침, 점프, 당구공을 칠 때처럼 강력하고 순간적인 속도 변화가 필요하며, 물체의 질량이 결과에 영향을 주어야 할 때 사용합니다.

    4.ForceMode.VelocityChange(순간적, 질량 영향 X)
    작동 방식: Rigidbody의 속도(v)를 질량과 관계없이 지정된 양만큼 단 한 번 변경합니다.AddForce에 전달된 값이 Δv(속도 변화량)로 간주됩니다.
    활용: 질량이 다른 여러 객체의 속도를 동일한 양만큼 즉시 보정하거나 변경해야 할 때 사용합니다.Impulse와 유사하지만 질량을 무시한다는 차이가 있습니다.
    */
  }
}
