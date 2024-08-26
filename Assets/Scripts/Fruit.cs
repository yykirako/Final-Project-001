using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameObject[] fruitPrefabs;

    private float yPosition;

    private int thisID;
    private int fruitIndex;

    /*private bool _fruitCollided = false;*/

    private void Awake()
    {

        fruitPrefabs = GameManager.Instance.FruitPrefabs;
        thisID = GetInstanceID();
        fruitIndex = FindFruitIndex(gameObject);
    }

    private void Update()
    {
        yPosition = transform.position.y;
        if (yPosition < -10)
        {
            Debug.Log("Fruit: out of boundry");
            Destroy(gameObject);
            DataManager.Instance.CurrentScore -= 5;
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        /*Debug.Log("fruit collided");*/

        if (this.CompareTag(collision.gameObject.tag) && !GameManager.Instance.IsFruitMerging)
        {
            /*Debug.Log("fruit tag compared");*/

            if (thisID < collision.gameObject.GetInstanceID()) // this makes sure the merging happens only ONCE while TWO fruits collide
            {
                /*Debug.Log("fruit ID compared");*/

                Vector3 mergePosition = (collision.transform.position + transform.position) / 2;

                if (fruitIndex >= 0 && fruitIndex < fruitPrefabs.Length - 1)
                {
                    /*Debug.Log("fruitColliding");*/
                    GameManager.Instance.MergeFruit(fruitIndex, mergePosition);

                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }       
            }
        }
        
    }
    private void OnDestroy()
    {
        GameManager.Instance.IsFruitMerging = false;
    }

    //chech the index of this fruit object in the list, to be used conveniently in other functions
    private int FindFruitIndex(GameObject fruit)
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruit.CompareTag(fruitPrefabs[i].tag))
            {
                return i;
            }
        }
        return -1;
    }
}
