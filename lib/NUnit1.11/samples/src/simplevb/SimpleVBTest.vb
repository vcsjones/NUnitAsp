Option Explicit On 

Imports System
Imports NUnit.Framework


Namespace NUnit.Samples
    Public Class SimpleVBTest
        Inherits TestCase


        Private fValue1 As Integer
        Private fValue2 As Integer

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

        Protected Overrides Sub SetUp()
            fValue1 = 2
            fValue2 = 3
        End Sub

        Shared ReadOnly Property Suite() As ITest
            Get
                Suite = New TestSuite(GetType(SimpleVBTest))
            End Get
        End Property

        Public Sub TestAdd()
            Dim result As Double

            result = fValue1 + fValue2
            Assertion.AssertEquals(6, result)
        End Sub

        Public Sub TestDivideByZero()
            Dim zero As Integer
            Dim result As Double

            zero = 0
            ' In VB7 Beta1, the below does not throw an exception. Result = 1.#INF after the below.
            ' All documentation seems to say it should throw an exception, so I am confused.
            result = 8 / zero
        End Sub

        Public Sub TestEquals()
            Assertion.AssertEquals(12, 12)
            Assertion.AssertEquals(CLng(12), CLng(12))

            Assertion.AssertEquals("Size", 12, 13)
            Assertion.AssertEquals("Capacity", 12, 11.99, 0)
        End Sub
    End Class
End Namespace