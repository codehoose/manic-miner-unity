using System.Collections;
using UnityEngine;

public class TravelatorController : MonoBehaviour
{
    const float CONVEYOR_SPEED = 0.025f;

    IEnumerator Start()
    {
        var gameController = GetComponent<GameController>();
        while (!gameController.IsReady)
            yield return null;

        var roomData = gameController.RoomData;

        while (gameController.IsPlaying)
        {
            byte[] tmp = roomData.ConveyorShape;

            if (roomData.ConveyorDirection == ConveyorDirection.Left)
            {
                tmp[0] = RotateLeft(tmp[0]);
                tmp[2] = RotateRight(tmp[2]);
            }
            else
            {
                tmp[0] = RotateRight(tmp[0]);
                tmp[2] = RotateLeft(tmp[2]);
            }

            roomData.ConveyorShape = tmp;

            yield return new WaitForSeconds(CONVEYOR_SPEED);
        }
    }

    private byte RotateLeft(byte v)
    {
        byte tmp = (byte)(v & 0x80);
        v = (byte)(v << 1);

        tmp = (byte)(tmp >> 7);
        return (byte)(v | tmp);
    }

    private byte RotateRight(byte v)
    {
        byte tmp = (byte)(v & 1);
        v = (byte)(v >> 1);

        tmp = (byte)(tmp << 7);

        return (byte)(v | tmp);
    }
}