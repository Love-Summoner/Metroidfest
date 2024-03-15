using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale_trigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject portal;
    private int num = 4;

    // Update is called once per frame
    void Update()
    {
        if (num == 0)
            portal.SetActive(true);
    }
    public void decremenr()
    {
        num--;
    }
}
