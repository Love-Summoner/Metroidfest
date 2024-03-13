using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    [SerializeField] private GameObject smoke;
    [SerializeField] public Rigidbody2D PlayerBody;

    public bool _hasSmoke = false;
    public bool canSmoke = false;
    public float smokeTime = 3f;
    public float smokeCool = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canSmoke)
        {
            StartCoroutine(Smokescreen());
        }

    }

    private IEnumerator Smokescreen()
    {
        smoke.transform.position = PlayerBody.transform.position;
        canSmoke = false;
        smoke.SetActive(true);
        yield return new WaitForSeconds(smokeTime);
        smoke.SetActive(false);
        yield return new WaitForSeconds(smokeCool);
        canSmoke = true;


    }

}
