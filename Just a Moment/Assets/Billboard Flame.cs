using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFlame : MonoBehaviour
{

    private Camera m_camera;
    private bool hasfound = false;
    
    private void Awake()
    {
        Invoke("findcam", 3f);
    }

    private void Update()
    {
        if (hasfound) {
            Vector3 camposition = new Vector3(m_camera.transform.position.x, transform.position.y, m_camera.transform.position.z);
        transform.LookAt(camposition, Vector3.up);
        }
    }
    private void findcam()
    {
        m_camera = Camera.main;
        if (m_camera != null) { hasfound = true; }
    }
}
