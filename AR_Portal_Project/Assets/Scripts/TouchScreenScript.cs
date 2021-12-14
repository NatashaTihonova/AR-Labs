using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TouchScreenScript : MonoBehaviour
{
    public GameObject flask;
    //public DrivingSurfaceManager DrivingSurfaceManager;

    public ARPlane CurrentPlane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (WasTapped())
        {
            var point = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(5);
            var obj = Instantiate(flask).GetComponent<Rigidbody>();
            obj.position = point;/*/

            var tmp = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = new List<ARRaycastHit>();
            DrivingSurfaceManager.RaycastManager.Raycast(tmp, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);

            CurrentPlane = null;
            ARRaycastHit? hit = null;
            if (hits.Count > 0)
            {
                // If you don't have a locked plane already...
                var lockedPlane = DrivingSurfaceManager.LockedPlane;
                hit = lockedPlane == null
                    // ... use the first hit in `hits`.
                    ? hits[0]
                    // Otherwise use the locked plane, if it's there.
                    : hits.SingleOrDefault(x => x.trackableId == lockedPlane.trackableId);
            }

            if (hit.HasValue)
            {
                CurrentPlane = DrivingSurfaceManager.PlaneManager.GetPlane(hit.Value.trackableId);
                // Move this reticle to the location of the hit.
                //transform.position = hit.Value.pose.position;
                var obj = Instantiate(flask).GetComponent<Rigidbody>();
                obj.position = hit.Value.pose.position + Vector3.up * 5;
            }//*/
        }
        
    }
    private bool WasTapped()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        if (Input.touchCount == 0)
        {
            return false;
        }

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
        {
            return false;
        }

        return true;
    }
}
