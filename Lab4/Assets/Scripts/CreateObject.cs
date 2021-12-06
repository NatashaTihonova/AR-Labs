using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateObject : MonoBehaviour
{
    public GameObject createObjCanvas;
    public GameObject startingCanvas;
    public GameObject inputs;
    public GameObject prefab;
    public Text info;
    public InputField latitude;
    public InputField longitude;
    public Text distance;

    private void Start()
    {
        ToBaseCanvas();
    }

    public void ToCreateCanvas()
    {
//        latitude.ActivateInputField();
  //      longitude.ActivateInputField();
        createObjCanvas.gameObject.SetActive(true);
        inputs.SetActive(true);
        startingCanvas.gameObject.SetActive(false);

        CalculateDist();
    }

    public void ToBaseCanvas()
    {
        createObjCanvas.gameObject.SetActive(false);
        inputs.SetActive(false);
        startingCanvas.gameObject.SetActive(true);
    }

    public void CreateObjectFinal()
    {
        float lat = float.Parse(latitude.text), lon = float.Parse(longitude.text);
        float xLocal = Mathf.Sin(lon * Mathf.Deg2Rad) * lat * Mathf.Deg2Rad;
        float zLocal = Mathf.Cos(lon * Mathf.Deg2Rad) * lat * Mathf.Deg2Rad;

        var point = Instantiate(prefab);
        point.transform.position = new Vector3(xLocal, -1, zLocal);//*/

        /*
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        info.text = "Object created";
        var point = Instantiate(prefab);
        point.transform.position = ray.GetPoint(10);*/

        info.text = "Object created";
        ToBaseCanvas();
    }

    public void CalculateDist()
    {
        float lat1 = GPS.Instance.latitude * Mathf.Deg2Rad, lon1 = GPS.Instance.longitude * Mathf.Deg2Rad,
              lat2 = float.Parse(latitude.text) * Mathf.Deg2Rad, lon2 = float.Parse(longitude.text) * Mathf.Deg2Rad;
        var r = 6371000;

        var dist = 2 * r * Mathf.Sqrt(Mathf.Pow(Mathf.Sin((lat2 - lat1) / 2f), 2) + 
                                      Mathf.Cos(lat1) * Mathf.Cos(lat2) * Mathf.Pow(Mathf.Sin((lon2 - lon1) / 2), 2));
        distance.text = "Distance between points are " + dist + " meters";
    }

}
