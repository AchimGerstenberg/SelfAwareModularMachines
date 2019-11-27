using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PiScript : linearAxis
{
    public linearAxis Axis0;
    public linearAxis Axis1;
    public linearAxis Axis2;

    

    
    
    public IEnumerator globalLength(linearAxis axis)
    {
        Vector3 point1;
        Vector3 point2;

        while (linearAxis.Moving == true)
            yield return new WaitForFixedUpdate();

        Debug.Log("calibration started: " + axis.ToString());
        yield return StartCoroutine(axis.LocalRelativeMoveTo(0));
        Debug.Log(GameObject.Find("Aruco").transform.position);
        point1 = GameObject.Find("Aruco").transform.position;

        yield return StartCoroutine(axis.LocalRelativeMoveTo(100));
        Debug.Log(GameObject.Find("Aruco").transform.position);
        point2 = GameObject.Find("Aruco").transform.position;

        axis.globalLength = Mathf.Abs((point2 - point1).magnitude);
        Debug.Log(axis.ToString() + axis.globalLength);

        axis.calStepLen = axis.globalLength / axis.length;
    }

    void Start () {
        //Axis0.Config(1f, 0, 10f);
        Axis1.Config(1.549f, 0, 5f);
        Axis1.addList();
        Debug.Log(Robot1[0].transform.position.x);
        Axis2.Config(0.845735f, 0, 15f);
        
        StartCoroutine(main());

        
       
    }
	
	public IEnumerator main()
    {
        // local calibration
        StartCoroutine(Axis1.LocalCalibration());
        StartCoroutine(Axis2.LocalCalibration());
        yield return StartCoroutine(Wait());
        // global length calibration
        /*
        yield return StartCoroutine(globalLength(Axis1));
        yield return StartCoroutine(globalLength(Axis2));

        Debug.Log("Aruco before: " + Aruco());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Axis1.globalMove(100));
        Debug.Log("Aruco after: " + Aruco());
        yield return new WaitForSeconds(1);
        */

        // acceleration tests
        //yield return StartCoroutine(Axis1.LocalRelativeMoveTo(100));
        yield return StartCoroutine(Axis2.LocalRelativeMoveTo(0));
        StartCoroutine(Axis0.Rotate(-90));
        //StartCoroutine(Axis1.LocalRelativeMoveTo(0));
        yield return StartCoroutine(Axis2.Acceleration());
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(Axis2.Acceleration());






        yield return new WaitForSeconds(10);
        yield return StartCoroutine(Axis1.LocalRelativeMoveTo(40));
        yield return StartCoroutine(Axis2.LocalRelativeMoveTo(60));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Axis2.Steps(-54));
        
        yield return StartCoroutine(Axis1.Steps(100));
        yield return StartCoroutine(Axis2.Steps(100));
        Debug.Log(Axis1.isMoving);
        yield return StartCoroutine(Axis1.Steps(-50));
        yield return StartCoroutine(Axis2.Steps(-50));

        Debug.Log(Axis1.stepcounter);
        Debug.Log(Axis2.stepcounter); 

    }

    void FixedUpdate()
    {
        
    }
}
