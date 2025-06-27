using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class BoncyBall : MonoBehaviour
{
    public projectileObj projectileObj;
    Rigidbody2D rb;
    public float timeExisted = 0;
    //bool once = true;
    //public bool parried = false;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        timeExisted += Time.deltaTime;
        // if(parried == true && once == true)
        // {
        //     rb.velocity = rb.velocity*-1;
        //     once = false;
        // }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // parried = false;
        // once = true;
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("U SUCK, CAN'T EVEN DODGE");
            timeExisted = 0;
            this.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Wall") && timeExisted >= projectileObj.lifeTime)
        {
            timeExisted = 0;
            this.gameObject.SetActive(false);
        }
    }
}