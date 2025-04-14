using System.Collections;
using UnityEngine;

public class DollFollow : MonoBehaviour
{
    [SerializeField] private GameObject Doll;
    private GameObject camera;

    public float timedEventInterval = 30.0f;
    public float dollDisplayDuration = 5.0f;
    private Rigidbody dollRigidbody;
    private float timer;
    private bool dollIsVisible = false;
    private bool timerRunning = true;

    private Coroutine timedCoroutine = null;

    void Start()
    {
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

        timedCoroutine = StartCoroutine(TimedDollCoroutine()); // Start the timed appearance coroutine

        FreezeDollRigidbody();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dollIsVisible)
        {
            // Start the door-triggered appearance coroutine
            StartCoroutine(TriggerDollCoroutine());
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
        if (Doll == null || camera == null)
        {
            Debug.LogWarning("Doll or Camera is null");
            yield break;
        }

        dollIsVisible = true;

        // Show the doll in front of the player
        Vector3 inFront = camera.transform.position + camera.transform.forward * 1.5f;
        inFront.y = 0.121f;
        Doll.transform.position = inFront;

        Vector3 dollRotation = Doll.transform.rotation.eulerAngles;
        dollRotation.y = camera.transform.eulerAngles.y + 180;
        Doll.transform.rotation = Quaternion.Euler(dollRotation);

        // Wait for the doll display duration
        yield return new WaitForSeconds(dollDisplayDuration);

        // Hide the doll again
        Doll.transform.position = new Vector3(0, -1000, 0);
        
        if (dollRigidbody != null)
        {
            FreezeDollRigidbody();
        }

        dollIsVisible = false;

        // Reset timer after the doll disappears
        timer = timedEventInterval; 
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
