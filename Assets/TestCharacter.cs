using UnityEngine;

public class TestCharacter : MonoBehaviour
{
  
    public GameObject bulletPrefab; // 탄환 프리팹
    public Transform firePoint;    // 발사 위치
    public int bulletCount = 5;    // 탄환 개수
    public float spreadAngle = 30f; // 퍼지는 각도
    public float bulletSpeed = 10f; // 탄환 속도

    [SerializeField] Vector3 angleVec;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject handle;

    private float _angle;

    



    // Update is called once per frame
    public  void Update()
    {
    
        
        
        Move();
        

    
       

    }

    
  

    private void Move()
    {
       
            float x;
            float y;
            //Move
    
            
                x = Input.GetAxisRaw("Horizontal");
                y = Input.GetAxisRaw("Vertical");
            

            angleVec = new Vector3(x, y, 0).normalized;

            float moveX = angleVec.x * 10 * Time.deltaTime;
            float moveY = angleVec.y * 10 * Time.deltaTime;
            transform.Translate(new Vector3(moveX, moveY, 0), Space.World);

            //Rotation
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 _dir;
     
            
            _dir = (mousePosition - transform.position).normalized;


            _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

            handle.transform.parent.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);

            if (_dir.x < 0) handle.transform.localScale = new Vector3(-1, 1);
            if(_dir.x>0)handle.transform.localScale = new Vector3(1, 1);
            
            if(Input.GetMouseButtonDown(0))FireShotgun();
    }
    
    void FireShotgun()
    {
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;

            // 현재 회전각도 계산
            Quaternion rotation = Quaternion.Euler(0, 0, _angle + currentAngle);

            // 탄환 생성 및 초기화
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = rotation * Vector2.right; // 방향 계산
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
