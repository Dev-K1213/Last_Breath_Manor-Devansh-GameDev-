using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; // light GameObject to control
    public bool flashlightIsOn { get; private set; }

    public bool canToggle = true;

    // Use this for initialization
    void Start()
    {
        lightGO.SetActive(false); // start with flashlight off
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
