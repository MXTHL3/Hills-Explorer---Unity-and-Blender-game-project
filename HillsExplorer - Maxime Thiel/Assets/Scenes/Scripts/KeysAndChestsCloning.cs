using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class KeysAndChestsCloning : MonoBehaviour
{
    [Header("Prefab et Terrains")]
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private GameObject chestMarker;
    [SerializeField] private GameObject terrainsParent;

    [Header("Param�tres de Spawn")]
    [SerializeField] private int numberOfKeysAndChests = 30;

    [Header("Audio")]
    [SerializeField]
    private AudioClip keySound;

    private List<GameObject> spawnedKeys = new List<GameObject>();
    private List<GameObject> spawnedChests = new List<GameObject>();
    public List<int> obtainedKeys = new List<int>();

    private int attributedNumber = 0;

    public Hearts hearts;

    void Start()
    {
        SpawnKeysAndChests();
    }

    private void SpawnKeysAndChests()
    {
        ClearExistingKeysAndChests();

        for (int i = 0; i < numberOfKeysAndChests; ++i)
        {
            float randomKeyX = Random.Range(-1000f, 1000f);
            float randomKeyZ = Random.Range(-1000f, 1000f);
            float randomChestX = Random.Range(-1000f, 1000f);
            float randomChestZ = Random.Range(-1000f, 1000f);
            float randomChestYAngle = Random.Range(0f, 360f);

            Vector3 keyPosition = new Vector3(randomKeyX, 100f, randomKeyZ);
            if (RaycastOnTerrains(keyPosition, out RaycastHit hit))
            {
                keyPosition.y = hit.point.y + 4f;
            }
            Vector3 chestPosition = new Vector3(randomChestX, 100f, randomChestZ);
            if (RaycastOnTerrains(chestPosition, out RaycastHit hit2))
            {
                chestPosition.y = hit2.point.y + 3f;
            }

            Quaternion keyRotation = Quaternion.Euler(45, 45, 0);
            Quaternion chestRotation = Quaternion.Euler(0, randomChestYAngle, 0);

            GameObject keyInstance = Instantiate(keyPrefab, keyPosition, keyRotation);
            spawnedKeys.Add(keyInstance);
            GameObject chestInstance = Instantiate(chestPrefab, chestPosition, chestRotation);
            spawnedChests.Add(chestInstance);
            GameObject chestMarkerInstance = Instantiate(chestMarker);
            Vector3 markerPosition = chestPosition;
            markerPosition.y += 10f; 
            chestMarkerInstance.transform.position = markerPosition;
            chestMarkerInstance.SetActive(false);

            Transform chestCoverTransform = chestInstance.transform.Find("chest cover");
            GameObject chestCover = chestCoverTransform.gameObject;

            var keyScript = keyInstance.AddComponent<AKey>();
            keyScript.Initialize(this);
            var chestScript = chestInstance.AddComponent<AChest>();
            chestScript.Initialize(this, chestCover, attributedNumber, chestMarkerInstance, hearts);

            KeysAndChestsCloning keyController = keyInstance.GetComponent<KeysAndChestsCloning>();
            Destroy(keyController);
            KeysAndChestsCloning chestController = chestInstance.GetComponent<KeysAndChestsCloning>();
            Destroy(chestController);
            KeysAndChestsCloning chestMarkerController = chestMarkerInstance.GetComponent<KeysAndChestsCloning>();
            Destroy(chestMarkerController);

            Renderer keyRenderer = keyInstance.GetComponent<Renderer>();
            keyRenderer.material.color = Random.ColorHSV();

            ++attributedNumber;
        }
    }

    private bool RaycastOnTerrains(Vector3 position, out RaycastHit hit)
    {
        foreach (Transform child in terrainsParent.transform)
        {
            if (Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
            {
                return true;
            }
        }
        hit = default;
        return false;
    }

    private void ClearExistingKeysAndChests()
    {
        foreach (GameObject key in spawnedKeys)
        {
            Destroy(key);
        }
        foreach (GameObject chest in spawnedChests)
        {
            Destroy(chest);
        }

        spawnedKeys.Clear();
        spawnedChests.Clear();
    }

    public void GetKey(AKey aKey)
    {
        GameObject keyObject = aKey.gameObject;
        int keyIndex = spawnedKeys.IndexOf(keyObject);
        obtainedKeys.Add(keyIndex);    
    }

    public AudioClip GetKeySound()
    {
        return keySound;
    }
}
