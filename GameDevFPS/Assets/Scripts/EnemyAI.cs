using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.Mathematics;
using Random = UnityEngine.Random;


public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPuaseTime;

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;


    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int gunRotateSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform gunPivot;
    Color colorOrig;

    float shoottimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDistOrig;

    bool PlayerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        //gamemanager.instance.updateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        shoottimer += Time.deltaTime;

        if (agent.remainingDistance < 0.01f)
            roamTimer += Time.deltaTime;

        if (PlayerInTrigger && canSeePlayer())
        {
            checkRoam();
        }
        else if (!PlayerInTrigger)
        {
            checkRoam();
        }
    }

    void checkRoam()
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPuaseTime)
        {
            roam();
        }

    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(transform.position, playerDir);

        agent.SetDestination(gamemanager.instance.player.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (angleToPlayer <= FOV)
            {

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (shoottimer >= shootRate)
                {
                    shoot();
                }

                gunRotate();

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }

        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = false;
        }
    }

    void shoot()
    {
        shoottimer = 0;
        Instantiate(bullet, shootPos.position, gunPivot.rotation);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gamemanager.instance.player.transform.position);

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
        }

    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void gunRotate()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, rot, Time.deltaTime * gunRotateSpeed);
    }
}
