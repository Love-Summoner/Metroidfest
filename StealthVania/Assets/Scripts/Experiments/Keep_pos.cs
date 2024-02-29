using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Keep_pos : MonoBehaviour
{
    [SerializeField] private Transform enemy_object;
    [SerializeField] private Collider2D cone;
    [SerializeField] private LayerMask layers;
    // Update is called once per frame
    private float base_scale;
    private void Start()
    {
        base_scale = transform.localScale.x;
    }
    private bool sees = false;
    void Update()
    {
        if (cone.IsTouchingLayers(layers))
        {
            Debug.Log("Hello)");
            sees = true;
        }
        else
            sees = false;
        transform.position = new Vector2(enemy_object.position.x, enemy_object.position.y + .5f);
    }
    public bool player_in_range()
    {
        return sees;
    }
}
