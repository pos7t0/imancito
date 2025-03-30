using UnityEngine;

public class CameraThirdPerson : MonoBehaviour
{

    [SerializeField] private Transform m_followTarget;

    [SerializeField] private float m_rotationSpeed=10f;
    [SerializeField] private float m_bottomClamp=-40f;
    [SerializeField] private float m_topClamp=70f;

    private float m_cinemachineTargetPitch;
    private float m_cinemachineTargetYaw;


    private void LateUpdate()
    {
        CameraLogic();
    }

    private void CameraLogic()
    {
        float mouseX = GetMouseInput("Mouse X"); 
        float mouseY = GetMouseInput("Mouse Y");

        m_cinemachineTargetPitch = UpdateRotation(m_cinemachineTargetPitch, mouseY, m_bottomClamp, m_topClamp, true);
        m_cinemachineTargetYaw = UpdateRotation(m_cinemachineTargetPitch, mouseX, m_bottomClamp, m_topClamp, false);

        AppliRotations(m_cinemachineTargetPitch,m_cinemachineTargetYaw);
    }

    private void AppliRotations(float pitch, float yaw)
    {
        m_followTarget.rotation = Quaternion.Euler(pitch,yaw,m_followTarget.eulerAngles.z);
    }


    private float UpdateRotation(float currentRotation,float input, float min, float max, bool isAxis)
    {
        currentRotation += isAxis ? -input : input;
        return Mathf.Clamp(currentRotation,min,max);
    }



    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis)*m_rotationSpeed*Time.deltaTime;
    }


}
