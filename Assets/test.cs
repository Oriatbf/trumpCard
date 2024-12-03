using System.Collections;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // 메인 카메라를 연결
    [SerializeField] private Transform gun;    // 총의 Transform

    void Update()
    {
        RotateGunTowardsMouse();
    }

    private void RotateGunTowardsMouse()
    {
        // 1. 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // 2D 게임의 경우 Z 값은 무시

        // 2. 총과 마우스 간 방향 벡터 계산
        Vector3 direction = (mousePosition - transform.position).normalized;

        // 3. 방향 벡터의 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. 총 회전 적용
        gun.rotation = Quaternion.Euler(0, 0, angle);
    }
}
