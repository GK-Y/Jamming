using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlatformControl : MonoBehaviour
{
    Vector2 inputDir;
    [SerializeField]
    float speed, dashCoolDown, dashPower, dashDur;
    Rigidbody2D rb;
    float xInp, yInp;
    bool canDash = true, isDashing = false, frozen = true;
    PlayerControl player;
    TrailRenderer trail;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
    void Update()
    {
        if(frozen == false && player.isSwitched == false)
        {
            frozen = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if(frozen == true && player.isSwitched == true)
        {
            frozen = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if(player.isSwitched == true && frozen == false)
        {
            xInp = Input.GetAxisRaw("Horizontal");
            yInp = Input.GetAxisRaw("Vertical");
            inputDir = new Vector2(xInp, yInp).normalized;
            if(isDashing == false)
            rb.velocity = inputDir * speed;
            if(Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        trail.enabled = true;
        if(inputDir.sqrMagnitude != 0)
        rb.AddForce(inputDir * dashPower, ForceMode2D.Impulse);
        else
        rb.AddForce(transform.right * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDur);
        trail.enabled = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
        
    }
}   
 