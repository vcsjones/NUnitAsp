namespace NUnit.Tests {

  using System;
  using NUnit.Framework;

  /// <summary>A helper test case for Testing whether the Testing method
  /// is run.</summary>
  class WasRun: TestCase {
    internal bool fWasRun= false;
        
    internal WasRun(String name) : base(name) {}
        
    protected override void RunTest() {
      fWasRun= true;
    }
  }
}
