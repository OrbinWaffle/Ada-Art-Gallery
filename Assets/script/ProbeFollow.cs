using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        transform.position = new Vector3(camPos.x, -camPos.y, camPos.z);
    }
}
