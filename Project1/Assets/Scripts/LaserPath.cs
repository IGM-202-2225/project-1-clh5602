using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPath : MonoBehaviour
{
    static float SPEED = 30.0f;
    private SpriteRenderer mySprite;

    static float camWidth;

    public int direction = 1;
    public bool deleteMe = false;



    // Start is called before the first frame update
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (direction < 0)
        {
            mySprite.flipX = true;
        }
        else
        {
            mySprite.flipX = false;
        }

        mySprite.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);

        transform.Translate(SPEED * Time.deltaTime * direction, 0, 0);

        if (Mathf.Abs(transform.position.x) > camWidth * 2)
        {
            deleteMe = true;
        }
    }

}
