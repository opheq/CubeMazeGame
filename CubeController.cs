using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Vector2 input;
    void Update()
    {
        input.y = Input.GetAxis("Horizontal");
        input.x = Input.GetAxis("Vertical");
        RotateCube(input);
    }


    private void RotateCube(Vector2 input){
        transform.Rotate(new Vector3 (input.x,0f,-input.y),Space.World);
    }
}
