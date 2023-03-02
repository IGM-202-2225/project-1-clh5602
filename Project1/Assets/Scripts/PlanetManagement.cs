using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManagement : MonoBehaviour
{
    public float rotationSpeed = 0.0f;
    private Camera mainCam;
    private const float camSizeMult = 0.09f;
    public float timeRemaining = 120.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        newZRotate = transform.rotation.z;
        newZRotate += rotationSpeed * Time.deltaTime;
        newZRotate = (newZRotate + 360) % 360;*/

        // Pan camera ahead and behind
        float panAmount = rotationSpeed * camSizeMult;

        // Decrease timer
        timeRemaining -= Time.deltaTime;

        mainCam.transform.position = new Vector3(panAmount, 0, -10);
        transform.position = new Vector3(panAmount * 0.7f, -4.99f, 3.85f);

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);
    }
}
