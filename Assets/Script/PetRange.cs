using UnityEngine;

public class PetRange : Creature
{
    public float Gunrange;
    protected override void Start()
    {
        base.Start();
        //총 펫일 때
        pathfindAI.AddCoolTime(stat.FinalValue().coolTime,()=>Attack.Inst.Shoot(shootInfor));
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
                pathfindAI.battleState = BattleState.Random;
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
