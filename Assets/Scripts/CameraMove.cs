using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

  // Use this for initialization
  void Start () {
    
  }
  
  // Update is called once per frame
  void Update () {
    var inputX = Input.GetAxis("Horizontal");
    var inputZ = Input.GetAxis("Vertical");

    if (Camera.current != null) {
      Camera.current.transform.Translate(new Vector3(inputX, 0f, inputZ));
    }
  }
}
