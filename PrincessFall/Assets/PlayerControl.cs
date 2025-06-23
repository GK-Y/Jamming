using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{

    public PlatformControl platform;
    Rigidbody2D rb;
    public float speed;
    public float JumpSpeed;
    bool canJump;

    public float buffer;//delay between platform and plr controls

    public float velLimit;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Move(-1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Move(+1);
        }

        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            Jump();
        }

        if (rb.velocity.x >= velLimit)
        {
            rb.velocity = new Vector2(velLimit, rb.velocity.y);
        }

        if (rb.velocity.x <= -velLimit)
        {
            rb.velocity = new Vector2(-velLimit, rb.velocity.y);
        }

    }

    void Move(int dir) //parameter for direction -1 left and +1 is right
    {
        rb.AddForce(rb.velocity * Time.deltaTime + speed * new Vector2(dir, 0), ForceMode2D.Force);
    }

    void Jump()
    {
        rb.AddForce(rb.velocity * Time.deltaTime + JumpSpeed * new Vector2(0, 1), ForceMode2D.Impulse);
        canJump = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //disabling plr contols when not on platform and enabling platform controls
        if (other.gameObject.name == "Platform")
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Platform")
        {
            gameObject.GetComponent<PlayerControl>().enabled = false;
            StartCoroutine(plaformEnable(buffer, platform)); //to start platform control after a delay
        }
    }

    IEnumerator plaformEnable(float buffer, PlatformControl platform)
    {
        yield return new WaitForSeconds(buffer);
        platform.enabled = true;
    }

}

