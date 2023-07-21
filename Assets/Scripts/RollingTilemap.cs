using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RollingTilemap : MonoBehaviour
{
    Tilemap __tilemap;
    [SerializeField] TileBase[] __spawnableTile;
    [SerializeField] Tilemap __corruptionTilemap;
    [SerializeField] List<TileBase> __corruptionTile;
    [SerializeField] Grid __grid;
    

    const int __HALF_WIDTH = 10;
    int __currentHalfWidth = 1;
    const int __HALF_HEIGHT = 6;
    const float __SPAWN_TILE_RATE = 1f;
    float __tileCorruptionRate = 0.01f;
    int __rollCount = 0;
    bool __isRolling = false;
    public bool IsRolling {  get { return __isRolling; } }
    float __rollingSpeed = 30f;

    public int RollCount { get { return __rollCount; } }
   
    void Awake()
    {
        __tilemap = GetComponent<Tilemap>();
        InitMap();
    }
    
    void SpawnRandomTileOnRow( int row )
    {
            for (int x = -__currentHalfWidth; x <= __currentHalfWidth; x++)
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
        __currentHalfWidth = (int)Mathf.Clamp(Mathf.Log(__rollCount,2), __currentHalfWidth, __HALF_WIDTH);
        for (int x = -__HALF_WIDTH; x <= __HALF_WIDTH; x++)
        {
            __tilemap.SetTile(new Vector3Int(x, -__HALF_HEIGHT + __rollCount), null);
            __corruptionTilemap.SetTile(new Vector3Int(x, -__HALF_HEIGHT + __rollCount), null);
        }
        StartCoroutine(RollingDownSmooth(transform.position - new Vector3(0, 1)));
        ++__rollCount;
        SpawnRandomTileOnRow(__HALF_HEIGHT+__rollCount);
        __tileCorruptionRate = Mathf.Min(__tileCorruptionRate + 0.001f, 0.1f);
    }
    public bool ExistTile( Vector3 pos )
    {
        Debug.Log(__grid.WorldToCell(pos));
        return __tilemap.GetTile(__grid.WorldToCell(pos)) != null;
    }
    public bool ExistTile( Vector3Int pos )
    {
        return __tilemap.GetTile(pos) != null;
    }
    public void CorruptionDeterioration()
    {
        for( int y = -__HALF_HEIGHT + __rollCount; y <= __HALF_HEIGHT + __rollCount;y++)
        {
            for( int x = -__HALF_WIDTH; x <= __HALF_WIDTH; x++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                TileBase corruptionTile = __corruptionTilemap.GetTile(new Vector3Int(x, y));
                if (corruptionTile)
                {
                    int currentCorruptionLevel = -1;
                    for( int i = 0; i < __corruptionTile.Count; i ++ )
                    {
                        if( corruptionTile == __corruptionTile[i] )
                        {
                            currentCorruptionLevel = i;
                            break;
                        }
                    }
                    if( currentCorruptionLevel + 1 == __corruptionTile.Count )
                    {
			    __tilemap.SetTile(pos, null);
			    __corruptionTilemap.SetTile(pos, null);
                    }
                    else
                    {
                        __corruptionTilemap.SetTile(pos, __corruptionTile[currentCorruptionLevel + 1]);
                    }
                }
                else if (__tilemap.GetTile(pos) && Random.Range(0f, 1f) < __tileCorruptionRate )
                {
                    __corruptionTilemap.SetTile(pos, __corruptionTile[0]);
                }
            }
        }
    }
    
    IEnumerator RollingDownSmooth( Vector3 dist )
    {
        __isRolling = true;
        while( !Mathf.Approximately(transform.position.y, dist.y)  )
        {
            transform.position = Vector3.Lerp(transform.position, dist, __rollingSpeed * Time.deltaTime);
            yield return null;
        }
        __isRolling = false;
    }
}
