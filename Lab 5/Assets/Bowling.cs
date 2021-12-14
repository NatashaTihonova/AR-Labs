using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bowling : MonoBehaviour
{
  public Text scoreText;
  public List<FellCheck> pins = new List<FellCheck>();
  public int score;

  private void Update()
  {
      for (var i = 0; i < pins.Count; i++)
      {
          if (pins[i] != null && pins[i].IsFell)
          {
              score += 1;
              pins[i] = null;
          }
      }
      
      scoreText.text = score.ToString();
  }
}