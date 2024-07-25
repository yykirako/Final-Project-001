/*This script manages player controls and (camera motions?) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private GameObject directionIndicator;
    private float projectionAngleX = 0f;
    private float projectionAngleY = 0f;

    private GameObject[] fruitPrefabs;
    private GameObject fruitSpawned;
    private Rigidbody fruitRb;

    [SerializeField] private GameObject playerHand;
    [SerializeField] private float pullForce = 10f;

    [SerializeField] Vector3 spawnOffset = new Vector3 (0, 0.5f, 1f); //fruit spawning positon to hand

    private bool isAFruitInHand = false;

    private void Awake()
    {
        fruitPrefabs = gameManager.GetComponent<GameManager>().fruitPrefabs;
    }
    // Update is called once per frame
    void Update()
    {
        SpawnRandomFruit();
    }
    private void SpawnRandomFruit()
    {
        Vector3 spawnPosition = playerHand.transform.position + spawnOffset;
        int fruitIndex = Random.Range(0, fruitPrefabs.Length-4);

        //spawn a random fruit when mouse down
        if (Input.GetMouseButtonDown(0) && !isAFruitInHand)
        {

            fruitSpawned = Instantiate(fruitPrefabs[fruitIndex], spawnPosition, fruitPrefabs[fruitIndex].transform.rotation);
            fruitSpawned.layer = 3;
            foreach (Transform child in fruitSpawned.gameObject.transform)
            {
                child.gameObject.layer = 3;
            }

            isAFruitInHand = true;
            fruitRb = fruitSpawned.GetComponent<Rigidbody>();
        }

        //increase the force while mouse is held down
        if (Input.GetMouseButton(0) && isAFruitInHand)
        {
            /*Debug.Log("left mouse button is held down");*/

            if (pullForce < 80f)
            {
                pullForce += 5f * Time.deltaTime;

            }
            Debug.Log("pullForce: " + pullForce);

            //create a trembling effect to indicate the force
            Vector3 pullingAnimation = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), (Random.Range(-0.01f, 0.0f) - pullForce * 0.005f));

            //while mouse being held down, check mouse movement to change the projection direction
           /* Debug.Log("mouse move: " + Input.GetAxis("Mouse X"));*/
            if((projectionAngleX < -15f && Input.GetAxis("Mouse X") > 0f)
                || (projectionAngleX >15f && Input.GetAxis("Mouse X") < 0f)
                || (projectionAngleX > -15f && projectionAngleX < 15f))
            {
                projectionAngleX += Input.GetAxis("Mouse X");
            }
            if ((projectionAngleY < -15f && Input.GetAxis("Mouse Y") > 0f)
                || (projectionAngleY > 15f && Input.GetAxis("Mouse Y") < 0f)
                || (projectionAngleY > -15f && projectionAngleY < 15f))
            {
                projectionAngleY += Input.GetAxis("Mouse Y");
            }

            directionIndicator.transform.eulerAngles = new Vector3(
                directionIndicator.transform.eulerAngles.x,
                90f + projectionAngleX,
                90f - projectionAngleY
            );

            fruitSpawned.transform.position = spawnPosition + pullingAnimation + new Vector3(projectionAngleX * 0.005f, projectionAngleY * 0.005f, 0f);



        }

        //project the fruit when mouse up
        if (Input.GetMouseButtonUp(0))
        {
            fruitRb.AddForce(pullForce * (fruitSpawned.transform.position - playerHand.transform.position), ForceMode.Impulse);
            Debug.Log("left mouse button up");
            isAFruitInHand = false;

            //reset force
            pullForce = 10f;
            //reset angle
            projectionAngleX = 0.0f;
            projectionAngleY = 0.0f;

        }
    }

   

 /*   private void DragFruit()
    {

    }*/
}
