using UnityEngine;

namespace Auxiliary {
    public class AuxFixedUpdate : MonoSingleton<AuxFixedUpdate>
    {
        public delegate void Callback();

        public event Callback callback;

        void OnDestroy() {
            callback = null;
        }

        void FixedUpdate() {
            if(callback != null)
                callback();
        }
    }
}