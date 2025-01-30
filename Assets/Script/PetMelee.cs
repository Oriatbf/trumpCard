using UnityEngine;

public class PetMelee : Creature
{
    public float Gunrange;
    protected override void Start()
    {
        base.Start();

        pathfindAI.AddCoolTime(stat.FinalValue().coolTime,()=>MeleeHit());
    }

    public override void Init(Character character)
    {
        base.Init(character);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (opponent != null)
        {
            base.Update();
            AI();
        }
        
    }

    public void MeleeHit()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _dir, 2f);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Health _health))
            {
                if (_health.characterType != health.characterType)
                {
                    _health.GetDamage(Critical.CriticalChance(stat));
                }
            }
        }

    }

    private void AI()
    {
        Vector3 _distance = (opponent.transform.position - transform.position);
        if (pathfindAI.PathfindInterval() || pathfind.targetPos == Vector2Int.RoundToInt(transform.position))
        {
            if (pathfindAI.IsBlocked(Vector2Int.RoundToInt(transform.position)))
            {
                //뒤에 벽
                pathfindAI.battleState = BattleState.Escape;
            }
            else if (_distance.sqrMagnitude < Gunrange * Gunrange && _distance.sqrMagnitude > (Gunrange - 3) * (Gunrange - 3))
            {
                //중간거리
                pathfindAI.battleState = BattleState.CloseToOpponent;
            }
            else if (_distance.sqrMagnitude < (Gunrange - 3) * (Gunrange - 3))
            { 
                //적과 가까움
                pathfindAI.battleState = BattleState.FarToOpponent;
            }
            else if (_distance.sqrMagnitude > Gunrange * Gunrange)
            {
                //적과 멈
                pathfindAI.battleState = BattleState.CloseToOpponent;
            }
            else
            {
                Debug.Log("아무상태도아님");
            }
        }
    }
    
    public  void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,Gunrange);
        Gizmos.DrawWireSphere(transform.position, Gunrange-3);
    }
}
