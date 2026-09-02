using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    public Animator animator;
    public float loadDelay = 0.35f;

    private bool hasFinished = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasFinished)
        {
            hasFinished = true;


            PlayerController player =
              other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.DisableMovement();
            }

            if (animator != null)
            {
                animator.SetTrigger("Complete");
            }

            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(loadDelay);

        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex =
            currentSceneIndex + 1;

        if (nextSceneIndex <
            SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
