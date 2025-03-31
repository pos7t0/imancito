using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Geometry;
using AideTool.Extensions;
using Code;

public class MagnetPlataform : MonoBehaviour
{
    [SerializeField] private Magnet m_sense;
    [SerializeField] private float m_forceAtractment;

    [Foldout("Cubo_1"), SerializeField] private float m_yOffset;
    [SerializeField] private Vector3 m_triggerExtends;
    [Foldout("Cubo_2"),SerializeField] private float m_yOffset2;
    [SerializeField] private Vector3 m_triggerExtends2;

    private PlayerMovement m_player = null;
    private bool m_exist;




    private void FixedUpdate()
    {
        Box trigger = new(transform.position +m_yOffset*Vector3.up, m_triggerExtends);
        Aide.DrawBox(trigger);
        Box trigger2 = new(transform.position + m_yOffset2 * Vector3.up, m_triggerExtends2);
        Aide.DrawBox(trigger2);
        
        Collider[] colliders2 = Physics.OverlapBox(transform.position + m_yOffset2 * Vector3.up, m_triggerExtends2);

        //m_exist =LimitCube(colliders2);
        
        Collider[] colliders = Physics.OverlapBox(transform.position + m_yOffset * Vector3.up, m_triggerExtends);

        HandleCollision(colliders);
        
    }

    private void HandleCollision(Collider[] cols)
    {
        bool exist=false;
        PlayerMovement previousPlayer = m_player; // Guarda la referencia anterior

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out m_player))
            {
                exist = true;
                break;
            }
        }

        Debug.Log(exist);

        if (exist)
        {
            float factor = (m_sense==Magnet.Up) ? 1:-1;
            m_player.MagnetState(m_forceAtractment*factor);
        }
        else if(previousPlayer != null)
        {
            
            previousPlayer.MagnetQuit();
            m_player = null;
        }



    }

    private bool LimitCube(Collider[] cols)
    {
        bool exist = false;
        PlayerMovement previousPlayer = m_player; // Guarda la referencia anterior

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out m_player))
            {
                exist = true;
                break;
            }
        }

        return exist;
    }


}
