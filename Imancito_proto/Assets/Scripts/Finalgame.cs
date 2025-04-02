using UnityEngine;
using UnityEngine.SceneManagement;

public class Finalgame : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            SceneManager.LoadScene(1);
            Debug.Log("CharacterController detectado!");
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
