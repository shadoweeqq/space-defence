using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIArrowPointer : MonoBehaviour
{
    public Transform target; // Object to point at
    private Transform player; // Player transform
    public RectTransform arrowUI; // UI Arrow
    public TextMeshProUGUI arrowText; // Text next to arrow
    public Camera mainCamera;
    public Canvas canvas; // UI Canvas
    public float edgeOffset = 20f; // Distance from screen edge
    public float closeDistance = 5f; // Distance at which arrow moves toward target
    public Color color;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        arrowText.text = target.gameObject.name;
        arrowText.color = color;
        arrowUI.GetComponent<Image>().color = color;
    }

    void Update()
    {
        if (!target || !player) return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        bool isOffScreen = viewportPos.z < 0 || viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;

        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.position);
        float distance = Vector3.Distance(player.position, target.position);

        if (isOffScreen || distance > closeDistance)
        {
            PositionAtScreenEdge(targetScreenPos);
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, targetScreenPos, mainCamera, out Vector2 uiPos);
            arrowUI.anchoredPosition = uiPos;
        }

        arrowText.text = target.gameObject.name + "\n" + (distance*5).ToString("F0") + "m";
        arrowText.rectTransform.anchoredPosition = arrowUI.anchoredPosition + new Vector2(50, 0);
    }

    void PositionAtScreenEdge(Vector3 targetScreenPos)
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 direction = (targetScreenPos - screenCenter).normalized;

        Vector3 edgePos = screenCenter + direction * (Screen.height / 2 - edgeOffset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, edgePos, mainCamera, out Vector2 uiPos);
        arrowUI.anchoredPosition = uiPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, angle);
    }
}
