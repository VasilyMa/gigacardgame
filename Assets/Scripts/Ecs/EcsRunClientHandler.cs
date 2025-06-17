using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    /// <summary>
    /// this ecs handler is needed for the client's systems only
    /// </summary>
    public class EcsRunClientHandler : EcsRunHandler
    {
        public override void Init()
        {
            //_systems
            //    .Add();

#if UNITY_EDITOR
            _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif

            _systems.Inject();
            _systems.Init();
        }

        public override void Run()
        {
            _systems.Run();
        }
        public override void Dispose()
        {
            _systems.Destroy();
        }

    }
}