using UnityEngine;
using System.Collections;

namespace Auxiliary {
    [RequireComponent(typeof(Renderer))]
    public class AuxVisible : MonoSingleton<AuxVisible>
    {
        public delegate void Callback();

        public event Callback visibleCallback;
        public event Callback invisibleCallback;

        void OnDestroy() {
            visibleCallback = null;
            invisibleCallback = null;
        }

        void OnBecameInvisible() {
            if(invisibleCallback != null)
                invisibleCallback();
        }

        void OnBecameVisible() {
            if(visibleCallback != null)
                visibleCallback();
        }
    }
}