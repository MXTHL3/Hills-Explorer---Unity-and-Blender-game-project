using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    [Header("Prefab et Positions")]
    public GameObject coinPrefab;
    public List<Vector3> positions = new List<Vector3>();

    [Header("Rotation")]
    public Vector3 rotationSpeed = new Vector3(90, 0, 0);

    private List<GameObject> coins = new List<GameObject>();
    public int caughtCoins;

    [Header("Nombre de pièces")]
    public int numberOfCoins = 500;

    [Header("Audio")]
    [SerializeField]
    private AudioClip coinSound;

    void Start()
    {
        GenerateRandomPositions();
        DuplicateCoins();
    }

    void Update()
    {
        RotateCoins();
    }

    float GetGroundHeight(float x, float z)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 500, z), Vector3.down, out hit, 1000f))
        {
            return hit.point.y; 
        }
        return -13; 
    }

    void GenerateRandomPositions()
    {
        positions.Clear(); 
        for (int i = 0; i < numberOfCoins; i++)
        {
            int x = Random.Range(-1000, 1001); 
            int z = Random.Range(-1000, 1001);
            float y = GetGroundHeight(x, z);
            positions.Add(new Vector3(x, y, z)); 
        }
    }

    void DuplicateCoins()
    {
        foreach (Vector3 position in positions)
        {
            var newCoin = Instantiate(coinPrefab, position, Quaternion.identity);
            coins.Add(newCoin);

            var coinScript = newCoin.AddComponent<Coin>();
            coinScript.Initialize(this);      

            CoinsController controller = newCoin.GetComponent<CoinsController>();
            if (controller != null)
            {
                Destroy(controller);
            }

            Rigidbody rb = newCoin.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = newCoin.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Collider col = newCoin.GetComponent<Collider>();
            if (col == null)
            {
                col = newCoin.AddComponent<BoxCollider>();
            }
            col.isTrigger = true;     
        }
    }

    void RotateCoins()
    {
        foreach (GameObject coin in coins)
        {
            if (coin != null)
            {
                coin.transform.Rotate(rotationSpeed * Time.deltaTime * 10, Space.World);
            }
        }
    }

    public void CollectCoin(Coin coin)
    {
        caughtCoins++;
    }

    public AudioClip GetCoinSound()
    {
        return coinSound;
    }
}