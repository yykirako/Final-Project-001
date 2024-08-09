/*This script manages player controls and (camera motions?) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _directionIndicator;
    [SerializeField] private Image _forceBar;

    private float _projectionAngleX = 0f;
    private float _projectionAngleY = 0f;

    private GameObject[] _fruitPrefabs;
    private GameObject _fruitSpawned;
    private Rigidbody _fruitRb;
    private GameObject _container;


    [SerializeField] private GameObject _playerHand;
    [SerializeField] private float _pullForce;
    [SerializeField] private float _forceIncrease = 10f;
    [SerializeField] private float _minForce = 5f;
    [SerializeField] private float _maxForce = 30f;

    [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.5f, 1f); //fruit spawning positon to hand
    private Vector3 _spawnPosition;
    private bool _isAFruitInHand = false;

    //dash mode attributes
    /*private Rigidbody _playerRb;
    [SerializeField] GameObject _transportationPosition;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 360f;
    [SerializeField] private Camera _followingCamera;*/
    private Vector3 _cameraForward = new Vector3(0f, -0.25f, 4.5f);//camera close to containor
    [SerializeField] private bool _isFruitSwallowed = false;
    private GameObject _fruitSwallowed;
    /*[SerializeField] private float _blastForce = 5f;*/
    private float _raycastLength = 5f;

    private void Start()
    {
        _fruitPrefabs = GameManager.Instance.FruitPrefabs;
        _container = GameManager.Instance.Container;
        /*_playerRb = GetComponent<Rigidbody>();*/
        if (_playerHand != null)
        {
            _spawnPosition = _playerHand.transform.position + _spawnOffset;
        }
        else
        {
            Debug.Log("PlayerController: `__playerHand` is null.");
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (_fruitPrefabs != null)
        {
            if (_container != null)
            {
                if (!GameManager.Instance.IsDashMode)
                {
                    SpawnRandomFruit();
                    if (GameManager.Instance.IsBatteryCharged)
                    {
                        if (Input.GetKeyUp(KeyCode.Space)) //enter "dimensional dash" mode
                        {
                            /*_followingCamera.enabled = true;
                            _mainCamera.enabled = false;*/
                            GameManager.Instance.IsDashMode = true;

                            _mainCamera.transform.position += _cameraForward;

                        }
                    }

                }
                else
                {
                    FruitVacuumAndBlast();
                }
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
        if (Input.GetMouseButtonDown(0) && !_isAFruitInHand)
        {
            int _fruitIndex = Random.Range(0, _fruitPrefabs.Length - 4);

            //initialize the fruit
            _fruitSpawned = Instantiate(_fruitPrefabs[_fruitIndex], _spawnPosition, _fruitPrefabs[_fruitIndex].transform.rotation, _container.transform);

            //set the collision layer
            _fruitSpawned.layer = 3;
            foreach (Transform child in _fruitSpawned.transform)
            {
                child.gameObject.layer = 3;
            }

            _isAFruitInHand = true;
            _fruitRb = _fruitSpawned.GetComponent<Rigidbody>();
            _pullForce = _minForce;

            //show the direction indicator
            _directionIndicator.SetActive(true);
            _directionIndicator.transform.position = _spawnPosition;

        }

        //increase the force while mouse is held down
        if (_fruitSpawned != null)
        {
            if (Input.GetMouseButton(0) && _isAFruitInHand)
            {
                if (_pullForce < _maxForce)
                {
                    _pullForce += _forceIncrease * Time.deltaTime;
                    _forceBar.fillAmount = Mathf.Clamp01(_pullForce / _maxForce);
                }

                //create a trembling effect to indicate the force
                Vector3 pullingAnimation = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), (Random.Range(-0.01f, 0.0f) - _pullForce * 0.005f));

                //while mouse being held down, check mouse movement to change the _projection direction
                if ((_projectionAngleX < -15f && Input.GetAxis("Mouse X") > 0f)
                    || (_projectionAngleX > 15f && Input.GetAxis("Mouse X") < 0f)
                    || (_projectionAngleX > -15f && _projectionAngleX < 15f))
                {
                    _projectionAngleX += Input.GetAxis("Mouse X");
                }
                if ((_projectionAngleY < -15f && Input.GetAxis("Mouse Y") > 0f)
                    || (_projectionAngleY > 15f && Input.GetAxis("Mouse Y") < 0f)
                    || (_projectionAngleY > -15f && _projectionAngleY < 15f))
                {
                    _projectionAngleY += Input.GetAxis("Mouse Y");
                }

                //show the force direction with the indicating arrow
                _directionIndicator.transform.eulerAngles = new Vector3(
                    _directionIndicator.transform.eulerAngles.x,
                    90f + _projectionAngleX,
                    90f - _projectionAngleY
                );


                //calculate the position of the fruit to show the trembling
                _fruitSpawned.transform.position = _spawnPosition + pullingAnimation + new Vector3(_projectionAngleX * 0.005f, _projectionAngleY * 0.005f, 0f);

            }
        }
        else //reset all status
        {
            /*Debug.Log("PlayerController: `fruitSpawned` is null.");*/
            _isAFruitInHand = false;
        }


        //project the fruit when mouse up
        if (_fruitRb != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //play a throwing sound effect
                gameObject.GetComponent<AudioSource>().Play();

                //apply the force to the fruit
                _fruitRb.AddForce(_pullForce * (_fruitSpawned.transform.position - _playerHand.transform.position), ForceMode.Impulse);

                _isAFruitInHand = false;

                //reset force
                _pullForce = _minForce;
                _forceBar.fillAmount = 0f;
                //reset angle
                _projectionAngleX = 0.0f;
                _projectionAngleY = 0.0f;
                //deactive directional indicator
                _directionIndicator.SetActive(false);
            }
        }
        else
        {
            /*Debug.Log("PlayerController: `fruitRb` is null.");*/
            _isAFruitInHand = false;
        }
    }


    private void FruitVacuumAndBlast()
    {
        /*Debug.Log("PlayerController: FruitVacuumAndBlast");*/
        if (Input.GetMouseButtonUp(0)) // Left-click to swallow or throw
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;

            // Cast a Ray and get all hits within the distance
            hits = Physics.RaycastAll(ray, _raycastLength);

            // Sort hits by distance to find the closest fruit
            System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            foreach (RaycastHit hit in hits)
            {
                GameObject _hitObject = hit.collider.gameObject;

                // Check if the hit object has a Fruit component
                if (_hitObject.GetComponent<Fruit>() != null)
                {
                    Debug.Log("PlayerController: Raycast hit fruit: " + _hitObject.name);
                    if (!_isFruitSwallowed)
                    {
                        Debug.Log("PlayerController: Fruit Vacuum");

                        _fruitSwallowed = _hitObject;
                        _fruitSwallowed.SetActive(false); // Hide the fruit
                        _isFruitSwallowed = true;
                        break; // Exit loop once the fruit is found

                    }
                    else
                    {
                        Debug.Log("PlayerController: Fruit Blast");
                        if (_fruitSwallowed != null)
                        {
                            _fruitSwallowed.transform.position = _hitObject.transform.position;
                            _fruitSwallowed.SetActive(true);
                        }
                        _isFruitSwallowed = false;
                    }

                }
            }

            Debug.DrawRay(ray.origin, ray.direction * _raycastLength, Color.red, 1f); // Visualize the ray


        }


    }
}
