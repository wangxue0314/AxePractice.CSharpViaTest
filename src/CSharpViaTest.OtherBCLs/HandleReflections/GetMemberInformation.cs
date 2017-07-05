using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CSharpViaTest.OtherBCLs.HandleReflections
{
    /* 
     * Description
     * ===========
     * 
     * This test will try get instance member information for a specified type. Please
     * note that the order of report should be in a constructor -> properties -> methods
     * manner. The constructors are ordered by Non-public/public and number of parameters.
     * The properties are ordered by name and the methods are ordered by name and number
     * of parameters.
     * 
     * Difficulty: Super Hard
     * 
     * Knowledge Point
     * ===============
     * 
     * - GetProperties(), GetMethods(), GetConstructors().
     * - BindingFlags enum,
     * - MemberInfo, MethodBase class
     * - Special named methods.
     */
    public class GetMemberInformation
    {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        class ForTestCtorProperty
        {
            public ForTestCtorProperty(string name) : this(name, null)
            {
            }

            ForTestCtorProperty(string name, string optional)
            {
                Name = name;
            }

            public string Name { get; }
            public int this[int index] => index;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        class ForTestMethod
        {
            public int CalculateSomething(int @base, string name)
            {
                return @base + name.Length;
            }
        }

        #region Please modifies the code to pass the test

        static IEnumerable<string> GetInstanceMemberInformation(Type type)
        {
            return new []{$"Member information for {type.FullName}"}
            .Concat(GetConstructors(type))
            .Concat(GetProperties(type))
            .Concat(GetMethods(type));
        }

        static IEnumerable<string> GetConstructors(Type type){
            return type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(info => info.IsPublic)
            .ThenBy(info => info.GetParameters().Length)
            .Select(info => {
                return $"{GetMethodAccess(info)} constructor: {GetParametes(info.GetParameters())}";
            });
        }

        private static string GetParametes(ParameterInfo[] parameterInfos)
        {
            return parameterInfos.Any()
            ? parameterInfos.Select(p => $"{p.ParameterType.Name} {p.Name}").Aggregate((p1,p2) => $"{p1}, {p2}")
            : "no parameter";
        }

        private static string GetMethodAccess(MethodBase info)
        {
            return info.IsPublic ? "Public" : "Non-public";
        }

        static IEnumerable<string> GetProperties(Type type){
            return type.GetProperties()
            .OrderBy(prop => !prop.GetIndexParameters().Any())
            .Select(prop => $"{GetPropType(prop)} property {prop.Name}: {GetGetterMethod(prop)}");
        }

        private static string GetGetterMethod(PropertyInfo prop)
        {
            return prop.GetGetMethod() == null ? "" : "Public getter.";
        }

        private static string GetPropType(PropertyInfo prop)
        {
            return prop.GetIndexParameters().Any() ? "Indexed" : "Normal";
        }

        static IEnumerable<string> GetMethods(Type type){
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .Select(m => $"{GetMethodAccess(m)} method {m.Name}: {GetParametes(m.GetParameters())}");
        }

        #endregion

        static IEnumerable<object[]> GetMemberTestCases()
        {
            return new[]
            {
                new object[]
                {
                    typeof(ForTestCtorProperty), new[]
                    {
                        "Member information for CSharpViaTest.OtherBCLs.HandleReflections.GetMemberInformation+ForTestCtorProperty",
                        "Non-public constructor: String name, String optional",
                        "Public constructor: String name",
                        "Indexed property Item: Public getter.",
                        "Normal property Name: Public getter."
                    }
                },
                new object[]
                {
                    typeof(ForTestMethod), new []
                    {
                        "Member information for CSharpViaTest.OtherBCLs.HandleReflections.GetMemberInformation+ForTestMethod",
                        "Public constructor: no parameter",
                        "Public method CalculateSomething: Int32 base, String name",
                    }
                }, 
            };
        }

        [Theory]
        [MemberData(nameof(GetMemberTestCases))]
        public void should_get_member_information(Type type, string[] expected)
        {
            Assert.Equal(expected, GetInstanceMemberInformation(type));
        }
    }
}