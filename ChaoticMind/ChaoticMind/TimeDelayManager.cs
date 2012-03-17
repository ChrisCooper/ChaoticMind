using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {

    //keeps track of timed events (reloading/shoooting/shifting)

    class TimeDelayManager {

        //stores a timer events data
        private class TimerEvent {

            private float _elapsed;
            private float _delayTime;
            
            public TimerEvent(float delay, float elapsed) {
                if (delay == 0)
                    throw new Exception("Delay amount cannot be 0");
                _elapsed = elapsed;
                _delayTime = delay;
            }

            //reset/modify the event timer
            public void Reset() {
                _elapsed = 0;
            }
            public void Reset(float delay) {
                if (delay == 0)
                    throw new Exception("Delay amount cannot be 0");
                _elapsed = 0;
                _delayTime = delay;
            }

            //add time to the timer
            public void AddTime(float time) {
                //don't bother updating if it's already there
                if (_elapsed < _delayTime)
                    _elapsed += time;
            }

            //accessors
            public float ElapsedTime {
                get { return _elapsed; }
            }
            public float DelayTime {
                get { return _delayTime; }
            }

        }

        //self reference
        static TimeDelayManager _self;

        //store the timer events
        List<TimerEvent> _timerEvents;

        //init the static class
        public static void Initilize() {
            _self = new TimeDelayManager();
            _self._timerEvents = new List<TimerEvent>(50); //many projectiles
        }

        //update every event's timing
        public static void Update(float deltaTime){
            foreach (TimerEvent t in _self._timerEvents) {
                if (t != null)
                    t.AddTime(deltaTime);
            }
        }

        //start a new timer and return the timer ID
        public static int InitTimer(float delay) {
            return InitTimer(delay, delay);
        }
        public static int InitTimer(float delay, float start) {
            //reuse a deleted timer
            for (int i = 0; i < _self._timerEvents.Count; i++) {
                if (_self._timerEvents[i] == null) {
                    _self._timerEvents[i] = new TimerEvent(delay, start);
                    return i;
                }
            }
            //no null timers, make a new one
            _self._timerEvents.Add(new TimerEvent(delay, start));
            return _self._timerEvents.Count - 1;
        }

        //remove a timer
        public static void DeleteTimer(int eventId) {
            _self._timerEvents[eventId] = null;
        }

        //modify/restart an existing timer
        public static void RestartTimer(int eventId) {
            if (_self._timerEvents[eventId] != null) {
                _self._timerEvents[eventId].Reset();
            }
            else {
                throw new Exception("Invalid timer ID");
            }
        }
        public static void RestartTimer(int eventId, float delay) {
            if (_self._timerEvents[eventId] != null) {
                _self._timerEvents[eventId].Reset(delay);
            }
            else {
                throw new Exception("Invalid timer ID");
            }
        }

        //return the percent a timer is finished
        public static float Percent(int eventId) {
            if (_self._timerEvents[eventId] != null) {
                float temp = _self._timerEvents[eventId].ElapsedTime / _self._timerEvents[eventId].DelayTime;
                return temp > 1 ? 1 : temp;
            }
            else {
                throw new Exception("Invalid timer ID");
            }
        }

        //return if the timer is finished or not
        public static bool Finished(int eventId) {
            if (_self._timerEvents[eventId] != null) {
                return _self._timerEvents[eventId].ElapsedTime >= _self._timerEvents[eventId].DelayTime;
            }
            else {
                throw new Exception("Invalid timer ID");
            }
        }
    }
}
