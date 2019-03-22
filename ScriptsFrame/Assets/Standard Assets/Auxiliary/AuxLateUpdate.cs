using UnityEngine;

namespace Auxiliary {
    public class AuxLateUpdate : MonoSingleton<AuxLateUpdate>
    {
        public delegate void Callback();

        public event Callback callback;

        void OnDestroy() {
            callback = null;
        }

        void LateUpdate() {
            if(callback != null)
                callback();
        }
    }
}