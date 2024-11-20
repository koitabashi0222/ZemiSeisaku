using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void GameOver()
    {
        SceneManager.LoadScene("Over");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("attack"))
        {
            GameOver();
        }
    }


}
