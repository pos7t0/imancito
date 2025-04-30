using UnityEngine;
using UnityEngine.SceneManagement;

public class Finalgame : MonoBehaviour
{

    private Vector3 inicio= new Vector3(-8, 2.11999989f, 0.970000029f);
    
    // Update is called once per frame
    void Update()
    {
        
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            SceneManager.LoadScene(2);
            Debug.Log("CharacterController detectado!");
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    
    public void RestartInit()
    {
        CheckPointManager.Instance.getPosPlayer(inicio);
        SceneManager.LoadScene(1);
    }
    
    public void GoLevel(int escena)
    {
        SceneManager.LoadScene(escena);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
