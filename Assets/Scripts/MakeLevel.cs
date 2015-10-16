using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshMaker))]
public class MakeLevel : MonoBehaviour {

    MeshMaker meshMaker;
    public GameObject wall, box;
    public Transform levelFloor;
    public Vector3 wallSize;
    public Vector3 lowWallSize;
    public int numRooms = 10;
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

        //MakePlane(levelSize, 11);
        //MakeWalls(levelSize, wallSize, 100, 100);
        //MakeWalls(levelSize, lowWallSize, 10, 100);
        //OldGenerateLevel(150, 1000);
        GenerateLevel(1, 150, 1500, levelSize);


    }

    // Update is called once per frame
    void Update () {
	
	}

    void MakeWalls(Vector3 levelSize, Vector3 wallDimentions, int amount, int tries)
    {
        for(int i = 0; i<amount;i++)
        {
            GameObject go = meshMaker.Box();
            go.transform.position = new Vector3(Random.Range(-(levelSize.x / 2), levelSize.x/2), wallDimentions.y / 2, Random.Range(-(levelSize.z / 2), levelSize.z/2));
            go.transform.localRotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f)));
            go.transform.localScale = wallDimentions;
            go.AddComponent<NavMeshObstacle>();
            go.GetComponent<NavMeshObstacle>().carving = true;
        }
    }

    void MakeRoom(Vector3 pos, Vector2 roomsize)
    {
        GameObject room = new GameObject("room");

        GameObject wall = meshMaker.Box();
        wall.transform.position = pos + new Vector3(roomsize.x / 2, 0f, 0f);
        wall.transform.SetParent(room.transform);
        wall.transform.localScale = new Vector3(1f, 1.5f, roomsize.x);
        wall.AddComponent<NavMeshObstacle>();
        wall.GetComponent<NavMeshObstacle>().carving = true;

        wall = meshMaker.Box();
        wall.transform.position = pos + new Vector3(-(roomsize.x / 2), 0f, 0f);
        wall.transform.SetParent(room.transform);
        wall.transform.localScale = new Vector3(1f, 1.5f, roomsize.x);
        wall.AddComponent<NavMeshObstacle>();
        wall.GetComponent<NavMeshObstacle>().carving = true;

        wall = meshMaker.Box();
        wall.transform.position = pos + new Vector3(0f, 0f, roomsize.y / 2);
        wall.transform.SetParent(room.transform);
        wall.transform.localScale = new Vector3(roomsize.y, 1.5f, 1f);
        wall.AddComponent<NavMeshObstacle>();
        wall.GetComponent<NavMeshObstacle>().carving = true;

        wall = meshMaker.Box();
        wall.transform.position = pos + new Vector3(0f, 0f, -(roomsize.y / 2));
        wall.transform.SetParent(room.transform);
        wall.transform.localScale = new Vector3(roomsize.y, 1.5f, 1f);
        wall.AddComponent<NavMeshObstacle>();
        wall.GetComponent<NavMeshObstacle>().carving = true;
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
                Instantiate(meshMaker.Box(), new Vector3(currentXCoordinate, 0, currentZCoordinate), Quaternion.identity);
            }
            ties++;
        }
    }

    void GenerateLevel(int roomSize, int maxRooms, int maxTries, Vector3 levelSize)
    {
        bool[,] tiles = new bool[100, 100];
        int rooms = 0;
        int tries = 0;
        int currentXCoordinate = 50;
        int currentYCoordinate = 50;

        
        while (maxRooms > rooms || maxTries > tries)
        {
            if (Random.value > 0.5f) //Vertical or horizontal
            {
                if (Random.value > 0.5f) //Up or down
                {
                    currentYCoordinate = Mathf.Clamp(currentYCoordinate + 1, 1, 98);
                }
                else
                {
                    currentYCoordinate = Mathf.Clamp(currentYCoordinate - 1, 1, 98);
                }
            }
            else if (Random.value > 0.5f) //Left or right
            {
                currentXCoordinate = Mathf.Clamp(currentXCoordinate + 1, 1, 98);
            }
            else
            {
                currentXCoordinate = Mathf.Clamp(currentXCoordinate - 1, 1, 98);
            }

            if(!tiles[currentXCoordinate,currentYCoordinate])
            {
                tiles[currentXCoordinate, currentYCoordinate] = true;
                rooms++;
            }
            tries++;
        }
        /*
        tiles[50, 50] = true;
        tiles[51, 50] = true;
        tiles[50, 51] = true;
        tiles[51, 51] = true;
        */
        for (int i = 0; i < 100; i++)
        {
            for(int j = 0; j < 100; j++)
            {
                if (!tiles[i,j])
                {
                    
                    if (i != 0 && i != 99 && j != 0 && j != 99)
                    {
                        if(tiles[i + 1, j] || tiles[i - 1, j] || tiles[i, j + 1] || tiles[i, j - 1])
                        {
                            GameObject go = meshMaker.Box();
                            go.transform.position = new Vector3(-50f + i, 0.5f, -50f + j);
                        }
                    } 
                    else
                    {
                        GameObject go = meshMaker.Box();
                        go.transform.position = new Vector3(-50f + i, 0.5f, -50f + j);
                    }
                    
                    //GameObject go = meshMaker.Box();
                    //go.transform.position = new Vector3(-50f + i, 0.5f, -50f + j);
                }
            }
        }
    }
}
