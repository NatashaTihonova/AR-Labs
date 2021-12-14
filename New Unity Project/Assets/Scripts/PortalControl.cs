using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PortalControl : MonoBehaviour
{
    private ARRaycastManager ARRaycastManagerScript;

    public GameObject Portal;
    private GameObject ARPortal;
    public GameObject Character;

    private Animation OpenPortalAnimation;
    private Animation ClosePortalAnimation;
    private Animation CharacterAppearanceAnimation;

    public bool ActivePortal = false;
    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPortal();
    }

    void SetPortal()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        Touch touch = Input.GetTouch(0);

        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0 && Input.touchCount > 0 && !ActivePortal)
        {
            ARPortal = Instantiate(Portal, hits[0].pose.position, Portal.transform.rotation);
            ActivePortal = true;
            OpenPortalAnimation = ARPortal.GetComponent<Animation>();
            OpenPortalAnimation.Play("Portal Appearance");

            StartCoroutine(routine: CharacterAppearance());

        }
    }

    private IEnumerator CharacterAppearance()
    {
        yield return new WaitForSeconds(2);
        Instantiate(Character, ARPortal.transform.position, Character.transform.rotation);
        CharacterAppearanceAnimation = Character.GetComponent<Animation>();
        CharacterAppearanceAnimation.Play("Character Animation");

        yield return new WaitForSeconds(2);

        ClosePortalAnimation = ARPortal.GetComponent<Animation>();
        ClosePortalAnimation.Play("Close Portal");
    }
}
