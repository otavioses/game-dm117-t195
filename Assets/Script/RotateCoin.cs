using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    public int rotateSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
