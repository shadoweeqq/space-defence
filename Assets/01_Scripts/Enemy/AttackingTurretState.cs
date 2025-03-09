using UnityEngine;

public class AttackingTurretState : IEnemyState
{
    private Transform targetTurret;
    bool attackPlayer;

    public void EnterState(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.angularVelocity = 0f;
        targetTurret = enemy.FindClosestTurret();
        attackPlayer = Random.value > 0.75f;
    }

    public void UpdateState(Enemy enemy)
    {
        if (targetTurret == null)
        {
            enemy.ChangeState(new AttackingStationState());
            return;
        }

        if (attackPlayer)
        {
            float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);
            if (distanceToPlayer <= enemy.detectionRange)
            {
                enemy.ChangeState(new AttackingPlayerState());
                return;
            }
        }

        float distanceToTurret = Vector2.Distance(enemy.transform.position, targetTurret.position);
        if (distanceToTurret > enemy.attackRange)
        {
            enemy.ChangeState(new MovingToStationState());
            return;
        }

        // stop movement
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.angularVelocity = 0f;

        // rotate towards turret
        Vector2 direction = (targetTurret.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angle);

        enemy.Attack(targetTurret);
    }
}
