using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStuff : MonoBehaviour
{

    [SerializeField]
    PlanetManagement planet;
    private Vector3 vertDirection = Vector3.zero;
    private Vector3 horiDirection = Vector3.zero;
    private float horiSpeed = 2.0f;
    private bool facingRight = true;

    private const float MAX_SPEED = 16.0f;
    private const float MIN_SPEED = 2.0f;
    private const float CHANGE_SPEED = 30.0f;

    private SpriteRenderer mySprite;

    private float futureX, futureY;

    static float cameraHalfHeight, cameraHalfWidth;

    public float invincible = 0;

    public float radius = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraHalfHeight = Camera.main.orthographicSize * 1f;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        futureX = transform.position.x;
        futureY = transform.position.y;

        mySprite = GetComponent<SpriteRenderer>();

        radius = mySprite.bounds.size.y / 2;

        InvokeRepeating("Flicker", 0.1f, 0.1f); // Sprite flashing
    }

    // Update is called once per frame
    void Update()
    {
        if (!planet.inGame)
        {
            if (transform.position.y > 0.05f)
            {
                vertDirection = new Vector2(0, -0.2f);
            }
            else if (transform.position.y < -0.05f) 
            { 
                vertDirection = new Vector2(0, 0.2f);
            }
            else
            {
                vertDirection = Vector2.zero;
            }
            
            horiDirection = Vector2.zero;
        }
        futureX = transform.position.x;
        futureY = transform.position.y + vertDirection.y * Time.deltaTime * 7;

        mySprite.flipX = !facingRight;

        // y movement
        if (futureY < -cameraHalfHeight)
        {
            futureY = -cameraHalfHeight;
        }
        if (futureY > cameraHalfHeight * 0.8)
        {
            futureY = cameraHalfHeight * 0.8f;
        }

        // turning stuff
        if (horiDirection == Vector3.zero)
        {
            if (facingRight)
            {
                if (horiSpeed > MIN_SPEED)
                {
                    horiSpeed -= CHANGE_SPEED/2 * Time.deltaTime;
                }
                else
                {
                    horiSpeed = MIN_SPEED;
                }
            }
            else
            {
                if (horiSpeed < -MIN_SPEED)
                {
                    horiSpeed += CHANGE_SPEED/2 * Time.deltaTime;
                }
                else
                {
                    horiSpeed = -MIN_SPEED;
                }
            }
        }
        else
        {
            if (facingRight)
            {
                if (horiDirection.x > 0)
                {
                    if (horiSpeed < MAX_SPEED)
                    {
                        horiSpeed += CHANGE_SPEED * Time.deltaTime;
                    }
                    else
                    {
                        horiSpeed = MAX_SPEED;
                    }
                }
                else
                {
                    horiSpeed -= CHANGE_SPEED * Time.deltaTime;
                    if (horiSpeed < 0)
                    {
                        facingRight = false;
                    }
                }
            }
            else
            {
                if (horiDirection.x < 0)
                {
                    if (horiSpeed > -MAX_SPEED)
                    {
                        horiSpeed -= CHANGE_SPEED * Time.deltaTime;
                    }
                    else
                    {
                        horiSpeed = -MAX_SPEED;
                    }
                }
                else
                {
                    horiSpeed += CHANGE_SPEED * Time.deltaTime;
                    if (horiSpeed > 0)
                    {
                        facingRight = true;
                    }
                }
            }
        }

        // Update vals
        transform.position = new Vector3(futureX, futureY, -8f);
        planet.rotationSpeed = horiSpeed * 3.0f;
        if (invincible > 0)
        {
            invincible -= Time.deltaTime;
        }
        
    }

    // When move happens
    public void AscendDescend(InputAction.CallbackContext context)
    {
        if (planet.inGame)
        {
            vertDirection = context.ReadValue<Vector2>();
        }
    }

    // When move happens
    public void LeftRight(InputAction.CallbackContext context)
    {
        if (planet.inGame)
        {
            horiDirection = context.ReadValue<Vector2>();
        }
    }

    // When laser happens
    public void CreateLaser(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && planet.inGame)
        {
            planet.GetComponent<PlanetManagement>().CreateLaser(transform.position.y - 0.09f, (horiSpeed < 0));

        }
    }

    public void Flicker()
    {
        if (invincible > 0)
        {
            mySprite.enabled = !mySprite.enabled;
        }
        else
        {
            mySprite.enabled = true;
        }
    }
}
