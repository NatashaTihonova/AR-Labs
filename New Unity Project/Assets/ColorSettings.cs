using UnityEngine;

public class ColorSettings : MonoBehaviour
{
  [SerializeField] private Renderer[] _renderers;

  public void ChangeColor()
  {
    var randColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    foreach (var renderer in _renderers)
    {
      renderer.material.color = randColor;
    }
  }
}