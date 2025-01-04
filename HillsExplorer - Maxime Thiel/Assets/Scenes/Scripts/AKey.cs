using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AKey : MonoBehaviour
{
    private KeysAndChestsCloning keysAndChestsCloning;
    private AudioSource audioSource;

    [Header("Audio")]
    public AudioClip keySound;

    private bool playedSound = false;

    public void Initialize(KeysAndChestsCloning controller)
    {
        keysAndChestsCloning = controller;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        keySound = keysAndChestsCloning.GetKeySound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {                    
            if (playedSound == false)
            {
                audioSource.clip = keySound;
                audioSource.Play();               
                playedSound = true;              
            }
            keysAndChestsCloning.GetKey(this);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            Invoke(nameof(DisableKey), audioSource.clip != null ? audioSource.clip.length : 0f);
        }
    }

    public void DisableKey()
    {
        gameObject.SetActive(false);
    }
}

