using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float damp;

    public Transform target;

    private Vector3 vel = Vector3.zero;
    private bool boss_room = false;

    // Update is called once per frame
    private void Update()
    {
        if (boss_room)
            return;
        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, damp);
    }

    public void freeze(Vector3 loc)
    {
        transform.position = loc;
        boss_room = true;
    }
}
