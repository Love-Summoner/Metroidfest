using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatIndication : MonoBehaviour
{
    public GameObject[] health;
    public GameObject[] abilities;
    //0 - Dash
    //1 - Invis
    //2 - Smoke
    public GameObject player;

    Color color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        setHealth();
        setAbilities();

    }

    // Update is called once per frame
    void Update()
    {
        setHealth();
        setAbilities();
    }

    public void setHealth()
    {
        for(int i = 0; i < health.Length; i++)
        {
            if(i < player.GetComponent<PlayerScript>().health)
            {
                changeColor(true);
                health[i].GetComponent<Image>().color = color;
            }
            else
            {
                changeColor(false);
                health[i].GetComponent <Image>().color = color;
            }
        }
    }

    public void setAbilities()
    {
        for(int i = 0;i < abilities.Length; i++)
        {
            //Dash
            if(i == 0 /*&& player.GetComponent<PlayerScript>().canDash*/)
            {
                changeColor(false);
                abilities[i].GetComponent<Image>().color = color;
            }//Invis
            else if(i == 1 && player.GetComponent<PlayerScript>().canInvis)
            {
                changeColor(false);
                abilities[i].GetComponent<Image>().color = color;
            }//Smoke
            else if(i == 2 && player.GetComponent<SmokeScreen>().canSmoke)
            {
                changeColor(false);
                abilities[i].GetComponent<Image>().color = color;
            }//Is Not Active
            else
            {
                changeColor(true);
                abilities[i].GetComponent<Image>().color = color;
            }
        }
    }

    private void changeColor(bool val)
    {
        //Is Active
        if (val)
        {
            color = Color.white;
            color.a = 1f;
        }
        else
        {
            color.g = .25f;
            color.b = .25f;
            color.r = .25f;
            color.a = .5f;
        }
    }
}
