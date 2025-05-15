using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // For UI components
using TMPro;



public class MoveObjectController : MonoBehaviour
{
    public GameObject Flashlight;
    private Animator anim;
    private Camera fpsCam;
    public GameObject potionKey; 
    private int pickupLayerMask;
    private int furnitureLayerMask;
    private GameObject player;
    private string temporaryMsg = "";
    private float msgTimer = 0f;
    private bool riddleBookHasAppeared = false;

    public GameObject riddleBook;
    private float riddleBookTimer = 0f;
    private bool riddleBookPending = false;

    private float msgDuration = 3f; 
    public GameObject windowCover;
    public FlashlightToggle flashlightToggle;
    private bool lastFlashlightState;

    public GameObject moonBook;
    public GameObject doorKey2;


    private const string animBoolName = "isOpen_Obj_";

    private bool playerEntered;
    private bool showInteractMsg;
    private bool showPickupMsg;
    private string msg;
    private string pickupMsg;

    private GUIStyle guiStyle;
    private GUIStyle pickupStyle;
    private InventoryManager inventory;



void Start()
{
    player = GameObject.FindGameObjectWithTag("Player");
    Flashlight = GameObject.FindGameObjectWithTag("Flashlight");
    fpsCam = Camera.main;

    anim = GetComponent<Animator>();
    anim.enabled = false;

    pickupLayerMask = 1 << LayerMask.NameToLayer("PickupLayer");
    furnitureLayerMask = 1 << LayerMask.NameToLayer("FurnitureLayer");

    setupGui();
    inventory = InventoryManager.Instance;


    if (flashlightToggle != null)
    {
        lastFlashlightState = flashlightToggle.flashlightIsOn;
        UpdateLightDependentObjects(lastFlashlightState);
    }


}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            playerEntered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerEntered = false;
            showInteractMsg = false;
            showPickupMsg = false;
        }
    }

   void Update()
{

if (msgTimer > 0)
    {
        msgTimer -= Time.deltaTime;
        if (msgTimer <= 0)
        {
            temporaryMsg = "";
        }
    }
        if (riddleBookPending)
        {
            riddleBookTimer -= Time.deltaTime;
            if (riddleBookTimer <= 0f)
            {
                riddleBookPending = false;

                if (riddleBook != null)
                {
                    riddleBook.SetActive(true);
                    Debug.Log("RiddleBook appeared.");
                    riddleBookHasAppeared = true;
                    flashlightToggle.SetFlashlightState(false);
                }


                if (windowCover != null)
                {
                    windowCover.tag = "WindowCover";  
                    Debug.Log("WindowCover tag set.");
                }

                if (moonBook != null)
                {
                    moonBook.SetActive(true);
                    Debug.Log("MoonBook appeared.");
                }

                if (doorKey2 != null)
                {
                    doorKey2.SetActive(false);
                    Debug.Log("DoorKey appeared.");
                }

                
            }

        }

        if (flashlightToggle != null)
        {
            bool currentFlashlightState = flashlightToggle.flashlightIsOn;
            if (currentFlashlightState != lastFlashlightState)
            {
                UpdateLightDependentObjects(currentFlashlightState);
                lastFlashlightState = currentFlashlightState;
            }
        }

    if (!playerEntered) return;

    Vector3 rayOrigin = fpsCam.transform.position;
    RaycastHit pickupHit, furnitureHit;

    bool foundPickup = Physics.Raycast(rayOrigin, fpsCam.transform.forward, out pickupHit, 3f, pickupLayerMask);
    bool foundFurniture = Physics.Raycast(rayOrigin, fpsCam.transform.forward, out furnitureHit, 3f, furnitureLayerMask);

    GameObject target = null;

    //PRIORITIZE PICKUP
    if (foundPickup)
{
    GameObject pickupTarget = pickupHit.collider.gameObject;

    // Check if furniture is blocking the pickup
    if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out RaycastHit blockHit, pickupHit.distance, furnitureLayerMask))
    {
        // Something in the way
        showPickupMsg = false;
    }
    else
    {
        if (pickupTarget.CompareTag("Key") || pickupTarget.CompareTag("Medkit") || pickupTarget.CompareTag("Bottle") || pickupTarget.CompareTag("InitialBook") 
        || pickupTarget.CompareTag("InitialBookKey") || pickupTarget.CompareTag("Potion") || pickupTarget.CompareTag("MoonBook") || pickupTarget.CompareTag("WindowCover") 
        || pickupTarget.CompareTag("DoorKey1") || pickupTarget.CompareTag("DoorKey2") || pickupTarget.CompareTag("PotionKey")|| pickupTarget.CompareTag("RiddleBook")
)
        {
            showPickupMsg = true;
            if (pickupTarget.CompareTag("Potion"))
            {
                pickupMsg = "Press E to Drink potion";
            }
            else if (pickupTarget.CompareTag("InitialBook")){
                pickupMsg = "Press E to Read Book";
            }
            else if (pickupTarget.CompareTag("Key")){
                pickupMsg = "Press E to pick up Silver Key";
            }
            else if (pickupTarget.CompareTag("Medkit")){
                pickupMsg = "Press E to pick up MedKit";
            }
            else if (pickupTarget.CompareTag("Bottle")){
                pickupMsg = "Press E to Drink Water";
            }
            else if (pickupTarget.CompareTag("InitialBookKey")){
                pickupMsg = "Press E to pick up Green Key";
            }
            else if (pickupTarget.CompareTag("PotionKey")){
                pickupMsg = "Press E to pick up Red Key";
            }
            else if (pickupTarget.CompareTag("WindowCover")){
                pickupMsg = "Press E to Remove Window Cover";
            }
            else if (pickupTarget.CompareTag("DoorKey1") || pickupTarget.CompareTag("DoorKey2")){
                pickupMsg = "Press E to pick up Door Key";
            }
            else if (pickupTarget.CompareTag("RiddleBook"))
            {
                pickupMsg = "\"The key for light is to embrace the dark\"";
            }

            else
            {
                pickupMsg = "Press E to pick up";
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                string tag = pickupTarget.tag;
                inventory.CollectItem(tag);
                    if (tag == "DoorKey2")
                    {
                        Destroy(pickupTarget); //destroy dkey2
                        Debug.Log("DoorKey2 picked up and destroyed.");
                    }
                    else
                    {
                        pickupTarget.SetActive(false);
                    }

                if (tag == "InitialBook" && potionKey != null)
                {
                    potionKey.SetActive(true);
                    Debug.Log("PotionKey activated!");

                    ShowTemporaryMessage("A key has appeared somewhere", msgDuration);

                }

                if (tag == "Potion")
                {
                    ShowTemporaryMessage("Something will appear in a bit", msgDuration);

                    riddleBookPending = true;
                    riddleBookTimer = 20f; // 20 sec wait for book to appear
                }


            }
        }
    }
}
else
{
    showPickupMsg = false;
}



    //Check Furniture Only if No Pickup
    if (!foundPickup && foundFurniture)
    {
        GameObject furnitureTarget = furnitureHit.collider.gameObject;
        MoveableObject moveableObject;
        if (isEqualToParent(furnitureHit.collider, out moveableObject))
        {
            showInteractMsg = true;
            string animBoolNameNum = animBoolName + moveableObject.objectNumber.ToString();
            bool isOpen = anim.GetBool(animBoolNameNum);
            msg = getGuiMsg(isOpen);
            if (moveableObject.isLocked)
            {
                bool hasAllKeys = true;
                foreach (string keyTag in moveableObject.requiredKeyTags)
                {
                    if (!inventory.HasItem(keyTag))
                    {
                        hasAllKeys = false;
                        break;
                    }
                }

                if (hasAllKeys)
                {
                    msg = $"Press Left Click to Unlock";

                    if (Input.GetButtonDown("Fire1"))
                    {
                        moveableObject.Unlock();
                        
                        // Use all required keys
                        foreach (string keyTag in moveableObject.requiredKeyTags)
                        {
                            inventory.UseItem(keyTag);
                        }

                        anim.enabled = true;
                        anim.SetBool(animBoolNameNum, true);
                        msg = getGuiMsg(true);
                    }
                }
                else
                {
                    msg = string.IsNullOrEmpty(moveableObject.lockedMessage)
                    ? $"Locked. You need {moveableObject.requiredKeyTags.Count} keys."
                    : moveableObject.lockedMessage;

                }
            }

                    else
                    {
                        msg = getGuiMsg(isOpen);

                        if (Input.GetButtonDown("Fire1"))
                        {
                            anim.enabled = true;
                            anim.SetBool(animBoolNameNum, !isOpen);
                            msg = getGuiMsg(!isOpen);
                        }
                    }

        }
        else
        {
            showInteractMsg = false;
        }
    }
    else
    {
        showInteractMsg = false;
    }





}

