namespace NUnit.GUI 
{
	using NUnit.Framework;

	public interface ITestRunner 
	{
		void Run(string assemblyQualifiedTestClassName);
	}
}