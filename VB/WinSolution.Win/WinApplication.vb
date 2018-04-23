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
        Public Sub CreateCollectionSourceEvent(ByVal sender As Object, ByVal args As CreateCustomCollectionSourceEventArgs) Handles MyBase.CreateCustomCollectionSource
            If args.ListViewID.EndsWith("_Deleted") Then
                args.CollectionSource = New DeletedObjectsCollectionSource(args.ObjectSpace, args.ObjectType)
            End If
        End Sub
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
        Protected Overrides Function CreateCollection() As Object
            Dim collection As XPCollection = CType(MyBase.CreateCollection(), XPCollection)
            If collection IsNot Nothing Then
                collection.SelectDeleted = True
            End If
            Return collection
        End Function
	End Class
End Namespace
