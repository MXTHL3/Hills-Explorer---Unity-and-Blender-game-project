using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Coin : MonoBehaviour
{
    private CoinsController coinsController;
    private AudioSource audioSource;

    [Header("Audio")]
    public AudioClip coinSound;

    private bool playedSound = false;

    public void Initialize(CoinsController controller)
    {
        coinsController = controller;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        coinSound = coinsController.GetCoinSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            if (playedSound == false)
            {
                FindObjectOfType<CounterController>().AddCoin();
                audioSource.clip = coinSound;
                audioSource.Play();
                playedSound = true;
            }       
            coinsController.CollectCoin(this);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            Invoke(nameof(DisableCoin), audioSource.clip != null ? audioSource.clip.length : 0f);        
        }
    }

    private void DisableCoin()
    {
        gameObject.SetActive(false);
    }
}
