using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public float restartDelay = 0.25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController =
                other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.Die();
            }

            StartCoroutine(RestartLevel());
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
