using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour ,IDamage
{
    //Controller
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int HP;

    //Movement
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    //Shooting
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject Furball;
    [SerializeField] GameObject Yarnball;
    [SerializeField] Transform shootPos;

    //References
    Vector3 moveDir;
    Vector3 playerVel;

    int HPOrig;
    int jumpCount;
    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        movement();
        sprint();
    }

    void movement()
    {
        //Debug.Log(moveDir);
        //Debug.Log(controller.gameObject.activeInHierarchy);
        //Debug.Log("Player starting position: " + transform.position);

        //Player Grounded
        if (controller.isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCount = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);
      
        jump();

        //Gravity
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        //Shooting
        shootTimer += Time.deltaTime;
        if (Input.GetButton("Fire1") && shootTimer > shootRate)
        {
            shootBall();
        }

        shootTimer += Time.deltaTime;
        if (Input.GetButton("Fire2") && shootTimer > shootRate)
        {
            shootYarn();
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void shootBall()
    {
        shootTimer = 0;

        //Furball Spawn
        Vector3 offset = Camera.main.transform.forward * 0.3f;
        Instantiate(Furball, shootPos.position + offset, Camera.main.transform.rotation);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist,~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
        }
    }

    void shootYarn()
    {
        shootTimer = 0;

        //Yarnball Spawn
        Vector3 offset = Camera.main.transform.forward * 0.3f;
        Instantiate(Furball, shootPos.position + offset, Camera.main.transform.rotation);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            //Die
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0) 
        {
            gamemanager.instance.youLose();


        }
    }
}