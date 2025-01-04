using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    public TextMeshProUGUI coinsTextMesh; 
    public TextMeshProUGUI monstersTextMesh; 

    private int coinsCount = 0; 
    private int monstersCount = 0; 

    public void AddCoin()
    {
        coinsCount++;
        coinsTextMesh.text = $"{coinsCount}";
    }

    public void AddMonster()
    {
        monstersCount++;
        monstersTextMesh.text = $"{monstersCount}";
    }
}

