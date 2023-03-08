using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    [SerializeField]
    private Vector3 StartPos;
    private float timer;
    public Vector3 rgb = new Vector3(1, 1, 1);
    public bool pause = false;
    TextMesh myText;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        myText = GetComponent<TextMesh>();
        myText.color = new Color(rgb.x, rgb.y, rgb.z, 0);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        
        if (timer < 0.5f)
        {
            transform.position = StartPos + new Vector3(Camera.main.transform.position.x, -0.5f * (1 - timer/ 0.5f), 0);
            myText.color = new Color(rgb.x, rgb.y, rgb.z, (timer / 0.5f));
        }
        else if (timer > 1.5f)
        {
            transform.position = StartPos + new Vector3(Camera.main.transform.position.x, 0.5f * ((timer-1.5f) / 0.5f), 0);
            myText.color = new Color(rgb.x, rgb.y, rgb.z, 1 - ((timer - 1.5f) / 0.5f));
        }
        else
        {
            transform.position = StartPos + new Vector3(Camera.main.transform.position.x, 0, 0);
            myText.color = new Color(rgb.x, rgb.y, rgb.z, 1);
            if (pause)
            {
                timer -= Time.deltaTime;
            }
        }

        if (timer > 2)
        {
            Destroy(this.gameObject);
        }
    }
}
