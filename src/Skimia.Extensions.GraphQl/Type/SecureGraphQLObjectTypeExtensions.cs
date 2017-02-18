using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GraphQLCore.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Skimia.Extensions.GraphQl.Type
{
    public static class SecureGraphQlObjectTypeExtensions
    {
        private static AuthorizationPolicy DefaultAuthorizationPolicy => new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

        #region SecurityFieldDeclarationWithDefaultPolicy
        public static void SecureField<T1>(this GraphQLObjectType @this, string name, Expression<Func<T1>> resolver)
        {
            @this.Field<T1>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2>> resolver)
        {
            @this.Field<T2>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3>> resolver)
        {
            @this.Field<T3>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4>> resolver)
        {
            @this.Field<T4>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5>> resolver)
        {
            @this.Field<T5>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5, T6>> resolver)
        {
            @this.Field<T6>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> resolver)
        {
            @this.Field<T7>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> resolver)
        {
            @this.Field<T8>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> resolver)
        {
            @this.Field<T9>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this GraphQLObjectType @this, string name, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> resolver)
        {
            @this.Field<T10>(name, @this.MakeSecuredLambda(DefaultAuthorizationPolicy, resolver));
        }
        #endregion

        #region SecurityFieldDeclarationWithCustomPolicy
        public static void SecureField<T1>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1>> resolver)
        {
            @this.Field<T1>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2>> resolver)
        {
            @this.Field<T2>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3>> resolver)
        {
            @this.Field<T3>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4>> resolver)
        {
            @this.Field<T4>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5>> resolver)
        {
            @this.Field<T5>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5, T6>> resolver)
        {
            @this.Field<T6>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> resolver)
        {
            @this.Field<T7>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> resolver)
        {
            @this.Field<T8>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> resolver)
        {
            @this.Field<T9>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }

        public static void SecureField<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this GraphQLObjectType @this, string name, Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> resolver)
        {
            @this.Field<T10>(name, @this.MakeSecuredLambda(ResolvePolicy(policyBuilder), resolver));
        }
        #endregion

        #region SecurityExecutions
        public static T1 ExecuteSecurity<T1>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1>> resolver)
        {
            return @this.CheckPolicy(policy) ? (T1)resolver.Compile().DynamicInvoke() : default(T1);
        }


        public static T2 ExecuteSecurity<T1, T2>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2>> resolver, T1 arg1)
        {
            return @this.CheckPolicy(policy) ? (T2)resolver.Compile().DynamicInvoke(arg1) : default(T2);
        }


        public static T3 ExecuteSecurity<T1, T2, T3>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3>> resolver, T1 arg1, T2 arg2)
        {
            return @this.CheckPolicy(policy) ? (T3)resolver.Compile().DynamicInvoke(arg1, arg2) : default(T3);
        }


        public static T4 ExecuteSecurity<T1, T2, T3, T4>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4>> resolver, T1 arg1, T2 arg2, T3 arg3)
        {
            return @this.CheckPolicy(policy) ? (T4)resolver.Compile().DynamicInvoke(arg1, arg2, arg3) : default(T4);
        }


        public static T5 ExecuteSecurity<T1, T2, T3, T4, T5>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return @this.CheckPolicy(policy) ? (T5)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4) : default(T5);
        }


        public static T6 ExecuteSecurity<T1, T2, T3, T4, T5, T6>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5, T6>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return @this.CheckPolicy(policy) ? (T6)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4, arg5) : default(T6);
        }


        public static T7 ExecuteSecurity<T1, T2, T3, T4, T5, T6, T7>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return @this.CheckPolicy(policy) ? (T7)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6) : default(T7);
        }


        public static T8 ExecuteSecurity<T1, T2, T3, T4, T5, T6, T7, T8>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return @this.CheckPolicy(policy) ? (T8)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7) : default(T8);
        }


        public static T9 ExecuteSecurity<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return @this.CheckPolicy(policy) ? (T9)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) : default(T9);
        }


        public static T10 ExecuteSecurity<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this GraphQLObjectType @this, AuthorizationPolicy policy, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> resolver, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return @this.CheckPolicy(policy) ? (T10)resolver.Compile().DynamicInvoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) : default(T10);
        } 
        #endregion

        private static AuthorizationPolicy ResolvePolicy(Expression<Func<AuthorizationPolicyBuilder, AuthorizationPolicy>> policyBuilder)
        {
            var builder = new AuthorizationPolicyBuilder();
            return policyBuilder.Compile().Invoke(builder);
        }

        private static bool CheckPolicy(this GraphQLObjectType @this, AuthorizationPolicy policy)
        {
            //Add logger to can log authorization faillures
            var httpContext = ((IHttpContextAccessor)GraphQl.ServiceProvider.GetService(typeof(IHttpContextAccessor))).HttpContext;

            var user = httpContext.User;

            var authorizationService = (IAuthorizationService)GraphQl.ServiceProvider.GetService(typeof(IAuthorizationService));

            //Implement later data resolver to can authorize specified data & send it to lambda (DataModels) if authorized
            return authorizationService.AuthorizeAsync(user, null, policy.Requirements).GetAwaiter().GetResult();
        }

        private static Expression[] GetArguments(this GraphQLObjectType @this, AuthorizationPolicy policy, LambdaExpression resolver)
        {
            var arguments = new List<Expression>()
            {
                Expression.Constant(@this),
                Expression.Constant(policy),
                Expression.Constant(resolver),
            };

            arguments.AddRange(resolver.Parameters);

            return arguments.ToArray();
        }

        private static MethodInfo GetExecuteSecurity(System.Type[] genereicTypes)
        {
            return typeof(SecureGraphQlObjectTypeExtensions)
                .GetMethods()
                .First(m => m.Name == "ExecuteSecurity" && m.GetGenericArguments().Length == genereicTypes.Length).MakeGenericMethod(genereicTypes);
        }

        private static LambdaExpression MakeSecuredLambda(this GraphQLObjectType @this, AuthorizationPolicy policy, LambdaExpression resolver)
        {
            var arguments = @this.GetArguments(policy, resolver);
            var secureExecution = GetExecuteSecurity(resolver.Type.GenericTypeArguments);
            var call  = Expression.Call(null, secureExecution, arguments.ToArray());

            return Expression.Lambda(call, resolver.Parameters);
        }
    }
}
