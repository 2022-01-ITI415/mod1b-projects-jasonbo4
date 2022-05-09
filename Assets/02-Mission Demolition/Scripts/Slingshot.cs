using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Slingshot : MonoBehaviour
{
     static private Slingshot S;

    [Header("Set in Inspector")]  //Sets up the section in the Unity Editor for Inspector set public variables
    
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    
    [Header("Set Dynamically")] //Sets up the section in the Unity Editor for Dynamically set public variables
    
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if(S == null) return Vector3.zero;

            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;

        //Why do we have to go through all this code, what is exactly happening here
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //The Player has pressed the mouse button down over the slingshot
        aimingMode = true;
        //Instatiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //Start it at the launch point
        projectile.transform.position = launchPos;
        //Set it to kinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update()
    {
        //If slingshot is not in aimingmode, don't run this code
        if(!aimingMode)
        {
            return;
        }

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D (What is delta?)
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Limit mousedelta to the radius of the slingshot collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            //MissionDemolition.ShotsFired();
            //ProjectileLine.S.poi = projectile;
        }
    }
}

