using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshMaker))]
public class MakeLevel : MonoBehaviour {

    MeshMaker meshMaker;
    public GameObject enemy,boss;
    public Transform levelFloor;
    public float wallHight;
    public int numRoomsProcent = 10;
    public int NumOfSideWalls = 100;
    public int amountOfEnemys = 10;
    public Material wallMat, portalMat;
    public Transform cam;

    public List<Vector3> aiPoints = new List<Vector3>();

    Vector3 levelSize;
    enum Dir
    {
        up,
        left,
        right,
    }

    public class FloorSection
    {
        public bool[,] FloorTiles = new bool[10, 10];
        public FloorSection()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    FloorTiles[i, j] = false;
                }
            }
        }
    }

    List<Vector3> objects = new List<Vector3>();

    // Use this for initialization
    void Start () {
        levelSize = levelFloor.GetComponent<Renderer>().bounds.size;
        meshMaker = transform.GetComponent<MeshMaker>();

        //OldGenerateLevel(150, 1000);
        
        
        GenerateLevel(((int)(NumOfSideWalls * NumOfSideWalls * (numRoomsProcent / 100f))), ((int)(NumOfSideWalls * NumOfSideWalls * (numRoomsProcent / 100f) * 10)), levelSize);
        MakeEnemys(1, boss);
        MakeEnemys(amountOfEnemys, enemy);

        GameObject go = meshMaker.Torus(portalMat);
        go.transform.position = Vector3.one;
        go.transform.LookAt(cam);
        go.transform.Rotate(new Vector3(90, 0, 0));

    }

    void OldGenerateLevel(int maxRooms, int maxTries)
    {
        int currentXCoordinate = 0;
        int currentZCoordinate = 0;
        int count = 0;
        int ties = 0;

        FloorSection[,] floorSections = new FloorSection[10, 10];

        while(count <= maxRooms || ties <= maxTries)
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
            if (!floorSections[floorSectionX, floorSectionZ].FloorTiles[floorTileX, floorTileZ])
            {
                count++;
                floorSections[floorSectionX, floorSectionZ].FloorTiles[floorTileX, floorTileZ] = true;
                Instantiate(meshMaker.Box(wallMat), new Vector3(currentXCoordinate, 0, currentZCoordinate), Quaternion.identity);
            }
            ties++;
        }
    }

    void GenerateLevel(int maxRooms, int maxTries, Vector3 levelSize)
    {
        Vector3 wallSize = new Vector3(levelSize.x / NumOfSideWalls, wallHight, levelSize.z / NumOfSideWalls);
        int internalSizeX = (int)(levelSize.x/ wallSize.x);
        int internalSizeZ = (int)(levelSize.z/ wallSize.z);
        GameObject level = new GameObject();
        level.name = "Level";
        bool[,] tiles = new bool[internalSizeX, internalSizeZ];
        int rooms = 1;
        int tries = 0;
        int currentXCoordinate = internalSizeX/2;
        int currentYCoordinate = internalSizeZ/2;

        

        tiles[currentXCoordinate, currentYCoordinate] = true;

        while (maxRooms > rooms || maxTries > tries)
        {
            if (Random.value > 0.5f) //Vertical or horizontal
            {
                if (Random.value > 0.5f) //Up or down
                {
                    currentYCoordinate = Mathf.Clamp(currentYCoordinate + 1, 1, internalSizeX -2);
                }
                else
                {
                    currentYCoordinate = Mathf.Clamp(currentYCoordinate - 1, 1, internalSizeX -2);
                }
            }
            else if (Random.value > 0.5f) //Left or right
            {
                currentXCoordinate = Mathf.Clamp(currentXCoordinate + 1, 1, internalSizeZ -2);
            }
            else
            {
                currentXCoordinate = Mathf.Clamp(currentXCoordinate - 1, 1, internalSizeZ -2);
            }

            if(!tiles[currentXCoordinate,currentYCoordinate])
            {
                tiles[currentXCoordinate, currentYCoordinate] = true;
                rooms++;
            }
            tries++;
            if (maxRooms <= rooms)
                break;
        }

        Debug.Log("Found " + rooms + " rooms in " + tries + " tries.");
        
        for (int i = 0; i < internalSizeX; i++)
        {
            for(int j = 0; j < internalSizeZ; j++)
            {
                if (!tiles[i,j])
                {
                    
                    if (i != 0 && i != internalSizeX-1 && j != 0 && j != internalSizeZ-1)
                    {
                        if(tiles[i + 1, j] || tiles[i - 1, j] || tiles[i, j + 1] || tiles[i, j - 1])
                        {
                            GameObject go = meshMaker.Box(wallMat);
                            go.transform.position = new Vector3((-(internalSizeX / 2) + i) * wallSize.x, wallSize.y / 2, (-(internalSizeZ / 2f) + j) * wallSize.z);
                            go.transform.localScale = wallSize;
                            go.transform.SetParent(level.transform);

                            go.AddComponent<NavMeshObstacle>();
                            go.GetComponent<NavMeshObstacle>().carving = true;
                            go.AddComponent<BoxCollider>();
                        }
                    } 
                    else
                    {
                        GameObject go = meshMaker.Box(wallMat);
                        go.transform.position = new Vector3((-(internalSizeX / 2f) + i)* wallSize.x, wallSize.y/2, (-(internalSizeZ / 2f) + j)*wallSize.z);
                        go.transform.localScale = wallSize;
                        go.transform.SetParent(level.transform);

                        go.AddComponent<NavMeshObstacle>();
                        go.GetComponent<NavMeshObstacle>().carving = true;
                        go.AddComponent<BoxCollider>();
                    }
                }
                else
                {
                    aiPoints.Add(new Vector3((-(NumOfSideWalls / 2f) + i) * wallSize.x, wallSize.y / 2, (-(NumOfSideWalls / 2f) + j) * wallSize.z));
                }
            }
        }
    }

    void MakeEnemys(int amount, GameObject type)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(type, aiPoints[Random.Range(0, aiPoints.Count)], Quaternion.identity) as GameObject;
            go.GetComponent<AIBehavior>().patrolPos = aiPoints;
        }
    }
}
