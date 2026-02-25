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

        Vector3 ranPos = Random.insideUnitSphere * spawnDist;
        ranPos += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, spawnDist, 1);

        Instantiate(objectToSpawn, hit.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }
}