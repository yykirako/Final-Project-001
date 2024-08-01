using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContainerRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30f;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {

            /*Debug.Log(Input.GetAxis("Horizontal"));*/

            float rotationAroundY = Input.GetAxis("Horizontal");

            Vector3 center = transform.GetChild(0).GetChild(0).position;

            transform.RotateAround(center, Vector3.up, rotationAroundY * rotationSpeed * Time.deltaTime);
            /*transform.Rotate(0, rotationAroundY*rotationSpeed*Time.deltaTime, 0, Space.World);*/
        }
    }
}
