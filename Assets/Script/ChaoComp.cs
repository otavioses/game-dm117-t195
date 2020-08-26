﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoComp : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehavior>())
        {
            collision.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
