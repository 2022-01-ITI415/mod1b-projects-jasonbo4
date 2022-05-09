using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this;

        line = GetComponent<LineRenderer>();
        line.enabled = false;

        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get
        {
            return(_poi);
        }
        
        set
        {
            _poi = value;
            
            if(_poi != null)
            {
                //when _poi is set to something new it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //if the point is not far enough from the last one it returns
            return;
        }

        if(points.Count == 0)
        {
            //If this is the launch point
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            line.enabled = true;
        }else{
            //Normal adding behavior
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }


    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                //If there are no points, return Vector3.zero
                return(Vector3.zero);
            }

            return(points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if(poi == null)
        {
            //if there is no POI search for one
            if(FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                   poi = FollowCam.POI;
                }else{
                   return;
                }
            }else{
                return;
            }
        }

        AddPoint();

        if(FollowCam.POI == null)
        {
            poi = null;
        }
    }
}