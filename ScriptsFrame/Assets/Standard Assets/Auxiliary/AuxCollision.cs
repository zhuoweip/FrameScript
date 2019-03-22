using UnityEngine;

namespace Auxiliary {
    public class AuxCollision : MonoSingleton<AuxCollision> {
        public delegate void Callback(Collision coll);

        public event Callback enterCallback;
        public event Callback stayCallback;
        public event Callback exitCallback;
        
        void OnCollisionEnter(Collision coll) {
            if(enterCallback != null)
                enterCallback(coll);
        }

        void OnCollisionStay(Collision coll) {
            if(stayCallback != null)
                stayCallback(coll);
        }

        void OnCollisionExit(Collision coll) {
            if(exitCallback != null)
                exitCallback(coll);
        }
    }
}