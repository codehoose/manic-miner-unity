using System.Collections;
using UnityEngine;

public class BiDirectionalSpriteController : MonoBehaviour
{
    IEnumerator Start()
    {
        var gameController = GetComponent<GameController>();
        while (!gameController.IsReady)
            yield return null;

        if (!gameController.IsBidirectionalSpriteRoom)
            yield break;

        var roomData = gameController.RoomData;

        foreach (var m in gameController.Mobs)
        {
            m.FrameDirection = m.Frame < 4 ? 1 : -1;
        }

        while (gameController.IsPlaying)
        {
            yield return new WaitForSeconds(0.1f);

            foreach (var m in gameController.Mobs)
            {
                m.Frame += m.FrameDirection;

                // is the sprite heading left to right?
                if (m.FrameDirection > 0 && m.Frame > 3)
                {
                    m.Frame = 0;
                    m.X += m.FrameDirection;

                    // Have they reached the end?
                    if (m.X > m.Right)
                    {
                        m.X = m.Right;
                        m.FrameDirection *= -1;
                        m.Frame = 7;
                    }
                }

                // the sprite is heading right to left
                if (m.FrameDirection < 0 && m.Frame < 4)
                {
                    m.Frame = 7;
                    m.X += m.FrameDirection;

                    if (m.X < m.Left)
                    {
                        m.X = m.Left;
                        m.FrameDirection *= -1;
                        m.Frame = 0;
                    }
                }
            }
        }
    }
}