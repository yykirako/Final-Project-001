/*This script manages player controls and (camera motions?) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _cameraPosition;

    [SerializeField] private GameObject _directionIndicator;
    [SerializeField] private Image _forceBar;
    [SerializeField] private Image _forceBarShiner;
    [SerializeField] private TMPro.TextMeshProUGUI _forceText;
    [SerializeField] private MainGameUI _mainGameUI;

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
    [SerializeField] private float _maxForce = 20f;
    [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.5f, 1f); //fruit spawning positon to hand
    private Vector3 _spawnPosition;
    private bool _isAFruitInHand = false;

    //dash mode attributes
    [SerializeField] private GameObject _wormholeMoment;
    private Vector3 _cameraForward = new Vector3(0f, -0.25f, 4.5f);//camera close to containor
    [SerializeField] private bool _isFruitSwallowed = false;
    private GameObject _fruitSwallowed;
    [SerializeField] private float _blastForce = 5f;
    private float _raycastLength = 5f;
    [SerializeField] private AudioSource _fruitVacuumSE;
    [SerializeField] private AudioSource _fruitBlastSE;
    [SerializeField] private AudioSource _watermelonEnjoyed;

    private void Start()
    {
        _fruitPrefabs = GameManager.Instance.FruitPrefabs;
        _container = GameManager.Instance.Container;

        if (_playerHand != null)
        {
            _spawnPosition = _playerHand.transform.position + _spawnOffset;
        }
        else
        {
            Debug.Log("PlayerController: `__playerHand` is null.");
        }
        _pullForce = _minForce;
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
                            DashModeIn();
                        }
                    }
                }
                else
                {
                    FruitVacuumAndBlast();
                    //set cursors
                    if (_isFruitSwallowed)
                    {
                        _mainGameUI.SetCursorFilled();
                    }
                    else
                    {
                        _mainGameUI.SetCursorNormal();

                    }
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
            SpawnFruit(_fruitIndex);
        }

        //increase the force while mouse is held down
        if (_fruitSpawned != null)
        {
            if (Input.GetMouseButton(0) && _isAFruitInHand)
            {
               IncreaseForce();
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
                ProjectFruit();
            }
        }
        else
        {
            /*Debug.Log("PlayerController: `fruitRb` is null.");*/
            _isAFruitInHand = false;
        }
    }
    private void SpawnFruit(int _fruitIndex)
    {
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
    private void IncreaseForce()
    {
        if (_pullForce < _maxForce)
        {
            _pullForce += _forceIncrease * Time.deltaTime;
            _forceBar.fillAmount = Mathf.Clamp01((_pullForce-_minForce) / (_maxForce-_minForce));

            float forcePercentage = Mathf.Round((_pullForce - _minForce) / (_maxForce - _minForce) * 100);
            _forceText.text = "Force: " + forcePercentage.ToString() + " %";
        }
        if (_pullForce >= _maxForce)
        {
            /*Debug.Log("PlayerController: IncreaseForce: pull force max: " + _pullForce);*/
            _forceBarShiner.gameObject.SetActive(true);
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
    private void ProjectFruit()
    {
        //play a throwing sound effect
        gameObject.GetComponent<AudioSource>().Play();

        //apply the force to the fruit
        _fruitRb.AddForce(_pullForce * (_fruitSpawned.transform.position - _playerHand.transform.position), ForceMode.Impulse);

        _isAFruitInHand = false;

        //reset force
        _pullForce = _minForce;
        _forceBar.fillAmount = 0f;
        _forceBarShiner.gameObject.SetActive(false);
        _forceText.text = "Force: 0%";

        //reset angle
        _projectionAngleX = 0.0f;
        _projectionAngleY = 0.0f;
        //deactive directional indicator
        _directionIndicator.SetActive(false);
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

            if (!_isFruitSwallowed)
            {
                foreach (RaycastHit hit in hits)
                {
                    GameObject _hitObject = hit.collider.gameObject;

                    // Check if the hit object has a Fruit component
                    if (_hitObject.GetComponent<Fruit>() != null)
                    {
                        /*Debug.Log("PlayerController: Raycast hit fruit: " + _hitObject.name);*/
                        Debug.Log("PlayerController: Fruit Vacuum");

                        if(_hitObject.CompareTag("Watermelon")){
                            _watermelonEnjoyed.Play();
                            Destroy( _hitObject );
                            DataManager.Instance.CurrentScore += 100;
                        }
                        else
                        {
                            _fruitSwallowed = _hitObject;
                            _fruitSwallowed.SetActive(false); // Hide the fruit
                            _isFruitSwallowed = true;
                            _fruitVacuumSE.Play();
                        }
                        break; // Exit loop once the fruit is found
                    }
                }
            }
            else//already swallowed a fruit
            {
                foreach (RaycastHit hit in hits)
                {
                    GameObject _hitObject = hit.collider.gameObject;

                    // Check if the hit object has a Fruit component
                    if (_hitObject.GetComponent<Fruit>() != null)
                    {
                        Debug.Log("PlayerController: Fruit Blast at another fruit");
                        if (_fruitSwallowed != null)
                        {
                            _fruitSwallowed.transform.position = _hitObject.transform.position;
                            _fruitSwallowed.SetActive(true);
                        }
                        _isFruitSwallowed = false;
                        break; // Exit loop once the fruit is found

                    }
                    //blast towards the container wall (the last hit) if no fruits in the direction
                    Debug.Log("PlayerController: Fruit Blast at cursor");
                    if (_fruitSwallowed != null && _hitObject.CompareTag("Container"))
                    {
                        _fruitSwallowed.transform.position = hit.transform.position - ray.direction;
                        _fruitSwallowed.SetActive(true);
                        _fruitSwallowed.GetComponent<Rigidbody>().AddForce(ray.direction * _blastForce);
                    }
                    _fruitBlastSE.Play();
                    _isFruitSwallowed = false;
                }


                Debug.DrawRay(ray.origin, ray.direction * _raycastLength, Color.red, 1f); // Visualize the ray in editor
}

        }

        
    }

    private void DashModeIn()
    {
        if (GameManager.Instance.IsGameOverCountingDown)
        {
            GameManager.Instance.IsGameOverCountingDown = false;
        }

        Debug.Log("Entering Dimensional Dash mode...");
        StartCoroutine(ShowAndHideTransmissionCanvas());

        StartCoroutine(AudioManager.Instance.PlayDashModeBGM());

        GameManager.Instance.IsDashMode = true;
        _mainCamera.transform.position += _cameraForward;
        _mainGameUI.SetCursorNormal();
    }
    public void DashModeOut()
    {
        StartCoroutine(ShowAndHideTransmissionCanvas());

        StartCoroutine(AudioManager.Instance.PlayNormalBGM());

        _mainGameUI.HideCustomCursor();
        _mainCamera.transform.position = _cameraPosition.transform.position;
        if(_isFruitSwallowed && _fruitSwallowed != null)
        {
            _fruitSwallowed.SetActive(true);
            _isFruitSwallowed = false;
        }
    }

    private IEnumerator ShowAndHideTransmissionCanvas()
    {
        if(_wormholeMoment!=null)
        {
            Debug.Log("PlayerController: show transmission canvas");

            _wormholeMoment.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _wormholeMoment.SetActive(false);
        }
        
    }
}
