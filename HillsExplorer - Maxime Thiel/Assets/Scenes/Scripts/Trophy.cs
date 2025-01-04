using UnityEngine;

public class Trophy : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;

    [Header("Victory Screen")]
    public GameObject victoryScreen;

    [Header("Audio Settings")]
    public AudioClip victorySound; 
    private AudioSource audioSource;

    public CameraMovementOrbital cameraMovementOrbital;

    private bool won = false;   

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; 
    }

    private void Update()
    {
        RotateTrophy();
    }

    private void RotateTrophy()
    {       
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body") && won == false)
        {
            rotationSpeed = 0f;        
            audioSource.clip = victorySound;
            audioSource.Play();
            cameraMovementOrbital.gameState = "victory";
            won = true;
        }
    }
}
