using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlatformControl : MonoBehaviour
{
    //Rigidbody2D rb;
    Transform tf;
    bool canMove;
    public float speed;
    [SerializeField] PlayerControl plr;

    void Start()
    {

        //rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && canMove)
        {
            MoveX(-1);
        }

        if (Input.GetKey(KeyCode.D) && canMove)
        {
            MoveX(+1);
        }

        if (Input.GetKey(KeyCode.W) && canMove)
        {
            MoveY(+1);
        }

        if (Input.GetKey(KeyCode.S) && canMove)
        {
            MoveY(-1);
        }

    }

    void MoveX(int dir) //parameter for direction -1 left and +1 is right
    {
        tf.position += new Vector3(dir * Time.deltaTime * speed, 0, 0);
    }

    void MoveY(int dir)
    {
        tf.position += new Vector3(0, dir * Time.deltaTime * speed, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //disabling platform controls when plr on it
        if (other.gameObject.name == "Princess")
        {
            plr.enabled = true;
            this.gameObject.GetComponent<PlatformControl>().enabled = false;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Princess")
        {
            this.gameObject.GetComponent<PlatformControl>().enabled = false;
        }
        
    }
}   
