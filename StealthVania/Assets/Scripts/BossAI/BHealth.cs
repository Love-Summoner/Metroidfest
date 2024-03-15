using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Phase
{
    PHASE1, PHASE2, PHASE3
}
public class BHealth : MonoBehaviour
{
    [SerializeField] private Collider2D Basic_hurt;
    [SerializeField] private Sight sight;
    [SerializeField] private LayerMask attack_layer;
    [SerializeField] private GameObject Player;
    private float invinc_time = .5f;
    [SerializeField] private int health = 4;
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
                Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(100 * transform.localScale.x, 0));
                multiplier = 0;
            }
            else
                multiplier = 2;
            if (Player.transform.position.x < transform.position.x && transform.localScale.x > 0)
                multiplier *= 2;
            else if (Player.transform.position.x > transform.position.x && transform.localScale.x < 0)
                multiplier *= 2;

            coroutine = invincible();
            StartCoroutine(coroutine);

            if (!invinc)
            {
                Debug.Log(health);
                health -= 1 * multiplier;
                invinc = true;
            }
        }
        if (health < 1)
            death();
    }

    private IEnumerator invincible()
    {
        yield return new WaitForSeconds(invinc_time);
        invinc = false;
    }

    private void death()
    {
        gameObject.SetActive(false);
    }
}
