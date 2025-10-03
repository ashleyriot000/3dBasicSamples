using UnityEngine;
using UnityEngine.InputSystem;

public class Translator : MonoBehaviour
{
  //이동처리 방식 열거형
  private enum TranslateType
  {
    Directional,
    Destination
  }

  #region Variables
  //Attribute : 변수나 메소드 앞에 []사이에 원하는 속성을 부여할 수 있음
  [Header("Basic Options"), SerializeField] 
  private TranslateType _type;
  public float moveSpeed = 1.0f;
  public float rotateSpeed = 90f;

  [Space(10f), Header("Directional Options")]
  public bool canRotate = false;

  [Space(10f), Header("Destination Options")]
  public bool useHold = false;
  public LayerMask mask;

  private bool _isPressed = false;
  private Vector3 _direction = Vector3.zero;
  private Vector3 _destination = Vector2.zero;
  private Quaternion _toward = Quaternion.identity;
  #endregion

  #region Public Method
  
  //인풋 액션에 할당된 Pick의 콜백 함수
  public void OnPick(InputValue value)
  {
    if (_type != TranslateType.Destination)
      return;

    if (_isPressed = value.Get<float>() > 0f)
    {
      FindDestination();
    }
  }
  //인풋 액션에 할당된 Move의 콜백 함수
  public void OnMove(InputValue value)
  {
    _direction = value.Get<Vector2>();
  }
  #endregion

  #region Private Method
  //키 입력 방향으로 회전과 이동
  private void TranslateViaDirectional()
  {
    //게임오브젝트를 이동시키거나 회전은 Transform 컴포넌트 안의 위치와 회전을 담당하는 변수값을 수정하면 됨.
    //transform.position = moveSpeed * Time.deltaTime * new Vector3(_direction.x, 0f, _direction.y) + transform.position;
    //transform.rotation = transform.rotation * Quaternion.Euler(_direction.x * rotateSpeed * Time.deltaTime * Vector3.up);

    if (canRotate)
    {
      //가장 기본적으로 사용되는 회전 함수. 파라메터에 Vector3의 x,y,z축에 원하는 회전각을 입력.
      transform.Rotate(_direction.x * rotateSpeed * Time.deltaTime * Vector3.up);
      //가장 기본적으로 사용되는 이동 함수. 파라메터에 Vector3의 x,y,z축에 원하는 이동각을 입력.
      //기본적으로 월드축이 아닌 로컬축을 기준으로 이동함.
      transform.Translate(_direction.y * moveSpeed * Time.deltaTime * Vector3.forward);
    }
    else
    {
      //월드축을 기준으로 이동하게 하려면, Space.World 열거형을 추가해주면 됨.
      transform.Translate(moveSpeed * Time.deltaTime * new Vector3(_direction.x, 0f, _direction.y), Space.World);
      //회전함수 또한 마찬가지.
      //transform.Rotate(_direction.x * rotateSpeed * Time.deltaTime * Vector3.up, Space.World);
    }

    _destination = transform.position;
    _toward = transform.rotation;
  }

  //목적지 알아내기
  private void FindDestination()
  {
    //마우스의 위치에서 카메라 방향으로 발사하는 Ray 객체를 생성.
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //레이캐스팅을 통해 검출된 충돌정보를 알아냄
    if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
    {
      //알아낸 충돌 위치를 목적지 변수에 대입
      _destination = hit.point;
      //목적지에서 현재 위치를 빼 목적지까지의 방향과 거리를 알아냄
      Vector3 toward = _destination - transform.position;
      //현재 위치가 목적지와 다르면 목적방향에 대입
      if (Vector3.SqrMagnitude(toward) > Vector3.kEpsilonNormalSqrt)
        _toward = Quaternion.LookRotation(toward.normalized, Vector3.up);
    }
  }

