using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); 
    public float transitionSpeed = 10f; 

    private Vector3 originalScale; 
    private Vector3 targetScale;

    public CameraMovementOrbital cameraMovementOrbital;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;     
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.name == "Play") 
        {
            cameraMovementOrbital.gameState = "inGame"; 
        }

        if (gameObject.name == "Exit to menu")
        {
            cameraMovementOrbital.gameState = "menu";
        }

        if (gameObject.name == "Quit")
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
