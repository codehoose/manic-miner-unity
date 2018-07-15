using UnityEngine;
using Com.SloanKelly.ZXSpectrum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Room.Renderers;

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

    // Private member fields
    private int score = 0;
    private int hiScore = 100;
    
    private RoomData roomData;
    private GameState state;

    private WillyStateMachine willy;

    private List<Mob> mobs = new List<Mob>();
    private byte[] keyColours = new byte[] { 3, 6, 6, 4 };
    private int currentKeyColour = 0;
    
    public bool isDemoMode = true;

    public Camera mainCamera;

    // Public member fields

    [Tooltip("The room number (0-19, -1 to start at room 0 (Central Cavern))")]
    public int roomId;
    
    public List<Mob> Mobs { get { return mobs; } }

    public int Score { get { return score; } }

    public int HiScore { get { return hiScore; } }

    public float EnvironmentSpeed { get { return ENVIRONMENT_SPEED; } }

    public List<RoomKey> RoomKeys { get { return roomData.RoomKeys; } }

    public RoomData RoomData { get { return roomData;} }

    public WillyStateMachine Willy { get { return willy; } }

    public bool IsPlaying { get { return !isDemoMode && state == GameState.Playing; } }

    public bool IsBidirectionalSpriteRoom
    {
        get
        {
            return (roomId >= 0 && roomId <= 6) || roomId == 9 || roomId == 15;
        }
    }

    public bool RenderRoom
    {
        get
        {
            return state == GameState.Playing || 
                   state == GameState.MoveToNextCavern;
        }
    }

    // When true, other components can start
    public bool IsReady { get; private set; }

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
        var willyMob = new Mob(store.MinerWillySprites, roomData.MinerWillyStart.X, roomData.MinerWillyStart.Y, 4, 0, 0, 7);
        willy = new WillyStateMachine(willyMob);

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

        IsReady = true;
        
        StartCoroutine(CheckPortalCollision(roomData));
        StartCoroutine(EndOfCavernCheck(roomData));

        if (isDemoMode) StartCoroutine(DemoNextSceen());

        //if ((roomId >= 0 && roomId <= 6) || roomId == 9 || roomId == 15)
        //{
        //    StartCoroutine(BidirectionalSprites());
        //}
    }

    private void SetupRenderer(RoomRenderer roomRenderer)
    {
        var tmp = RendererFactory.Create(willy.Mob, mobs, roomData, this);
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
            var willyTouchesPortal = BitCollision.DidCollide2x2(willy.Mob.X, willy.Mob.Y, willy.Mob.Frames[willy.Mob.Frame],
                                     roomData.Portal.X, roomData.Portal.Y, roomData.Portal.Shape);

            if (willyTouchesPortal)
            {
                state = GameState.MoveToNextCavern;
            }

            yield return null;
        }
    }
}
