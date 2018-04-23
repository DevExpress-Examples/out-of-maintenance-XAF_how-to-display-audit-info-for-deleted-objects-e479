Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
				Dim master As Master = ObjectSpace.CreateObject(Of Master)()
				master.Name = "Master"
				For i As Integer = 0 To 9
					Dim child As Child = ObjectSpace.CreateObject(Of Child)()
					child.Master = master
					child.Name = "Child " & i.ToString()
				Next i
				ObjectSpace.CommitChanges()
		End Sub
	End Class
End Namespace
