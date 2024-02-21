using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Keep_pos : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }
}
