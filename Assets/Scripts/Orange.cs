using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : MonoBehaviour
{
    

    [SerializeField] private GameObject applePrefab;
    private Vector3 mergePosition = new Vector3();
    private int thisID;



    private void Start()
    {
        thisID = GetInstanceID();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag(collision.gameObject.tag))
        {
            mergePosition = (collision.transform.position + transform.position) / 2;
            if(thisID < collision.gameObject.GetInstanceID()) // this makes sure the merging happens only ONCE while TWO fruits collide
            {
                Instantiate(applePrefab, mergePosition, applePrefab.transform.rotation);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }

        }
    }


}
