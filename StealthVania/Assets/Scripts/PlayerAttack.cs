using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;
    private float attackTime = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(1).gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attack();
        }
        if (attacking)
        {
            timer += Time.deltaTime;
            if(timer >= attackTime)
            {
                attacking = false;
                timer = 0f;
                attackArea.SetActive(attacking);
            }
        }

    }
    
    private void attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
}
