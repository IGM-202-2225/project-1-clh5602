using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    List<TextMesh> words;

    public bool visible = true;

    Color visibleCol = new Color(1, 1, 1, 1);
    Color invisibleCol = new Color(1, 1, 1, 0);

    SpriteRenderer mySprite;

    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        mySprite.enabled = visible;
        transform.position = new Vector3(Camera.main.transform.position.x - 5.1889f, 1.5612f, 0);

        foreach (TextMesh text in words)
        {
            if (visible)
            {
                text.color = visibleCol;
            }
            else
            {
                text.color = invisibleCol;
            }
        }
    }
}
