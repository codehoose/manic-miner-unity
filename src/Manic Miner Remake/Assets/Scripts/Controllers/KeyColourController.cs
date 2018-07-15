using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
internal class KeyColourController : MonoBehaviour
{
    private int keyColourIndex;

    private List<byte[]> keyColours = new List<byte[]>
    {
        new byte[] { 3, 6, 5, 4 },
        new byte[] { 6, 5, 4, 3 },
        new byte[] { 5, 4, 3, 6 },
        new byte[] { 4, 3, 6, 5 }
    };

    IEnumerator Start()
    {
        var gameController = GetComponent<GameController>();
        while (!gameController.IsReady)
            yield return null;

        var roomKeys = gameController.RoomKeys;

        while (gameController.RenderRoom)
        {
            for (int i = 0; i < roomKeys.Count; i++)
            {
                var listIndex = i % keyColours.Count;
                roomKeys[i].Attr = keyColours[listIndex][keyColourIndex];
            }

            keyColourIndex++;
            keyColourIndex %= 4;

            yield return new WaitForSeconds(gameController.EnvironmentSpeed);
        }
    }
}