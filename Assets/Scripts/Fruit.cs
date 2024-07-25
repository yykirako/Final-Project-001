using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    private GameObject[] fruitPrefabs;

    /*public bool isFruitCollided = false;
    public int fruitCollided = -1;*/

    private float xPosition;
    private float yPosition;
    private float zPosition;
    private float width;

    private int thisID;
    private GameObject fruitMerged;
    private int fruitIndex;


    private void Awake()
    {

        /*xPosition = transform.position.x;
        zPosition = transform.position.z;
        width = transform.localScale.x;

        IsInContainer();*/
        thisID = GetInstanceID();
        fruitPrefabs = gameManager.GetComponent<GameManager>().fruitPrefabs;
        fruitIndex = FindFruitIndex(gameObject);
    }

    private void Update()
    {
        yPosition = transform.position.y;
        if (yPosition < -50)
        {
            Destroy(gameObject);
        }
    }
    private void MergeFruits(int fruitCollided, Vector3 mergePosition)
    {
        if (fruitCollided > -1 && fruitCollided < fruitPrefabs.Length-1)
        {
            fruitMerged = Instantiate(fruitPrefabs[fruitCollided + 1], mergePosition, fruitPrefabs[fruitCollided + 1].transform.rotation);

            //make sure the new fruit object and children have the correct layer setting
            fruitMerged.layer = 6;
            foreach (Transform child in fruitMerged.gameObject.transform)
            {
                child.gameObject.layer = 6;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag(collision.gameObject.tag))
        {
            Vector3 mergePosition = (collision.transform.position + transform.position) / 2;
            if (thisID < collision.gameObject.GetInstanceID()) // this makes sure the merging happens only ONCE while TWO fruits collide
            {
                if (fruitIndex >= 0 && fruitIndex < fruitPrefabs.Length - 1)
                {
                    MergeFruits(fruitIndex, mergePosition);
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }       
            }
        }
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
