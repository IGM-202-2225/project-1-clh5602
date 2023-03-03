using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManagement : MonoBehaviour
{
    public float rotationSpeed = 0.0f;
    private Camera mainCam;
    private const float camSizeMult = 0.09f;
    public float timeRemaining = 120.0f;

    private const int MAX_LASERS = 3;

    public List<GameObject> lasers = new List<GameObject>();

    [SerializeField]
    GameObject laserPref;

    [SerializeField]
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Pan camera ahead and behind
        float panAmount = rotationSpeed * camSizeMult;
        mainCam.transform.position = new Vector3(panAmount, 0, -10);
        transform.position = new Vector3(panAmount * 0.7f, -4.99f, 3.85f);

        // Decrease timer
        timeRemaining -= Time.deltaTime;

        // Check lasers
        HandleLasers();

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);
    }

    public void CreateLaser(float height, bool left)
    {
        if (lasers.Count < MAX_LASERS)
        {
            GameObject newLaser = Instantiate(laserPref, new Vector3(0, height, -6), Quaternion.identity);

            if (left)
            {
                newLaser.GetComponent<LaserPath>().direction = -1;
            }

            lasers.Add(newLaser);
        }
        
    }

    void HandleLasers()
    {
        for (int i = lasers.Count-1; i >= 0; i--)
        {
            if (lasers[i].GetComponent<LaserPath>().deleteMe) {
                Destroy(lasers[i]);
                lasers.RemoveAt(i);
            }
        }
    }

}
