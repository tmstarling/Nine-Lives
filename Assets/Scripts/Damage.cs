using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum DamageTypes { PlayerMoving, Moving, Homing, DmgOvrTime }

    [SerializeField] DamageTypes type;
    [SerializeField] Rigidbody rigid;
    [SerializeField] int DamageAmount;
    [SerializeField] int Speed;
    [SerializeField] int DestroyTime;
    [SerializeField] float damageRate;

    bool isDamaging;
    public enemyAI sourceEnemy;

    float aimOffset = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sourceEnemy != null)
            aimOffset = sourceEnemy.aimOffset;

        if (type == DamageTypes.Moving || type == DamageTypes.Homing)
        {
            Destroy(gameObject, DestroyTime);

            if (type == DamageTypes.Moving)
            {
                Vector3 targetPos = gamemanager.instance.player.transform.position + Vector3.up * aimOffset;
                Vector3 direction = (targetPos - transform.position).normalized;

                rigid.linearVelocity = direction * Speed;
                transform.forward = direction;
            }

            if (type == DamageTypes.Homing)
            {
                rigid.linearVelocity = (gamemanager.instance.player.transform.position).normalized * Speed * Time.deltaTime;
            }
        }

        else if (type == DamageTypes.PlayerMoving)
        {
            Destroy(gameObject, DestroyTime);

            if (type == DamageTypes.PlayerMoving)
            {
                Vector3 targetPos = gamemanager.instance.player.transform.position + Vector3.up * aimOffset;
                Vector3 direction = (targetPos - transform.position).normalized;

                rigid.linearVelocity = transform.forward * Speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageTypes.Homing)
        {
            Vector3 targetPos = gamemanager.instance.player.transform.position + Vector3.up * aimOffset;
            Vector3 direction = (targetPos - transform.position).normalized;

            rigid.linearVelocity = direction * Speed;
            transform.forward = direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage damage = other.GetComponent<IDamage>();

        

        if (damage != null && type != DamageTypes.DmgOvrTime)
        {
            damage.TakeDamage(DamageAmount);
        }

        if (type == DamageTypes.Moving || type == DamageTypes.Homing)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == DamageTypes.DmgOvrTime && !isDamaging)
        {
            StartCoroutine(DamageOther(dmg));
        }
    }

    IEnumerator DamageOther(IDamage damage)
    {
        isDamaging = true;
        damage.TakeDamage(DamageAmount);
        yield return new WaitForSeconds(DamageAmount);
        isDamaging = false;
    }
}