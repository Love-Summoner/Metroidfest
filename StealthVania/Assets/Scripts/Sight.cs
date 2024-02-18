using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class Sight : MonoBehaviour
{
    Ray2D ray;
    RaycastHit2D hit;
    [SerializeField] private Collider2D cone;
    private float angle = -11.303f * Mathf.PI/180;
    private float base_angle = -11.303f * Mathf.PI / 180;

    public LayerMask hittable;
    public LayerMask play_layer;
    // Start is called before the first frame update
    void Start()
    {

        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y+.5f), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        UnityEngine.Debug.Log(ray.direction);
    }

    private void checkforObject()
    {
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, hittable);
       if(hit && hit.collider.gameObject.name == "Player")
        {
            UnityEngine.Debug.Log("Can see player");
        }
        angle += .1f;
        if(angle > -base_angle)
        {
            angle = base_angle;
        }
    }
    private void Update()
    {
        ray = new Ray2D(new Vector2(transform.position.x, transform.position.y+.5f), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

        if(player_in_range())
            checkforObject();
        Vector2 test = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        UnityEngine.Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }

    private bool player_in_range()
    {
        if (cone.IsTouchingLayers(play_layer))
        {
            return true;
        }
        return false;
    }
}
