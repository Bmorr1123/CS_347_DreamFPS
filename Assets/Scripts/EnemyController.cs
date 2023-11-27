using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Animator animator;

    public bool isRunning = false, isAlive = true;
    bool hasPerformedDeathAnimation = false;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").transform;

    }

    void Update()
    {
        if (isAlive)
        {
            navMeshAgent.destination = playerTransform.position;
        }
        else
        {
            navMeshAgent.destination = transform.position;

        }

        if (!isAlive && this.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            this.hasPerformedDeathAnimation = true;
        }
        if (this.hasPerformedDeathAnimation && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Destroy(gameObject);
        }
        animator.SetBool("IsRunning", isRunning);
    }

    void Die()
    {

        print("I AM DYING!!!");
        isAlive = false;
        animator.SetTrigger("Die");
        this.GetComponent<Rigidbody>().freezeRotation = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            this.Die();
        }
    }
}
