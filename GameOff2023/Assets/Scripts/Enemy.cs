using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private DayManager dayManager;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private Vector2 searchAreaMin;
    [SerializeField] private Vector2 searchAreaMax;
    [SerializeField] private float aggroDistance = 5f;
    [SerializeField] private float loseInterestDistance = 10f;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Vector3 nextPoint;
    private Vector3 targetPosition;
    private bool isFollowingPlayer = false;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        PickNextPatrolPoint();
    }


    void Update()
    {
        CheckPlayerAggro();
        Move();
        FlipSprite();
    }


    private void CheckPlayerAggro()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= aggroDistance)
        {
            isFollowingPlayer = true;
        }
        else if (distanceToPlayer > loseInterestDistance)
        {
            isFollowingPlayer = false;
        }
    }


    private void Move()
    {
        if (isFollowingPlayer)
        {
            // Target position is the player's position
            targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Target position is the next point
            targetPosition = nextPoint;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the next point is reached
            if (Vector3.Distance(transform.position, nextPoint) < 0.1f)
            {
                PickNextPatrolPoint();
            }
        }
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = (targetPosition.x > transform.position.x);
    }


    private void PickNextPatrolPoint()
    {
        float randomX = Random.Range(searchAreaMin.x, searchAreaMax.x);
        float randomY = Random.Range(searchAreaMin.y, searchAreaMax.y);
        nextPoint = new Vector3(randomX, randomY, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            dayManager.EndDay(true);
        }
    }
}