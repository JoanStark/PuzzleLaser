﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private Transform guide;
    [SerializeField] private Transform origin;
    private Rigidbody grabbedObject;
    [SerializeField] [Range (0f , 1f)] private float velocityDemultiplier;
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(origin.position, origin.forward, out hit, 7.5f) && hit.transform.GetComponent<Rigidbody>())
        {
            grabbedObject = hit.transform.gameObject.GetComponent<Rigidbody>();
            grabbedObject.useGravity = false;
            grabbedObject.freezeRotation = true;
        }
        if (Input.GetMouseButtonUp(0) && grabbedObject)
        {
            grabbedObject.useGravity = true;
            grabbedObject.freezeRotation = false;
            grabbedObject.velocity *= velocityDemultiplier;
            grabbedObject = null;
        }
    }
    void FixedUpdate()
    {
        if (grabbedObject)
        {
            grabbedObject.velocity = 10 * (guide.position - grabbedObject.position);
        }
    }
}