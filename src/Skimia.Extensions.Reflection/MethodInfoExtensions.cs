using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Skimia.Extensions.Reflection
{
    public static class MethodInfoExtensions
    {
        public static Delegate CreateParamsDelegate(this MethodInfo method, params Type[] delegParams)
        {
            var methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (delegParams.Length != methodParams.Length)
                throw new Exception("Method parameters count != delegParams.Length");

            var dynamicMethod = new DynamicMethod(string.Empty, null, new[] { typeof(object) }.Concat(delegParams).ToArray(), true);
            var ilGenerator = dynamicMethod.GetILGenerator();

            if (!method.IsStatic)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(method.DeclaringType.GetTypeInfo().IsClass ? OpCodes.Castclass : OpCodes.Unbox, method.DeclaringType);
            }

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].GetTypeInfo().IsSubclassOf(delegParams[i]) || methodParams[i].HasInterface(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].GetTypeInfo().IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception(string.Format("Cannot cast {0} to {1}", methodParams[i].Name, delegParams[i].Name));
            }

            ilGenerator.Emit(OpCodes.Call, method);

            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(Expression.GetActionType(new[] { typeof(object) }.Concat(delegParams).ToArray()));
        }
    }
}
