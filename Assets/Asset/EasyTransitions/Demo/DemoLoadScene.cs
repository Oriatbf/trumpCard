using UnityEngine;

namespace EasyTransition
{

    public class DemoLoadScene : MonoBehaviour
    {
        public static DemoLoadScene Inst;

        public TransitionSettings transition;
        public float startDelay;

        private void Awake()
        {
            Inst = this;
        }

        public void LoadScene(string _sceneName)
        {
            TransitionManager.Instance().Transition(_sceneName, transition, startDelay);
        }   
    }

}


