using UnityEngine;

public class AChest : MonoBehaviour
{
    private KeysAndChestsCloning keysAndChestsCloning;
    private int attributedNumber;
    public GameObject chestCover;
    public GameObject chestMarker;
    private Hearts hearts;
    private Quaternion openRotation;
    private Quaternion closeRotation;
    private bool isAnimating = false;
    private bool isPlayerInArea = false;
    private bool healed = false;

    public void Initialize(KeysAndChestsCloning controller, GameObject cCover, int aN, GameObject cM, Hearts h)
    {
        keysAndChestsCloning = controller;
        chestCover = cCover;
        attributedNumber = aN;
        chestMarker = cM;
        hearts = h;

        chestMarker.SetActive(false);
    }

    private void Start()
    {
        closeRotation = chestCover.transform.localRotation;
        openRotation = Quaternion.Euler(closeRotation.eulerAngles.x - 90f, closeRotation.eulerAngles.y, closeRotation.eulerAngles.z);
    }

    private void Update()
    {
        if (keysAndChestsCloning.obtainedKeys.Contains(attributedNumber))
        {
            chestMarker.SetActive(true);        
        }

        if (isPlayerInArea && keysAndChestsCloning.obtainedKeys.Exists(key => key == attributedNumber))
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                OpenChest();
                if (healed == false && hearts.ind > 1)
                {
                    hearts.Heal();
                    hearts.Heal();
                    healed = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                CloseChest();
            }
        }
    }


    public void OpenChest()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateChest(openRotation));
        }
    }

    public void CloseChest()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateChest(closeRotation));
        }
    }

    private System.Collections.IEnumerator AnimateChest(Quaternion targetRotation)
    {
        isAnimating = true;

        Quaternion startRotation = chestCover.transform.localRotation;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            chestCover.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        chestCover.transform.localRotation = targetRotation;
        isAnimating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            isPlayerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            isPlayerInArea = false;
        }
    }
}

