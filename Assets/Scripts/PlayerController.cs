using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour ,IDamage,IPickup
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
    [SerializeField] Animator anim;

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


    //Variables
    int pickUpsCount = 0;
    int HPOrig;
    int jumpCount;
    float shootTimer;

    //Cheats
    public bool godMode;
    public bool invulnerability;
    public bool wallHack;
    public int speedOrig;
    public int speedBoost;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        UpdateHealthBarFill();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", controller.velocity.magnitude);

        if (!gamemanager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
            movement();
            sprint();
        }
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
    }

    void shootYarn()
    {
        shootTimer = 0;

        //Yarnball Spawn
        Vector3 offset = Camera.main.transform.forward * 0.3f;
        Instantiate(Furball, shootPos.position + offset, Camera.main.transform.rotation);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        UpdateHealthBarFill();
        StartCoroutine(damageFlashScreen());

        if (HP <= 0) 
        {
            gamemanager.instance.youLose();
        }

        if (invulnerability == true)
        {
            HP += amount; 
        }
    }

    public void UpdateHealthBarFill()
    {
        gamemanager.instance.playeHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator damageFlashScreen()
    {
        gamemanager.instance.playerDamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamagePanel.SetActive(false);
    }

    public void OnPickup(pickUpStats stats)
    {
        HP += stats.bonusHealth;
        speed *= stats.speedBoost;
        shootDamage *= stats.damageBoost;
        pickUpsCount++;

        stats.pickUpsCount++;
    }

    public bool CanBePickedUp(GameObject player)
    {
        return pickUpsCount < 3;
    }

    //===CHEAT MODE===//
    public void EnableGodMode()
    {
        godMode = true;
    }

    public void EnableSpeedBoost()
    {
        speedOrig = speed;
        speedBoost = speedOrig * 5;
        speed = speedBoost;
    }
    public void EnableWallHack()
    {
        wallHack = true;
        Collider[] colliders = GetComponents<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }

    public void EnableInvulnerability()
    {
        invulnerability = true;
    }
}