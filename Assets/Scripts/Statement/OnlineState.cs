using System.Collections.Generic;
using Leopotam.EcsLite;
using Client;

using Photon.Pun;

namespace Statement
{
    public class OnlineState : BattleState
    {
        protected Dictionary<string, EcsPackedEntity> dictionaryEntities;
        protected override void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                EcsRunHandler = new EcsRunHostHandler();
            }
            else
            {
                EcsRunHandler = new EcsRunClientHandler();
            }
        }

        protected override void Start()
        {
            EcsRunHandler?.Init();
        }

        protected override void Update()
        {
            EcsRunHandler?.Run();
        }

        protected override void OnDestroy()
        {
            EcsRunHandler?.Dispose();
        }


        public override void AddEntity(string key, int entity)
        {
            if (dictionaryEntities.ContainsKey(key)) return;

            dictionaryEntities.Add(key, EcsRunHandler.World.PackEntity(entity));
        }
        public override bool TryGetEntity(string key, out EcsPackedEntity packedEntity)
        {
            if (dictionaryEntities.ContainsKey(key))
            {
                packedEntity = dictionaryEntities[key];
                return true;
            }

            packedEntity = default(EcsPackedEntity);
            return false;
        }
        public override bool TryGetEntity(string key, out int unpackedEntity)
        {
            if (dictionaryEntities.ContainsKey(key))
            {
                if (dictionaryEntities[key].Unpack(EcsRunHandler.World, out int entity))
                {
                    unpackedEntity = entity;
                    return true;
                }
            }

            unpackedEntity = -1;
            return false;
        }
    }
}