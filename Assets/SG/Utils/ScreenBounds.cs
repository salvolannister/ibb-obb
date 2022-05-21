using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: If Camera.main is going to move or rotate at all, then it will need to
//  have a Rigidbody attached so that the physics engine properly updates the 
//  position and rotation of this BoxCollider.

/// <summary>
/// This class should be attached to a child of Camera.main. It triggers various
///  behaviors to happen when a GameObject exits the screen.<para/>
/// NOTE: Camera.main must be orthographic.<para/>
/// NOTE: This GameObject must have a BoxCollider attached.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class ScreenBounds : Manager<ScreenBounds>
{

    public float zScale = 10;

    Camera cam;
    BoxCollider boxColl;
    float cachedOrthographicSize, cachedAspect;
    Vector3 cachedCamScale;


    protected override void Awake()
    {
       base.Awake();

        cam = Camera.main;
        // Need to make sure that the camera is Orthographic for this to work
        if (!cam.orthographic)
        {
            Debug.LogError("ScaleToCamera:Start() - Camera.main needs to be orthographic " +
                           "for ScaleToCamera to work, but this camera is not orthographic.");
        }

        boxColl = GetComponent<BoxCollider>();
        // Setting boxColl.size to 1 ensures that other calculations will be correct.
        boxColl.size = Vector3.one;

        transform.position = Vector3.zero;
        ScaleSelf();
    }


    private void Update()
    {
        ScaleSelf();
    }


    // Scale this Transform to match what cam can see.
    private void ScaleSelf()
    {
        // Check here to see whether anything has changed about cam.orthographicSize
        //  or cam.aspect. If those values are the same as cached, then there is no
        //  need to change the localScale of this.transform.
        if (cam.orthographicSize != cachedOrthographicSize || cam.aspect != cachedAspect
            || cam.transform.localScale != cachedCamScale)
        {
            transform.localScale = CalculateScale();
        }
    }


    private Vector3 CalculateScale()
    {
        cachedOrthographicSize = cam.orthographicSize;
        cachedAspect = cam.aspect;
        cachedCamScale = cam.transform.localScale;

        Vector3 scaleDesired, scaleColl;

        scaleDesired.z = zScale;
        scaleDesired.y = cam.orthographicSize * 2;
        scaleDesired.x = scaleDesired.y * cam.aspect;

        scaleDesired.x /= cachedCamScale.x;
        scaleDesired.y /= cachedCamScale.y;
        scaleDesired.z /= cachedCamScale.z;

        scaleColl = scaleDesired;

        return scaleColl;
    }





    static public Bounds BOUNDS
    {
        get
        {
            if (Get() == null)
            {
                Debug.LogError("ScreenBounds.BOUNDS - ScreenBounds.S is null!");
                return new Bounds();
            }
            if (Get().boxColl == null)
            {
                Debug.LogError("ScreenBounds.BOUNDS - ScreenBounds.S.boxColl is null!");
                return new Bounds();
            }
            return Get().boxColl.bounds;
        }
    }


    static public bool OOB(Vector3 worldPos)
    {
        Vector3 locPos = Get().transform.InverseTransformPoint(worldPos);
        // Find in which dimension the locPos is furthest from the origin
        float maxDist = Mathf.Max(Mathf.Abs(locPos.x), Mathf.Abs(locPos.y));
        // If that furthest distance is >0.5f, then worldPos is out of bounds
        return (maxDist > 0.5f);
    }

    static public Vector3 CloserToBound(Vector3 first, Vector3 second)
    {
        Vector3 locPosFirst = Get().transform.InverseTransformPoint(first);
        Vector3 locPosSecond = Get().transform.InverseTransformPoint(second);

        // Find in which dimension the locPos is furthest from the origin
        float maxDist = Mathf.Max(Mathf.Abs(locPosFirst.x), Mathf.Abs(locPosFirst.y));
        float maxDist2 = Mathf.Max(Mathf.Abs(locPosSecond.x), Mathf.Abs(locPosSecond.y));
        
        if(maxDist > maxDist2)
        {
            return first;
        }
        else
        {
            return second;
        }
       
    }

    static public int OOB_X(Vector3 worldPos)
    {
        Vector3 locPos = Get().transform.InverseTransformPoint(worldPos);
        return OOB_(locPos.x);
    }
    static public int OOB_Y(Vector3 worldPos)
    {
        Vector3 locPos = Get().transform.InverseTransformPoint(worldPos);
        return OOB_(locPos.y);
    }



    static private int OOB_(float num)
    {
        if (num > 0.5f) return 1;
        if (num < -0.5f) return -1;
        return 0;
    }
}
