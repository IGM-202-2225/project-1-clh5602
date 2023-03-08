using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    public Vector3 relativePosition = Vector3.zero;
    public Vector3 originPos = new Vector3(0, -10, -4.6f);

    const float speedMult = 10.0f;

    public bool deleteMe = false;

    void Start()
    {
        transform.position = originPos;
    }

    // Update is called once per frame
    void Update()
    {
        relativePosition += direction * Time.deltaTime * speedMult;

        transform.position = originPos + relativePosition;

        transform.position = new Vector3(transform.position.x, transform.position.y, -4.5f);

        // Offscreen check
        if (Mathf.Abs(transform.position.x) > 20 || transform.position.y > 4.3f)
        {
            deleteMe = true;
        }
    }
}
