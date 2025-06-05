using Hapiga.Core.Runtime.Utils;
using UnityEngine;

namespace Hapiga.Core.Runtime.Process
{
    public class TranformInterpolateProcess : Process
    {
        private Transform movetransform;
        private Vector3 startPos, endPos;
        private float speed, timer;
        private float journeyLength, distCovered, fracJourney;
        public Vector3 CurrentPosition { get; set; }
        public TranformInterpolateProcess()
        {
            startPos = Vector3.zero;
            endPos = Vector3.zero;
            CurrentPosition = Vector3.zero;
        }

        public override void OnBegin()
        {
        }

        public override void OnTerminate()
        {
        }

        public override void Update(float dt)
        {
            timer += dt;
            distCovered = timer * speed;
            fracJourney = distCovered / journeyLength;
            movetransform.position = Vector3.Lerp(startPos, endPos, fracJourney);
            if (fracJourney >= 1)
            {
                Terminate();
            }
        }

        public void SetInfo(Transform _transform, Vector3 _startPosition, Vector3 _endPosition, float _speed)
        {
            startPos = _startPosition;
            endPos = _endPosition;
            journeyLength = Vector3Utils.FastDistance(ref startPos, ref endPos);
            movetransform = _transform;
            speed = _speed;
            timer = 0;
        }

        public override void Pause(bool isPause)
        {		
        }
    }
}
