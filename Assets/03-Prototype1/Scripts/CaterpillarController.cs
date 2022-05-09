using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    public Transform head;
    public float moveSpeed, rotationSpeed;
    public GameObject bodyPrefab;

    private List<GameObject> bodyParts = new List<GameObject>();

    private void Start()
    {
        bodyParts.Add(head.gameObject);
    }

    private void Update()
    {
        head.position += head.forward * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            head.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            head.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AddBodyPart();
        }
    }

    public void AddBodyPart()
    {
        GameObject previousLastPart = bodyParts[bodyParts.Count - 1];
        GameObject bodyPart = Instantiate(bodyPrefab, transform);
        bodyPart.transform.localPosition = previousLastPart.transform.localPosition - previousLastPart.transform.forward * 5f;
        bodyPart.transform.forward = previousLastPart.transform.forward;

        HingeJoint joint = bodyPart.GetComponent<HingeJoint>();
        joint.connectedBody = previousLastPart.GetComponent<Rigidbody>();
        bodyParts.Add(bodyPart);
    }
}