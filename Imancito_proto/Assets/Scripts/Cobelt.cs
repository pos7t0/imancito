using UnityEngine;

public class Cobelt : MonoBehaviour
{
    [SerializeField] private float m_timePowerUp;
    [SerializeField] private float m_timePowe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
        {
            other.GetComponent<PlayerMovement>().PowerUpCobelt(m_timePowerUp);
            //Destroy(gameObject);
        }
    }
}
