using UnityEngine;

public class CheckPointManager : MonoBehaviour
{

    public static CheckPointManager Instance;

    [SerializeField]private Vector3 m_posPlayer;



    private void Awake()
    {
        if (CheckPointManager.Instance==null)
        {
            CheckPointManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
    
    public void getPosPlayer(Vector3 pos)
    {
        m_posPlayer = pos;
    }

    public Vector3 LoadCheckpoint()
    {
        return m_posPlayer;
    }
}
