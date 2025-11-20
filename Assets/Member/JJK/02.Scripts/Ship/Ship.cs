using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float disembarkInterval = 0.5f; // 적이 내리는 간격(초)

    private Vector3 landPoint;

    // 새로 저장해둘 값들
    private int enemyCount;
    private bool canFlip;

    public void Initialize(Vector3 position, int count, bool canFlip)
    {
        landPoint = position;
        enemyCount = count;
        this.canFlip = canFlip;

        // 적은 아직 생성하지 않고, 배만 움직이기 시작
        StartCoroutine(MoveToLandPoint());
    }

    private void FlipY(GameObject obj)
    {
        obj.transform.Rotate(0, 0, 180);
    }

    private IEnumerator MoveToLandPoint()
    {
        while (Vector3.Distance(transform.position, landPoint) > 0.1f)
        {
            Vector2 dir = landPoint - transform.position;
            float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, landPoint, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Land();
    }

    private void Land()
    {
        // 도착하면 시간차를 두고 적을 내리는 코루틴 시작
        StartCoroutine(DisembarkRoutine());
    }

    private IEnumerator DisembarkRoutine()
    {
        // 상륙 시작 플래그 (필요에 따라 첫 번째 적이 내려갈 때만 켜도 됨)
        InvasionManager.Instance.isLanding = true;

        for (int i = 0; i < enemyCount; i++)
        {
            // 스폰 포인트가 적 수보다 적을 수도 있으니 모듈로 처리
            Transform point = spawnPoint[i % spawnPoint.Length];

            // 배 위(스폰 포인트 위치)에 적 생성
            GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);

            // 방향 반전 옵션
            if (canFlip)
                FlipY(enemy);

            // NavMeshAgent 활성화
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;

            // 유닛 매니저 등록
            UnitManager.Instance.RegisterEnemy(enemy.transform);

            // 다음 적 내릴 때까지 대기
            yield return new WaitForSeconds(disembarkInterval);
        }

        // 모두 내린 뒤 배 제거
        Destroy(gameObject);
    }
}
