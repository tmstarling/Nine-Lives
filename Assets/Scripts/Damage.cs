using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum DamageTypes { Moving, Homing, DmgOvrTime }

    [SerializeField] DamageTypes type;
    [SerializeField] Rigidbody rigid;
    [SerializeField] int DamageAmount;
    [SerializeField] int Speed;
    [SerializeField] int DestroyTime;
    [SerializeField] float damageRate;

    bool isDamaging;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == DamageTypes.Moving || type == DamageTypes.Homing)
        {
            Destroy(gameObject, DestroyTime);

            if (type == DamageTypes.Moving)
            {
                rigid.linearVelocity = transform.forward * Speed;
            }

            if (type == DamageTypes.Homing)
            {
                rigid.linearVelocity = (gamemanager.instance.player.transform.position).normalized * Speed * Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageTypes.Homing)
        {

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