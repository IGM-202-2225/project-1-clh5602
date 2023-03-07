using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{

    public float myHoriPos = 0.0f;
    private const float scale = 0.3f;

    public bool deleteMe = false;

    public PlanetManagement planet;

    public PlayerStuff player;
    public GameObject cannonPref;
    public GameObject myCannon;
    private float cannonAngle = 0;

    public float difference;

    public bool type = true;

    private float horiSpeed;
    private float magnitude;
    private float waveCount = 0;
    private float cycleTime;
    private float startY;
    private float fireTimer = 0;

    public float radius = 0;

    private float myY;

    // Start is called before the first frame update
    void Start()
    {
        do
        {
            horiSpeed = Random.Range(-20, 20);
        } while (Mathf.Abs(horiSpeed) < 7);

        magnitude = Random.Range(1, 3);
        cycleTime = Random.Range(1, 6);
        waveCount = Random.Range(0, cycleTime);
        startY = Random.Range(-1, 1);

        radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        myY = transform.position.y;

        // Clamp hori pos
        myHoriPos = (myHoriPos + 360) % 360;

        //if (Mathf.Abs(myHoriPos - planet.transform.rotation.z))

        if (planet.transform.eulerAngles.y < 180)
        {
            if (myHoriPos > planet.transform.eulerAngles.y + 180)
            {
                difference = planet.transform.eulerAngles.y - (myHoriPos - 360);
            }
            else
            {
                difference = planet.transform.eulerAngles.y - myHoriPos;
            }
        }
        else
        {
            if (myHoriPos < planet.transform.eulerAngles.y - 180)
            {
                difference = (myHoriPos + 360) - planet.transform.eulerAngles.y;
                difference *= -1;
            }
            else
            {
                difference = planet.transform.eulerAngles.y - myHoriPos;
            }
        }

        difference *= -1;

        transform.position = new Vector3((difference) * scale, myY, -5);

        if (!type) // Enemy 2
        {
            myCannon = Instantiate(cannonPref);
            myCannon.transform.position = new Vector3(transform.position.x, transform.position.y, -4.68f);
            //myCannon.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        myY = transform.position.y;


        if (type)
        {
            Enemy1Movement();
        }

        // Clamp hori pos
        myHoriPos = (myHoriPos + 360) % 360;

        //if (Mathf.Abs(myHoriPos - planet.transform.rotation.z))

        if (planet.transform.eulerAngles.y < 180)
        {
            if (myHoriPos > planet.transform.eulerAngles.y + 180)
            {
                difference = planet.transform.eulerAngles.y - (myHoriPos - 360);
            }
            else
            {
                difference = planet.transform.eulerAngles.y - myHoriPos;
            }
        }
        else
        {
            if (myHoriPos < planet.transform.eulerAngles.y - 180)
            {
                difference = (myHoriPos + 360) - planet.transform.eulerAngles.y;
                difference *= -1;
            }
            else
            {
                difference = planet.transform.eulerAngles.y - myHoriPos;
            }
        }

        difference *= -1;

        transform.position = new Vector3((difference) * scale, myY, -5);

        if (!type && Mathf.Abs(difference) <= 50) // Enemy 2
        {
            cannonAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            cannonAngle *= 180 / Mathf.PI;
            cannonAngle -= 90;
            myCannon.transform.position = new Vector3(transform.position.x, transform.position.y, -4.68f);
            myCannon.transform.rotation = Quaternion.Euler(0, 0, cannonAngle);
        }


    }

    void Enemy1Movement()
    {
        myHoriPos += horiSpeed * Time.deltaTime;

        waveCount += Time.deltaTime;
        waveCount %= cycleTime;

        myY = magnitude * Mathf.Sin((waveCount / cycleTime)  * (Mathf.PI * 2));

        // For firing
        fireTimer += Time.deltaTime;

        if (fireTimer > Random.Range(1, 3))
        {
            // Try to fire
            fireTimer = Random.Range(-1, 0);

            if (Mathf.Abs(difference) < 20)
            {
                // Fire
            }
        }
    }

    // Circle check
    public void LaserCollisionCheck(Vector3 laserPos)
    {
        if (Mathf.Pow(radius, 2) >= Mathf.Pow(laserPos.x - transform.position.x, 2) + Mathf.Pow(laserPos.y - transform.position.y, 2))
        {
            deleteMe = true;
        }
    }

}
