using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

namespace Statement
{
    public class SingleState : BattleState
    {
        protected Dictionary<string, EcsPackedEntity> dictionaryEntities;

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

        protected override void Awake()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDestroy()
        {
            throw new System.NotImplementedException();
        }

        protected override void Start()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}