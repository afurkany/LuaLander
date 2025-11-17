using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private void Awake()
    {
        Destroy(gameObject, 1.5f);
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }
}
