using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
  public float Xspeed = 200.0f;
  public float Yspeed = 200.0f;
  public int YminLimit = -80;
  public int YmaxLimit = 80;
  public float Dampening = 5.0f;



  private float ZoomAmount = 0; //With Positive and negative values
  private float MaxToClamp = 10;
  private float RotateSpeed = 10;
  private float RotateSpeedForButtonScroll = 3;

  private float xDeg = 0.0f;
  private float yDeg = 0.0f;
  private Quaternion currentRotation;
  private Quaternion desiredRotation;
  private Quaternion rotation;

  void Update()
  {
    // If right mouse? Orbit
    if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
    {
      xDeg += Input.GetAxis("Mouse X") * Xspeed * 0.01f;
      yDeg -= Input.GetAxis("Mouse Y") * Yspeed * 0.01f;

      //Orbit Angle
      //Clamp the vertical axis for the orbit
      yDeg = ClampAngle(yDeg, YminLimit, YmaxLimit);
      // set camera rotation 
      desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
      currentRotation = transform.rotation;

      //rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * dampening);
      rotation = desiredRotation;
      transform.rotation = rotation;
    }
    // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
    else if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
    {
      //grab the rotation of the camera so we can move in a psuedo local XY space
      transform.Translate(Vector3.right * -Input.GetAxis("Mouse X"));
      transform.Translate(Vector3.up * -Input.GetAxis("Mouse Y"));
    }

    //Mouse Scroll button
    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
    {
      if (Input.GetAxis("Mouse X") > 0f || Input.GetAxis("Mouse X") < 0f)
      {
        var translateWithButtonForX = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse X")));
        gameObject.transform.Translate(0, 0, translateWithButtonForX * RotateSpeedForButtonScroll * Mathf.Sign(Input.GetAxis("Mouse X")));
      }

      var translateWithButtonForY = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse Y")));
      gameObject.transform.Translate(0, 0, translateWithButtonForY * RotateSpeedForButtonScroll * Mathf.Sign(Input.GetAxis("Mouse Y")));
    }

    //Mouse ScrollWheel
    ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
    ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
    var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp - Mathf.Abs(ZoomAmount));
    gameObject.transform.Translate(0, 0, translate * RotateSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));


    //Vertical movement of the camera
    float _y = Mathf.Clamp(transform.position.y + Input.GetAxis("Vertical") * 10 * Time.deltaTime, 1, 500);
    transform.position = new Vector3(transform.position.x, _y, transform.position.z);
  }

  private static float ClampAngle(float angle, float min, float max)
  {
    if (angle < -360)
      angle += 360;
    if (angle > 360)
      angle -= 360;
    return Mathf.Clamp(angle, min, max);
  }
}

