using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {


    //  public GameObject player;       //Public variable to store a reference to the player game object

    public float smoothSpeed = 0.1f;
    Vector3 vec = Vector3.one;
    Vector3 vel = Vector3.one;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - GameManagerScript.instance.currentPos;// .transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
      //  transform.position =                      GameManagerScript.instance.currentPos + offset;

        vec = Vector3.SmoothDamp(transform.position, GameManagerScript.instance.currentPos + offset, ref vel, smoothSpeed);
        transform.position = vec;
    }
}
