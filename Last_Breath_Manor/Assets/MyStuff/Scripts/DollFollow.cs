using System.Collections;
using UnityEngine;

public class DollFollow : MonoBehaviour
{

    private AudioSource jumpScareAudio;
    [SerializeField] private GameObject Doll;
    private GameObject camera;
    public float timedEventInterval = 30.0f;
    public float dollDisplayDuration = 5.0f;
    private Rigidbody dollRigidbody;
    private float timer;
    private bool dollIsVisible = false;
    private bool timerRunning = true;

    private Coroutine timedCoroutine = null;

   public FlashlightToggle toggleFlash; 
   private int doorTriggerCount = 0;

    void Start()
    {
        
        jumpScareAudio = GetComponent<AudioSource>();

        camera = Camera.main.gameObject;

        if (camera == null)
            Debug.LogError("Main camera not found");

        if (Doll == null)
            Debug.LogError("Doll not found");

        // Hide the doll offscreen at start
        if (Doll != null)
            Doll.transform.position = new Vector3(0, -1000, 0);

        timer = timedEventInterval;

        dollRigidbody = Doll.GetComponent<Rigidbody>();

        //timedCoroutine = StartCoroutine(TimedDollCoroutine()); // Start the timed appearance coroutine

        FreezeDollRigidbody();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dollIsVisible)
        {
            doorTriggerCount++;
            
        
        if (doorTriggerCount >= 10)
        {
            StartCoroutine(TriggerDollCoroutine());
            
            doorTriggerCount = 0;

        }

        }
    }

    private IEnumerator TimedDollCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer); // Wait for the interval
            StartCoroutine(ShowDollTemporarily());
        }
    }

    private IEnumerator TriggerDollCoroutine()
    {
        if (dollIsVisible)
            yield break; // If the doll is already visible, do nothing

        // Pause the timed event coroutine when the door is triggered
        if (timedCoroutine != null)
        {
            StopCoroutine(timedCoroutine);
            timerRunning = false;
        }

        // Show the doll temporarily when the player triggers the door
        yield return StartCoroutine(ShowDollTemporarily());

        // After the door-triggered doll is done, resume the timed event coroutine
        timedCoroutine = StartCoroutine(TimedDollCoroutine());
        timerRunning = true;
    }

private IEnumerator ShowDollTemporarily()
{
    dollIsVisible = true;

    if (toggleFlash == null)
    {
        toggleFlash = FindObjectOfType<FlashlightToggle>();
    }

    // 👉 Turn flashlight OFF and wait in darkness
    bool wasFlashlightOn = toggleFlash.flashlightIsOn;
    

    if (wasFlashlightOn)
    {
        toggleFlash.SetFlashlightState(false);
        toggleFlash.canToggle = false;
        FindObjectOfType<CharacterMovement>().canMove = false;
        

    }

    yield return new WaitForSeconds(0.7f); // dramatic pause...

    // 👉 Start flickering flashlight
    if (flashingCoroutine == null)
    {
        flashingCoroutine = StartCoroutine(FlashlightStrobe());
    }

    // 👉 Show doll in front of player
    Vector3 inFront = camera.transform.position + camera.transform.forward * 1.2f;
    inFront.y = 0.121f;
    Doll.transform.position = inFront;

    Vector3 dollRotation = Doll.transform.rotation.eulerAngles;
    dollRotation.y = camera.transform.eulerAngles.y + 180;
    Doll.transform.rotation = Quaternion.Euler(dollRotation);

    FindObjectOfType<CharacterMovement>().canMove = false;

    // Play jumpscare audio
    if (jumpScareAudio != null && !jumpScareAudio.isPlaying)
    {
        jumpScareAudio.Play();
    }

    // Wait while doll is visible
    yield return new WaitForSeconds(dollDisplayDuration);

    // 👉 Hide doll again
    Doll.transform.position = new Vector3(0, -1000, 0);

    if (jumpScareAudio != null && jumpScareAudio.isPlaying)
    {
        jumpScareAudio.Stop();
    }

    // 👉 Stop flicker and restore flashlight to original state
    if (flashingCoroutine != null)
    {
        StopCoroutine(flashingCoroutine);
        flashingCoroutine = null;
    }

    
    toggleFlash.canToggle = true;
    FindObjectOfType<CharacterMovement>().canMove = true;


    if (dollRigidbody != null)
    {
        FreezeDollRigidbody();
    }

    dollIsVisible = false;
    timer = timedEventInterval;
}


private Coroutine flashingCoroutine;
private IEnumerator FlashlightStrobe(float flashSpeed = 0.07f)
{
    while (true)
    {
        toggleFlash.lightGO.SetActive(!toggleFlash.lightGO.activeSelf);
        yield return new WaitForSeconds(flashSpeed);
    }
}



    private void FreezeDollRigidbody()
    {
        if (dollRigidbody != null)
        {
            
            dollRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            dollRigidbody.useGravity = false;
        }
    }
}
