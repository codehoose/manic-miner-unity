public class WillyStateMachine
{
    public Mob Mob { get; private set; }

    public WillyStateMachine(Mob minerWilly)
    {
        Mob = minerWilly;
    }

    public void MoveRight(RoomData data)
    {
        int attrRight = data.Attributes[Mob.Y * 32 + (Mob.X + 1)];
        bool wallToRight = data.Blocks[attrRight].BlockType == BlockType.Wall;

        if (Mob.Frame > 3)
        {
            Mob.Frame -= 4;
        }

        if (!wallToRight || (wallToRight && Mob.Frame != 3))
        {
            Mob.Frame += 1;
        }

        if (Mob.Frame > 3)
        {
            Mob.Frame = 0;

            if (!wallToRight)
            {
                Mob.X++;
            }
        }
    }

    public void MoveLeft(RoomData data)
    {
        int attrLeft = data.Attributes[Mob.Y * 32 + (Mob.X - 1)];
        bool wallToLeft = data.Blocks[attrLeft].BlockType == BlockType.Wall;

        if (Mob.Frame < 4)
        {
            Mob.Frame += 4;
        }

        if (!wallToLeft || (wallToLeft && Mob.Frame != 4))
        {
            Mob.Frame -= 1;
        }

        if (Mob.Frame < 4)
        {
            Mob.Frame = 7;
            if (!wallToLeft)
            {
                Mob.X--;
            }
        }
    }
}