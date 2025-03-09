using UnityEngine;

public class AttackingPlayerState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero; // Stop movement
        enemy.rb.angularVelocity = 0f; // Stop rotation drift
    }

    public void UpdateState(Enemy enemy)
    {
        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

        // If player is out of detection range, switch back to attacking the station
        if (distanceToPlayer > enemy.detectionRange)
        {
            enemy.rb.simulated = true;
            enemy.ChangeState(new MovingToStationState());
            return;
        }

        // Predict player's future position
        Vector2 predictedPosition = PredictPlayerPosition(enemy);

        // Aim firePoint towards predicted position
        Vector2 direction = (predictedPosition - (Vector2)enemy.firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        enemy.firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // Attack the player
        enemy.Attack(enemy.player);

        if (distanceToPlayer > enemy.distanceFromPlayer)
        {
            enemy.MoveTowards(enemy.player.position);
        }
        else
        {
            enemy.rb.linearVelocity = Vector2.zero;
        }
        
    }

    private Vector2 PredictPlayerPosition(Enemy enemy)
    {
        Vector2 playerPos = enemy.player.position;
        Vector2 playerVelocity = enemy.player.GetComponent<Rigidbody2D>().linearVelocity;
        float bulletSpeed = enemy.bulletPrefab.GetComponent<Bullet>().speed;

        float distance = Vector2.Distance(enemy.transform.position, playerPos);
        float timeToImpact = distance / bulletSpeed;

        return playerPos + (playerVelocity * timeToImpact);
    }
}
