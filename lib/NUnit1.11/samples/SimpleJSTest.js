import System;
import NUnit.Framework;

class SimpleJSTest extends TestCase {
	protected var fValue1 : int;
	protected var fValue2 : int;

	function SimpleJSTest(name : String) {
		super(name);
	}

	protected override function SetUp() {
		fValue1= 2;
		fValue2= 3;
	}

	static function get Suite() : ITest {
		return new TestSuite(SimpleJSTest);
	}

	function TestAdd() {
		var result : double = fValue1 + fValue2;
		Assertion.Assert(result == 6);
	}

	function TestDivideByZero() {
	  var zero : int = 0;
	  var result : int = 8/zero;
	}

	function TestEquals() {
	  Assertion.AssertEquals(12, 12);
	  Assertion.AssertEquals(long(12), long(12));
	  Assertion.AssertEquals(new Object(12), new Object(12));
			
	  Assertion.AssertEquals("Size", 12, 13);
	  Assertion.AssertEquals("Capacity", 12.0, 11.99, 0.0);
	}
}