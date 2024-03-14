using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    // public Transform roomPos;
    private GameObject player;
    [SerializeField] private BoxCollider2D room;
    [SerializeField] private LayerMask playerLayer;
    

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (room.IsTouchingLayers(playerLayer))
        {
            timer += Time.deltaTime;
            if (timer > .5f)
            {
                timer = 0;
                shoot();
            }

        }


    }
    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
