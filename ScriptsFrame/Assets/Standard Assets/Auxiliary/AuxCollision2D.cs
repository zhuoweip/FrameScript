using UnityEngine;

namespace Auxiliary {
    public class AuxCollision2D : MonoSingleton<AuxCollision2D>
    {
        public delegate void Callback(Collision2D coll);

        public event Callback enterCallback;
        public event Callback stayCallback;
        public event Callback exitCallback;
        
        void OnCollisionEnter2D(Collision2D coll) {
            if(enterCallback != null)
                enterCallback(coll);
        }

        void OnCollisionStay2D(Collision2D coll) {
            if(stayCallback != null)
                stayCallback(coll);
        }

        void OnCollisionExit2D(Collision2D coll) {
            if(exitCallback != null)
                exitCallback(coll);
        }
    }
}