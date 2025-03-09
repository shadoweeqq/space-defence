using UnityEngine;

public class MovingToStationState : IEnemyState
{
    bool attackTurrets;
    public void EnterState(Enemy enemy)
    {
        attackTurrets = Random.value > 0.5f;
    }

    public void UpdateState(Enemy enemy)
    {
        float distanceToStation = Vector2.Distance(enemy.transform.position, enemy.station.position);

        // attack turret
        //if (attackTurrets)
        //{
        //    if (Vector2.Distance(enemy.transform.position, enemy.FindClosestTurret().position) <= enemy.detectionRange)
        //    {
        //        enemy.ChangeState(new AttackingTurretState());
        //        return;
        //    }
        //}

        // attack player
        if (Vector2.Distance(enemy.transform.position, enemy.player.position) <= enemy.detectionRange)
        {
            //enemy.rb.simulated = true;
            enemy.ChangeState(new AttackingPlayerState());
            return;
        }

        if (distanceToStation <= enemy.stationAttackRange)
        {
            enemy.ChangeState(new AttackingStationState());
        }
        else
        {
            enemy.MoveTowards(enemy.station.position);
        }
    }
}
