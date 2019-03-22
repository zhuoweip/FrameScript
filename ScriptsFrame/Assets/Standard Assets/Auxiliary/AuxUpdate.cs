using UnityEngine;

namespace Auxiliary {
    public class AuxUpdate : MonoSingleton<AuxUpdate> {
        public delegate void Callback();

        public event Callback callback;

        void OnDestroy() {
            callback = null;
        }

        void Update() {
            if(callback != null)
                callback();
        }
    }
}