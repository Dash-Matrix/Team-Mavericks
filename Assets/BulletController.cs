using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform ShootingPoint;
    public float bulletspeed = 10f;
    public float firerate = 0.2f;
    public float nextfiretime;
    // Start is called before the first frame update
    void Start()
    {
        nextfiretime = Time.time;
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
        Rigidbody bulletrigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletrigidbody != null)
        {
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
