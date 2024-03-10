using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockon : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private bool boss_room = false;
    // Update is called once per frame
    void Update()
    {
        if (!boss_room)
            transform.position = new Vector3(player.position.x, 0, -10);
    }
}
