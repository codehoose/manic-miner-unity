using UnityEngine;
using Com.SloanKelly.ZXSpectrum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RoomStore))]
[RequireComponent(typeof(SpectrumScreen))]
[RequireComponent(typeof(RoomRenderer))]
public class GameController : MonoBehaviour, IScoreInformation
{
    enum GameState
    {
        Playing,
        MoveToNextCavern,
        Dead
    }

    const float ENVIRONMENT_SPEED = 0.1f;
    const float CONVEYOR_SPEED = 0.025f;
    const float MINER_WILLY_SPEED = 0.1f;

    // Private member fields
    private int score = 0;
    private int hiScore = 100;
    
    private RoomData roomData;
    private GameState state;
    private Mob minerWilly;
    private List<Mob> mobs = new List<Mob>();
    private byte[] keyColours = new byte[] { 3, 6, 6, 4 };
    private int currentKeyColour = 0;
    
    public bool isDemoMode = true; // TODO : CHANGE THIS!!

    public Camera mainCamera;

    // Public member fields

    [Tooltip("The room number (0-19, -1 to start at room 0 (Central Cavern))")]
    public int roomId;
    
    public int Score { get { return score; } }

    public int HiScore { get { return hiScore; } }

    IEnumerator Start()
    {
        if (roomId == -1)
        {
            roomId = 0;
        }
        else
        {
            roomId = PlayerPrefs.GetInt("_room");
            score = PlayerPrefs.GetInt("_score");
        }

        var store = GetComponent<RoomStore>();
        var roomRenderer = GetComponent<RoomRenderer>();

        while (!store.IsReady)
        {
            yield return null;
        }
        
        roomData = store.Rooms[roomId];

        // Get Miner Willy data from store and from the room
        minerWilly = new Mob(store.MinerWillySprites, roomData.MinerWillyStart.X, roomData.MinerWillyStart.Y, 4, 0, 0, 7);

        // Set up the horizontal guardians
        roomData.HorizontalGuardians.ForEach(g => mobs.Add(new Mob(roomData.GuardianGraphics, g.StartX, g.StartY, g.Left, g.Right, g.StartFrame, g.Attribute)));

        // Set up the conveyor shape
        foreach (var block in roomData.Blocks.Values)
        {
            if (block.BlockType == BlockType.Conveyor)
            {
                roomData.ConveyorShape = block.Shape;
                break;
            }
        }

        // Set the border colour
        mainCamera.backgroundColor = ZXColour.Get(roomData.BorderColour);

        SetupRenderer(roomRenderer);

        // HACK: Make portal available
        // REMOVE THIS LATER
        roomData.Portal.Attr.Flashing = true;

        StartCoroutine(DrawScreen(roomRenderer, roomData));
        StartCoroutine(LoseAir(roomData));
        if (!isDemoMode) StartCoroutine(MoveWilly(minerWilly, roomData));
        StartCoroutine(CycleColours(roomData.RoomKeys));
        StartCoroutine(UpdateConveyor(roomData));
        StartCoroutine(CheckPortalCollision(roomData));
        StartCoroutine(EndOfCavernCheck(roomData));

        if (isDemoMode) StartCoroutine(DemoNextSceen());

        if ((roomId>=0 && roomId <=6) || roomId==9 || roomId==15)
        {
            StartCoroutine(BidirectionalSprites());
        }
    }

    private void SetupRenderer(RoomRenderer roomRenderer)
    {
        var tmp = new List<IRenderer>();

        tmp.Add(new MinerWillyRenderer(minerWilly, roomData));
        tmp.Add(new BlockRenderer(roomData));
        tmp.Add(new ItemsRenderer(roomData));
        tmp.Add(new HorizontalGuardianRenderer(roomData, mobs));
        tmp.Add(new PortalRenderer(roomData));
        tmp.Add(new RoomNameRenderer(roomData));
        tmp.Add(new AirSupplyRenderer(roomData));
        tmp.Add(new PlayerScoreRenderer(this));

        roomRenderer.Init(tmp);
    }

