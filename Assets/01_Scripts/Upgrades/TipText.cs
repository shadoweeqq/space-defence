using TMPro;
using UnityEngine;

public class TipText : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public static TipText Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ChangeText(string text)
    {
        tipText.text = text;
    }

    public void ResetText()
    {
        tipText.text = "";
    }

    public void SetTimedText(string text, float time)
    {
        CancelInvoke();
        tipText.text = text;
        Invoke(nameof(ResetText), time);
    }
}
