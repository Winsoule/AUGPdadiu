using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshMaker))]
public class RandomTileGenerator : MonoBehaviour
{
    MeshMaker meshMaker;
    public Material wallMat;
    class FloorSection
    {
        public bool[,] FloorTiles = new bool[10, 10];
        public FloorSection()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    FloorTiles[i, j] = true;
                }
            }
        }
    }
    int currentXCoordinate = 50;
    int currentZCoordinate = 50;

    FloorSection[,] floorSections = new FloorSection[10, 10];
    // Use this for initialization
    void Start()
    {
        meshMaker = transform.GetComponent<MeshMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value > 0.5f) //Vertical or horizontal
        {
            if (Random.value > 0.5f) //Up or down
            {
                currentZCoordinate = Mathf.Clamp(currentZCoordinate + 1, 0, 99);
            }
            else
            {
                currentZCoordinate = Mathf.Clamp(currentZCoordinate - 1, 0, 99);
            }
        }
        else if (Random.value > 0.5f) //Left or right
        {
            currentXCoordinate = Mathf.Clamp(currentXCoordinate + 1, 0, 99);
        }
        else
        {
            currentXCoordinate = Mathf.Clamp(currentXCoordinate - 1, 0, 99);
        }

        int floorSectionX = currentXCoordinate / 10;
        int floorSectionZ = currentZCoordinate / 10;
        int floorTileX = Mathf.Abs(currentXCoordinate % 10);
        int floorTileZ = Mathf.Abs(currentZCoordinate % 10);

        //print("coordinate: (" + floorSectionX + ", " + floorSectionZ + ") (" + floorTileX + ", " + floorTileZ + ")");
        if (floorSections[floorSectionX, floorSectionZ] == null)
        {
            floorSections[floorSectionX, floorSectionZ] = new FloorSection();
        }
        if (floorSections[floorSectionX, floorSectionZ].FloorTiles[floorTileX, floorTileZ])
        {
            floorSections[floorSectionX, floorSectionZ].FloorTiles[floorTileX, floorTileZ] = false;
            Instantiate(meshMaker.Box(wallMat), new Vector3(currentXCoordinate, 0, currentZCoordinate), Quaternion.identity);
        }
        //if(floorSections[])
        //Instantiate(FloorTilePrefab, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
    }
}