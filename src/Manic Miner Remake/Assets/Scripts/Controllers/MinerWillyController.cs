using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class MinerWillyController : MonoBehaviour
{
    const float MINER_WILLY_SPEED = 0.07f;

    IEnumerator Start()
    {
        var gameController = GetComponent<GameController>();

        while (!gameController.IsReady)
            yield return null;

        var willy = gameController.Willy;
        var minerWilly = gameController.Willy.Mob;
        var data = gameController.RoomData;
        
        while (gameController.IsPlaying)
        {
            bool didMove = false;

            if (Input.GetKey(KeyCode.W))
            {
                willy.MoveRight(data);
                didMove = true;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                willy.MoveLeft(data);
                didMove = true;
            }

            if (didMove)
            {
                yield return new WaitForSeconds(MINER_WILLY_SPEED);
            }
            else
            {
                yield return null;
            }
        }
    }
}