using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class Timer {

            private float _elapsedSeconds;
            private float _secondsToCountTo;

            public Timer(float secondsToCountTo) {
                if (secondsToCountTo <= 0) {
                    throw new Exception("Timer secondsToCountTo must be positive");
                }
                _elapsedSeconds = 0.0f;
                _secondsToCountTo = secondsToCountTo;
            }

            public void Reset() {
                _elapsedSeconds = 0;
            }

            public void Update(float deltaTime) {
                _elapsedSeconds += deltaTime;
            }

            public bool isFinished {
                get {
                    return _elapsedSeconds >= _secondsToCountTo;
                }
            }

            public float ElapsedSeconds {
                get { return _elapsedSeconds; }
            }
            public float SecondsToCountTo {
                get { return _secondsToCountTo; }
            }

            public float percentComplete {
                get {
                    float percent = _elapsedSeconds / _secondsToCountTo;
                    return percent > 1.0f ? 1.0f : percent;
                } 
            }

    }
}
