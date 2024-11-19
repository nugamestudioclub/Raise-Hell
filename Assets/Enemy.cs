using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    public Rigidbody2D rigidbody;
    public float speed;
    public float health;
    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage) {
        health-=damage;
        if(health<=0) {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }
}
