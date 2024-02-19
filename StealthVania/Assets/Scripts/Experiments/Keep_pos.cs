using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Keep_pos : MonoBehaviour
{
    [SerializeField] private Transform Enemy_loc;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Enemy_loc.position.x, Enemy_loc.position.y+.5f);
    }
}
