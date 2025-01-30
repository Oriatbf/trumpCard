using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Creature : MonoBehaviour
{

    [SerializeField] protected Stat stat = new Stat();
    protected SpriteRenderer spr;
    public Character opponent;
    public Health health;
    public PathFind pathfind;
    public PathfindAI pathfindAI;
    protected int currentPathIndex = 0;
    private List<Node> pathToPlayer = new List<Node>();
    public Transform shootPoint;
    private float _lastPathfindingTime;

    public float _angle;
    public Vector2 _dir;

    protected ShootInfor shootInfor;

    private void Awake()
    {
        health = GetComponent<Health>();
        spr = GetComponent<SpriteRenderer>();
        pathfind = GetComponent<PathFind>();
        pathfindAI = GetComponent<PathfindAI>();
        _lastPathfindingTime = Time.time;

    }

    protected virtual void Start()
    {
        shootInfor = new ShootInfor(this, stat);
        pathfindAI.Init(health.characterType,stat.FinalValue().speed);
    }


    protected virtual void Update()
    {
        shootInfor.dir = _dir;
        shootInfor.angle = _angle;
        _dir = (opponent.transform.position - transform.position).normalized;
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.localScale = new Vector3(_dir.x < 0 ? -1 : 1, 1);
    }
    
    


    public virtual void Init(Character character)
    {
        health.characterType = character.unitHealth.characterType;
        opponent = GameManager.Inst.GetOpponent(health.characterType);
        health.ResetHp(stat.FinalValue().hp,stat.FinalValue().hp);
    }
    
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void UpgradeStat(float multiplier)
    {
        stat.originStatValue = stat.originStatValue * multiplier;
    }

}
