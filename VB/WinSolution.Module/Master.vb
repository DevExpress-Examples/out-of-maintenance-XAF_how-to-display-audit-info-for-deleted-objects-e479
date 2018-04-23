Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

Namespace WinSolution.Module
	<DefaultClassOptions> _
	Public Class Master
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		<Association("Master-Children"), Aggregated> _
		Public ReadOnly Property Children() As XPCollection(Of Child)
			Get
				Return GetCollection(Of Child)("Children")
			End Get
		End Property
		Private _name As String
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _name, value)
			End Set
		End Property
	End Class
End Namespace
