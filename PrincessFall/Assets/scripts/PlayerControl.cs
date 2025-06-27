using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{

    [SerializeField]float Maxspeed, acceleration, jumpHeight, coyoteTime, jumpBufferTime, Friction;
    Rigidbody2D rb;
    bool jumping;
    [HideInInspector]public bool isGrounded, isSwitched;
    float xInp, JBcounter, CTcounter, TargetSpeed, SpeedDiff, currentFriction, switchCounter, slowCount;
    RaycastHit2D hit;
    [SerializeField]float switchDelay, slowStart;
    [Header("Ground Check Hit Box")]
    [SerializeField]float groundCheckDistance;
    [SerializeField]Vector2 collisionSize;
    [SerializeField]Transform groundPoint;
    public LayerMask groundLayer;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        //below part for basic needs
        GroundCheck();
        if(isSwitched == false)
        {
            slowCount -= Time.deltaTime;


            //below part for jumpbuffer and jumping etc.
            if(Input.GetKeyDown(KeyCode.Space))
            JBcounter = jumpBufferTime;
            else
            JBcounter -= Time.deltaTime;
            if(JBcounter >= 0 && CTcounter >=0)
            Jump();


            //below part for jump height control(basically allows you to make smaller jump by just leaving space)
            if(jumping == true && Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y/1.5f);
            }

            if(slowCount <= 0)
            xInp = Input.GetAxisRaw("Horizontal");
            else
            xInp = 0;
        }
        //below part for faster falling
        if(CTcounter < 0 && rb.velocity.y < 0.1f)
        rb.gravityScale=0.5f;
        else
        rb.gravityScale = 1.3f;

        //switching logic
        if(switchCounter <= 0)
        {
            slowCount = slowStart;
            isSwitched = true;
        }
        else
        isSwitched = false;
    }
    void FixedUpdate()
    {
        Xvelocity();
    }

    void Xvelocity()
    {
        //below part is for basic movement
        TargetSpeed = Maxspeed * xInp;
        SpeedDiff = TargetSpeed - rb.velocity.x;
        rb.AddForce(Vector2.right * SpeedDiff * acceleration, ForceMode2D.Force);


        //below part is for friction
        if(xInp == 0 && CTcounter >= 0)
        {
            currentFriction = Mathf.Min(Mathf.Abs(rb.velocity.x) , Friction);
            currentFriction *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -currentFriction, ForceMode2D.Impulse);
        }
    }
    
    void GroundCheck()
    {
        if(hit = Physics2D.BoxCast(groundPoint.position,collisionSize,0,-transform.up,groundCheckDistance,groundLayer))
        {
            isGrounded = true;
            jumping = false;
            if(hit.collider.gameObject.CompareTag("platform") && switchCounter <= switchDelay)
            switchCounter = switchDelay;
            else
            switchCounter -= Time.deltaTime;
            CTcounter = coyoteTime;
        }
        else
        {
            isGrounded = false;
            CTcounter -= Time.deltaTime;
            switchCounter -= Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(groundPoint.position.x,groundPoint.position.y-groundCheckDistance,0),collisionSize);
    }

    void Jump()
    {
        if(jumping == false)
        { 
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Sqrt(jumpHeight*2*9.8f));
            jumping = true;
            CTcounter = 0;
        }
        
    }

}

