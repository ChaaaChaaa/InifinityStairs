using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            this.transform.position = target.transform.position + new Vector3(0,2f,-10f);
        }
        
    }
}
