using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTeleport : MonoBehaviour
{
    public GameObject menu;
    public GameObject anchor;
    public float OffSet = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TeleportMenu()
    {
        menu.transform.position = anchor.transform.position + anchor.transform.forward * OffSet;
        menu.transform.rotation = anchor.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
