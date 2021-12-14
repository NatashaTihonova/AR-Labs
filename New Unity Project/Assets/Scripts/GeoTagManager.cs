using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeoTagManager : MonoBehaviour
{
  [SerializeField] private Toggle rotationToggle;
  [SerializeField] private Toggle positionToggle;
  [SerializeField] private Toggle scaleToggle;
  [SerializeField] private Toggle colorToggle;
  [SerializeField] private GameObject togglesPanel;

  public CanvasGroup createGeoTagCanvas;
  public GameObject onePrefab;
  public GameObject twoPrefab;
  public GameObject threePrefab;

  private bool _isCreating;
  private LocationInfo _currentLocation;
  private GameObject currentGameObject;

  private bool isRotateCnahging;
  private bool isPositionCnahging;
  private bool isScaleCnahging;
  private bool isColorCnahging;

  private readonly float _rotateSpeed = .05f;
  private readonly float _translateSpeed = .001f;

  private float initialFingersDistance;
  private Vector3 initialScale;
  
  private void OnEnable()
  {
    rotationToggle.onValueChanged.AddListener(OnRotationToggleValueChanged);
    positionToggle.onValueChanged.AddListener(OnPositionToggleValueChanged);
    scaleToggle.onValueChanged.AddListener(OnScaleToggleValueChanged);
    colorToggle.onValueChanged.AddListener(OnColorToggleValueChanged);
  }

  private void OnRotationToggleValueChanged(bool active) => 
    isRotateCnahging = active;

  private void OnPositionToggleValueChanged(bool active) => 
    isPositionCnahging = active;

  private void OnScaleToggleValueChanged(bool active) => 
    isScaleCnahging = active;

  private void OnColorToggleValueChanged(bool active) => 
    isColorCnahging = active;

  public void AddGeoTag()
  {
    isRotateCnahging = isPositionCnahging = isScaleCnahging = false;
    rotationToggle.isOn = false;
    positionToggle.isOn = false;
    scaleToggle.isOn = false;
    colorToggle.isOn = false;
    
    togglesPanel.SetActive(false);
    
    createGeoTagCanvas.alpha = 1;
    createGeoTagCanvas.blocksRaycasts = true;
  }

  public void CreateNewGeoTag(int prefabNum)
  {
    if (!_isCreating)
      StartCoroutine(FetchLocationData(prefabNum));
  }

  private IEnumerator FetchLocationData(int prefabNum)
  {
    // First, check if user has location service enabled
    if (!Input.location.isEnabledByUser)
    {
      //statusText.text = "! Input.location.isEnabledByUser";
      yield break;
    }

    _isCreating = true;

    // Start service before querying location
    Input.location.Start();
    //statusText.text = "Fetching Location..";

    // Wait until service initializes
    int maxWait = 20;
    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    {
      yield return new WaitForSeconds(1);
      maxWait--;
    }

    // Service didn't initialize in 20 seconds
    if (maxWait < 1)
    {
      //statusText.text = "Location Timed out";
      yield break;
    }

    // Connection has failed
    if (Input.location.status == LocationServiceStatus.Failed)
    {
      //statusText.text = "Unable to determine device location";
      yield break;
    }

    //Create object
    switch (prefabNum)
    {
      case 1:
        GameObject obj1 = Instantiate(onePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1),
          Quaternion.identity);
        obj1.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        currentGameObject = obj1;
        break;
      case 2:
        GameObject obj2 = Instantiate(twoPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1),
          Quaternion.identity);
        obj2.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        currentGameObject = obj2;
        break;
      case 3:
        GameObject obj3 = Instantiate(threePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1),
          Quaternion.identity);
        obj3.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        currentGameObject = obj3;
        break;
    }
    positionToggle.isOn = true;
    
    togglesPanel.SetActive(true);

    createGeoTagCanvas.alpha = 0;
    createGeoTagCanvas.blocksRaycasts = false;
    Input.location.Stop();
    _isCreating = false;
  }

  private void Update()
  {
    if (isRotateCnahging)
      ChangeRotation();
    else if (isPositionCnahging)
      ChangePosition();
    else if (isScaleCnahging)
      ChangeScale();
    else if (isColorCnahging)
      ChangeColor();
  }

  private static void ChangeColor()
  {
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
      Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
      RaycastHit raycastHit;
      if (Physics.Raycast(raycast, out raycastHit))
      {
        if (raycastHit.collider.CompareTag("Player"))
          raycastHit.collider.gameObject.GetComponent<ColorSettings>().ChangeColor();
      }
    }
  }

  private void ChangePosition()
  {
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    {
      Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
      if (currentGameObject)
        currentGameObject.transform.Translate(0, touchDeltaPosition.y * _translateSpeed, 0);
    }
  }

  private void ChangeRotation()
  {
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    {
      Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

      if (currentGameObject)
        currentGameObject.transform.Rotate(touchDeltaPosition.y * _rotateSpeed, touchDeltaPosition.x * _rotateSpeed,
          touchDeltaPosition.y * _rotateSpeed);
    }
  }

  private void ChangeScale()
  {
    int fingersOnScreen = 0;
    // If there are two touches on the device...
    foreach (Touch touch in Input.touches)
    {
      fingersOnScreen++;
 
      if (fingersOnScreen == 2)
      {
        //First set the initial distance between fingers so you can compare.
        if (touch.phase == TouchPhase.Began)
        {
          initialFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
          initialScale = currentGameObject.transform.localScale;
        }
        else
        {
          var currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
          var scaleFactor = currentFingersDistance / initialFingersDistance;
          currentGameObject.transform.localScale = initialScale * scaleFactor;
        }
      }
    }
  }
}