  //목적지를 향해 회전과 이동.
  private void TranslateViaDestination()
  {
    if(useHold && _isPressed)
      FindDestination();

    //지정된 현재 위치에서 목표 위치를 향하는 위치값을 계산. 설정된 최대 거리까지만 계산됨.
    Vector3 position = Vector3.MoveTowards(transform.position, _destination, moveSpeed * Time.deltaTime);
    //지정된 현재 회전에서 목표 회전으로 향하는 회전값을 계산. 설정된 최대 각까지만 계산됨.
    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, _toward, rotateSpeed * Time.deltaTime);

    //위치값과 회전값을 한번에 변경하는 함수.
    transform.SetPositionAndRotation(position, rotation);    
  }
  #endregion

#region Unity Event Method
/*
유니티 이벤트 함수의 호출 메커니즘
유니티 엔진의 C++ 코드가 개발자가 작성한 C# 스크립트를 검사하고, 이름이 일치하는 함수를 발견하면 이를 직접 호출함.

  1. 매직 메소드 (Magic Methods)
  Awake, Start, Update 같은 함수들은 흔히 매직 메소드(Magic Methods) 또는 메시지 함수(Message Functions)라고 부름.

  이 함수들은 MonoBehaviour 클래스 내부에 정의되어 있지 않음 (정확히는 virtual 함수로 선언되어 있지 않습니다.)
  따라서 개발자가 작성한 스크립트에서 이 함수들을 재정의(override)하는 것이 아니라, 단순히 이름과 시그니처가 일치하는 함수를 선언하는 것.

  2. 엔진의 스크립트 검사 및 캐싱 (Reflection & Caching)
  유니티 엔진(C++ 코어)은 게임이 시작되거나 컴포넌트가 로드될 때 다음과 같은 단계를 거쳐 이벤트 함수를 찾아냄.

  리플렉션 사용: 유니티 엔진의 C++ 네이티브 코드는 리플렉션을 사용하여 개발자가 작성한 C# 클래스(예: MyScript)를 검사함
  함수 존재 확인: 엔진은 Awake, Start 등 호출 목록에 있는 함수 이름과 정확히 일치하는 함수가 해당 스크립트에 정의되어 있는지 확인함.
  호출 정보 캐싱: 함수가 존재하면, 엔진은 해당 함수의 **메소드 포인터(함수의 주소)**를 저장해두고 직접 호출함. 이 캐싱 작업은 런타임 성능 저하를 최소화하기 위해 주로 최초 로드 시에 이루어짐.

  3. 네이티브 코드의 직접 호출 (Messaging)
  게임 실행 루프가 돌 때, 유니티 엔진은 C++ 코드 레벨에서 캐시된 함수 포인터를 사용하여 해당 C# 함수를 직접 호출합니다.

  4. 유니티 이벤트 함수들의 호출 순서
  https://docs.unity3d.com/kr/2022.3/Manual/ExecutionOrder.html
*/

private void Awake()
{
  Debug.Log("Translator::Awake()");
}

private void OnEnable()
{
  Debug.Log("Translator::OnEnable()");
}

private void Start()
{
  Debug.Log("Translator::Start()");
  _destination = transform.position;
  _toward = transform.rotation;
}


//활성화 되어 있는 동안 매 프레임에 한번 호출
private void Update()
{
  //Debug.Log("Translator::Update()");
  switch (_type)
  {
    case TranslateType.Directional:
      TranslateViaDirectional();
      break;
    case TranslateType.Destination:
      TranslateViaDestination();
      break;
    default:
      break;      
  }
}

//활성화 되어 있는 동안 매 프레임에 한번 호출. 업데이트 함수보다 늦게 호출됨.
private void LateUpdate()
{
  //Debug.Log("Translator::LateUpdate()");    
}

//활성화 되어 있는 동안 유니티의 물리 업데이트 주기에 맞춰 호출됨. 업데이트보다 먼저 호출됨.
private void FixedUpdate()
{
  //Debug.Log("Translator::FixedUpdate()");
}

//그 외 다양한 이벤트 함수들이 준비되어 있음.
#endregion
}
