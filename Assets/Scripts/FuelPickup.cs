using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
