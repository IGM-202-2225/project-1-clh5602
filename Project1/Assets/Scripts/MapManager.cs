using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    PlanetManagement planet;

    [SerializeField]
    GameObject mapIcon;

    public List<EnemyMiniMap> enemyIcons = new List<EnemyMiniMap>();

    public void CreateEnemyIcon(EnemyStuff enemyRef)
    {
        GameObject newIcon = Instantiate(mapIcon);
        newIcon.transform.SetParent(transform);
        newIcon.transform.localScale = Vector3.one;
        newIcon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (!enemyRef.type) // Blue icon
        {
            newIcon.GetComponent<Image>().color = new Color(0.39f, 0.74f, 1, 1);
        }

        EnemyMiniMap iconCode = newIcon.GetComponent<EnemyMiniMap>();
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
