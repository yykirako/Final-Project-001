using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContainerRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50f;
    void Update()
    {
        if (Input.anyKey)
        {
            float rotationAroundY = Input.GetAxis("Horizontal");

            Vector3 center = transform.GetChild(0).GetChild(0).position;

            
            //rotate the container and what inside around Y axis
            transform.RotateAround(center, Vector3.up, -rotationAroundY * rotationSpeed * Time.deltaTime);
        }
        
        
    }
}
