using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanVisibleArea : MonoBehaviour
{
    
    private RectTransform myRect;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Camera.main.transform.position.x;
        offset *= 0.01f;

        myRect.anchorMin = new Vector2(0.415f + offset, 0);
        myRect.anchorMax = new Vector2(0.585f + offset, 1);
    }
}
