using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using NUnit.Extensions.Asp.HtmlTester;
using NUnit.Framework;

namespace NUnit.Extensions.Asp.Test
{
	/// <summary>
	/// Makes sure that all Testers have the correct constructors.
	/// </summary>
	[TestFixture]
	public class ConstructorTests
	{
		[Test]
		public virtual void CheckTesters()
		{
			Type[] derivedTesters = GetAllDerivedClasses(typeof(HtmlControlTester), typeof(AnchorTester));
			foreach (Type tester in derivedTesters)
			{
				ConstructorInfo[] constructors = tester.GetConstructors();
				Assert.IsTrue(HasConstructor(constructors, typeof(string)), String.Format("Class '{0}': HtmlControlTester(string htmlId) missing.", tester.Name));
				Assert.IsTrue(HasConstructor(constructors, typeof(string), typeof(Tester)), String.Format("Class '{0}': HtmlControlTester(string aspId, Tester container) missing.", tester.Name));
				Assert.IsTrue(HasConstructor(constructors, typeof(string), typeof(string)), String.Format("Class '{0}': HtmlControlTester(string xpath, string description) missing.", tester.Name));
				Assert.IsTrue(HasConstructor(constructors, typeof(string), typeof(string), typeof(Tester)), String.Format("Class '{0}': HtmlControlTester(string xpath, string description, Tester container) missing.", tester.Name));
			}
		}

		private bool HasConstructor(ConstructorInfo[] constructors, params Type[] parameterList)
		{
			bool found = false;
			foreach (ConstructorInfo constructor in constructors)
			{
				if (found == true) break;

				ParameterInfo[] parameters = constructor.GetParameters();
				if (parameters.Length == parameterList.Length)
				{
					found = DoParametersMatch(parameters, parameterList);
				}
			}
			return found;
		}

		private static bool DoParametersMatch(ParameterInfo[] parameters, Type[] parameterList)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[0].ParameterType != parameterList[0])
					return false;
			}
			return true;
		}

		private static Type[] GetAllDerivedClasses(Type baseClass, params Type[] excludedTypes)
		{
			Type[] types = Assembly.GetAssembly(baseClass).GetTypes();
			ArrayList derivedTesters = new ArrayList();
			ArrayList exclusionList = new ArrayList(excludedTypes);
			foreach (Type type in types)
			{
				if (baseClass.IsAssignableFrom(type) && ! baseClass.Equals(type) && !exclusionList.Contains(type))
				{
					derivedTesters.Add(type);
				}
			}
			return (Type[]) derivedTesters.ToArray(typeof(Type));
		}
	}
}
