using UnityEngine;

namespace MonkeTV.Behaviours
{
    public class Slider : MonoBehaviour
    {
        internal float yPos;
        public float mPlier;
        public float fValue;

        internal void Start() => yPos = transform.position.y;
        internal void LateUpdate() => fValue = 1 - ((yPos - transform.position.y) * mPlier);

        public float _Position(float _fValue, float bigValue, float smallValue)
        {
            return (bigValue - smallValue) * _fValue;
        }
    }

}
