using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeoTagDisplay: MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI location;
    public void Initialize(GeoTag geoTag)
    {
        this.title.text = geoTag.name;
        this.location.text = geoTag.latitude.ToString() + ", " + geoTag.longitude;
    }
}