namespace NUnit.Tests {

  using System;

  using NUnit.Framework;

  /// <summary>Test class used in SuiteTest.</summary>
  public class NotVoidTestCase: TestCase {
	  /// <summary>
	  /// 
	  /// </summary>
	  /// <param name="name"></param>
    public NotVoidTestCase(String name) : base(name) {}
	  /// <summary>
	  /// 
	  /// </summary>
	  /// <returns></returns>
    public int TestNotVoid() {
      return 1;
    }
	  /// <summary>
	  /// 
	  /// </summary>
    public void TestVoid() {
    }
  }
}
