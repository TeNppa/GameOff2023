using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private DayManager dayManager;
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float torchSpeed = 1.5f;
    [SerializeField] private Vector2 searchAreaMin;
    [SerializeField] private Vector2 searchAreaMax;
    [SerializeField] private float aggroDistance = 5f;
    [SerializeField] private float loseInterestDistance = 10f;
    [SerializeField] private float torchDestroyTime = 0.5f;
    [SerializeField] private EnemyMoveState moveState;
    private float torchDestroyTimer;
    
    private float moveSpeed;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Vector3 targetPosition;
    private GameObject torchToDestroy;
    private bool reachedDest => Vector3.Distance(transform.position, targetPosition) < 0.1f;


    void Start()
    {
        moveSpeed = baseSpeed;
        player = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        torchDestroyTimer = torchDestroyTime;
        PickNextPatrolPoint();
    }


    void Update()
    {
        Move();
        FlipSprite();
    }


    private void CheckPlayerAggro()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (moveState != EnemyMoveState.ChasingPlayer && distanceToPlayer <= aggroDistance)
        {
            ChangeState(EnemyMoveState.ChasingPlayer);
        }
        else if (moveState == EnemyMoveState.ChasingPlayer)
        {
            if (distanceToPlayer > loseInterestDistance)
            {
                ChangeState(EnemyMoveState.Patrolling);
            }
            else
            {
                targetPosition = player.transform.position;
            }
        }
    }


    private void Move()
    {
        if (moveState == EnemyMoveState.GoingForTorch)
        {
            if (reachedDest)
            {
                torchDestroyTimer -= Time.deltaTime;
                if (torchDestroyTimer <= 0)
                {
                    Destroy(torchToDestroy);
                    torchDestroyTimer = torchDestroyTime;
                    ChangeState(EnemyMoveState.Patrolling);
                    return;
                }
            }
        }
        else
        {
            CheckPlayerAggro();
            if (moveState != EnemyMoveState.ChasingPlayer && reachedDest)
            {
                PickNextPatrolPoint();
            }
        }
        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = (targetPosition.x > transform.position.x);
    }

    private void ChangeState(EnemyMoveState state)
    {
        switch (state)
        {
            case EnemyMoveState.ChasingPlayer:
                targetPosition = player.transform.position;
                moveSpeed = chaseSpeed;
                moveState = EnemyMoveState.ChasingPlayer;
                break;
            case EnemyMoveState.Patrolling:
                PickNextPatrolPoint();
                moveSpeed = baseSpeed;
                moveState = EnemyMoveState.Patrolling;
                break;
            case EnemyMoveState.GoingForTorch:
                moveState = EnemyMoveState.GoingForTorch;
                moveSpeed = torchSpeed;
                break;
        }
    }


    private void PickNextPatrolPoint()
    {
        float randomX = Random.Range(searchAreaMin.x, searchAreaMax.x);
        float randomY = Random.Range(searchAreaMin.y, searchAreaMax.y);
        targetPosition = new Vector3(randomX, randomY, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dayManager.EndDay(true);
        }
        else if (other.gameObject.CompareTag("Torch"))
        {
            targetPosition = other.transform.position;
            torchToDestroy = other.gameObject;
            ChangeState(EnemyMoveState.GoingForTorch);
        }
    }

    private enum EnemyMoveState
    {
        Patrolling,
        GoingForTorch,
        ChasingPlayer
    }
}