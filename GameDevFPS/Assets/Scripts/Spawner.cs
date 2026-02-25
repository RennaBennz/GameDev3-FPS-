using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int spawnAmount;
    [SerializeField] int spawnRate;
    [SerializeField] int spawnDist;

    int spawnCount;
    float spawnTimer;

    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager.instance.updateGameGoal(spawnAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning)
        {
            spawnTimer += Time.deltaTime;

            if (spawnCount < spawnAmount && spawnTimer > spawnRate)
            {
                //hey! Spawn something!
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    void spawn()
    {
        spawnTimer = 0;
        spawnCount++;

        Vector2 circle = Random.insideUnitCircle * spawnDist;
        Vector3 ranPos = new Vector3(circle.x, 0f, circle.y) + transform.position;

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(ranPos, out hit, spawnDist, NavMesh.AllAreas))
        {
            Debug.LogWarning("Spawn cancelled: No NavMesh near spawner. Is there NavMesh near this spawner?");
            spawnCount--;
            return;
        }

        Instantiate(objectToSpawn, hit.position, Quaternion.Euler(0f, Random.Range(0, 360), 0f));
    }
}