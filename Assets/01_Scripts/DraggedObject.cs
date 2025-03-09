using UnityEngine;

public class DraggedObject
{
    public Rigidbody2D rb;
    private Transform shipTransform;
    private GameObject lineObject;
    private LineRenderer lineRenderer;

    private float minDistance = 2f; // Minimum distance from ship
    private float maxDistance = 5f; // Maximum allowed drag distance

    public DraggedObject(Rigidbody2D rb, Transform shipTransform)
    {
        this.rb = rb;
        this.shipTransform = shipTransform;
        CreateLineRenderer();
    }

    public void UpdateDrag(float dragForce)
    {
        if (rb == null) return;

        Vector2 shipPosition = (Vector2)shipTransform.position - (Vector2.up * 1.5f); // Offset behind ship
        Vector2 directionToShip = (shipPosition - rb.position);
        float distance = directionToShip.magnitude;

        //if (distance > maxDistance)
        //{
        //    // If too far, pull the object in faster
        //    rb.linearVelocity = directionToShip.normalized * dragForce;
        //}
        //else if (distance > minDistance)
        //{
        //    // If within allowed range, apply a soft force (like a spring effect)
        //    rb.AddForce(directionToShip.normalized * dragForce * 0.5f);
        //}
        //else
        //{
        //    // If too close, STOP applying force to prevent pushing
        //    rb.linearVelocity *= 0.8f; // Slowly dampen movement so it doesn't jitter
        //}

        if (distance <= maxDistance)
        {
            rb.linearVelocity *= 0.8f;
        }
        else
        {
            rb.linearVelocity = directionToShip.normalized * dragForce;
        }



        UpdateLineRenderer();
    }

    private void CreateLineRenderer()
    {
        lineObject = new GameObject("DragLine");
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.black;
        lineRenderer.positionCount = 2;
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, shipTransform.position);
            lineRenderer.SetPosition(1, rb.position);
        }
    }

    public void DestroyLine()
    {
        if (lineObject != null)
        {
            Object.Destroy(lineObject);
        }
    }
}
