using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Playables;


public class Raycast : MonoBehaviour
{

    public GameObject spawn_prefab;
    GameObject spawned_object;
    bool object_spawned;
    ARRaycastManager arrayman;
    ARPlaneManager arplneman;
    Vector2 First_touch;
    Vector2 Second_touch;
    float distance_current;
    float distance_previous;
    bool first_pinch = true;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Animator _anim;

    void Start()
    {
        object_spawned = false;
        arrayman = GetComponent<ARRaycastManager>();
        arplneman = GetComponent<ARPlaneManager>();
        
    }


    void Update()
    {
        if (Input.touchCount > 0 && !object_spawned)
        {
            if (arrayman.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                {
                    var hitpose = hits[0].pose;
                    spawned_object = Instantiate(spawn_prefab, hitpose.position, hitpose.rotation);

                    object_spawned = true;
                    arrayman.enabled = false;
                    //  arplneman.planePrefab.GetComponent<MeshRenderer>().enabled = false;
                }
                /*  else
                  {
                      spawned_object.transform.position = hitpose.position; // we can move the object because it was in a update loop
                  }*/
            }
        }
        if (Input.touchCount > 1 && object_spawned)
        {
            First_touch = Input.GetTouch(0).position;
            Second_touch = Input.GetTouch(1).position;
            distance_current = Second_touch.magnitude - First_touch.magnitude;
            if (first_pinch)
            {
                distance_previous = distance_current;
                first_pinch = false;
            }
            if (distance_current != distance_previous)
            {
                Vector3 scale_value = spawned_object.transform.localScale * (distance_current / distance_previous);
                spawned_object.transform.localScale = scale_value;
                distance_previous = distance_current;
            }
        }
        else
        {
            first_pinch = transform;
        }

    }
    public void StartAnim()
    {
        _anim = spawned_object.GetComponent<Animator>();
        _anim.Play("PlayCube");
    }


}
