using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Linq;

public class RoomFirstDungeonGenerator : SimpleRandomWalkGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    public Tilemap floorTilemap, wallTilemap;

    public Transform parentObject;

    public GameObject playerPrefab;
    public GameObject slime;
    public GameObject mage;
    public GameObject slime1;
    public GameObject zombie;

    public Transform enemies;

    public PlayerHealth playerHealth;

    public GameObject player;

    public int numberOfEnemies = 10;

    private HashSet<Vector2Int> floorPositions, corridorPositions;

    protected override void RunProceduralGeneration()
    {
        HealPlayer();
        DestroyAllEnemies();
        CreateRooms();
    }

    private Vector2Int GetRandomFloorPosition(HashSet<Vector2Int> floorPositions)
    {
        if (floorPositions == null || floorPositions.Count == 0)
        {
            Debug.LogWarning("Floor positions not provided or empty.");
            return Vector2Int.zero;
        }

        List<Vector2Int> floorList = new List<Vector2Int>(floorPositions);

        int randomIndex = Random.Range(0, floorList.Count);

        return floorList[randomIndex];
    }

    private void HealPlayer()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.Heal();
    }

    public void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            DestroyImmediate(enemy);
        }

        GameObject[] wizards = GameObject.FindGameObjectsWithTag("Mage");

        foreach (GameObject wizard in wizards)
        {
            DestroyImmediate(wizard);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject bullet in bullets)
        {
            DestroyImmediate(bullet);
        }
    }

    private Vector3 EnemySpawnCalulations(Tilemap floorTilemap, Vector2Int randomPosition)
    {
        Vector3Int spawnPositionCell = floorTilemap.WorldToCell((Vector3Int)randomPosition);
        Vector3Int worldOffset = spawnPositionCell - (Vector3Int)randomPosition;
        spawnPositionCell = floorTilemap.WorldToCell((Vector3Int)randomPosition) - worldOffset;
        Vector3 spawnPosition = floorTilemap.GetCellCenterWorld((Vector3Int)spawnPositionCell);
        Vector3 trueSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        return trueSpawnPosition;
    }

    private void CreateRooms()
    {
        var roomsList = GenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        Vector2Int playerSpawn = new Vector2Int();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
            playerSpawn = PlayerSpawn(roomsList.Select(room => (Vector2Int)Vector3Int.RoundToInt(room.center)).ToList());
            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
            if(existingPlayer == null)
            {
                GameObject newPlayer = Instantiate(playerPrefab, (Vector3Int)playerSpawn, Quaternion.identity);
                Vector3 newPosition = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -1f);
                newPlayer.transform.position = newPosition;
                Debug.Log(newPosition);
                newPlayer.tag = "Player";
            }
            else if (existingPlayer != null)
            {
                existingPlayer.SetActive(true);
                Vector3 playerSpawn1 = parentObject.TransformPoint(playerSpawn.x, playerSpawn.y, 0f);
                existingPlayer.transform.position = (Vector3)playerSpawn1;
                Debug.Log(playerSpawn);
                Debug.Log(existingPlayer.transform.position);
                existingPlayer = GameObject.FindGameObjectWithTag("Player");
                if ((Vector3Int)playerSpawn != existingPlayer.transform.position)
                {
                    Debug.Log((Vector3Int)playerSpawn - existingPlayer.transform.position);
                }
                existingPlayer.SetActive(true);
            }
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector2Int randomPosition = GetRandomFloorPosition(floor);
                //type 0
                Vector3 enemySpawn = EnemySpawnCalulations(floorTilemap, randomPosition);
                GameObject newEnemy = Instantiate(slime, enemySpawn, Quaternion.identity);
                newEnemy.tag = "Enemy";
                Vector3 newPosition = new Vector3(newEnemy.transform.position.x, newEnemy.transform.position.y, -1f);
                newEnemy.transform.position = newPosition;
                newEnemy.layer = 7;
                newEnemy.transform.SetParent(enemies);

                Vector2Int randomPosition1 = GetRandomFloorPosition(floor);
                //type 1
                Vector3 enemySpawn1 = EnemySpawnCalulations(floorTilemap, randomPosition1);
                GameObject newEnemy1 = Instantiate(mage, enemySpawn1, Quaternion.identity);
                newEnemy1.tag = "Enemy";
                Vector3 newPosition1 = new Vector3(newEnemy1.transform.position.x, newEnemy1.transform.position.y, -1f);
                newEnemy1.transform.position = newPosition1;
                newEnemy1.layer = 7;
                newEnemy1.transform.SetParent(enemies);

                Vector2Int randomPosition2 = GetRandomFloorPosition(floor);
                //type2
                Vector3 enemySpawn2 = EnemySpawnCalulations(floorTilemap, randomPosition2);
                GameObject newEnemy2 = Instantiate(slime1, enemySpawn2, Quaternion.identity);
                newEnemy2.tag = "Enemy";
                Vector3 newPosition2 = new Vector3(newEnemy2.transform.position.x, newEnemy2.transform.position.y, -1f);
                newEnemy2.transform.position = newPosition2;
                newEnemy2.layer = 7;
                newEnemy2.transform.SetParent(enemies);

                Vector2Int randomPosition3 = GetRandomFloorPosition(floor);
                //type 3
                Vector3 enemySpawn3 = EnemySpawnCalulations(floorTilemap, randomPosition3);
                GameObject newEnemy3 = Instantiate(zombie, enemySpawn3, Quaternion.identity);
                newEnemy3.tag = "Enemy";
                Vector3 newPosition3 = new Vector3(newEnemy3.transform.position.x, newEnemy3.transform.position.y, -1f);
                newEnemy3.transform.position = newPosition3;
                newEnemy3.layer = 7;
                newEnemy3.transform.SetParent(enemies);
                /*Debug.Log($"Slime{newPosition}   {newEnemy.transform.position}");
                Debug.Log($"Mage{newPosition1}   {newEnemy1.transform.position}");
                Debug.Log($"Slime{newPosition2}   {newEnemy2.transform.position}");
                Debug.Log($"Zombie{newPosition3}  {newEnemy3.transform.position}");*/
            }
        }

        List<Vector2Int> roomCenters = roomsList.Select(room => (Vector2Int)Vector3Int.RoundToInt(room.center)).ToList();
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        corridorPositions = new HashSet<Vector2Int>(corridors);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private Vector2Int PlayerSpawn(List<Vector2Int> roomCenters)
    {
        var playerSpawn = roomCenters[Random.Range(0, roomCenters.Count)];
        return playerSpawn;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}