using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class linearAxis : MonoBehaviour
{
    // axis parameters
    public float steplength;    // global steplength in mm
    public float calStepLen;   // calibrated step length as determined from the sumber of steps to move a distance in the global world
    public float length;
    public long stepcounter;    // keeping track of how many steps where moved 
    public float speed;         // steps per FixedUpdate
    public float stepaccel;         // change of steps per fixed update per second squared
    private float parentScale;
    public bool isMoving;
    public bool endStop;
    private bool calibrated = false;
    public bool globalCalibrated = false;
    public float globalLength;
    public Vector3 accel;
    public float accelZ;

    public static bool Moving = false;

    public List<linearAxis> Robot1 = new List<linearAxis>();

    public IEnumerator Rotate(int degrees)
    {
        this.isMoving = true;
        linearAxis.Moving = true;

        degrees = (int)(degrees * 10 / speed);
        
        for (int i = 0; i < Mathf.Abs(degrees); i++)
        {
            this.transform.Rotate(0.1f * speed * Vector3.up * Mathf.Sign(degrees) );
            yield return new WaitForFixedUpdate();
        }

        this.isMoving = false;
        linearAxis.Moving = false;

    }

    public IEnumerator Acceleration()
    {
        Vector3 pos1 = transform.position;
        //Debug.Log("pos1: " + pos1);
        yield return new WaitForFixedUpdate();
        Vector3 pos2 = transform.position;
        //Debug.Log("pos2: " + pos2);
        yield return new WaitForFixedUpdate();
        Vector3 pos3 = transform.position;
        //Debug.Log("pos3: " + pos3);

        Vector3 vel1 = (pos2 - pos1) / Time.deltaTime;
        //Debug.Log("vel1: " + vel1);
        Vector3 vel2 = (pos3 - pos2) / Time.deltaTime;
        //Debug.Log("vel2: " + vel2);

        accel = (vel2 - vel1) / Time.deltaTime;
        //Debug.Log("accel: " + accel);

        Debug.Log("magnitude: " + accel.magnitude);
    }

    // constructor that does not work
    public linearAxis()
    {
        steplength = 1f;
        stepcounter = 0;
        speed = 1.0f;
        stepaccel = 1.0f;
    }
    
    public void addList()
    {
        Robot1.Add(this);
    }
    //instead of constructor
    public void Config(float steplength, long stepcounter, float speed)
    {
        this.steplength = steplength;
        this.stepcounter = stepcounter;
        this.speed = speed;
    }

    // calibration
    public IEnumerator LocalCalibration()
    {
        float oldspeed = this.speed;
        Debug.Log(this.ToString() + " calibration");


        while (this.endStop == false)
        {
            this.speed = 10;
            yield return StartCoroutine(this.Steps(-1));
        }
        yield return StartCoroutine(this.Steps(1));

        while (this.endStop == false)
        {
            this.speed = 1;
            yield return StartCoroutine(this.Steps(-1));
        }
        this.stepcounter = 0;
        Debug.Log(this.ToString() + " stepcounter set to: " + this.stepcounter);
        this.speed = 10;
        yield return StartCoroutine(this.Steps(1));



        while (this.endStop == false)
        {
            this.speed = 10;
            yield return StartCoroutine(this.Steps(1));
        }
        yield return StartCoroutine(this.Steps(-1));

        while (this.endStop == false)
        {
            this.speed = 1;
            yield return StartCoroutine(this.Steps(1));
        }
        this.length = stepcounter;
        Debug.Log(this.ToString() + " length set to: " + this.length);
        this.speed = oldspeed;
        this.calibrated = true;
        yield return StartCoroutine(Steps(-(int)(this.length / (2 * this.speed))));
    }

    // readout
    public Vector3 giveGlobal()
    {
        Debug.Log(this);
        Debug.Log(this.transform.position);
        return this.transform.position;
    }
    public Vector3 giveLocal()
    {
        Debug.Log(this);
        Debug.Log(this.transform.localPosition);
        return this.transform.localPosition;
    }
    public Vector3 Aruco()
    {
        return GameObject.Find("Aruco").transform.position;
    }

    // movement control
    public IEnumerator Steps(int steps)
    {
        this.isMoving = true;
        linearAxis.Moving = true;
        for(int counter = 1; counter <= Mathf.Abs(steps); counter++)
        {
            if (Mathf.Abs(this.transform.localPosition.z) <= 0.5)
            {
                this.transform.Translate(Vector3.forward * steplength * Mathf.Sign(steps) * (int)speed);
                this.endStop = false;
            }                
            else
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.5f * Mathf.Sign(this.transform.localPosition.z));
                this.endStop = true;
                Debug.Log("endstop");
            }

            stepcounter += (int)Mathf.Sign(steps) * (int)speed;
            yield return new WaitForFixedUpdate();
        }

        // if it ends at maximum
        if (Mathf.Abs(this.transform.localPosition.z) > 0.5)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.5f * Mathf.Sign(this.transform.localPosition.z));
            this.endStop = true;
        }
            
        this.isMoving = false;
        linearAxis.Moving = false;
    }
    public IEnumerator LocalRelativeMoveTo(float percent)
    {
        if (this.calibrated == true)
        {
            int target = (int)((percent / (100) * this.length));

            while (stepcounter < target)
            {
                yield return StartCoroutine(this.Steps(1));
            }
            while (stepcounter > target)
            {
                yield return StartCoroutine(this.Steps(-1));
            }
        }
        else
            Debug.Log(this.ToString() + " is not calibrated");
        yield return new WaitForFixedUpdate();
    }
    public IEnumerator globalMove(float distance)
    {
        int steps = (int)(distance / calStepLen + 0.5*Mathf.Sign(distance));
        Steps(steps);
        yield return new WaitForFixedUpdate();
    }
    public IEnumerator Wait()
    {
        while(linearAxis.Moving == true)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    
    

   

    void Start ()
    {
        this.isMoving = false;
        if (Mathf.Abs(this.transform.localPosition.z) > 0.5)
            this.endStop = true;
        else
            this.endStop = false;

        parentScale = this.transform.parent.localScale.z;
    }

    void FixedUpdate()
    {
       
    }

}
