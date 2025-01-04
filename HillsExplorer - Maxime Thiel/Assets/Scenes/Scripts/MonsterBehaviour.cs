using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    public GameObject terrains;
    private Rigidbody rb;
    private Collider col;
    private Quaternion initialRotation;
    private Transform mousy;

    [Header("Movement Settings")]
    [SerializeField] private float minSteps = 15f;
    [SerializeField] private float maxSteps = 30f;
    [SerializeField] private float moveSpeed = 10f;

    private int life = 7;

    private bool bodyInMonsterArea = false;

    public Transform sword;
    public Sword swordCs;
    public CameraMovementOrbital cameraMovementOrbital;

    public AudioClip punchSound;
    private AudioSource audioSource;

    private Hearts hearts;

    public CounterController counterController;

    public void SetParameters(GameObject terrainsParent, Transform mousyTransform, AudioClip punch, 
        Hearts heartsRef, Transform swordTransform, Sword swordScript, CounterController cntController,
        CameraMovementOrbital cMO)
    {
        terrains = terrainsParent;
        mousy = mousyTransform;
        punchSound = punch;
        hearts = heartsRef;
        sword = swordTransform;
        swordCs = swordScript;
        counterController = cntController;
        cameraMovementOrbital = cMO;
    }

    private void Start()
    {
        initialRotation = transform.rotation;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MoveMonster());
    }

    private float GetTerrainHeight(Vector3 position)
    {
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(position.x, 1000f, position.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity))
        {
            if (terrains != null && hit.collider.transform.IsChildOf(terrains.transform))
            {
                return hit.point.y;
            }
        }
        return position.y;
    }

    private System.Collections.IEnumerator MoveMonster()
    {
        while (true)
        {
            Vector3 targetPosition;

            if (bodyInMonsterArea && mousy != null)
            {
                StartCoroutine(RotatePattern());

                Vector3 directionToMousy = new Vector3(mousy.position.x - transform.position.x, 0f, mousy.position.z - transform.position.z).normalized;
                transform.rotation = Quaternion.LookRotation(directionToMousy);

                targetPosition = mousy.position;
            }
            else
            {
                float randomRotation = Random.Range(0f, 360f);
                transform.rotation = Quaternion.Euler(0, randomRotation, 0);

                float randomDistance = Random.Range(minSteps, maxSteps);
                targetPosition = transform.position + transform.forward * randomDistance;

                targetPosition.x = Mathf.Clamp(targetPosition.x, -1000f, 1000f);
                targetPosition.z = Mathf.Clamp(targetPosition.z, -1000f, 1000f);
                targetPosition.y = GetTerrainHeight(targetPosition);
            }
        
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (!bodyInMonsterArea)
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private System.Collections.IEnumerator RotatePattern()
    {
        while (bodyInMonsterArea == true)
        {       
            transform.rotation *= Quaternion.Euler(0, 30f, 0);
            yield return new WaitForSeconds(0.01f);
            PlayPunchSound();
            if (Vector3.Distance(transform.position, mousy.position) <= 7f)
            {
                hearts.Hurt();             
            }

            transform.rotation *= Quaternion.Euler(0, -60f, 0);
            yield return new WaitForSeconds(0.01f);

            transform.rotation *= Quaternion.Euler(0, 30f, 0);        
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            if (cameraMovementOrbital.gameState == "inGame")
                bodyInMonsterArea = true;
            else
                bodyInMonsterArea = false;
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            bodyInMonsterArea = false;
        }
    }

    private void Update()
    {
        if (swordCs.obtainedSword == true)
        {
            float distanceToMousy = Vector3.Distance(transform.position, mousy.position);
            Vector3 directionToMousy = (mousy.position - transform.position).normalized;
            float angleToMousy = Vector3.Angle(transform.forward, directionToMousy);
            
            if (distanceToMousy <= 7f && angleToMousy <= 70f && Input.GetKeyDown(KeyCode.F))
            {
                --life;
                Debug.Log(life);
                if (life <= 0)
                {
                    counterController.AddMonster();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void PlayPunchSound()
    {
        audioSource.PlayOneShot(punchSound);
    }
}
