using UnityEngine;

public class HelperTrigger : MonoBehaviour
{
    public GameObject helperPlatform;
    public AudioSource helperSound;

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            hasActivated = true;

            helperPlatform.SetActive(true);

            if(helperSound != null)
            {
                helperSound.Play();
            }
        }
    }
}
