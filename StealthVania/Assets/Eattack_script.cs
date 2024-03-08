using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Eattack_script : MonoBehaviour
{
    // Start is called before the first frame update
    Stopwatch timer;
    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.ElapsedMilliseconds > 300)
            Destroy(gameObject);
    }
}
