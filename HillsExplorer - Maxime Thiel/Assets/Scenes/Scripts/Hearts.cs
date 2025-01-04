using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    [Header("Heart States")]
    public List<Sprite> heartStates;
    public int ind = 0;

    public Image heartImage;
    public CameraMovementOrbital cameraMovementOrbital;

    public RectTransform heartRectTransform;

    private void Start()
    {
        heartImage.sprite = heartStates[ind];
    }

    public void Hurt()
    {
        ++ind;
        if (ind >= heartStates.Count)
        {
            cameraMovementOrbital.gameState = "dead";            
        }
        else
        {
            heartRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartRectTransform.rect.width - (130f / 6));
            heartImage.sprite = heartStates[ind];
        }
    }   

    public void Heal()
    {
        if (ind > 0)
        {
            --ind;
            heartRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartRectTransform.rect.width + (130f / 6));
            heartImage.sprite = heartStates[ind];
        }

    }
}
