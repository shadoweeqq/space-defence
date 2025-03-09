using UnityEngine;

public class AttackingStationState : IEnemyState
{
    bool attackTurrets;
    bool attackPlayer;
    public void EnterState(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.angularVelocity = 0f;
        attackTurrets = Random.value > 0.5f;
        attackPlayer = Random.value > 0.25f;
    }

    public void UpdateState(Enemy enemy)
    {
        // attack player
        if (attackPlayer)
        {
            if (Vector2.Distance(enemy.transform.position, enemy.player.position) <= enemy.detectionRange)
            {
                enemy.ChangeState(new AttackingPlayerState());
                return;
            }
        }
        
        // attack turret
        if (attackTurrets)
        {
            if (Vector2.Distance(enemy.transform.position, enemy.FindClosestTurret().position) <= enemy.detectionRange)
            {
                enemy.ChangeState(new AttackingTurretState());
                return;
            }
        }

        float distanceToStation = Vector2.Distance(enemy.transform.position, enemy.station.position);

        if (distanceToStation > enemy.attackRange)
        {
            enemy.ChangeState(new MovingToStationState());
            return;
        }

        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.angularVelocity = 0f;

        // rotate towards station
        Vector2 direction = (enemy.station.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angle);

        enemy.Attack(enemy.station);
    }
}
