using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{
    public int level = 1;
    public GameObject dirt;

    public float size;
    public float depth;

    // Use this for initialization
    void Start()
    {
        itemID = 4;
        itemCap = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (used)
        {
            useTimer -= Time.deltaTime;
            if (useTimer < 0)
            {
                used = false;
            }
        }
    }

    public override void PrimaryUse(ClickType click)
    {
        switch (click)
        {
            case ClickType.Single:
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                Debug.Log("Shovel");
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Ground"))
                    {
                        Debug.Log("Shovel");
                        used = true;
                        useTimer = useRate;

                        /*
                        TerrainData terrainData = hit.transform.GetComponent<Terrain>().terrainData;
                        int heightmapWidth = terrainData.heightmapWidth;
                        int heightmapHeight = terrainData.heightmapHeight;

                        float[,] currentHeights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

                        float[,] newHeights = new float[heightmapWidth, heightmapHeight];
                        float terrainHeight = terrainData.size.y;
                        
                        int xPos = (int)Mathf.Round(hit.point.x);
                        int zPos = (int)Mathf.Round(hit.point.z);

                        for (int y = 0; y < heightmapWidth; y++)
                        {
                            for (int x = 0; x < heightmapHeight; x++)
                            {
                                if (Vector2.Distance( new Vector2(x,y), new Vector2(xPos + (heightmapWidth /2), zPos + (heightmapWidth / 2))) < size)
                                {
                                    newHeights[y, x] = Mathf.Clamp01(currentHeights[y, x] + (depth / terrainHeight));
                                }
                                else
                                    newHeights[y, x] = currentHeights[y, x];
                            }
                        }
                        terrainData.SetHeights(0, 0, newHeights);
                        */

                          Instantiate(dirt, hit.point, transform.rotation);
                         if (level > 1)
                             Instantiate(dirt, hit.point + (transform.up * 1.5f), transform.rotation);
                         if (level > 2)
                             Instantiate(dirt, hit.point + (transform.up * 3), transform.rotation);
                    }
                }
                break;

            case ClickType.Hold:
                if (!used)
                {
                    //   base.UseTool();
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                    Debug.Log("Shovel");
                    if (Physics.Raycast(ray, out hit, rayMaxDist))
                    {
                        if (hit.transform.CompareTag("Ground"))
                        {
                            used = true;
                            useTimer = useRate;
                            Instantiate(dirt, hit.point, transform.rotation);
                            if (level > 1)
                                Instantiate(dirt, hit.point + (transform.up * 1.5f), transform.rotation);
                            if (level > 2)
                                Instantiate(dirt, hit.point + (transform.up * 3), transform.rotation);
                        }
                    }
                }
                break;
        }

    }
}
