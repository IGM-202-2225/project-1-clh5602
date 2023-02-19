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

    // Start is called before the first frame update
    void Start()
    {
        cameraHalfHeight = Camera.main.orthographicSize * 1f;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        futureX = transform.position.x;
        futureY = transform.position.y;

        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        futureX = transform.position.x;
        futureY = transform.position.y + vertDirection.y * Time.deltaTime * 7;

        mySprite.flipX = !facingRight;

        // y movement
        if (futureY < -cameraHalfHeight)
        {
            futureY = -cameraHalfHeight;
        }
        if (futureY > cameraHalfHeight)
        {
            futureY = cameraHalfHeight;
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
        transform.position = new Vector3(futureX, futureY, -9.5f);
        planet.rotationSpeed = horiSpeed * 3.0f;
    }

    // When move happens
    public void AscendDescend(InputAction.CallbackContext context)
    {
        vertDirection = context.ReadValue<Vector2>();
    }

    // When move happens
    public void LeftRight(InputAction.CallbackContext context)
    {
        horiDirection = context.ReadValue<Vector2>();
        Debug.Log(horiDirection.x);
        

    }
}