using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parry : MonoBehaviour
{
    [SerializeField]
    float startDur, parryDur, parryRadius;
    float timer;
    public bool isParrying = false;
    public BoxCollider2D parryHitBox;
    PlayerControl playerControl;
    projectile parriedInstance;
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isParrying == false)
        {
            StartCoroutine(Parry());
        }
    }
    IEnumerator Parry()
    {
        isParrying = true;
        playerControl.enabled = false;
        yield return new WaitForSeconds(startDur);
        parryHitBox.enabled = true;
        yield return new WaitForSeconds(parryDur);
        parryHitBox.enabled = false;
        playerControl.enabled = true;
        isParrying = false;

    }
}
