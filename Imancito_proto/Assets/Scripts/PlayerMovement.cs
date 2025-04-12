using UnityEngine;
using AideTool;
using AideTool.ExtendedEditor;
using AideTool.Extensions;
using Code;
using Core.Input;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [Foldout("Componentes"), SerializeField] private CharacterController m_controller;
    [SerializeField] private MeshRenderer m_meshRenderer;
    [SerializeField] private Material[] m_materialsRed;
    [SerializeField] private Material[] m_materialsBlue;
    [SerializeField] private Material m_currentMaterial;
    [SerializeField] private GameObject m_shield;

    [Foldout("Variables"), SerializeField] private float m_speed;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_gravity = -10f;
    [SerializeField] public float m_magnetForce = 0;
    [SerializeField] private float m_lookSensitivy;
    [SerializeField] private float m_weight = 5f;

    [Foldout("Atraer"), SerializeField] private float m_attractorStrength = 5f;
    [SerializeField] private float m_attractorRanged = 5f;
    [SerializeField] private int m_rustyStage = 0;
    private float m_weightObject = 0;

    [SerializeField] private float m_extraTime;
    private bool m_blinking=false;

    [Foldout("Elementos de Muerte?"), SerializeField] private float m_durationDead;
    [SerializeField] private float m_maxDuration;
    [SerializeField] private bool m_isWater=false;


    private bool m_stopGame=false;
    private PlayerState m_playerState;
    private PowerUps m_powerUp;
    private float m_timer=0f;
    private float TimerPowerUp
    {
        get => m_timer;
        set
        {
            m_timer = Mathf.Max(0, value);

            // Luego decidir el estado del power-up
            if (m_timer > 0)
            {
                m_powerUp = PowerUps.Cobelt;
            }
            else if(!m_blinking&& m_powerUp==PowerUps.Cobelt) // Cuando llega a 0
            {
                m_blinking = true;
                StartBlinking(2f, 0.2f);
                
            }

        }
    }

    private float TimerLife
    {
        get => m_durationDead;
        set 
        {
            m_durationDead = value;

            if (m_durationDead>m_maxDuration)
            {
                m_durationDead = m_maxDuration;
                return;
            }

            if (m_durationDead > 8f)
            {
                m_rustyStage = 0;
            }
            else if (m_durationDead > 6f)
            {
                m_rustyStage = 1;
            }
            else if (m_durationDead > 4f)
            {
                m_rustyStage = 2;
            }
            else if (m_durationDead > 2f)
            {
                m_rustyStage = 3;
            }
            else if (m_durationDead <=0f)
            {
                m_rustyStage = 4;
                m_stopGame = true;
            }

            if(m_stopGame && m_durationDead < -2f)
                SceneManager.LoadScene(1);
            
            ChangePolarity(false);


        }
    }
    
    private float m_polarity=1f;

    private Vector3 m_appliedMovement = Vector3.zero;
    public float AppliedX { get => m_appliedMovement.x; set => m_appliedMovement.x = value; }
    public float AppliedZ { get => m_appliedMovement.z; set => m_appliedMovement.z = value; }
    public float AppliedY { get => m_appliedMovement.y; set => m_appliedMovement.y = value; }

    public float m_mouseX;

    private readonly InputAxis2D m_move = new();

    public void OnMovement(InputAction.CallbackContext context) => m_move.SetValues(context);




    // Update is called once per frame
    void Update()
    {
        if (!m_stopGame)
        {
            CheckIsGrounded();
            HandleInput();
            HandleGravity();
            HandleMovement();
        }
        
        HandlePowerUp();
        HandleDurationDead();
        Debug.Log(TimerLife);
        //Debug.Log(m_isWater);

    }
    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        if(m_powerUp==PowerUps.Nothing)
        HandleAttract();
    }

    #region Inputs



    private void HandleInput()
    {
        AppliedX = m_move.X;
        AppliedZ = m_move.Y;
        m_mouseX = Input.GetAxis("Mouse X");


        if (JumpConditions)
            Jump();


        if (Input.GetMouseButtonDown(0))
            ChangePolarity(true);
    }

    #endregion

    #region Saltar
    private void Jump()
    {
        m_playerState=PlayerState.Jump;
        float previousYSpeed = AppliedY;
        float nextYSpeed = AppliedY + m_jumpForce;
        float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
        AppliedY = avgYSpeed;
        
    }
    private bool JumpConditions
    {
        get
        {
            bool[] conditions =
            {
                    Input.GetKeyDown(KeyCode.Space),
                    //m_playerState == PlayerState.Grounded
                    m_controller.isGrounded
                };

            return AideMath.AndCheck(conditions);
        }
    }

    #endregion
    

    private void HandleMovement()
    {

        
        transform.Rotate(0f,m_mouseX*m_lookSensitivy,0f);

        Vector3 currVector = transform.rotation*m_appliedMovement;
        
            
        m_controller.Move(Time.deltaTime*m_speed*currVector);
    }


    #region Gravedad

    private void CheckIsGrounded()
    {
        if(m_playerState!= PlayerState.Magnet)
        {
            if (m_controller.isGrounded)
            {
                m_playerState =PlayerState.Grounded;
                return;
            }
            m_playerState = PlayerState.Fall;
        }
        
    }

    private void HandleGravity()
    {
        if (m_playerState== PlayerState.Grounded)
        {
            AppliedY = -0.5f;
            return;
        }

        

        float factor = 0;

        if (m_playerState == PlayerState.Magnet)
            factor =  m_magnetForce * m_polarity;
        if (m_playerState == PlayerState.Jump)
            factor =m_jumpForce;
        if (m_playerState == PlayerState.Fall)
            factor = m_gravity;

        float previousYSpeed = AppliedY;
        float nextYSpeed = AppliedY + (factor*Time.deltaTime);
        float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
        AppliedY = avgYSpeed;
        //Debug.Log(factor);
        //AppliedY += m_gravity * Time.deltaTime;
    }

    #endregion

    #region Mecanica

    public void MagnetState( float magnetForce)
    {
        m_playerState = PlayerState.Magnet;
        m_magnetForce = magnetForce;
    }
    public void MagnetQuit()
    {
        m_playerState = PlayerState.Fall;
        m_magnetForce = 0f;
    }
    public bool PlusPolarity()
    {
        return (m_polarity > 0);
    }


    private void ChangePolarity(bool change)
    {
        if(change)
        m_polarity = (m_polarity > 0) ? -1 : 1;

        if (m_polarity > 0)
        {
            m_meshRenderer.material = m_materialsRed[m_rustyStage];
            
        
        }
        else
        {
            m_meshRenderer.material = m_materialsBlue[m_rustyStage];
        }
        
        
    }


    private void HandleAttract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,m_attractorRanged);
        float numObject = 0f ;
        foreach(Collider hit in hitColliders)
        {
            if (hit.CompareTag("Metal"))
            {

                numObject++;
                Vector3 forceDir = transform.position - hit.transform.position;
                hit.GetComponent<Rigidbody>().AddForce(forceDir.normalized*m_attractorStrength*m_polarity);
            }
        }
        m_weightObject =numObject;

    }
    public float TotalWeight()
    {
        return m_weight+m_weightObject;
    }
    #endregion

    #region PowerUp

    private void HandlePowerUp()
    {
        if (TimerPowerUp!= 0)
        {
            TimerPowerUp -= Time.deltaTime;
            
        }
            
    }

    public void PowerUpCobelt(float time)
    {
        m_timer = time;
        m_shield.SetActive(true);
    }
    public PowerUps PowerUpState()
    {
        return m_powerUp;
    }

    public void StartBlinking(float blinkDuration, float blinkInterval)
    {
        StartCoroutine(BlinkCoroutine(blinkDuration, blinkInterval));
    }

    private IEnumerator BlinkCoroutine(float blinkDuration, float blinkInterval)
    {
        float timer = 0f;
        bool isBlinking = false;

        while (timer < blinkDuration)
        {
            // Alternar entre materiales
            if (isBlinking)
                //RecoveryMaterial();
                m_shield.SetActive(false);
            else
                m_shield.SetActive(true);

            isBlinking = !isBlinking;

            // Esperar un poco
            yield return new WaitForSeconds(blinkInterval);

            // Avanzar el tiempo
            timer += blinkInterval;
        }

        // Al terminar, dejar el material normal
        m_powerUp = PowerUps.Nothing;
        m_blinking = false;
        //RecoveryMaterial();
        m_shield.SetActive(false);
    }


    #endregion

    #region Sistema de Muerte

    public void IsTouchWater(bool isTouch)
    {
        m_isWater = isTouch;
    }

    private void HandleDurationDead()
    {
        if (m_isWater)
        {
            TimerLife -= Time.deltaTime;
            Debug.Log("HOLAAAAA");
        }
        if (!m_isWater&&TimerLife!=m_maxDuration)
        {
            TimerLife += Time.deltaTime;
        }
    }

    #endregion



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,m_attractorRanged);
    }



}
