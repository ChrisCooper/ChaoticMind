using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    class Timer {

            private float _elapsedSeconds;
            private float _secondsToCountTo;

            public Timer(float secondsToCountTo) : this(secondsToCountTo, false) {
            }

            public Timer(float secondsToCountTo, bool shouldBeginAtCompletion) {
                if (secondsToCountTo <= 0) {
                    throw new Exception("Timer secondsToCountTo must be positive");
                }

                _secondsToCountTo = secondsToCountTo;
                _elapsedSeconds = shouldBeginAtCompletion ? _secondsToCountTo : 0.0f;
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
