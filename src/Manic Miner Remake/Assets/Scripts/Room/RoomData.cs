using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    int[] _attrs;

    public string RoomName { get; set; }

    public int[] Attributes { get { return _attrs; } }

    public Dictionary<int, BlockData> Blocks { get; private set; }

    public byte[] ConveyorShape { get; set; }

    public List<RoomKey> RoomKeys { get; private set; }

    public List<HorizontalGuardian> HorizontalGuardians { get; private set; }

    public CellPoint MinerWillyStart { get; set; }

    public Portal Portal { get; set; }

    public byte[] KeyShape { get; set; }

    public AirSupply AirSupply { get; set; }

    public List<byte[]> SpecialGraphics { get; set; }

    public List<byte[]> GuardianGraphics { get; set; }

    public ConveyorDirection ConveyorDirection { get; set; }
    public byte BorderColour { get; internal set; }

    public RoomData()
    {
        _attrs = new int[32 * 16];
        Blocks = new Dictionary<int, BlockData>();
        RoomKeys = new List<RoomKey>();
        HorizontalGuardians = new List<HorizontalGuardian>();
        SpecialGraphics = new List<byte[]>();
        GuardianGraphics = new List<byte[]>();
    }

    public void SetAttr(int x, int y, int attr)
    {
        _attrs[y * 32 + x] = attr;
    }

    internal void AddPortal(byte portalColour, byte[] portalShape, int x, int y)
    {
        Portal = new Portal(portalColour, portalShape, x, y);
    }
}
