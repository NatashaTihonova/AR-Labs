using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public static GPS Instance { set; get; }
    public float latitude = 47.25717f;
    public float longitude = 39.63843f;
    public Text info;

    public GameObject prefab;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    public void UpdateLocation()
    {
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User is not enable GPS");
            info.text = "User is not enable GPS";
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing &&
                maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
            info.text = "Loading. Please, wait...";
        }

        if (maxWait <= 0)
        {
            Debug.Log("Timed out");
            info.text = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device loacation");
            info.text = "Unable to determine device loacation";
            yield break;
        }

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        info.text = "Current location\nLat: " + latitude + "\nLon: " + longitude;
        Input.location.Stop();
        yield break;
    }

    public void CreateObject()
    {
        /*float xLocal = Mathf.Sin(longitude * Mathf.Deg2Rad) * latitude * Mathf.Deg2Rad;
        float zLocal = Mathf.Cos(longitude * Mathf.Deg2Rad) * latitude * Mathf.Deg2Rad;

        info.text = latitude + " " + longitude + "\n" + xLocal + " " + zLocal;
        info.text = "Object created";
        var point = Instantiate(prefab);
        point.transform.position = new Vector3(xLocal, -1, zLocal);//*/


        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        info.text = "Object created";
        var point = Instantiate(prefab);
        point.transform.position = ray.GetPoint(10);
    }
}
