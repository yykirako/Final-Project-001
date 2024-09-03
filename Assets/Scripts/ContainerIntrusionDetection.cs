/*This script checks collision in/out the container via the front side*/
/*The main purpose is to switch the collsion layers of objects so that they can enter and stay in the container*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerIntrusionDetection : MonoBehaviour
{    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && other.gameObject.GetComponent<Fruit>())
        {
            Debug.Log(other.gameObject.GetComponent<Fruit>());
            other.gameObject.layer = 6;
            other.gameObject.transform.SetParent(other.gameObject.transform.parent.GetChild(0), true);//move the frtui into the same hierachy as the container
            foreach (Transform child in other.gameObject.transform)
            {
                child.gameObject.layer = 6;
            }
        }
    }

}
