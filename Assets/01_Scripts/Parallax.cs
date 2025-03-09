using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform player; // Reference to the player spaceship
    public float parallaxFactor = 0.05f; // Lower value = slower background movement
    public Vector2 starTileSize = new Vector2(20f, 20f); // Size of one tile

    private Vector2 startPosition;

    void Start()
    {
        if (!player)
        {
            Debug.LogError("Parallax: Assign the player transform!");
            return;
        }
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        if (!player) return;

        // Create a parallax effect by slightly offsetting the background based on player's position
        Vector2 offset = (Vector2)player.position * parallaxFactor;
        transform.position = startPosition + offset;

        // Seamless tiling effect
        float xDist = Mathf.Abs(player.position.x - transform.position.x);
        float yDist = Mathf.Abs(player.position.y - transform.position.y);

        if (xDist > starTileSize.x / 2)
        {
            startPosition.x += Mathf.Sign(player.position.x - transform.position.x) * starTileSize.x;
        }
        if (yDist > starTileSize.y / 2)
        {
            startPosition.y += Mathf.Sign(player.position.y - transform.position.y) * starTileSize.y;
        }
    }
}
