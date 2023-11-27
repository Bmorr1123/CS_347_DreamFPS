using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    Animator animator;

    private Transform playerPos;

    public GameObject bulletPrefab;

    public float minShootDelay;
    public float maxShootDelay;

    public float bulletSpeed;

    public Transform bulletSpawn;

    private float nextShootTime;

    public bool isRunning = false, isAlive = true;
    bool hasPerformedDeathAnimation = false;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private GameManager gameManager;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").transform;
        nextShootTime = Time.timeSinceLevelLoad + maxShootDelay;

        playerTransform = GameObject.Find("Player/PlayerBody").transform;

        GameManager.TotalSkeletonsOnMap += 1;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        isRunning = (navMeshAgent.velocity.magnitude > 0);

        if (isAlive)
        {
            navMeshAgent.destination = playerTransform.position;

            if (nextShootTime < Time.timeSinceLevelLoad)
            {
                ShootAtPlayer();

                nextShootTime = Time.timeSinceLevelLoad + Random.Range(minShootDelay, maxShootDelay);
            }
            if (gameManager.gameOver)
            {
                this.Die();
            }
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
        if (isAlive)
        {
            GameManager.TotalSkeletonsOnMap -= 1;
            GameManager.SkeletonsKilled++;
            // print("I AM DYING!!!");
            isAlive = false;
            animator.SetTrigger("Die");
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            this.Die();
        }
    }

    void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.transform.LookAt(playerPos);
        Rigidbody brb = bullet.GetComponent<Rigidbody>();
        brb.velocity = bullet.transform.forward * bulletSpeed;
    }
}
