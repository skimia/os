using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class AttributeDiscover : IAttributeDiscover
    {
        public AttributeDiscover()
        {
        }

        private Dictionary<Assembly, Dictionary<Type, List<IAttributeDiscoveredHandler>>> _attributeDiscoveredhandlers = new Dictionary<Assembly, Dictionary<Type, List<IAttributeDiscoveredHandler>>>();
        private Dictionary<Assembly, Dictionary<Type, List<IHandler>>> _attributeHandlers = new Dictionary<Assembly, Dictionary<Type, List<IHandler>>>();

        public IEnumerable<IAttributeDiscoveredHandler> GetAttributeDiscoveredHandlers(Type handlerAttributeType)
        {
            foreach (var list in _attributeDiscoveredhandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IAttributeDiscoveredHandler> handlersList;
                if (list.TryGetValue(handlerAttributeType, out handlersList))
                    foreach (var handler in handlersList)
                        yield return handler;
            }

            // note : disabled yet.

            // recursivity to handle message from base class
            //if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
            //    foreach (var handler in GetHandlers(messageType.BaseType, token))
            //    {
            //        if (handler.Attribute.HandleChildMessages)
            //            yield return handler;
            //    }
        }

        public IEnumerable<IHandler> GetHandlers(Type handlerAttributeType)
        {
            foreach (var list in _attributeHandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IHandler> handlersList;
                if (list.TryGetValue(handlerAttributeType, out handlersList))
                    foreach (var handler in handlersList)
                        yield return handler;
            }

            // note : disabled yet.

            // recursivity to handle message from base class
            //if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
            //    foreach (var handler in GetHandlers(messageType.BaseType, token))
            //    {
            //        if (handler.Attribute.HandleChildMessages)
            //            yield return handler;
            //    }
        }

        public void RegisterAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
            {
                RegisterAssembly(assembly);
            }
        }

        public void RegisterAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            foreach (var type in assembly.GetTypes())
            {
                RegisterStaticContainer(type);
            }
        }

        public void RegisterStaticContainer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var methods = type.GetMethods(BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach(var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(AttributeDiscoveredHandlerAttribute), false) as AttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterAttributeDiscoveredHandler(method, null, attributes);
            }

            foreach(var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(HandlerAttribute), false) as HandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterHandler(method, null, attributes); 
            }

        }

        public void RegisterInstanceContainer(object container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var type = container.GetType();

            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(AttributeDiscoveredHandlerAttribute), false) as AttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterAttributeDiscoveredHandler(method, container, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(HandlerAttribute), false) as HandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterHandler(method, container, attributes);
            }
        }

        #region Handler
        private void RegisterHandler(MethodInfo method, object container, HandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            foreach (var attribute in attributes)
            {
                RegisterHandler(method.DeclaringType, method, attribute, method.IsStatic ? null : container);
            }
        }

        private void RegisterHandler(Type containerType, MethodInfo method, HandlerAttribute attribute, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributeHandlers.ContainsKey(assembly))
                _attributeHandlers.Add(assembly, new Dictionary<Type, List<IHandler>>());

            if (!_attributeHandlers[assembly].ContainsKey(attribute.GetType()))
                _attributeHandlers[assembly].Add(attribute.GetType(), new List<IHandler>());

            var handler = new Handler(attribute);

            _attributeHandlers[assembly][attribute.GetType()].Add(handler);

            HandleRegisteredHandler(handler);
        } 
        #endregion

        #region AttributeDiscoveredHandler
        private void RegisterAttributeDiscoveredHandler(MethodInfo method, object container, AttributeDiscoveredHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || (!parameters[0].ParameterType.HasInterface(typeof(IHandler)) && parameters[0].ParameterType != typeof(IHandler))
                || (!parameters[1].ParameterType.GetTypeInfo().IsSubclassOf(typeof(HandlerAttribute)) && parameters[1].ParameterType != typeof(HandlerAttribute))
                )
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(IHandler, HandlerAttribute)", method));
            }

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

            Action<object, IHandler, HandlerAttribute> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, IHandler, HandlerAttribute>)method.CreateParamsDelegate(typeof(IHandler), typeof(HandlerAttribute));
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(IHandler, HandlerAttribute) : {1}", method, e.Message));
            }

            foreach (var attribute in attributes)
            {
                RegisterAttributeDiscoveredHandler(attribute.HandlerAttributeType, method.DeclaringType, attribute, handlerDelegate, method.IsStatic ? null : container);
            }
        }

        private void RegisterAttributeDiscoveredHandler(Type handlerAttributeType, Type containerType, AttributeDiscoveredHandlerAttribute attribute, Action<object, IHandler, HandlerAttribute> action, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributeDiscoveredhandlers.ContainsKey(assembly))
                _attributeDiscoveredhandlers.Add(assembly, new Dictionary<Type, List<IAttributeDiscoveredHandler>>());

            if (!_attributeDiscoveredhandlers[assembly].ContainsKey(handlerAttributeType))
                _attributeDiscoveredhandlers[assembly].Add(handlerAttributeType, new List<IAttributeDiscoveredHandler>());

            var handler = new AttributeDiscoveredHandler(handlerAttributeType, container, containerType, attribute, action);
            _attributeDiscoveredhandlers[assembly][handlerAttributeType].Add(handler);

            if (attribute.HandleAlreadyDiscoveredAttributes)
                HandleBeforeRegisteredHandlers(handlerAttributeType, action, container);
        } 
        #endregion

        private void HandleBeforeRegisteredHandlers(Type handlerAttributeType, Action<object, IHandler, HandlerAttribute> action, object container)
        {
            var handlers = GetHandlers(handlerAttributeType);

            foreach (var handler in handlers)
            {
                action(container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleRegisteredHandler(IHandler handler)
        {
            var discoveredHandlers = GetAttributeDiscoveredHandlers(handler.HandlerAttribute.GetType());

            foreach (var discoveredHandler in discoveredHandlers)
            {
                discoveredHandler.Action(discoveredHandler.Container, handler, handler.HandlerAttribute);
            }
        }
    }
}
