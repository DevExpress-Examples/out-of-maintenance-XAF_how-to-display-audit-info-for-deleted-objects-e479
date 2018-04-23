Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

Namespace WinSolution.Module
	Public Class Child
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _name As String
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _name, value)
			End Set
		End Property
		Private _master As Master
		<Association("Master-Children")> _
		Public Property Master() As Master
			Get
				Return _master
			End Get
			Set(ByVal value As Master)
				SetPropertyValue("Master", _master, value)
			End Set
		End Property
	End Class
End Namespace
