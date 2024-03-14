using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EHealth : MonoBehaviour
{
    [SerializeField] private Collider2D Basic_hurt;
    [SerializeField] private Sight sight;
    [SerializeField] private LayerMask attack_layer;
    [SerializeField] private GameObject Player;
    private float invinc_time = .5f;
    [SerializeField] private int health = 4;
    [SerializeField] private Sprite dead;
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
    }

    private void death()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<MovementAI>().enabled = false;
        gameObject.GetComponent<Sight>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<EWalk_code>().enabled = false;
        gameObject.GetComponent<EHealth>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = dead;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1f);
        gameObject.GetComponent<LineRenderer>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
    }
}
