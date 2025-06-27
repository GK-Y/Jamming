using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileShooter : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb, bulletbody;
    public shooterObj shooterObj;
    float timer = 0;
    List<GameObject> pooledInstances;
    public GameObject bullet;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        pooledInstances = new List<GameObject>();
        GameObject temp;
        for(int i=0;i<shooterObj.poolSize;i++)
        {
            temp = Instantiate(bullet);
            pooledInstances.Add(temp);
            temp.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 target = player.transform.position - transform.position;
        if(target.magnitude <= shooterObj.range)
        {
            rb.rotation = Mathf.Atan2(target.y,target.x) * Mathf.Rad2Deg;
            timer += Time.deltaTime;
            if(timer >= shooterObj.shootDelay)
            {
                GameObject bullet = GetPooledInstance();
                timer = 0;
                if(bullet != null)
                {
                    bullet.SetActive(true);
                    bulletbody = bullet.GetComponent<Rigidbody2D>();
                    bullet.transform.position = transform.position;
                    bulletbody.velocity = target.normalized * shooterObj.speed;
                }
            }
        } 
    }
    public GameObject GetPooledInstance()
    {
        for(int i=0;i<shooterObj.poolSize;i++)
        {
            if(pooledInstances[i].activeInHierarchy == false)
            {
                Debug.Log("this also works");
                return pooledInstances[i];
            }
        }
        return null;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shooterObj.range);
    }
}
