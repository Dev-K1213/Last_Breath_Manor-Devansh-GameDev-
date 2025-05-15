using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; 
    public bool flashlightIsOn { get; private set; }

    public bool canToggle = true;

    
    void Start()
    {
        lightGO.SetActive(true); // start with flashlight on
        flashlightIsOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canToggle && Input.GetKeyDown(KeyCode.X))
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        flashlightIsOn = !flashlightIsOn;
        lightGO.SetActive(flashlightIsOn);
    }

    public void SetFlashlightState(bool state)
    {
        flashlightIsOn = state;
        lightGO.SetActive(state);
    }
}
