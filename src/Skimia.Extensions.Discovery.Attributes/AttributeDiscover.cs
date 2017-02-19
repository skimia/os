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
        private Dictionary<Assembly, Dictionary<Type, List<IClassAttributeDiscoveredHandler>>> _classAttributeDiscoveredhandlers = new Dictionary<Assembly, Dictionary<Type, List<IClassAttributeDiscoveredHandler>>>();
        private Dictionary<Assembly, Dictionary<Type, List<IClassHandler>>> _attributeClassHandlers = new Dictionary<Assembly, Dictionary<Type, List<IClassHandler>>>();

        private Dictionary<Assembly, Dictionary<Type, List<IPropertyAttributeDiscoveredHandler>>> _propertyAttributeDiscoveredhandlers = new Dictionary<Assembly, Dictionary<Type, List<IPropertyAttributeDiscoveredHandler>>>();
        private Dictionary<Assembly, Dictionary<Type, List<IPropertyHandler>>> _attributePropertyHandlers = new Dictionary<Assembly, Dictionary<Type, List<IPropertyHandler>>>();

        private Dictionary<Assembly, Dictionary<Type, List<IMethodAttributeDiscoveredHandler>>> _methodAttributeDiscoveredhandlers = new Dictionary<Assembly, Dictionary<Type, List<IMethodAttributeDiscoveredHandler>>>();
        private Dictionary<Assembly, Dictionary<Type, List<IMethodHandler>>> _attributeMethodHandlers = new Dictionary<Assembly, Dictionary<Type, List<IMethodHandler>>>();

        public IEnumerable<IMethodAttributeDiscoveredHandler> GetMethodAttributeDiscoveredHandlers(Type handlerAttributeType)
        {
            foreach (var list in _methodAttributeDiscoveredhandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IMethodAttributeDiscoveredHandler> handlersList;
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

        public IEnumerable<IClassAttributeDiscoveredHandler> GetClassAttributeDiscoveredHandlers(Type handlerAttributeType)
        {
            foreach (var list in _classAttributeDiscoveredhandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IClassAttributeDiscoveredHandler> handlersList;
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

        public IEnumerable<IPropertyAttributeDiscoveredHandler> GetPropertyAttributeDiscoveredHandlers(Type handlerAttributeType)
        {
            foreach (var list in _propertyAttributeDiscoveredhandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IPropertyAttributeDiscoveredHandler> handlersList;
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

        public IEnumerable<IMethodHandler> GetMethodHandlers(Type handlerAttributeType)
        {
            foreach (var list in _attributeMethodHandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IMethodHandler> handlersList;
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

        public IEnumerable<IClassHandler> GetClassHandlers(Type handlerAttributeType)
        {
            foreach (var list in _attributeClassHandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IClassHandler> handlersList;
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

        public IEnumerable<IPropertyHandler> GetPropertyHandlers(Type handlerAttributeType)
        {
            foreach (var list in _attributePropertyHandlers.Values.ToArray()) // ToArray : to avoid error if handler are added in the same time
            {
                List<IPropertyHandler> handlersList;
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

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MethodAttributeDiscoveredHandlerAttribute), false) as MethodAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterMethodAttributeDiscoveredHandler(method, null, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ClassAttributeDiscoveredHandlerAttribute), false) as ClassAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterClassAttributeDiscoveredHandler(method, null, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(PropertyAttributeDiscoveredHandlerAttribute), false) as PropertyAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterPropertyAttributeDiscoveredHandler(method, null, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MethodHandlerAttribute), false) as MethodHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterMethodHandler(method, null, attributes); 
            }

            var fields = type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var properties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(PropertyHandlerAttribute), false) as PropertyHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterFieldHandler(field, null, attributes);
            }

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(PropertyHandlerAttribute), false) as PropertyHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterPropertyHandler(property, null, attributes);
            }

            var classAttributes = type.GetTypeInfo().GetCustomAttributes(typeof(ClassHandlerAttribute), false) as ClassHandlerAttribute[];

            if (classAttributes == null || classAttributes.Length == 0)
                return;

            RegisterClassHandler(type, classAttributes, null);
        }

        public void RegisterInstanceContainer(object container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var type = container.GetType();

            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MethodAttributeDiscoveredHandlerAttribute), false) as MethodAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterMethodAttributeDiscoveredHandler(method, container, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ClassAttributeDiscoveredHandlerAttribute), false) as ClassAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterClassAttributeDiscoveredHandler(method, container, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(PropertyAttributeDiscoveredHandlerAttribute), false) as PropertyAttributeDiscoveredHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterPropertyAttributeDiscoveredHandler(method, container, attributes);
            }

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(MethodHandlerAttribute), false) as MethodHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterMethodHandler(method, container, attributes);
            }

            var fields = type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var properties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(PropertyHandlerAttribute), false) as PropertyHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterFieldHandler(field, container, attributes);
            }

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(PropertyHandlerAttribute), false) as PropertyHandlerAttribute[];

                if (attributes == null || attributes.Length == 0)
                    continue;

                RegisterPropertyHandler(property, container, attributes);
            }

            var classAttributes = type.GetTypeInfo().GetCustomAttributes(typeof(ClassHandlerAttribute), false) as ClassHandlerAttribute[];

            if (classAttributes == null || classAttributes.Length == 0)
                return;

            RegisterClassHandler(type, classAttributes, container);
        }

        #region MethodHandler
        private void RegisterMethodHandler(MethodInfo method, object container, MethodHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            foreach (var attribute in attributes)
            {
                RegisterMethodHandler(method.DeclaringType, method, attribute, method.IsStatic ? null : container);
            }
        }

        private void RegisterMethodHandler(Type containerType, MethodInfo method, MethodHandlerAttribute attribute, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributeMethodHandlers.ContainsKey(assembly))
                _attributeMethodHandlers.Add(assembly, new Dictionary<Type, List<IMethodHandler>>());

            if (!_attributeMethodHandlers[assembly].ContainsKey(attribute.GetType()))
                _attributeMethodHandlers[assembly].Add(attribute.GetType(), new List<IMethodHandler>());

            var handler = new MethodHandler(attribute, method, containerType, container);

            _attributeMethodHandlers[assembly][attribute.GetType()].Add(handler);

            HandleRegisteredMethodHandler(handler);
        }
        #endregion

        #region ClassHandler
        private void RegisterClassHandler(Type type, ClassHandlerAttribute[] attributes, object container)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (attributes == null || attributes.Length == 0)
                return;

            foreach (var attribute in attributes)
            {
                RegisterClassHandler(type, attribute, container);
            }
        }

        private void RegisterClassHandler(Type containerType, ClassHandlerAttribute attribute, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributeClassHandlers.ContainsKey(assembly))
                _attributeClassHandlers.Add(assembly, new Dictionary<Type, List<IClassHandler>>());

            if (!_attributeClassHandlers[assembly].ContainsKey(attribute.GetType()))
                _attributeClassHandlers[assembly].Add(attribute.GetType(), new List<IClassHandler>());

            var handler = new ClassHandler(attribute, containerType, container);

            _attributeClassHandlers[assembly][attribute.GetType()].Add(handler);

            HandleRegisteredClassHandler(handler);
        }
        #endregion

        #region FieldHandler
        private void RegisterFieldHandler(FieldInfo method, object container, PropertyHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            foreach (var attribute in attributes)
            {
                RegisterFieldHandler(method.DeclaringType, method, attribute, method.IsStatic ? null : container);
            }
        }

        private void RegisterFieldHandler(Type containerType, FieldInfo method, PropertyHandlerAttribute attribute, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributePropertyHandlers.ContainsKey(assembly))
                _attributePropertyHandlers.Add(assembly, new Dictionary<Type, List<IPropertyHandler>>());

            if (!_attributePropertyHandlers[assembly].ContainsKey(attribute.GetType()))
                _attributePropertyHandlers[assembly].Add(attribute.GetType(), new List<IPropertyHandler>());

            var handler = new PropertyHandler(attribute);

            _attributePropertyHandlers[assembly][attribute.GetType()].Add(handler);

            HandleRegisteredPropertyHandler(handler);
        }
        #endregion
        
        #region PropertyHandler
        private void RegisterPropertyHandler(PropertyInfo method, object container, PropertyHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            foreach (var attribute in attributes)
            {
                RegisterPropertyHandler(method.DeclaringType, method, attribute, container);
            }
        }

        private void RegisterPropertyHandler(Type containerType, PropertyInfo method, PropertyHandlerAttribute attribute, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_attributePropertyHandlers.ContainsKey(assembly))
                _attributePropertyHandlers.Add(assembly, new Dictionary<Type, List<IPropertyHandler>>());

            if (!_attributePropertyHandlers[assembly].ContainsKey(attribute.GetType()))
                _attributePropertyHandlers[assembly].Add(attribute.GetType(), new List<IPropertyHandler>());

            var handler = new PropertyHandler(attribute);

            _attributePropertyHandlers[assembly][attribute.GetType()].Add(handler);

            HandleRegisteredPropertyHandler(handler);
        }
        #endregion

        #region MethodAttributeDiscoveredHandler
        private void RegisterMethodAttributeDiscoveredHandler(MethodInfo method, object container, MethodAttributeDiscoveredHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || (!parameters[0].ParameterType.HasInterface(typeof(IMethodHandler)) && parameters[0].ParameterType != typeof(IMethodHandler))
                || (!parameters[1].ParameterType.GetTypeInfo().IsSubclassOf(typeof(MethodHandlerAttribute)) && parameters[1].ParameterType != typeof(MethodHandlerAttribute))
                )
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(IMethodHandler, MethodHandlerAttribute)", method));
            }

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

            Action<object, IMethodHandler, MethodHandlerAttribute> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, IMethodHandler, MethodHandlerAttribute>)method.CreateParamsDelegate(typeof(IMethodHandler), typeof(MethodHandlerAttribute));
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Method handler {0} has incorrect parameters. Right definition is Handler(IMethodHandler, MethodHandlerAttribute) : {1}", method, e.Message));
            }

            foreach (var attribute in attributes)
            {
                RegisterMethodAttributeDiscoveredHandler(attribute.HandlerAttributeType, method.DeclaringType, attribute, handlerDelegate, method.IsStatic ? null : container);
            }
        }

        private void RegisterMethodAttributeDiscoveredHandler(Type handlerAttributeType, Type containerType, MethodAttributeDiscoveredHandlerAttribute attribute, Action<object, IMethodHandler, MethodHandlerAttribute> action, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_methodAttributeDiscoveredhandlers.ContainsKey(assembly))
                _methodAttributeDiscoveredhandlers.Add(assembly, new Dictionary<Type, List<IMethodAttributeDiscoveredHandler>>());

            if (!_methodAttributeDiscoveredhandlers[assembly].ContainsKey(handlerAttributeType))
                _methodAttributeDiscoveredhandlers[assembly].Add(handlerAttributeType, new List<IMethodAttributeDiscoveredHandler>());

            var handler = new MethodAttributeDiscoveredHandler(handlerAttributeType, container, containerType, attribute, action);
            _methodAttributeDiscoveredhandlers[assembly][handlerAttributeType].Add(handler);

            if (attribute.HandleAlreadyDiscoveredAttributes)
                HandleBeforeRegisteredMethodHandlers(handlerAttributeType, action, container);
        }
        #endregion

        #region ClassAttributeDiscoveredHandler
        private void RegisterClassAttributeDiscoveredHandler(MethodInfo method, object container, ClassAttributeDiscoveredHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || (!parameters[0].ParameterType.HasInterface(typeof(IClassHandler)) && parameters[0].ParameterType != typeof(IClassHandler))
                || (!parameters[1].ParameterType.GetTypeInfo().IsSubclassOf(typeof(ClassHandlerAttribute)) && parameters[1].ParameterType != typeof(ClassHandlerAttribute))
                )
            {
                throw new ArgumentException(string.Format("Class handler {0} has incorrect parameters. Right definition is Handler(IClassHandler, ClassHandlerAttribute)", method));
            }

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

            Action<object, IClassHandler, ClassHandlerAttribute> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, IClassHandler, ClassHandlerAttribute>)method.CreateParamsDelegate(typeof(IClassHandler), typeof(ClassHandlerAttribute));
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Class handler {0} has incorrect parameters. Right definition is Handler(IMethodHandler, HandlerAttribute) : {1}", method, e.Message));
            }

            foreach (var attribute in attributes)
            {
                RegisterClassAttributeDiscoveredHandler(attribute.HandlerAttributeType, method.DeclaringType, attribute, handlerDelegate, method.IsStatic ? null : container);
            }
        }

        private void RegisterClassAttributeDiscoveredHandler(Type handlerAttributeType, Type containerType, ClassAttributeDiscoveredHandlerAttribute attribute, Action<object, IClassHandler, ClassHandlerAttribute> action, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_classAttributeDiscoveredhandlers.ContainsKey(assembly))
                _classAttributeDiscoveredhandlers.Add(assembly, new Dictionary<Type, List<IClassAttributeDiscoveredHandler>>());

            if (!_classAttributeDiscoveredhandlers[assembly].ContainsKey(handlerAttributeType))
                _classAttributeDiscoveredhandlers[assembly].Add(handlerAttributeType, new List<IClassAttributeDiscoveredHandler>());

            var handler = new ClassAttributeDiscoveredHandler(handlerAttributeType, container, containerType, attribute, action);
            _classAttributeDiscoveredhandlers[assembly][handlerAttributeType].Add(handler);

            if (attribute.HandleAlreadyDiscoveredAttributes)
                HandleBeforeRegisteredClassHandlers(handlerAttributeType, action, container);
        }
        #endregion

        #region PropertyAttributeDiscoveredHandler
        private void RegisterPropertyAttributeDiscoveredHandler(MethodInfo method, object container, PropertyAttributeDiscoveredHandlerAttribute[] attributes)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (attributes == null || attributes.Length == 0)
                return;

            var parameters = method.GetParameters();

            if (parameters.Length != 2
                || (!parameters[0].ParameterType.HasInterface(typeof(IPropertyHandler)) && parameters[0].ParameterType != typeof(IPropertyHandler))
                || (!parameters[1].ParameterType.GetTypeInfo().IsSubclassOf(typeof(PropertyHandlerAttribute)) && parameters[1].ParameterType != typeof(PropertyHandlerAttribute))
                )
            {
                throw new ArgumentException(string.Format("Property handler {0} has incorrect parameters. Right definition is Handler(IPropertyHandler, PropertyHandlerAttribute)", method));
            }

            if (!method.IsStatic && container == null || method.IsStatic && container != null)
                return;

            Action<object, IPropertyHandler, PropertyHandlerAttribute> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, IPropertyHandler, PropertyHandlerAttribute>)method.CreateParamsDelegate(typeof(IPropertyHandler), typeof(PropertyHandlerAttribute));
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Property handler {0} has incorrect parameters. Right definition is Handler(IPropertyHandler, PropertyHandlerAttribute) : {1}", method, e.Message));
            }

            foreach (var attribute in attributes)
            {
                RegisterPropertyAttributeDiscoveredHandler(attribute.HandlerAttributeType, method.DeclaringType, attribute, handlerDelegate, method.IsStatic ? null : container);
            }
        }

        private void RegisterPropertyAttributeDiscoveredHandler(Type handlerAttributeType, Type containerType, PropertyAttributeDiscoveredHandlerAttribute attribute, Action<object, IPropertyHandler, PropertyHandlerAttribute> action, object container)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var assembly = containerType.GetTypeInfo().Assembly;

            // handlers are organized by assemblies to build an hierarchie
            // if the assembly is not registered yet we add it to the end
            if (!_propertyAttributeDiscoveredhandlers.ContainsKey(assembly))
                _propertyAttributeDiscoveredhandlers.Add(assembly, new Dictionary<Type, List<IPropertyAttributeDiscoveredHandler>>());

            if (!_propertyAttributeDiscoveredhandlers[assembly].ContainsKey(handlerAttributeType))
                _propertyAttributeDiscoveredhandlers[assembly].Add(handlerAttributeType, new List<IPropertyAttributeDiscoveredHandler>());

            var handler = new PropertyAttributeDiscoveredHandler(handlerAttributeType, container, containerType, attribute, action);
            _propertyAttributeDiscoveredhandlers[assembly][handlerAttributeType].Add(handler);

            if (attribute.HandleAlreadyDiscoveredAttributes)
                HandleBeforeRegisteredPropertyHandlers(handlerAttributeType, action, container);
        }
        #endregion

        private void HandleBeforeRegisteredMethodHandlers(Type handlerAttributeType, Action<object, IMethodHandler, MethodHandlerAttribute> action, object container)
        {
            var handlers = GetMethodHandlers(handlerAttributeType);

            foreach (var handler in handlers)
            {
                action(container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleBeforeRegisteredClassHandlers(Type handlerAttributeType, Action<object, IClassHandler, ClassHandlerAttribute> action, object container)
        {
            var handlers = GetClassHandlers(handlerAttributeType);

            foreach (var handler in handlers)
            {
                action(container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleBeforeRegisteredPropertyHandlers(Type handlerAttributeType, Action<object, IPropertyHandler, PropertyHandlerAttribute> action, object container)
        {
            var handlers = GetPropertyHandlers(handlerAttributeType);

            foreach (var handler in handlers)
            {
                action(container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleRegisteredMethodHandler(IMethodHandler handler)
        {
            var discoveredHandlers = GetMethodAttributeDiscoveredHandlers(handler.HandlerAttribute.GetType());

            foreach (var discoveredHandler in discoveredHandlers)
            {
                discoveredHandler.Action(discoveredHandler.Container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleRegisteredClassHandler(IClassHandler handler)
        {
            var discoveredHandlers = GetClassAttributeDiscoveredHandlers(handler.HandlerAttribute.GetType());

            foreach (var discoveredHandler in discoveredHandlers)
            {
                discoveredHandler.Action(discoveredHandler.Container, handler, handler.HandlerAttribute);
            }
        }

        private void HandleRegisteredPropertyHandler(IPropertyHandler handler)
        {
            var discoveredHandlers = GetPropertyAttributeDiscoveredHandlers(handler.HandlerAttribute.GetType());

            foreach (var discoveredHandler in discoveredHandlers)
            {
                discoveredHandler.Action(discoveredHandler.Container, handler, handler.HandlerAttribute);
            }
        }
    }
}
