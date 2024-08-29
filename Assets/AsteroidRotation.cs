using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRotation : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed = 1f;
   
    void Update()
    {
        gameObject.transform.eulerAngles += new Vector3(0, 0, _rotatingSpeed*Time.deltaTime);
    }
}
