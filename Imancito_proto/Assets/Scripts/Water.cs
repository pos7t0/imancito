using UnityEngine;

public class Water : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement _))
        {
            other.GetComponent<PlayerMovement>().IsTouchWater(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement _))
        {
            other.GetComponent<PlayerMovement>().IsTouchWater(false);
        }
    }
}
