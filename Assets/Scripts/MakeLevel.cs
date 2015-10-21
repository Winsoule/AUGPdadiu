using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshMaker))]
public class MakeLevel : MonoBehaviour {

    public int spawnAmount;
    MeshMaker meshMaker;
    public GameObject enemy,boss, slasher;
    public Transform levelFloor;
    public float wallHight;
    public int numRoomsProcent = 10;
    public int numOfSideWalls = 100;
    public Material wallMat, portalMat, black;
    public Transform cam;
    public List<Vector3> aiPoints = new List<Vector3>();
    public List<GameObject> bosses = new List<GameObject>();
    public List<GameObject> enemys = new List<GameObject>();

    UnitManager manager;
    Vector3 levelSize;
    List<Vector3> objects = new List<Vector3>();

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("GameManager").GetComponent<UnitManager>();
        spawnAmount = manager.spawnAmount;
        numRoomsProcent = manager.levelProcent;
        numOfSideWalls = manager.levelSize;
        levelSize = levelFloor.GetComponent<Renderer>().bounds.size;
        meshMaker = transform.GetComponent<MeshMaker>();
        
        GenerateLevel(((int)(numOfSideWalls * numOfSideWalls * (numRoomsProcent / 100f))), ((int)(numOfSideWalls * numOfSideWalls * (numRoomsProcent / 100f) * 10)), levelSize);
        MakeEnemys(1 * transform.GetComponent<UnitManager>().level, boss, true);
        MakeEnemys(spawnAmount / 2 * transform.GetComponent<UnitManager>().level, enemy, false);
        MakeEnemys(spawnAmount / 2 * transform.GetComponent<UnitManager>().level, slasher, false);
    }

    void GenerateLevel(int maxRooms, int maxTries, Vector3 levelSize)
    {
        Vector3 wallSize = new Vector3(levelSize.x / numOfSideWalls, wallHight, levelSize.z / numOfSideWalls);
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

        while (maxRooms > rooms && maxTries > tries)
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
        }

        Debug.Log("Found " + rooms + " rooms in " + tries + " tries.");
        
        for (int i = 0; i < internalSizeX; i++)
        {
            for (int j = 0; j < internalSizeZ; j++)
            {
                if (!tiles[i, j])
                {

                    if (i != 0 && i != internalSizeX - 1 && j != 0 && j != internalSizeZ - 1)
                    {
                        if (tiles[i + 1, j] || tiles[i - 1, j] || tiles[i, j + 1] || tiles[i, j - 1])
                        {
                            GameObject go = meshMaker.Box(wallMat);
                            go.transform.position = new Vector3((-(internalSizeX / 2f) + i) * wallSize.x + wallSize.x / 2, wallSize.y / 2f, (-(internalSizeZ / 2f) + j) * wallSize.z + wallSize.z / 2f);
                            go.transform.localScale = wallSize;
                            go.transform.SetParent(level.transform);

                            go.AddComponent<NavMeshObstacle>();
                            go.GetComponent<NavMeshObstacle>().carving = true;
                            go.AddComponent<BoxCollider>();
                            go.GetComponent<BoxCollider>().size = new Vector3(go.GetComponent<BoxCollider>().size.x, go.GetComponent<BoxCollider>().size.y + 10f, go.GetComponent<BoxCollider>().size.z);
                        }
                        else
                        {
                            GameObject go = meshMaker.Box(wallMat);
                            go.transform.position = new Vector3((-(internalSizeX / 2f) + i) * wallSize.x + wallSize.x / 2, wallSize.y / 2f, (-(internalSizeZ / 2f) + j) * wallSize.z + wallSize.z / 2f);
                            go.transform.localScale = wallSize;
                            go.transform.SetParent(level.transform);
                        }
                    }
                    else
                    {
                        GameObject go = meshMaker.Box(wallMat);
                        go.transform.position = new Vector3((-(internalSizeX / 2f) + i) * wallSize.x + wallSize.x / 2, wallSize.y / 2f, (-(internalSizeZ / 2f) + j) * wallSize.z + wallSize.z / 2f);
                        go.transform.localScale = wallSize;
                        go.transform.SetParent(level.transform);

                        go.AddComponent<NavMeshObstacle>();
                        go.GetComponent<NavMeshObstacle>().carving = true;
                        go.AddComponent<BoxCollider>();
                        go.GetComponent<BoxCollider>().size = new Vector3(go.GetComponent<BoxCollider>().size.x, go.GetComponent<BoxCollider>().size.y + 10f, go.GetComponent<BoxCollider>().size.z);
                    }
                }
                else if ((i == internalSizeX / 2 + 1 && j == internalSizeZ / 2 + 1) || (i == internalSizeX / 2 - 1 && j == internalSizeZ / 2)
                    || (i == internalSizeX / 2 && j == internalSizeZ / 2 - 1) || (i == internalSizeX / 2 - 1 && j == internalSizeZ / 2 - 1)
                    || (i == internalSizeX / 2 - 1 && j == internalSizeZ / 2 + 1) || (i == internalSizeX / 2 + 1 && j == internalSizeZ / 2 - 1)
                    || (i == internalSizeX / 2 && j == internalSizeZ / 2 + 1) || (i == internalSizeX / 2 + 1 && j == internalSizeZ / 2))
                       
                {

                }
                else
                {
                    aiPoints.Add(new Vector3((-(numOfSideWalls / 2f) + i) * wallSize.x + wallSize.x / 2, wallSize.y / 2, (-(numOfSideWalls / 2f) + j) * wallSize.z + wallSize.z / 2));
                }
            }
        }
    }

    void MakeEnemys(int amount, GameObject type, bool isBoss)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(type, aiPoints[Random.Range(0, aiPoints.Count)], Quaternion.identity) as GameObject;
            if(type.GetComponent<LaserBossBehavior>() == true)
            {
                go.GetComponent<LaserBossBehavior>().patrolPos = aiPoints;
                go.GetComponent<Health>().isBoss = isBoss;
            }
            else if (type != slasher)
            {
                go.GetComponent<AIBehavior>().patrolPos = aiPoints;
                go.GetComponent<Health>().isBoss = isBoss;
            }
            else
            {
                go.GetComponent<AISlasherBehavior>().patrolPos = aiPoints;
                go.GetComponent<Health>().isBoss = isBoss;
            }
            if(isBoss)
            {
                bosses.Add(go);
            }
            else
            {
                enemys.Add(go);
            }
        }
    }
}
