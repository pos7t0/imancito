using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Geometry;
using AideTool.Extensions;
using Code;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MagnetPlataform : MonoBehaviour
{
    [SerializeField] private Magnet m_sense;
    [SerializeField] private float m_forceAtractment;
    [SerializeField] private bool m_isKiller=false;

    [Foldout("Cubo_1"), SerializeField] private float m_yOffset;
    [SerializeField] private Vector3 m_triggerExtends;

    private bool m_activationKiller = false;
    private PlayerMovement m_player = null;
    private float Min
    {
        get => transform.position.y + (m_yOffset - m_triggerExtends.y);
    }
    private float Max
    {
        get => transform.position.y + (m_yOffset + m_triggerExtends.y);
    }


    private void FixedUpdate()
    {
        Box trigger = new(transform.position +m_yOffset*Vector3.up, m_triggerExtends);
        Aide.DrawBox(trigger);
        //Box trigger2 = new(transform.position + (m_yOffset+m_triggerExtends.y) * Vector3.up, new Vector3(1,1,1)) ;
        //Aide.DrawBox(trigger2);

        

        Collider[] colliders = Physics.OverlapBox(transform.position + m_yOffset * Vector3.up, m_triggerExtends);
        
        HandleCollision(colliders);
        
    }

    private void HandleCollision(Collider[] cols)
    {
        bool exist=false;
        PlayerMovement previousPlayer = m_player; // Guarda la referencia anterior
        float posY = 0f;

        foreach (Collider col in cols)
        {
            
            
            if (col.TryGetComponent(out m_player))
            {
                if (m_player.PowerUpState()==PowerUps.Cobelt)
                {
                    return;
                }
                posY = col.transform.position.y;
                exist = true;
                break;
            }
        }
        
        float percent = Max - Min;
        float force = m_forceAtractment;
        //Debug.Log((1f - (posY - Min) / percent));
        if (exist)
        {
            if (m_sense==Magnet.Down)
            {
                if (!m_player.PlusPolarity())
                    force = MagnetDownMenus(force, percent, posY);
                else
                    force = MagnetDownPlus(force, percent, posY);
            }
            else
            {
                if (!m_player.PlusPolarity())
                    force = MagnetUpMenus(force, percent, posY);
                else
                    force = MagnetUpPlus(force, percent, posY);

                
            }
            

        }


        if (exist)
        {
            float factor = (m_sense==Magnet.Up) ? 1:-1;
            m_player.MagnetState(force * factor);
        }
        else if(previousPlayer != null)
        {
            
            previousPlayer.MagnetQuit();
            m_player = null;
        }


    }


    private float MagnetDownMenus(float force, float percent, float posY)
    {
        if (0.8f < (1f - (posY - Min) / percent))
            force *= 2f;
        if (0.5f < (1f - (posY - Min) / percent))
            force *= 1.2f;
        if (0.1f < (1f - (posY - Min) / percent))
        {
            force *= 0.3f;
        }
        if (0f >= (1f - (posY - Min) / percent))
        {
            force = 0f;
            if (m_isKiller && !m_activationKiller)
            {
                m_activationKiller = true;
                StartCoroutine(RestartGame());
            }
        }
        
        return force;

    }
    
    private float MagnetDownPlus(float force, float percent, float posY)
    {
        force *= 5;
        if (0.9f <= (1f - (posY - Min) / percent))
        {
            force = 0f;
        }


        if (m_player.CheckAnyFLoor() && m_isKiller && !m_activationKiller)
        {
            m_activationKiller = true;
            StartCoroutine(RestartGame());
        }

        return force;

    }
    
    private float MagnetUpMenus(float force, float percent, float posY)
    {
        force *= 5;
        if (1f <= (1f - (posY - Min) / percent))
        {
            force = 0f;
        }

        if (m_player.CheckAnyFLoor() && m_isKiller && !m_activationKiller)
        {
            m_activationKiller = true;
            StartCoroutine(RestartGame());
        }


        return force;
        


    }
    
    private float MagnetUpPlus(float force, float percent, float posY)
    {
        if (0.8f < (1f - (posY - Min) / percent))
            force *= 1.2f;
        if (0.5f < (1f - (posY - Min) / percent))
            force *= 2f;
        if (0.1f <(1f - (posY - Min) / percent))
        {
            force *= 2.2f;
            
        }
        if (0.1f >= (1f - (posY - Min) / percent))
        {
            force = 0f;
            if (m_isKiller && !m_activationKiller)
            {
                m_activationKiller = true;
                StartCoroutine(RestartGame());
            }
        }
        if (m_player != null)
        {
            if (m_player.AppliedY < 0)
            {
                force = m_forceAtractment;
            }

        }
        return force;

    }

    private IEnumerator RestartGame()
    {
        m_player.StopController();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }




}
