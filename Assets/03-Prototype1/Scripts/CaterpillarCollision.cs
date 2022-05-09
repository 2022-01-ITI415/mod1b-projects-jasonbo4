using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollisions : MonoBehaviour
{

    public GameObject gameOverCanvas;
    public GameObject snake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Body" || other.tag == "Wall")
        {
            gameOverCanvas.SetActive(true);
            Destroy(snake);
        }
    }
}