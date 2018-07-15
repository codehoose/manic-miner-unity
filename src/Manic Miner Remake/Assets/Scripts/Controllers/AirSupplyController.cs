using System.Collections;
using UnityEngine;

public class AirSupplyController : MonoBehaviour
{
    IEnumerator Start()
    {
        var gameController = GetComponent<GameController>();
        while (!gameController.IsReady)
            yield return null;

        var roomData = gameController.RoomData;
        
        while (gameController.IsPlaying)
        {
            yield return new WaitForSeconds(1);

            roomData.AirSupply.Tip = (byte)(roomData.AirSupply.Tip << 1);
            roomData.AirSupply.Tip = (byte)(roomData.AirSupply.Tip & 0xff);

            if (roomData.AirSupply.Tip == 0)
            {
                roomData.AirSupply.Length--;
                roomData.AirSupply.Tip = 255;

                // TODO: Fix game over state here...
                //gameOver = roomData.AirSupply.Length < 0;
            }
        }
    }
}
