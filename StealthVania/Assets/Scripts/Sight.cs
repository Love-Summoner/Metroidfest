using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    Ray2D ray, lowerRay;
    RaycastHit2D hit, hit2;
    [SerializeField] private Collider2D cone;

    public LayerMask hittable;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray2D(transform.position, new Vector3(-Mathf.Cos(22.5f), -Mathf.Sin(22.5f), 0));
        lowerRay = new Ray2D(transform.position, new Vector3(-Mathf.Cos(-22.5f), -Mathf.Sin(-22.5f), 0));
        checkforObject();
    }

    private void checkforObject()
    {
        hit = Physics2D.Raycast(ray.origin, ray.direction, 10, hittable);
        hit2 = Physics2D.Raycast(lowerRay.origin, lowerRay.direction, 10, hittable);
        if (hit)
        {
            Debug.Log("It hit" + hit.collider.name);
        }
        else if (hit2)
        {
            Debug.Log("It hit" + hit2.collider.name);
        }
    }
    private void Update()
    {
        ray = new Ray2D(transform.position, ray.direction);
        lowerRay = new Ray2D(transform.position, lowerRay.direction);
        checkforObject();
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        Debug.DrawRay(lowerRay.origin, lowerRay.direction, Color.red);
    }
}
