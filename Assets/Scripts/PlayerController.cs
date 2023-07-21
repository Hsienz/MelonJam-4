using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] RollingTilemap __rollingTilemapScript;
    const float __MOVE_DISTANCE = 1.0f;
    bool __isWalking = false;
    float __walkingSpeed = 30f;
    private void Update()
    {
        if (__isWalking || __rollingTilemapScript.IsRolling ) return;
        bool actioned = false;
        if( Input.GetKeyDown(KeyCode.W) )
        {
            __rollingTilemapScript.RollDown();
            actioned = true;
        }
        if( Input.GetKeyDown(KeyCode.D) )
        {
            Vector2 dist = (Vector2)transform.position + new Vector2(__MOVE_DISTANCE, 0);
            StartCoroutine(SmoothWalk(dist));
            actioned = true;
        }
        if( Input.GetKeyDown(KeyCode.A) )
        {
            Vector2 dist = (Vector2)transform.position - new Vector2(__MOVE_DISTANCE, 0);
            StartCoroutine(SmoothWalk(dist));
            actioned = true;
        }

        if( actioned )
        {
            __rollingTilemapScript.CorruptionDeterioration();
        }

        if( !__rollingTilemapScript.ExistTile(transform.position + new Vector3(0,__rollingTilemapScript.RollCount )) )
        {
            Debug.Log("Game Over");
        }
    }
    IEnumerator SmoothWalk( Vector2 dist )
    {
        __isWalking = true;
        while( !Mathf.Approximately(dist.x, transform.position.x) )
        {
            transform.position = Vector2.Lerp(transform.position, dist, __walkingSpeed * Time.deltaTime);
            yield return null;
        }
        __isWalking = false;
    }
}
