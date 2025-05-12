using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Survived");
        }
    }
}
