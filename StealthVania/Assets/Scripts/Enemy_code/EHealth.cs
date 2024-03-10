using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHealth : MonoBehaviour
{
    [SerializeField] private Collider2D Basic_hurt;
    [SerializeField] private Sight sight;
    [SerializeField] private LayerMask attack_layer;
    [SerializeField] private GameObject Player;
    private float invinc_time = .5f;
    private int health = 4;
    private bool invinc = false;
    // Start is called before the first frame update
    // Update is called once per frame
    private IEnumerator coroutine;
    private int multiplier = 1;
    void Update()
    {
        if (Basic_hurt.IsTouchingLayers(attack_layer))
        {
            if (sight.get_sees_player())
            {
                multiplier = 1;
            }
            else
                multiplier = 2;
            if(Player.transform.position.x < transform.position.x && transform.localScale.x > 0)
                multiplier *= 2;
            else if(Player.transform.position.x > transform.position.x && transform.localScale.x < 0)
                multiplier *= 2;

            coroutine = invincible();
            StartCoroutine(coroutine);

            if (!invinc)
            {
                health -= 1*multiplier;
                invinc = true;
            }
        }
        if(health < 1)
            death();
    }
    
    private IEnumerator invincible() {
        yield return new WaitForSeconds(invinc_time);
        invinc = false;
        Debug.Log(health);
    }

    private void death()
    {
        gameObject.SetActive(false);
    }
}