    IEnumerator DemoNextSceen()
    {
        yield return new WaitForSeconds(3);
        roomId++;
        if (roomId >= 20)
        {
            roomId = 0;
        }

        PlayerPrefs.SetInt("_room", roomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator EndOfCavernCheck(RoomData roomData)
    {
        while (state == GameState.Playing) yield return null;

        if (state == GameState.MoveToNextCavern)
        {
            yield return MoveToNextCavern(roomData);
        }
        else
        {
            yield return GivePlayerTheBoot(roomData);
        }
    }

    IEnumerator MoveToNextCavern(RoomData roomData)
    {
        while (roomData.AirSupply.Length > 0)
        {
            roomData.AirSupply.Tip = (byte)(roomData.AirSupply.Tip << 1);
            roomData.AirSupply.Tip = (byte)(roomData.AirSupply.Tip & 0xff);

            if (roomData.AirSupply.Length > 0 && roomData.AirSupply.Tip == 0)
            {
                roomData.AirSupply.Length--;
                roomData.AirSupply.Tip = 255;
            }

            score += 10;

            yield return null;
        }

        roomId++;
        PlayerPrefs.SetInt("_room", roomId);
        PlayerPrefs.SetInt("_score", score);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator GivePlayerTheBoot(RoomData roomData)
    {
        yield return null;
    }

    IEnumerator CheckPortalCollision(RoomData roomData)
    {
        while (state == GameState.Playing)
        {
            var willyTouchesPortal = BitCollision.DidCollide2x2(minerWilly.X, minerWilly.Y, minerWilly.Frames[minerWilly.Frame],
                                     roomData.Portal.X, roomData.Portal.Y, roomData.Portal.Shape);

            if (willyTouchesPortal)
            {
                state = GameState.MoveToNextCavern;
            }

            yield return null;
        }
    }

    IEnumerator UpdateConveyor(RoomData roomData)
    {
        while(state == GameState.Playing)
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

    IEnumerator CycleColours(List<RoomKey> roomKeys)
    {
        while(state == GameState.Playing)
        {
            foreach (var key in roomKeys)
            {
                key.Attr = keyColours[currentKeyColour];
            }

            currentKeyColour++;
            currentKeyColour %= keyColours.Length;

            yield return new WaitForSeconds(ENVIRONMENT_SPEED);
        }
    }

    IEnumerator MoveWilly(Mob minerWilly, RoomData data)
    {
        while (state == GameState.Playing)
        {
            int attrRight = data.Attributes[minerWilly.Y * 32 + (minerWilly.X + 1)];
            int attrLeft = data.Attributes[minerWilly.Y * 32 + (minerWilly.X - 1)];

            bool wallToRight = data.Blocks[attrRight].BlockType == BlockType.Wall;
            bool wallToLeft = data.Blocks[attrLeft].BlockType == BlockType.Wall;

            bool didMove = false;

            if (Input.GetKey(KeyCode.W))
            {
                if (minerWilly.Frame > 3)
                {
                    minerWilly.Frame -= 4;
                }

                if (!wallToRight|| (wallToRight && minerWilly.Frame != 3))
                {
                    minerWilly.Frame += 1;
                }
                
                if (minerWilly.Frame > 3)
                {
                    minerWilly.Frame = 0;

                    if (!wallToRight)
                    {
                        minerWilly.X++;
                    }
                }
                didMove = true;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                if (minerWilly.Frame < 4)
                {
                    minerWilly.Frame += 4;
                }

                if (!wallToLeft || (wallToLeft && minerWilly.Frame != 4))
                {
                    minerWilly.Frame -= 1;
                }
                
                if (minerWilly.Frame < 4)
                {
                    minerWilly.Frame = 7;
                    if (!wallToLeft)
                    {
                        minerWilly.X--;
                    }
                }
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

    IEnumerator DrawScreen(RoomRenderer roomRenderer, RoomData roomData)
    {
        while (state == GameState.Playing || state == GameState.MoveToNextCavern)
        {
            roomRenderer.DrawScreen();
            yield return null;
        }
    }

    IEnumerator LoseAir(RoomData roomData)
    {
        while (state == GameState.Playing)
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

    IEnumerator BidirectionalSprites()
    {
        foreach (var m in mobs)
        {
            m.FrameDirection = m.Frame < 4 ? 1 : -1;
        }

        while (state == GameState.Playing)
        {
            yield return new WaitForSeconds(0.1f);

            foreach (var m in mobs)
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
