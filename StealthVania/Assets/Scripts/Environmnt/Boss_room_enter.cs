using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_room_enter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject box;
    [SerializeField] private CameraFollow room;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            box.GetComponent<BoxCollider2D>().excludeLayers = 0;
            box.GetComponent<SpriteRenderer>().enabled = true;
            room.freeze(new Vector3(150.5f, 13.5f, -10));
        }
    }
}
