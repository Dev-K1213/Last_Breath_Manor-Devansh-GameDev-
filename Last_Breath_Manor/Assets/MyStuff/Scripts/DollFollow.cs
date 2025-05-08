using System.Collections;
using UnityEngine;

public class DollFollow : MonoBehaviour
{

    private AudioSource jumpScareAudio;
    [SerializeField] private GameObject Doll;

    [SerializeField] private AudioClip terror;

    private GameObject camera;
    [SerializeField] private GameObject gameOverScreen;

    public float timedEventInterval;
    public float dollDisplayDuration;
    private Rigidbody dollRigidbody;
    private float timer;
    private bool dollIsVisible = false;
    private bool timerRunning = true;

    private Coroutine timedCoroutine = null;

   public FlashlightToggle toggleFlash; 
   private int doorTriggerCount = 0;

   RaycastHit light;

   public float rayrange = 5f;

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

        timedCoroutine = StartCoroutine(TimedDollCoroutine());

        FreezeDollRigidbody();

        StartCoroutine(CheckBottlePickup());

    }

    private IEnumerator CheckBottlePickup()
{
    yield return new WaitForSeconds(20f);

    if (!InventoryManager.Instance.HasItem("Bottle"))
    {
        StartCoroutine(GameOverJumpscare());
    }
}

private IEnumerator GameOverJumpscare()
{
    // Freeze player movement
    FindObjectOfType<CharacterMovement>().canMove = false;

    // Position doll in front of player
    Vector3 inFront = camera.transform.position + camera.transform.forward * 1.2f;
    inFront.y = 0.121f;
    Doll.transform.position = inFront;

    Vector3 dollRotation = Doll.transform.rotation.eulerAngles;
    dollRotation.y = camera.transform.eulerAngles.y + 180;
    Doll.transform.rotation = Quaternion.Euler(dollRotation);

    // Play jumpscare sound
    if (jumpScareAudio != null && terror != null)
    {
        jumpScareAudio.volume = 0.5f;
        jumpScareAudio.PlayOneShot(terror);
    }

    // Optional: Flashlight off / effects
    if (toggleFlash != null)
    {
        toggleFlash.SetFlashlightState(false);
        toggleFlash.canToggle = false;
    }

    yield return new WaitForSeconds(2f); // Wait for dramatic pause

    // Show game over UI
    Time.timeScale = 0f; // Pause game
    ShowGameOverScreen(); // We'll add this function next
}

private void ShowGameOverScreen()
{
    if (gameOverScreen != null)
    {
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dollIsVisible)
        {
            doorTriggerCount++;
            
        
        if (doorTriggerCount >= 6)
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
            yield return new WaitForSeconds(timer);
            yield return StartCoroutine(ShowDollBehindPlayer());

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
            timer += 5f;
            timerRunning = false;
        }

        // Show the doll temporarily when the player triggers the door
        yield return StartCoroutine(ShowDollTemporarily());

        // After the door-triggered doll is done resume the timed event coroutine
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

    // turn flashlight off
    bool wasFlashlightOn = toggleFlash.flashlightIsOn;
    

    if (wasFlashlightOn)
    {
        toggleFlash.SetFlashlightState(false);
        toggleFlash.canToggle = false;
        FindObjectOfType<CharacterMovement>().canMove = false;
        

    }

    yield return new WaitForSeconds(0.7f); //pause

    //Start flickering flashlight
    if (flashingCoroutine == null)
    {
        flashingCoroutine = StartCoroutine(FlashlightStrobe());
    }

    //Show doll in front of player
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

    // Hide doll 
    Doll.transform.position = new Vector3(0, -1000, 0);

    if (jumpScareAudio != null && jumpScareAudio.isPlaying)
    {
        jumpScareAudio.Stop();
    }

    //Stop flashlight flicker
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

private IEnumerator ShowDollBehindPlayer()
{
    timerRunning = false;
    
    /*
    //wait untill sound finish
    if (terror != null && jumpScareAudio != null)
    {
        jumpScareAudio.volume = 0.2f;
        jumpScareAudio.PlayOneShot(terror);
        yield return new WaitForSeconds(terror.length);
        
    }

      */  yield return new WaitForSeconds(1f);
    // Doll appears behind the player
    Vector3 behindPlayer = camera.transform.position - camera.transform.forward * 1.5f;
    behindPlayer.y = 0.121f;
    Doll.transform.position = behindPlayer;

    Vector3 dollRotation = Doll.transform.rotation.eulerAngles;
    dollRotation.y = camera.transform.eulerAngles.y;
    Doll.transform.rotation = Quaternion.Euler(dollRotation);

    dollIsVisible = true;

    FindObjectOfType<CharacterMovement>().canMove = false;

    // Wait for player to look at it
    while (true)
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out light, rayrange))
        {
            if (light.collider.gameObject == Doll)
            {
                    if (jumpScareAudio != null && !jumpScareAudio.isPlaying)
                        {
                            jumpScareAudio.Play();
                        }
                yield return StartCoroutine(TriggerLookJumpscare());
                timerRunning = true;
                FindObjectOfType<CharacterMovement>().canMove = true;
                yield break;
            }
        }

        yield return null;
    }
}



private IEnumerator TriggerLookJumpscare()
{
    //all same as other jumpscare

    if (toggleFlash == null)
        toggleFlash = FindObjectOfType<FlashlightToggle>();

    bool wasFlashlightOn = toggleFlash.flashlightIsOn;

    if (wasFlashlightOn)
    {
        toggleFlash.SetFlashlightState(false);
        toggleFlash.canToggle = false;
    }

    FindObjectOfType<CharacterMovement>().canMove = false;

    yield return new WaitForSeconds(0.7f);

    if (flashingCoroutine == null)
    {
        flashingCoroutine = StartCoroutine(FlashlightStrobe());
    }



    yield return new WaitForSeconds(dollDisplayDuration);

    Doll.transform.position = new Vector3(0, -1000, 0);

    if (jumpScareAudio != null && jumpScareAudio.isPlaying)
    {
        jumpScareAudio.Stop();
    }

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
