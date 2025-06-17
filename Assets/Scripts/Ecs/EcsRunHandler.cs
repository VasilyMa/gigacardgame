using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public abstract class EcsRunHandler
    {
        protected readonly EcsWorld _world;
        protected readonly EcsSystems _systems;
        protected readonly EcsData _data;
        public EcsWorld World { get; }
        public EcsRunHandler()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _data);
        }

        public abstract void Init();   
        public abstract void Run();
        public abstract void Dispose();
    }
}

public class EcsData
{

}