﻿using UnityEngine;

namespace Auxiliary {
    public class AuxTrigger : MonoSingleton<AuxTrigger>
    {
        public delegate void Callback(Collider other);

        public event Callback enterCallback;
        public event Callback stayCallback;
        public event Callback exitCallback;
        
        void OnTriggerEnter(Collider other) {
            if(enterCallback != null)
                enterCallback(other);
        }

        void OnTriggerStay(Collider other) {
            if(stayCallback != null)
                stayCallback(other);
        }

        void OnTriggerExit(Collider other) {
            if(exitCallback != null)
                exitCallback(other);
        }
    }
}