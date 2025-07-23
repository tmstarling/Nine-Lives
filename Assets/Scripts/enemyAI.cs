using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IOpen
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;

    [SerializeField] int HP = 100;
    [SerializeField] int fov = 90;
    [SerializeField] int faceTargetSpeed = 5;
    [SerializeField] int roamDist = 10;
    [SerializeField] int roamPauseTime = 3;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate = 2f;

    Color colorOrg;
    float shootTimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDistOrig;

    bool playerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;

    void Start()
    {
        colorOrg = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

    }

    void Update()
    {
        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        }

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        if (playerInTrigger && !CanSeePlayer())
        {
            RoamCheck();
        }
        else if (!playerInTrigger)
        {
            RoamCheck();
        }
    }

    void RoamCheck()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            Roam();
        }
    }

    void Roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist + startingPos;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(ranPos, out hit, roamDist, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    bool CanSeePlayer()
    {
        if (gamemanager.instance == null || gamemanager.instance.player == null) return false;

        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir.normalized * 20f, Color.red);

        if (Physics.Raycast(headPos.position, playerDir.normalized, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= fov)
            {
                shootTimer += Time.deltaTime;

                if (shootTimer >= shootRate)
                {
                    Shoot();
                }

                agent.SetDestination(gamemanager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    void FaceTarget()
    {
        Vector3 flatDir = new Vector3(playerDir.x, 0f, playerDir.z);
        if (flatDir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(flatDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;

        if (gamemanager.instance != null && gamemanager.instance.player != null)
        {
            agent.SetDestination(gamemanager.instance.player.transform.position);
        }

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        if (model != null)
        {
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            model.material.color = colorOrg;
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        if (bullet != null && shootPos != null)
        {
            Instantiate(bullet, shootPos.position, transform.rotation);
        }
    }

    // IOpen interface
    private bool isOpen;

    public void Open(GameObject opener)
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    public bool CanOpen(GameObject opener)
    {
        return true;
    }

    public bool IsOpen => isOpen;
}
