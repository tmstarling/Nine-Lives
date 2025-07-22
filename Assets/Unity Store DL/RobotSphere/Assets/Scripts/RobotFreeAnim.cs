using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotFreeAnim : MonoBehaviour {

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;

    [SerializeField] int HP;
    [SerializeField] int FOV;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] int roamDis;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animSpeedTrans;


    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    Color colorOrg;

    float shootTimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDisOrig;

    bool playerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;

    Vector3 rot = Vector3.zero;
	float rotSpeed = 40f;

	// Use this for initialization
	void Awake()
	{
        colorOrg = model.material.color;
        startingPos = transform.position;
        stoppingDisOrig = agent.stoppingDistance;

        gameObject.transform.eulerAngles = rot;
	}

	// Update is called once per frame
	void Update()
	{
        setAnimations();

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        if (playerInTrigger && !canSeePlayer())
        {
            roamCheck();
        }
        else if (!playerInTrigger)
        {
            roamCheck();
        }

        gameObject.transform.eulerAngles = rot;
	}

    void setAnimations()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }

    void roamCheck()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDis;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDis, 1);
        agent.SetDestination(hit.position);
    }

    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                shootTimer += Time.deltaTime;

                if (shootTimer >= shootRate)
                {
                    shoot();
                }

                agent.SetDestination(gamemanager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                    faceTarget();

                agent.stoppingDistance = stoppingDisOrig;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
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
        model.material.color = colorOrg;
    }

    void shoot()
    {
        shootTimer = 0;

        anim.SetTrigger("Shoot");

        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    //void CheckKey()
    //{
    //	// Walk
    //	if (Input.GetKey(KeyCode.W))
    //	{
    //		anim.SetBool("Walk_Anim", true);
    //	}
    //	else if (Input.GetKeyUp(KeyCode.W))
    //	{
    //		anim.SetBool("Walk_Anim", false);
    //	}

    //	// Rotate Left
    //	if (Input.GetKey(KeyCode.A))
    //	{
    //		rot[1] -= rotSpeed * Time.fixedDeltaTime;
    //	}

    //	// Rotate Right
    //	if (Input.GetKey(KeyCode.D))
    //	{
    //		rot[1] += rotSpeed * Time.fixedDeltaTime;
    //	}

    //	// Roll
    //	if (Input.GetKeyDown(KeyCode.Space))
    //	{
    //		if (anim.GetBool("Roll_Anim"))
    //		{
    //			anim.SetBool("Roll_Anim", false);
    //		}
    //		else
    //		{
    //			anim.SetBool("Roll_Anim", true);
    //		}
    //	}

    //	// Close
    //	if (Input.GetKeyDown(KeyCode.LeftControl))
    //	{
    //		if (!anim.GetBool("Open_Anim"))
    //		{
    //			anim.SetBool("Open_Anim", true);
    //		}
    //		else
    //		{
    //			anim.SetBool("Open_Anim", false);
    //		}
    //	}
    //}

}
