using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject terrainsParent;
    public Transform mousy;
    public AudioClip punch;
    public Hearts hearts;
    public Transform sword;
    public Sword swordScript;
    public CounterController counterController;
    public CameraMovementOrbital cameraMovementOrbital;
    public int numberOfMonsters = 30;
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minZ = -1000f;
    public float maxZ = 1000f;
    public float yOffset = 1f;

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject monster = Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            monster.AddComponent<MonsterBehaviour>();
            MonsterBehaviour monsterBehaviour = monster.GetComponent<MonsterBehaviour>();
            monsterBehaviour.SetParameters(terrainsParent, mousy, punch, hearts, sword, swordScript, 
                counterController, cameraMovementOrbital);

            MonsterSpawner controller = monster.GetComponent<MonsterSpawner>();
            if (controller != null)
            {
                Destroy(controller);
            }          
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        RaycastHit hit;
        Vector3 rayStart = new Vector3(randomX, 1000f, randomZ);
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayStart, rayDirection, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {        
            if (hit.collider.transform.IsChildOf(terrainsParent.transform))
            {
                return new Vector3(randomX, hit.point.y + yOffset, randomZ);
            }
        }

        return new Vector3(randomX, 0f, randomZ);
    }
}

