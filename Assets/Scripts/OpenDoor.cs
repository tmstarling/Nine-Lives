using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : MonoBehaviour
{
    [Header("Animator")]
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Open");

            Debug.Log("Opening Door");
        }
    }
}
