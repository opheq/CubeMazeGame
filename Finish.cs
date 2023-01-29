using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private GameObject _ball;
    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player")
            Destroy(col.gameObject);
    }
}
