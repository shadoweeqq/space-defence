using System.Collections.Generic;
using UnityEngine;

public class DraggingObjects : MonoBehaviour
{
    public float maxDragWeight = 10f; // Maximum weight the ship can pull
    public float dragForce = 2f; // Force applied to pulled objects
    public float slowdownFactor = 0.05f; // How much each weight unit slows the ship

    private List<DraggedObject> draggedObjects = new List<DraggedObject>(); // Track objects being dragged
    private Rigidbody2D shipRb;

    public KeyCode breakAllLines = KeyCode.F;

    private void Start()
    {
        shipRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to select/deselect objects
        {
            SelectOrDeselectObject();
        }

        if (Input.GetKey(breakAllLines))
        {
            foreach (DraggedObject draggedObject in draggedObjects)
            {
                draggedObject.DestroyLine();
                draggedObjects.Remove(draggedObject);
            }
        }

        UpdateDragging();
    }

    private void SelectOrDeselectObject()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);

        if (hit != null && hit.attachedRigidbody != null)
        {
            Rigidbody2D objRb = hit.attachedRigidbody;

            // Check if already dragging
            DraggedObject existingObject = draggedObjects.Find(o => o.rb == objRb);
            if (existingObject != null)
            {
                // Deselect (remove from list)
                existingObject.DestroyLine();
                draggedObjects.Remove(existingObject);
            }
            else
            {
                // Check if adding this object would exceed the max weight
                float objectWeight = objRb.mass;
                if (GetTotalDragWeight() + objectWeight <= maxDragWeight)
                {
                    // Create new DraggedObject
                    DraggedObject newDraggedObject = new DraggedObject(objRb, transform);
                    draggedObjects.Add(newDraggedObject);
                }
            }
        }
    }

    private void UpdateDragging()
    {
        for (int i = draggedObjects.Count - 1; i >= 0; i--)
        {
            if (draggedObjects[i] == null || draggedObjects[i].rb == null)
            {
                // Remove the object and destroy its line when it's destroyed
                draggedObjects[i]?.DestroyLine();
                draggedObjects.RemoveAt(i);
                continue;
            }

            draggedObjects[i].UpdateDrag(dragForce);
        }
    }

    public float GetTotalDragWeight()
    {
        float totalWeight = 0f;
        foreach (DraggedObject obj in draggedObjects)
        {
            totalWeight += obj.rb.mass;
        }
        return totalWeight;
    }

    public float GetSpeedModifier()
    {
        return Mathf.Clamp(1f - (GetTotalDragWeight() * slowdownFactor), 0.1f, 1f);
    }
}
