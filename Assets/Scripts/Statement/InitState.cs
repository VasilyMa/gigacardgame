using UnityEngine.SceneManagement;

namespace Statement
{
    public class InitState : State
    {
        protected override void Awake()
        {

        }


        protected override void Start()
        {
            ConfigModule.Initialize(this, onComplete);
        }
        void onComplete()
        {
            SceneManager.LoadScene(1);
        }

        protected override void Update()
        {
        }
        protected override void OnDestroy()
        {
        }

    }
}