using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool obtainedSword = false;
    public Transform mousyTransform;
    public float swingSpeed = 200f;

    public Vector3 positionOffset = new Vector3(1.4f, 1.66f, 0.75f);
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

    private Vector3 originalRotation;
    private bool isSwinging = false;

    public AudioClip swingSound; 
    private AudioSource audioSource;

    private void Start()
    {
        originalRotation = transform.localEulerAngles;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body") && !obtainedSword)
        {
            obtainedSword = true;
            mousyTransform = other.transform;
        }
    }

    private void Update()
    {
        if (obtainedSword)
        {
            transform.position = mousyTransform.position + mousyTransform.TransformDirection(positionOffset);
            transform.rotation = mousyTransform.rotation * Quaternion.Euler(rotationOffset);

            if (Input.GetKeyDown(KeyCode.F) && !isSwinging)
            {
                StartCoroutine(SwingSequence());
            }
        }
    }

    private System.Collections.IEnumerator SwingSequence()
    {
        isSwinging = true;

        Quaternion targetRotation1 = Quaternion.Euler(transform.localEulerAngles + new Vector3(0f, 0f, 70f));
        yield return RotateToTarget(targetRotation1, 0.05f);
        PlaySwingSound();

        Quaternion targetRotation2 = Quaternion.Euler(transform.localEulerAngles + new Vector3(0f, 0f, -140f));
        yield return RotateToTarget(targetRotation2, 0.05f);

        Quaternion targetRotation3 = Quaternion.Euler(transform.localEulerAngles + new Vector3(0f, 0f, 70f));
        yield return RotateToTarget(targetRotation3, 0.05f);

        isSwinging = false;
    }

    private System.Collections.IEnumerator RotateToTarget(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRotation;
    }

    private void PlaySwingSound()
    {
        audioSource.PlayOneShot(swingSound);       
    }
}