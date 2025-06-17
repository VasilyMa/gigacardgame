using UnityEngine;

namespace Statement
{
    public abstract class State : MonoBehaviour
    {
        protected abstract void Awake();
        protected abstract void Start();
        protected abstract void Update();
        protected abstract void OnDestroy();
    }
}