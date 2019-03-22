using UnityEngine;

namespace Auxiliary {
    public class AuxTrigger2D : MonoSingleton<AuxTrigger2D>
    {
        public delegate void Callback(Collider2D other);

        public event Callback enterCallback;
        public event Callback stayCallback;
        public event Callback exitCallback;
        
        void OnTriggerEnter2D(Collider2D other) {
            if(enterCallback != null)
                enterCallback(other);
        }

        void OnTriggerStay2D(Collider2D other) {
            if(stayCallback != null)
                stayCallback(other);
        }

        void OnTriggerExit2D(Collider2D other) {
            if(exitCallback != null)
                exitCallback(other);
        }
    }
}