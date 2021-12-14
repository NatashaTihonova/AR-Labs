using UnityEngine;

public class FellCheck : MonoBehaviour
{
    public bool IsFell;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsFell && other.tag == "Floor")
        {
            IsFell = true;
        }
    }
}
