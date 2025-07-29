using _Project.Scripts.Core.Helper;

namespace _Project.Scripts.Core.Event
{
    public class GameEventManager : Singleton<GameEventManager>
    {
        public BetAreaEvents BetAreaEvents;
        public RouletteEvents RouletteEvents;

        protected override void Awake()
        {
            Configure(config =>
            {
                config.Persist = true;
                config.DestroyOthers = true;
                config.Lazy = true;
            });
            base.Awake();
            BetAreaEvents = new BetAreaEvents();
            RouletteEvents = new RouletteEvents();
        }
    }
}