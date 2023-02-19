using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManagement : MonoBehaviour
{
    public float rotationSpeed = 0.0f;
    private float newZRotate;

    // Start is called before the first frame update
    void Start()
    {
        newZRotate = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        newZRotate = transform.rotation.z;
        newZRotate += rotationSpeed * Time.deltaTime;
        newZRotate = (newZRotate + 360) % 360;*/

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);
    }
}
