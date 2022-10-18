using System;
using System.Diagnostics;
using nanoFramework.Json;
using TestEsp32Thing.Libs.GenericTcpRW;

namespace TestEsp32Thing.Libs.Eventer
{
    public class NanoEventer : IObservable
    {
        public void OnEventFromSource(SeasonableTarget seasonableTarget)
        {
            Debug.WriteLine($"Got event {seasonableTarget.TargetInfo}");
            notifyObservers(seasonableTarget);
        }


        private void notifyObservers(SeasonableTarget target)
        {
            foreach (var observerWithTarget in observers)
            {
                if (observerWithTarget.TargetName == target.TargetInfo)
                    observerWithTarget.Observer.Update(target.ContainedClass);
            }
        }
        

        private class ObserverWithTarget
        {
            public ObserverWithTarget(IObserver observer, string target)
            {
                Observer = observer;
                TargetName = target;
            }

            public string TargetName { get; }
            public IObserver Observer { get; }
        }


        ObserverWithTarget[] observers = new ObserverWithTarget[0];


        public void Subscribe(IObserver observer, string targetName)
        {
            var newObservers = new ObserverWithTarget[observers.Length + 1];
            Array.Copy(observers, newObservers, observers.Length);
            newObservers[observers.Length] = new ObserverWithTarget(observer, targetName);
            observers = newObservers;
        }

        public void Unsubscribe(IObserver observer)
        {
            var newObservers = new ObserverWithTarget[observers.Length - 1];
            var index = 0;
            foreach (var observerWithTarget in observers)
            {
                if (observerWithTarget.Observer != observer)
                {
                    newObservers[index] = observerWithTarget;
                    index++;
                }
            }

            observers = newObservers;
        }

        public void Notify()
        {
            // This will not be implemented, notify is thrown by the source in this implementation
            throw new("Will not be implemented");
        }
    }

    public interface IObserver
    {
        void Update(byte[] value);
    }

    public interface IObservable
    {
        void Subscribe(IObserver observer, string targetName);
        void Unsubscribe(IObserver observer);
        void Notify();
    }
}