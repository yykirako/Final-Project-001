/*This script manages player controls and (camera motions?) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private Image forceBar;

    private float projectionAngleX = 0f;
    private float projectionAngleY = 0f;

    private GameObject[] fruitPrefabs;
    private GameObject fruitSpawned;
    private Rigidbody fruitRb;
    private GameObject container;


    [SerializeField] private GameObject playerHand;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float forceIncrease = 10f;
    [SerializeField] private float maxForce = 50f;

    [SerializeField] private Vector3 spawnOffset = new Vector3 (0, 0.5f, 1f); //fruit spawning positon to hand
    private Vector3 spawnPosition;
    private bool isAFruitInHand = false;

    private void Start()
    {
        fruitPrefabs = GameManager.Instance.FruitPrefabs;
        container = GameManager.Instance.Container;
        if(playerHand != null)
        {
            spawnPosition = playerHand.transform.position + spawnOffset;
        }
        else
        {
            Debug.Log("PlayerController: `playerHand` is null.");
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(fruitPrefabs != null)
        {
            if(container != null)
            {
                SpawnRandomFruit();
            }
            else
            {
                Debug.Log("PlayerController: `fruitPrefabs` is null.");
            }
        }
        else
        {
            Debug.Log("PlayerController: `container` is null.");
        }
    }
    private void SpawnRandomFruit()
    {

        //spawn a random fruit when mouse down
        if (Input.GetMouseButtonDown(0) && !isAFruitInHand)
        {
            int fruitIndex = Random.Range(0, fruitPrefabs.Length - 4);

            fruitSpawned = Instantiate(fruitPrefabs[fruitIndex], spawnPosition, fruitPrefabs[fruitIndex].transform.rotation, container.transform);
            fruitSpawned.layer = 3;
            foreach (Transform child in fruitSpawned.transform)
            {
                child.gameObject.layer = 3;
            }

            isAFruitInHand = true;
            fruitRb = fruitSpawned.GetComponent<Rigidbody>();

        }

        //increase the force while mouse is held down
        if (fruitSpawned != null)
        {
            if (Input.GetMouseButton(0) && isAFruitInHand)
            {
                /*Debug.Log("left mouse button is held down");*/

                if (pullForce < maxForce)
                {
                    pullForce += forceIncrease * Time.deltaTime;
                    forceBar.fillAmount = Mathf.Clamp01(pullForce / maxForce);


                }
                /*Debug.Log("pullForce: " + pullForce);*/

                //create a trembling effect to indicate the force
                Vector3 pullingAnimation = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), (Random.Range(-0.01f, 0.0f) - pullForce * 0.005f));

                //while mouse being held down, check mouse movement to change the projection direction
                /* Debug.Log("mouse move: " + Input.GetAxis("Mouse X"));*/
                if ((projectionAngleX < -15f && Input.GetAxis("Mouse X") > 0f)
                    || (projectionAngleX > 15f && Input.GetAxis("Mouse X") < 0f)
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
        }
        else //reset all status
        {
            /*Debug.Log("PlayerController: `fruitSpawned` is null.");*/

            isAFruitInHand = false;
        }
       

        //project the fruit when mouse up
        if(fruitRb!=null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                gameObject.GetComponent<AudioSource>().Play();

                fruitRb.AddForce(pullForce * (fruitSpawned.transform.position - playerHand.transform.position), ForceMode.Impulse);
                /*Debug.Log("left mouse button up");*/
                isAFruitInHand = false;

                //reset force
                pullForce = 10f;
                //reset angle
                projectionAngleX = 0.0f;
                projectionAngleY = 0.0f;
            }
        }
        else
        {
            /*Debug.Log("PlayerController: `fruitRb` is null.");*/

            isAFruitInHand = false;
        }
    }

   

 /*   private void DragFruit()
    {

    }*/
}
