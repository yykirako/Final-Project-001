/*This script checks collision in/out the container via the front side*/
/*The main purpose is to switch the collsion layers of objects so that they can enter and stay in the container*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerIntrusionDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /*layer 3 -> OutsideContainer*/
        /*layer 6 -> InsideContainer*/

        Debug.Log(gameObject.name+ ": onTriggerEnter ");
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 3 && !other.gameObject.CompareTag("Container") )
        {
            other.gameObject.layer = 6;
            foreach (Transform child in other.gameObject.transform)
            {
                child.gameObject.layer = 6;
            }
        }
        Debug.Log(other.gameObject.layer);
    }

}
