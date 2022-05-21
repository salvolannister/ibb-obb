using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public GameObject targetTwo;
    public int smoothvalue = 2;
    public float posY = 1;

    private Transform targetOneTrs;
    private Transform targetTwoTrs;
    private Transform targetTrs;
    // Use this for initialization
    public Coroutine my_co;

    private void OnEnable()
    {
        Assert.IsNotNull(target, " Set the first target for the cameraCameraController");
        Assert.IsNotNull(targetTwo, " Set the second target for the cameraController");
      
    }
    void Start()
    {
        targetOneTrs = target.transform;
        targetTwoTrs = targetTwo.transform;
    }


    void Update()
    {
        // TODO: include also the second target in those calculation
        if (targetTwoTrs.position.x > targetOneTrs.position.x)
        {
            targetTrs = targetTwoTrs;
        }
        else
        {
            targetTrs = targetOneTrs;
        }
        Vector3 oldPos = transform.position;
        Vector3 targetpos = new Vector3(targetTrs.position.x, targetTrs.position.y + posY, -10);
        transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * smoothvalue);

        if (!IsPositionValid())
        {
            transform.position = oldPos;
        }


    }

    private bool IsPositionValid()
    {
        if (targetTrs != targetOneTrs && ScreenBounds.OOB(targetOneTrs.position))
        {
            return false;
        }
        else if (ScreenBounds.OOB(targetTwoTrs.position))
        {
            return false;
        }

        return true;
    }
}
