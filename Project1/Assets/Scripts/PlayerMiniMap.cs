using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniMap : MonoBehaviour
{
    [SerializeField]
    PlayerStuff player;
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
        float myPos = 0.84f * ((player.transform.position.y + half) / cameraHeight) + 0.08f;

        myRect.anchorMin = new Vector2(0.497f, myPos-0.04f);
        myRect.anchorMax = new Vector2(0.503f, myPos + 0.03f);
    }
}
