using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] RollingTilemap __rollingTilemapScript;
    const float __MOVE_DISTANCE = 1.0f;
    bool __isWalking = false;
    float __walkingTime = 0.5f;
    private void Update()
    {
        if (__isWalking || __rollingTilemapScript.IsRolling ) return;
        if( Input.GetKeyDown(KeyCode.W) )
        {
            __rollingTilemapScript.RollDown();
        }
        if( Input.GetKeyDown(KeyCode.S) )
        {
        }
        if( Input.GetKeyDown(KeyCode.D) )
        {
            StartCoroutine(SmoothWalk((Vector2)transform.position + new Vector2(__MOVE_DISTANCE, 0)));
        }
        if( Input.GetKeyDown(KeyCode.A) )
        {
            StartCoroutine(SmoothWalk((Vector2)transform.position - new Vector2(__MOVE_DISTANCE, 0)));
        }
    }
    IEnumerator SmoothWalk( Vector2 dist )
    {
        __isWalking = true;
        float startTime = Time.time;
        while( Time.time - startTime < __walkingTime )
        {
            transform.position = Vector2.Lerp(transform.position, dist, Time.time - startTime);
            yield return null;
        }
        __isWalking = false;
    }
}
