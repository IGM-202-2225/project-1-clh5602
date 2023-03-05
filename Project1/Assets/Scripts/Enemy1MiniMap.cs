using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1MiniMap : MonoBehaviour
{
    public EnemyStuff enemy;
    static float cameraHeight, half;
    private RectTransform myRect;

    // Start is called before the first frame update
    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 1.8f;
        half = Camera.main.orthographicSize;
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float myY = 0.84f * ((enemy.transform.position.y + half) / cameraHeight) + 0.08f;
        float myX = 0.97f * ((enemy.difference + 180) / 360) + 0.015f;

        myRect.offsetMin = new Vector2(0, 0);
        myRect.offsetMax = new Vector2(0, 0);

        myRect.anchorMin = new Vector2(myX - 0.003f, myY - 0.04f);
        myRect.anchorMax = new Vector2(myX + 0.003f, myY + 0.03f);
    }
}
