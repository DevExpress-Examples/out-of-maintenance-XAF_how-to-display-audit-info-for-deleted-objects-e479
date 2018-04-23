Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Win
Imports DevExpress.Data.Filtering

Namespace WinSolution.Win
	Partial Public Class WinSolutionWindowsFormsApplication
		Inherits WinApplication
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub WinSolutionWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
			e.Updater.Update()
			e.Handled = True
		End Sub
		Protected Overrides Function CreateCollectionSourceCore(ByVal objectSpace As ObjectSpace, ByVal objectType As Type, ByVal listViewID As String) As CollectionSourceBase
			Dim cs As CollectionSourceBase
			If listViewID.EndsWith("_Deleted") Then
				cs = New DeletedObjectsCollectionSource(objectSpace, objectType)
			Else
				cs = MyBase.CreateCollectionSourceCore(objectSpace, objectType, listViewID)
			End If
			Return cs
		End Function
	End Class
	Public Class DeletedObjectsCollectionSource
		Inherits CollectionSource
		Public Shared ReadOnly DeletedObjectsCriteria As CriteriaOperator = CriteriaOperator.Parse("GCRecord is not null")
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal objectType As Type)
			MyBase.New(objectSpace, objectType)
		End Sub
		Protected Overrides Sub ApplyCriteriaCore(ByVal criteria As CriteriaOperator)
			MyBase.ApplyCriteriaCore(criteria And DeletedObjectsCriteria)
		End Sub
		Protected Overrides Function RecreateCollection(ByVal criteria As CriteriaOperator, ByVal sortings As SortingCollection) As System.Collections.IList
			Dim collection As XPCollection = CType(MyBase.RecreateCollection(criteria And DeletedObjectsCriteria, sortings), XPCollection)
			collection.SelectDeleted = True
			Return collection
		End Function
	End Class
End Namespace
