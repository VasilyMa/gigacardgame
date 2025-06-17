using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;

namespace Statement
{
    public abstract class BattleState : State
    {
        public EcsRunHandler EcsRunHandler;

        public abstract void AddEntity(string key, int entity);

        public abstract bool TryGetEntity(string key, out EcsPackedEntity packedEntity);

        public abstract bool TryGetEntity(string key, out int unpackedEntity);
    }
}