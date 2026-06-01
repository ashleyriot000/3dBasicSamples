using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
  public enum State { Patrol, Chase }

  [Header("상태 및 컴포넌트")]
  [SerializeField] private State currentState = State.Patrol;
  private NavMeshAgent agent;
  [SerializeField] private Transform targetTransform;

  [Header("정찰(Patrol) 설정")]
  [SerializeField] private Transform[] waypoints;
  private int currentWaypointIndex = 0;

  [Header("감지(Detection) 설정")]
  [SerializeField] private float detectionRange = 10f; // 감지 반경
  [Range(0, 360)]
  [SerializeField] private float viewAngle = 120f;     // 전방 시야각 (양옆으로 총 120도)
  [SerializeField] private LayerMask targetLayer;      // 타겟 레이어
  [SerializeField] private LayerMask obstacleLayer;    // 벽, 기둥 등 장애물 레이어

  [Header("눈 위치 설정")]
  [SerializeField] private Transform eyePosition;      // 레이캐스트를 쏠 적의 눈/머리 위치 (없으면 본인 Transform 사용)


  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    if (eyePosition == null) eyePosition = transform;  

    if (waypoints.Length > 0) MoveToNextWaypoint();
  }

  void Update()
  {
    // 시야각 + 장애물 검사를 포함한 플레이어 감지
    bool canSeeTarget = CheckForTargetWithFOV();

    if (canSeeTarget)
    {
      currentState = State.Chase;
    }
    else
    {
      if (currentState == State.Chase)
      {
        currentState = State.Patrol;
        MoveToNextWaypoint();
      }
    }

    switch (currentState)
    {
      case State.Patrol:
        UpdatePatrolBehavior();
        break;
      case State.Chase:
        UpdateChaseBehavior();
        break;
    }
  }

  private void UpdatePatrolBehavior()
  {
    if (waypoints.Length == 0) return;

    if (!agent.pathPending && agent.remainingDistance < 0.5f)
    {
      currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
      MoveToNextWaypoint();
    }
  }

  private void MoveToNextWaypoint()
  {
    if (waypoints.Length == 0) return;
    agent.SetDestination(waypoints[currentWaypointIndex].position);
  }

  private void UpdateChaseBehavior()
  {
    if (targetTransform != null)
    {
      agent.SetDestination(targetTransform.position);
    }
  }

  // ⭐ 핵심: 시야각 및 시야 차단(Raycast) 통합 감지 로직
  private bool CheckForTargetWithFOV()
  {
    // 1단계: 반경 내에 타겟이 있는지 체크
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);

    if (hitColliders.Length > 0)
    {
      Transform target = hitColliders[0].transform;

      // 적의 위치에서 타겟 위치를 바라보는 방향 벡터 (Y축 차이로 인한 오차를 줄이기 위해 평면화)
      Vector3 directionToTarget = (target.position - eyePosition.position).normalized;

      // 2단계: 적의 정면(forward)과 플레이어 방향 사이의 각도 계산
      float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToTarget);

      // 계산된 각도가 시야각의 절반(좌/우 범위) 안에 들어오는지 확인
      if (angleBetweenEnemyAndPlayer < viewAngle / 2f)
      {
        // 플레이어까지의 실제 거리 계산
        float distanceToTarget = Vector3.Distance(eyePosition.position, target.position);

        // 3단계: 장애물 체크 (Raycast)
        // 눈 위치에서 타겟 방향으로 레이저를 쏩니다. 이때 장애물 레이어에 걸리는지 확인합니다.
        if (!Physics.Raycast(eyePosition.position, directionToTarget, distanceToTarget, obstacleLayer))
        {
          // obstacleLayer에 부딪히지 않았다면 시야가 확보된 상태이므로 플레이어 발견!
          targetTransform = hitColliders[0].transform;
          return true;
        }
        else
        {
          targetTransform = null;
        }
      }
    }

    return false;
  }

  // 에디터에서 시야각(FOV)을 부채꼴 모양으로 시각화하여 확인하기 위한 디버깅 코드
  private void OnDrawGizmos()
  {
    // 시야각 부채꼴 라인 그리기
    if (eyePosition == null) eyePosition = transform;

    Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
    Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

    Handles.color = Color.white;
    Handles.DrawWireDisc(eyePosition.position, Vector3.up, detectionRange);
    if(currentState == State.Patrol)
    {
      Handles.color = Color.yellow;
      Gizmos.color = Color.yellow;
    }
    else if(currentState == State.Chase)
    {
      Handles.color = Color.red;
      Gizmos.color = Color.red;
      Handles.DrawLine(eyePosition.position, targetTransform.position);
    }
      
    Handles.DrawWireArc(eyePosition.position, Vector3.up, leftBoundary, viewAngle, detectionRange);
    Gizmos.DrawLine(eyePosition.position, eyePosition.position + leftBoundary * detectionRange);
    Gizmos.DrawLine(eyePosition.position, eyePosition.position + rightBoundary * detectionRange);


  }
}