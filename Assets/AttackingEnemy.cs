using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    
    private Vector2 moveDirection = Vector2.right;
    private Rigidbody2D rb;
    private GameObject player;
    private bool isShooting = false;
    private Vector2 startPosition;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if(player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            isShooting = false;
        }
    }

    private IEnumerator Shoot()
    {
        while(player != null && Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            if (transform.localScale.x > 0) {
            Vector2 shootDirection = shootingPoint.right;  
            
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            
            bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = shootDirection * bulletSpeed;

            yield return new WaitForSeconds(1f);
        } else {
            yield return null;
        }
        }
    }

    private void MoveToPosition(Vector2 targetPosition)
    {
        StartCoroutine(MoveSmoothly(targetPosition));
    }

    private IEnumerator MoveSmoothly(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            MoveToPosition(startPosition + Vector2.right * moveDistance);
            yield return new WaitForSeconds(waitTime);

            MoveToPosition(startPosition + Vector2.left * moveDistance);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            moveDirection = -moveDirection;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
}