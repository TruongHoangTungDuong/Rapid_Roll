using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private int speed;
    [SerializeField] private bool isSafeBar;
    
    void Start()
    {
        speed = 2;
    }

    void Update()
    {
        Move();
    }

    
    private void Move()
    {
        transform.position = transform.position + new Vector3 (0f, speed * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
            if (isSafeBar == true)
            {
                gameObject.tag = "SafeBar";
            }
            if (isSafeBar == false)
            {
                gameObject.tag = "DangerousBar";
            }
           
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameObject.tag = "Untagged";
        }
    }      
}
