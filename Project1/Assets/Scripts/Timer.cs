using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    PlanetManagement planet;
    private Text myText;

    // Start is called before the first frame update
    void Start()
    {
        myText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = String.Format("{0:000.00}", Math.Round(planet.timeRemaining, 2));
        if (planet.timeRemaining > 15)
        {
            myText.color = new Color(1, 1, 1, 1);
        }
        else
        {
            myText.color = new Color(1, 0, 0, 1);
        }
    }
}
