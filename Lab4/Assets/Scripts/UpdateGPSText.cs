using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour
{
    public Text coordinates;

    // Update is called once per frame
    void Update()
    {
        //coordinates.text = "Current location\nLat: " + GPS.Instance.latitude + "\nLon: " + GPS.Instance.longitude;
    }
}
