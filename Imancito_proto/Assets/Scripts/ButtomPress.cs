using UnityEngine;
using UnityEngine.Events;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Geometry;
using AideTool.Extensions;
using Code;

public class ButtomPress : MonoBehaviour
{

    [Foldout("Cubo"), SerializeField] private float m_yOffset;
    [SerializeField] private Vector3 m_triggerExtends;

    [SerializeField] private UnityEvent m_open;
    [SerializeField] private MeshRenderer m_renderer;
    [SerializeField] private Material m_yellow;
    [SerializeField] private float m_weightRequeriment;
    private bool m_enable=false;

    private void FixedUpdate()
    {
        Box trigger = new(transform.position + m_yOffset * Vector3.up, m_triggerExtends);
        Aide.DrawBox(trigger);


        Collider[] colliders = Physics.OverlapBox(transform.position + m_yOffset * Vector3.up, m_triggerExtends);

        HandleCollision(colliders);

    }

    private void HandleCollision(Collider[] cols)
    {

        bool player=false;
        bool metal = false;
        foreach (Collider col in cols)
        {
            player = col.TryGetComponent(out PlayerMovement playerWeight);
            metal = col.TryGetComponent(out Metal metalWeight);
            if (!m_enable&& player&& m_weightRequeriment<=playerWeight.TotalWeight()|| !m_enable && metal && m_weightRequeriment <=metalWeight.Weight())
            {
                m_renderer.material = m_yellow;
                m_enable=true;
                m_open.Invoke();
                return;
            }
        }

    }




}
