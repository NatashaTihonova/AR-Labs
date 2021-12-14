using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System;
public class GeoTagManager : MonoBehaviour
{
    public Text statusText;
    public InputField geoTagName;
    public CanvasGroup createGeoTagCanvas;
    public GameObject displayPrefab;
    private bool isCreating = false;
    private List<GeoTag> geoTags = new List<GeoTag>();
    private LocationInfo currentLocation;

    public void AddGeoTag()
    {
        createGeoTagCanvas.alpha = 1;
        createGeoTagCanvas.blocksRaycasts = true;
    }

    public void CreateNewGeoTag()
    {
        if (!isCreating)
            StartCoroutine(FetchLocationData());
    }

    private IEnumerator FetchLocationData()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            statusText.text = "! Input.location.isEnabledByUser";
            yield break;
        }
        isCreating = true;
        // Start service before querying location
        Input.location.Start();
        statusText.text = "Fetching Location..";
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
            statusText.text = "Fetching Location " + maxWait;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            statusText.text = "Location Timed out";
            yield break;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            statusText.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            //Create GeoTag
            GeoTag geoTag = new GeoTag();

            geoTag.latitude = Input.location.lastData.latitude;
            geoTag.longitude = Input.location.lastData.longitude;
            geoTag.name = geoTagName.text;

            geoTags.Add(geoTag);

            //Create tag display
            statusText.text = "Create tag display";
            var obj = Instantiate(displayPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 1), Quaternion.identity);
            obj.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            obj.GetComponent<GeoTagDisplay>().Initialize(geoTag);
        }

        createGeoTagCanvas.alpha = 0;
        createGeoTagCanvas.blocksRaycasts = false;
        Input.location.Stop();
        isCreating = false;
    }
}