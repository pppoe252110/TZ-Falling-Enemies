using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Utils
{
    public interface IEventBus<T>
    {
        void Shoot(T argument);
        IDisposable Subscribe(Action<T> handler);
    }
    
    public interface IEventBus<T1, T2>
    {
        void Shoot(T1 argument1, T2 argument2);
        IDisposable Subscribe(Action<T1, T2> handler);
    }
    
    public interface ISignalBus<in TBase>
    {
        void Shoot<T>(T argument)
            where T : TBase;

        IDisposable Subscribe<T>(Action<T> handler)
            where T : TBase;
    }

    public interface ISignalBus 
        : ISignalBus<object>
    {
        
    }
    
    public class SignalBus<TBase>
        : ISignalBus<TBase>
    {
        private readonly IDictionary<Type, ICollection<Action<object>>> _handlers 
            = new Dictionary<Type, ICollection<Action<object>>>(); 
        
        public void Shoot<T>(T argument)
            where T : TBase
        {
            var hasHandlers = _handlers.TryGetValue(typeof(T), out var handlers);
            if (hasHandlers)
            {
                var incoming = new List<Action<object>>(handlers);
                foreach (var handler in incoming)
                {
                    handler.Invoke(argument);
                }
            }
        }

        public IDisposable Subscribe<T>(Action<T> handler)
            where T : TBase
        {
            var result = new Action<object>(value => handler.Invoke((T) value));
            
            var hasHandlers = _handlers.TryGetValue(typeof(T), out var handlers);
            if (hasHandlers)
            {
                handlers.Add(result);
            }
            else
            {
                handlers = new List<Action<object>>();
                handlers.Add(result);
                
                _handlers[typeof(T)] = handlers;
            }
            
            var token = new CallbackDisposable(Remove);
            return token;

            void Remove()
            {
                handlers.Remove(result);
                if (!handlers.Any())
                {
                    _handlers.Remove(typeof(T));
                }
            }
        }
    }
    
    public sealed class SignalBus 
        : SignalBus<object>
    {
    
    }
    

    public sealed class SignalBusToEventBusAdapter<T>
        : IEventBus<T>
    {
        public SignalBusToEventBusAdapter(ISignalBus<T> signalBus)
        {
            _signalBus = signalBus;
        }

        private readonly ISignalBus<T> _signalBus;

        public void Shoot(T argument)
        {
            _signalBus.Shoot(argument);
        }

        public IDisposable Subscribe(Action<T> handler)
        {
            var result = _signalBus.Subscribe(handler);
            return result;
        }
    }
    
    public static class ContextBus<T>
    {
        private static readonly Lazy<ISignalBus> LazySelf
            = new Lazy<ISignalBus>();

        public static ISignalBus Self => LazySelf.Value;
    }

    public static class ContextBus<T, TBase>
    {
        private static readonly Lazy<ISignalBus<TBase>> LazySelf
            = new Lazy<ISignalBus<TBase>>();

        public static ISignalBus<TBase> Self => LazySelf.Value;
    }
    
    public static class StaticEventBus<T>
    {
        private static readonly Lazy<IEventBus<T>> LazySelf
            = new Lazy<IEventBus<T>>(() => new SignalBusToEventBusAdapter<T>(new SignalBus<T>()));

        public static IEventBus<T> Self => LazySelf.Value;
    }

    public static class SignalHelper
    {
        public static IEventBus<T> CreateEventBus<T>()
        {
            var result = new SignalBus<T>()
                .AsEventBus();
            
            return result;
        }
    }

    public static class SignalExtensions
    {
        public static IEventBus<T> AsEventBus<T>(this ISignalBus<T> self)
        {
            var result = new SignalBusToEventBusAdapter<T>(self);
            return result;
        }
    }
}
