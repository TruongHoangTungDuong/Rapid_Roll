using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private float speed;
    private int max_health;
    public int cur_health;
    private int score;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        speed = 3;
        max_health = 3;
        cur_health = max_health;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
    }
   
    private void Move()
    {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "DangerousBar")
        {
            cur_health -= 1;
            transform.position = new Vector2(gameManager.newestSafeBar.transform.position.x, 
                                             gameManager.newestSafeBar.transform.position.y +1);
            UIManager.Instance.setHeartText(Convert.ToString(cur_health));
            UIManager.Instance.audioList[3].Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "SafeBar")
        {
            score += 1;
            if (speed < 6)
            {
                speed += 0.03f;
            }
            UIManager.Instance.setScoreText(Convert.ToString(score));
            UIManager.Instance.audioList[0].Play();
        }
        if (col.gameObject.tag == "Food")
        {
            if (cur_health < max_health)
            {
                cur_health += 1;
            }
            else
            {
                cur_health = max_health;
            }
            UIManager.Instance.setHeartText(Convert.ToString(cur_health));
            UIManager.Instance.audioList[1].Play();
        }
    }
}
