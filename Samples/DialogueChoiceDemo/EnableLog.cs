using System;
using UnityEngine;

namespace Samples
{
    public class EnableLog:MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log($"ENABLE {this}", this);
        }
        
        private void OnDisable()
        {
            Debug.Log($"DISABLE {this}", this);
        }
    }
}