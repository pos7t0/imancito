using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
        {
            CheckPointManager.Instance.getPosPlayer(other.transform.position);
            Debug.Log("Holaaaaaqqaqq");
        }
    }
}
