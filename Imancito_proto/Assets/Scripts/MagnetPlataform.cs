using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Geometry;
using AideTool.Extensions;
using Code;

public class MagnetPlataform : MonoBehaviour
{
    [Foldout("Variable"),SerializeField] private float m_forceAtractment;
    [SerializeField] private Magnet m_sense;
    [SerializeField] private float m_yOffset;
    [SerializeField] private Vector3 m_triggerExtends;

    private PlayerMovement m_player = null;





    private void FixedUpdate()
    {
        Box trigger = new(transform.position +m_yOffset*Vector3.up, m_triggerExtends);
        Aide.DrawBox(trigger);

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


}
