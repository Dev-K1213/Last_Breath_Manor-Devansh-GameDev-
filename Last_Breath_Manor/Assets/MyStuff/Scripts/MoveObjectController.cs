using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveObjectController : MonoBehaviour
{
    public float reachRange = 1.8f;
    public GameObject Flashlight;

    private Animator anim;
    private Camera fpsCam;
    private GameObject player;

    private const string animBoolName = "isOpen_Obj_";

    private bool playerEntered;
    private bool showInteractMsg;
    private bool showPickupMsg;
    private string msg;
    private string pickupMsg;

    private GUIStyle guiStyle;
    private GUIStyle pickupStyle;

    private int rayLayerMask;
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

        LayerMask iRayLM = LayerMask.NameToLayer("InteractRaycast");
        rayLayerMask = 1 << iRayLM.value;

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


        

        RaycastHit hit;

        

        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, reachRange, rayLayerMask))
        {
            GameObject target = hit.collider.gameObject;
            
            

            // Handle drawers/doors
            MoveableObject moveableObject = null;
            if (isEqualToParent(hit.collider, out moveableObject))
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

            Debug.Log("Hit object: " + target.name + " Tag: " + target.tag);

            // Handle item pickup
            if (target.CompareTag("FloppyYellow") || target.CompareTag("FloppyRed") || target.CompareTag("Key") ||
                target.CompareTag("Medkit") || target.CompareTag("Bottle"))
            {
                showPickupMsg = true;
                pickupMsg = "Press E to pick up";


                if (Input.GetKeyDown(KeyCode.E))
                {
                    string tag = target.tag;
                    inventory.CollectItem(tag);
                    Destroy(target);


                    if (tag == "FloppyYellow")
                        StartCoroutine(SpawnRedFloppy());
                }
            }
            else if (target.CompareTag("Computer") && inventory.HasItem("FloppyYellow"))
            {
                showPickupMsg = true;
                pickupMsg = "Press E to view timer";
                if (Input.GetKeyDown(KeyCode.E))
                {
                   
                    //Timer code
                }
            }
            else if (target.CompareTag("Computer") && inventory.HasItem("FloppyRed"))
            {
                showPickupMsg = true;
                pickupMsg = "Press E to destroy computer";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //destroy code
                }
            }
            else
            {
                showPickupMsg = false;
            }
        }
        else
        {
            showInteractMsg = false;
            showPickupMsg = false;
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
