using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{

    public float myHoriPos = 0.0f; // Determines x position 
    private const float scale = 0.3f;

    public bool deleteMe = false;
    public bool giveReward = true;

    public PlanetManagement planet;

    public PlayerStuff player;

    [SerializeField]
    GameObject cannonPref;
    public GameObject myCannon;

    [SerializeField]
    GameObject bulletPref;
    public EnemyBullet myBullet = null;

    private float cannonAngle = 0;

    public float difference;

    public bool type = true; // true means enemy 1, false means enemy 2

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
        // Random hori speed
        do
        {
            horiSpeed = Random.Range(-20, 20);
        } while (Mathf.Abs(horiSpeed) < 7);

        // Random movement for enemy 1
        if (type)
        {
            magnitude = Random.Range(1, 3);
            cycleTime = Random.Range(1, 6);
            waveCount = Random.Range(0, cycleTime);
            startY = Random.Range(-1, 1);
        } 

        radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        myY = transform.position.y;

        // Clamp hori pos
        myHoriPos = (myHoriPos + 360) % 360;

        // Determine starting placement (to left or right of player)
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

        // Enemy 2
        if (!type)
        {
            // Create and attach cannon
            myCannon = Instantiate(cannonPref);
            myCannon.transform.position = new Vector3(transform.position.x, transform.position.y, -4.68f);
        }
    }

    // Update is called once per frame
    void Update()
    {


        // Enemy 1 Movement
        if (type)
        {
            myY = transform.position.y;
            Enemy1Movement();
        }
        else
        {
            myY = -5;
        }

        // Clamp hori pos
        myHoriPos = (myHoriPos + 360) % 360;

        // For wrapping
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

        // Enemy 2 adjust cannon angle
        if (!type && Mathf.Abs(difference) <= 50)
        {
            cannonAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            cannonAngle *= 180 / Mathf.PI;
            myCannon.transform.position = new Vector3(transform.position.x, transform.position.y, -4.68f);
            myCannon.transform.rotation = Quaternion.Euler(0, 0, cannonAngle-90);
            
        }

        // Bullet management
        if (!type)
        {
            Enemy2Firing();
        }

    }


    // Enemy 1 movement pattern
    void Enemy1Movement()
    {
        myHoriPos += horiSpeed * Time.deltaTime;

        // Wave count is to determine where on the sin wave the enemy is
        waveCount += Time.deltaTime;
        waveCount %= cycleTime;

        myY = magnitude * Mathf.Sin((waveCount / cycleTime)  * (Mathf.PI * 2));
    }


    // If enemy 2, call this function to try firing.
    void Enemy2Firing()
    {
        // Only 1 bullet at a time
        if (myBullet == null) {
            
            fireTimer += Time.deltaTime * 2.2f;

            // If fire timer up
            if (fireTimer > Random.Range(1, 3))
            {
                // Reset timer
                fireTimer = Random.Range(-1, 0);

                // If onscreen
                if (Mathf.Abs(difference) < 20)
                {
                    // Fire
                    myBullet = Instantiate(bulletPref).GetComponent<EnemyBullet>();
                    // Assign the bullet's direction and starting position
                    myBullet.direction = new Vector3(Mathf.Cos(cannonAngle * Mathf.PI / 180), Mathf.Sin(cannonAngle * Mathf.PI / 180), 0);
                    myBullet.originPos = transform.position;
                }
            }
        }
        else // Bullet already exists
        {
            // Update bullet's original position (for screen scrolling)
            myBullet.originPos = transform.position;
            
            // Delete bullet if need be
            if (myBullet.deleteMe)
            {
                Destroy(myBullet.gameObject);
                myBullet = null;
            }
        }
    }


    // Circle check w/ player lasers
    public void LaserCollisionCheck(Vector3 laserPos)
    {
        if (Mathf.Pow(radius, 2) >= Mathf.Pow(laserPos.x - transform.position.x, 2) + Mathf.Pow(laserPos.y - transform.position.y, 2))
        {
            deleteMe = true;
        }
    }

}
