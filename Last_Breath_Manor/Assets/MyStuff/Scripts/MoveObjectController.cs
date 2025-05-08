using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveObjectController : MonoBehaviour
{
    public GameObject Flashlight;

    private Animator anim;
    private Camera fpsCam;

    private int pickupLayerMask;
    private int furnitureLayerMask;

    private GameObject player;

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

        if (fpsCam == null)
            Debug.LogError("A camera tagged 'MainCamera' is missing.");

        anim = GetComponent<Animator>();
        anim.enabled = false;

        pickupLayerMask = 1 << LayerMask.NameToLayer("PickupLayer");
        furnitureLayerMask = 1 << LayerMask.NameToLayer("FurnitureLayer");


        setupGui();
        inventory = InventoryManager.Instance;
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
    if (!playerEntered) return;

    Vector3 rayOrigin = fpsCam.transform.position;
    RaycastHit pickupHit, furnitureHit;

    bool foundPickup = Physics.Raycast(rayOrigin, fpsCam.transform.forward, out pickupHit, 3f, pickupLayerMask);
    bool foundFurniture = Physics.Raycast(rayOrigin, fpsCam.transform.forward, out furnitureHit, 3f, furnitureLayerMask);

    GameObject target = null;

    // === PRIORITIZE PICKUP ===
    if (foundPickup)
    {
        target = pickupHit.collider.gameObject;

        if (target.CompareTag("Key") || target.CompareTag("Medkit") || target.CompareTag("Bottle") || target.CompareTag("Computer"))
        {
            showPickupMsg = true;
            pickupMsg = "Press E to pick up";

            if (Input.GetKeyDown(KeyCode.E))
            {
                string tag = target.tag;
                inventory.CollectItem(tag);
                Destroy(target);

            }
        }
    }
    else
    {
        showPickupMsg = false;
    }

    // === Check Furniture Only if No Pickup ===
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

            if (Input.GetButtonDown("Fire1"))
            {
                anim.enabled = true;
                anim.SetBool(animBoolNameNum, !isOpen);
                msg = getGuiMsg(!isOpen);
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
 


    private IEnumerator SpawnRedFloppy()
    {
        yield return new WaitForSeconds(20f);
        GameObject redFloppy = GameObject.FindWithTag("FloppyRed");
        if (redFloppy != null)
            redFloppy.SetActive(true);
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
    }
}
