using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;
    public float coolTime,curCoolTime;
   // [SerializeField] private Image attackCoolImage;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform player;
     
    private float _angle;
    private Vector3 _dir;


    public Animator animator;
    private Camera _camera;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        //회전
        _dir = (player.position - transform.position).normalized;
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);

        //이동
        float moveX = _dir.x * speed * Time.deltaTime;
        float moveY = _dir.y * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);
        
        
    }
}
