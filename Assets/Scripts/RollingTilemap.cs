using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RollingTilemap : MonoBehaviour
{
    Tilemap __tilemap;
    [SerializeField] TileBase[] __spawnableTile;
    

    const int __HALF_WIDTH = 10;
    const int __HALF_HEIGHT = 8;
    const float __SPAWN_TILE_RATE = 0.99f;
    const float __TILE_CORRUPTION_RATE = 0.1f;
    int __rollCount = 0;
    bool __isRolling = false;
    public bool IsRolling {  get { return __isRolling; } }
    float __rollingTime = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        __tilemap = GetComponent<Tilemap>();
        InitMap();
    }
    
    void SpawnRandomTileOnRow( int row )
    {
            for (int x = -__HALF_WIDTH; x <= __HALF_WIDTH; x++)
            {
                if (Random.Range(0f, 1f) < __SPAWN_TILE_RATE)
                {
                    __tilemap.SetTile(new Vector3Int(x, row), __spawnableTile[Random.Range(0, __spawnableTile.Length)]);
                }
            }

    }

    void InitMap()
    {
        for (int y = -__HALF_HEIGHT; y <= __HALF_HEIGHT; y++)
        {
            SpawnRandomTileOnRow(y);
        }
    }

    public void RollDown()
    {
        ++__rollCount;
        StartCoroutine(RollingDownSmooth(transform.position - new Vector3(0, 1)));
        SpawnRandomTileOnRow(__HALF_HEIGHT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator RollingDownSmooth( Vector3 dist )
    {
        __isRolling = true;
        float startTime = Time.time;
        while( Time.time - startTime < __rollingTime )
        {
            transform.position = Vector3.Lerp(transform.position, dist, Time.time - startTime);
            yield return null;
        }
        transform.position = new Vector3();
        for (int y = -__HALF_HEIGHT; y <= __HALF_HEIGHT; y++)
        {
            for (int x = -__HALF_WIDTH; x <= __HALF_WIDTH; x++)
            {
                __tilemap.SetTile(new Vector3Int(x, y), __tilemap.GetTile(new Vector3Int(x, y + 1)));
            }
        }
        __isRolling = false;
    }
}
