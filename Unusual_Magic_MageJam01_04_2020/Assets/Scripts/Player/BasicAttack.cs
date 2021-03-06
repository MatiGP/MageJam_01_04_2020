﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, IProjectile
{
    [SerializeField] float cookieTravelSpeed;
    [SerializeField] float cookieLifeTime;
    [SerializeField] AudioSource onHitSource;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("DestroyCookie", cookieLifeTime);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.localScale.x < 0)
        {
            rb2d.velocity = new Vector2(-cookieTravelSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(cookieTravelSpeed, rb2d.velocity.y);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().TakeDamage();
            DestroyProjectile();

        }
        else if(collision.tag == "Colliders")
        {
            DestroyProjectile();
        }
        
    }

    public void DestroyProjectile()
    {
        AudioSource.PlayClipAtPoint(onHitSource.clip, transform.position);
        Destroy(gameObject);
    }
}
