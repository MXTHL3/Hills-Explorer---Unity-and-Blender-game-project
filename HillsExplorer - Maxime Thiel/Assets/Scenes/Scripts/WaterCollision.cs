using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [Header("References")]
    public Hearts hearts; 
    public UIManager uiManager; 

    private bool isInWater = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            isInWater = true;
            StartCoroutine(ApplyDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            isInWater = false;
        }
    }

    private System.Collections.IEnumerator ApplyDamage()
    {
        while (isInWater)
        {
            hearts.Hurt(); 
            if (uiManager.deadScreen.activeSelf)
            {
                break;
            }

            yield return new WaitForSeconds(1f); 
        }
    }
}