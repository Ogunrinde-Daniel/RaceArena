using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public float rotationSpeed = 900f;

    public float steerSpeed = 50f;
    private float snapBackSpeed = 0f;
    public bool turnLeft = false;
    public bool turnRight = false;

    public float maxSteer = 45f;
    public bool steering = false;

    void Start()
    {
        snapBackSpeed = steerSpeed * 2;
    }

    void Update()
    {

        float yAngleToAdd = 0f;
        float yAngle = Mathf.Asin(transform.localRotation.y) * Mathf.Rad2Deg * 2f;                                                                           //float xAngle = Mathf.Asin(transform.localRotation.x) * Mathf.Rad2Deg * 2f;

        if (turnLeft && yAngle > -maxSteer)
        {
            yAngleToAdd -= steerSpeed * Time.deltaTime;
        }
        else if (turnRight && yAngle < maxSteer)
        {
            yAngleToAdd += steerSpeed * Time.deltaTime;
        }
        else if (yAngle < -1)
        {
            yAngleToAdd += snapBackSpeed * Time.deltaTime;
        }
        else if (yAngle > 1)
        {
            yAngleToAdd -= snapBackSpeed * Time.deltaTime;
        }
       
        

        if (!Mathf.Approximately(yAngleToAdd, 0))
        {
            Quaternion rotationQuaternion = Quaternion.Euler(0.0f, yAngle + yAngleToAdd, 0.0f);
            transform.localRotation = rotationQuaternion;
        }
        else
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);

    }

    /*void Update()
    {


        steering = false;
        float yAngle = Mathf.Asin(transform.localRotation.y) * Mathf.Rad2Deg * 2f; //--convert rotation to degrees
                                                                                   //float xAngle = Mathf.Asin(transform.localRotation.x) * Mathf.Rad2Deg * 2f;
        Debug.Log(yAngle);
        if (turnLeft && yAngle > -maxSteer)
        {
            yAngle -= steerSpeed * Time.deltaTime;
            steering = true;
        }
        else if (turnRight && yAngle < maxSteer)
        {
            yAngle += steerSpeed * Time.deltaTime;
            steering = true;

        }
        else if (yAngle < -1 || yAngle > 1)
        {
            if (yAngle < 0)
            {
                steering = true;
                yAngle += snapBackSpeed * Time.deltaTime;
            }
            if (yAngle > 0)
            {
                steering = true;
                yAngle -= snapBackSpeed * Time.deltaTime;
            }
        }


        if (steering)
        {
            var rotation = transform.localRotation;
            Quaternion rotationQuaternion = Quaternion.Euler(0.0f, yAngle, 0.0f);
            rotation.y = rotationQuaternion.y;
            transform.localRotation = rotationQuaternion;
            //transform.eulerAngles = rotation;
        }
        if (!steering)
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);

    }*/
}
