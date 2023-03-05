using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    PlanetManagement planet;

    [SerializeField]
    GameObject mapIcon;

    public List<Enemy1MiniMap> enemyIcons = new List<Enemy1MiniMap>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemyIcon(EnemyStuff enemyRef)
    {
        GameObject newIcon = Instantiate(mapIcon);
        newIcon.transform.SetParent(transform);
        newIcon.transform.localScale = Vector3.one;
        newIcon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        Enemy1MiniMap iconCode = newIcon.GetComponent<Enemy1MiniMap>();
        iconCode.enemy = enemyRef;

        enemyIcons.Add(iconCode);
    }

    public void CheckIconsForDelete()
    {
        for (int i = enemyIcons.Count-1; i >= 0; i--)
        {
            if (enemyIcons[i].enemy.deleteMe)
            {
                Destroy(enemyIcons[i].gameObject);
                enemyIcons.RemoveAt(i);
            }
        }
    }
}
