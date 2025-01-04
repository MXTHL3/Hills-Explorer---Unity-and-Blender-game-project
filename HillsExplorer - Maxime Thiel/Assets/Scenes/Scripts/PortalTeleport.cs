using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [Header("Portals")]
    public Transform portal1; 
    public Transform portal2; 
    public float teleportOffset = 5f; 

    private bool isTeleporting = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body") && !isTeleporting)
        {
            if (gameObject.name == "Portal 1")
            {
                Teleport(other.transform, portal2);            
            }
            else if (gameObject.name == "Portal 2")
            {
                Teleport(other.transform, portal1);
            }
        }
    }

    private void Teleport(Transform body, Transform targetPortal)
    {
        isTeleporting = true;
        Vector3 teleportPosition = targetPortal.position + targetPortal.right * teleportOffset;
        CharacterController controller = body.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            body.position = teleportPosition;
            controller.enabled = true;
        }
        else
        {
            body.position = teleportPosition;
        }
        Invoke(nameof(ResetTeleport), 0.1f);
    }

    private void ResetTeleport()
    {
        isTeleporting = false;
    }
}