private void UpdateLightDependentObjects(bool flashlightIsOn)
{
    if (!riddleBookHasAppeared) return; //Skip unless riddle book active

    if (moonBook != null)
        moonBook.SetActive(flashlightIsOn); //MoonBook active only if flashlight is on

    if (doorKey2 != null)
        doorKey2.SetActive(!flashlightIsOn); //DoorKey2 active only if flashlight is off
}


private void ShowTemporaryMessage(string message, float duration)
{
    temporaryMsg = message;
    msgTimer = duration;
}

    private bool isEqualToParent(Collider other, out MoveableObject draw)
    {
        draw = null;
        bool rtnVal = false;
        try
        {
            int maxWalk = 6;
            draw = other.GetComponent<MoveableObject>();
            GameObject currentGO = other.gameObject;

            for (int i = 0; i < maxWalk; i++)
            {
                if (currentGO.Equals(this.gameObject))
                {
                    rtnVal = true;
                    if (draw == null) draw = currentGO.GetComponentInParent<MoveableObject>();
                    break;
                }
                if (currentGO.transform.parent != null)
                {
                    currentGO = currentGO.transform.parent.gameObject;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        return rtnVal;
    }

    private void setupGui()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 16;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.white;

        pickupStyle = new GUIStyle(guiStyle);
        pickupStyle.fontSize = 18;
    }

    private string getGuiMsg(bool isOpen)
    {
        return isOpen ? "Left Click to Close" : "Left Click to Open";
    }

    void OnGUI()
    {
        if (showPickupMsg)
        {
            GUI.Label(new Rect(50, Screen.height - 100, 300, 40), pickupMsg, pickupStyle);
        }

        if (showInteractMsg)
        {
            GUI.Label(new Rect(50, Screen.height - 60, 300, 40), msg, guiStyle);
        }

        if (!string.IsNullOrEmpty(temporaryMsg))
        {
            GUI.Label(new Rect(50, Screen.height - 140, 400, 40), temporaryMsg, pickupStyle);
        }

    }
}
