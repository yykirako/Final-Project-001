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

            Debug.Log(Input.GetAxis("Horizontal"));

            // Rotate smoothly by converting the angles into a quaternion.
            //Referece:https://docs.unity3d.com/ScriptReference/Transform-rotation.html
            /*float rotationAroundY = Input.GetAxis("Horizontal");
            Quaternion target = Quaternion.Euler(0f, rotationAroundY,0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime *smooth);*/
            float rotationAroundY = Input.GetAxis("Horizontal");

            Vector3 center = transform.GetChild(0).position;

            transform.RotateAround(center, Vector3.up, rotationAroundY * rotationSpeed * Time.deltaTime);
            /*transform.Rotate(0, rotationAroundY*rotationSpeed*Time.deltaTime, 0, Space.World);*/
        }
    }
}
