using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{

    public float speed =80f;


    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0), Space.World);
    }
}
