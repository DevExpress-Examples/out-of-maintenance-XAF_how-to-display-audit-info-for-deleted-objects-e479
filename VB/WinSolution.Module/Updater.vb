Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Using uow As New UnitOfWork(Session.DataLayer)
				Dim master As New Master(uow)
				master.Name = "Master"
				For i As Integer = 0 To 9
					Dim child As New Child(uow)
					child.Master = master
					child.Name = "Child " & i.ToString()
				Next i
				uow.CommitChanges()
			End Using
		End Sub
	End Class
End Namespace
