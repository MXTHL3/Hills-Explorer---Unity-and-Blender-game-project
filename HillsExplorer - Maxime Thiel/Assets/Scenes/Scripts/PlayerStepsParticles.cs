using UnityEngine;

public class PlayerStepsParticles : MonoBehaviour
{
    public Transform mousy;
    public PlayerMovement playerMovement;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emissionModule;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
        emissionModule.enabled = false;
    }

    private void Update()
    {
        transform.position = mousy.position;
        Vector3 rotationOffset = mousy.rotation.eulerAngles;
        rotationOffset.y += 180f;
        transform.rotation = Quaternion.Euler(rotationOffset);

        if (playerMovement.IsMoving)
        {
            if (!emissionModule.enabled)
            {
                emissionModule.enabled = true;
            }
        }
        else
        {
            if (emissionModule.enabled)
            {
                emissionModule.enabled = false;
            }
        }
    }
}

