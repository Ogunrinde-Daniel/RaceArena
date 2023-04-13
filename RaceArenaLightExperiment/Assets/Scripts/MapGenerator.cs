using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth = 10;
    public int mapHeight = 10;
 
    public GameObject tile;

    private float tilexOffset = 1.5f + 0.02f;
    private float tileZOffset = 1.7f + 0.06f;

    void Awake()
    {
        generateMap();
    }

    void generateMap()
    {

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                var hex = Instantiate(tile);
                if (x % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * tilexOffset, 0 , z * tileZOffset);
                }
                else
                {
                    hex.transform.position = new Vector3(x * tilexOffset, 0, z * tileZOffset + tileZOffset / 2f);
                }
                hex.transform.eulerAngles = new Vector3( hex.transform.eulerAngles.x, hex.transform.eulerAngles.y -90, hex.transform.eulerAngles.z );
                hex.transform.SetParent(this.transform);
            }
        }

    }

    public void ResetMap()
    {

        foreach (Transform hex in transform)
        {
            hex.GetComponent<Hexagon>().ResetHexagon();
        }

    }


}
