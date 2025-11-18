using UnityEngine;

public class LowFuelUI : MonoBehaviour
{
    [SerializeField] private Transform container;

    private void Start()
    {
        Hide();
    }

    public void Update()
    {
        float lowFuelAmount = .3f;
        if (Lander.Instance.GetFuelAmountNormalized() < lowFuelAmount)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        if (!container.gameObject.activeSelf)
        {
            container.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (container.gameObject.activeSelf)
        {
            container.gameObject.SetActive(false);
        }
    }
}

