using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class config : PiScript
{
	void Awake ()
    {
        Axis1.steplength = 1.5f;
        Axis1.stepcounter = 0;
        Axis1.speed = 10f;

        Axis1.steplength = 1.0f;
        Axis1.stepcounter = 0;
        Axis1.speed = 5f;
    }
}
