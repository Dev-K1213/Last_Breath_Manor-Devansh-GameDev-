using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollFollow : MonoBehaviour
{
    private GameObject camera;
    private GameObject Doll;

    public float timer = 10.0f;

    void Start()
    {
        
        camera = Camera.main.gameObject; 
        
        Doll = GameObject.Find("Doll"); 
    }

    void Update()
    {
        timer -= Time.deltaTime;  

        if (timer < 0.0f)
        {
            Doll.SetActive(true); 
            

            if (camera != null)
            {
                camera.SetActive(true); 
            }

            // Set the doll's position and rotation to match the camera
            Doll.transform.position = new Vector3(camera.transform.position.x, 0.121f, camera.transform.position.z + 1.22300f);
            
            Vector3 dollRotation = Doll.transform.rotation.eulerAngles;
            dollRotation.y = camera.transform.rotation.eulerAngles.y + 180; 
            Doll.transform.rotation = Quaternion.Euler(dollRotation);
        }
        else
        {
            Doll.SetActive(false); // Deactivate the doll only, not the camera
        }
    }
}
