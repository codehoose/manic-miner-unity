using System.Collections.Generic;

namespace Assets.Scripts.Room.Renderers
{
    internal static class RendererFactory
    {
        public static IList<IRenderer> Create(Mob minerWilly, List<Mob> mobs, RoomData roomData, IScoreInformation scoreInformation)
        {
            var tmp = new List<IRenderer>();

            tmp.Add(new MinerWillyRenderer(minerWilly, roomData));
            tmp.Add(new BlockRenderer(roomData));
            tmp.Add(new ItemsRenderer(roomData));
            tmp.Add(new HorizontalGuardianRenderer(roomData, mobs));
            tmp.Add(new PortalRenderer(roomData));
            tmp.Add(new RoomNameRenderer(roomData));
            tmp.Add(new AirSupplyRenderer(roomData));
            tmp.Add(new PlayerScoreRenderer(scoreInformation));

            return tmp;
        }
    }
}
