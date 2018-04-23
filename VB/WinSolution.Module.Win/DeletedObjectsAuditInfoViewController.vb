Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.XtraEditors
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.Persistent.AuditTrail

Namespace WinSolution.Module.Win
	Public Class DeletedObjectsAuditInfoViewController
		Inherits ViewController
		Private showAuditInfoActionCore As SimpleAction
		Public Sub New()
			showAuditInfoActionCore = New SimpleAction(Me, "ShowAuditInfo", DevExpress.Persistent.Base.PredefinedCategory.View)
			showAuditInfoActionCore.Caption = "Show  Audit Info"
			showAuditInfoActionCore.ImageName = "Attention"
			AddHandler showAuditInfoActionCore.Execute, AddressOf showAuditInfoActionCore_Execute
			showAuditInfoActionCore.PaintStyle = ActionItemPaintStyle.CaptionAndImage
		End Sub
		Public Function GetAuditInfo(ByVal session As Session, ByVal targetType As Type) As XPCollection(Of AuditDataItemPersistent)
			Dim auditObjectWeakReference As AuditedObjectWeakReference = session.FindObject(Of AuditedObjectWeakReference)(New BinaryOperator("TargetType", session.GetObjectType(session.GetClassInfo(targetType))))
			If auditObjectWeakReference IsNot Nothing Then
				auditObjectWeakReference.AuditDataItems.BindingBehavior = CollectionBindingBehavior.AllowNone
				Return auditObjectWeakReference.AuditDataItems
			End If
			Return Nothing
		End Function
		Protected Overridable Sub ShowAuditInfo(ByVal e As SimpleActionExecuteEventArgs)
			Dim auditInfo As XPCollection(Of AuditDataItemPersistent) = GetAuditInfo((CType(View.ObjectSpace, ObjectSpace)).Session, View.ObjectTypeInfo.Type)
			If auditInfo IsNot Nothing Then
				Dim cs As CollectionSourceBase = New CollectionSource(View.ObjectSpace, GetType(AuditDataItemPersistent))
				e.ShowViewParameters.CreatedView = Application.CreateListView(Application.FindListViewId(GetType(AuditDataItemPersistent)), cs, False)
				e.ShowViewParameters.CreatedView.Caption = String.Format("{0} History", e.ShowViewParameters.CreatedView.ObjectTypeInfo.Name)
				cs.Criteria("AllAuditInfo") = New InOperator(View.ObjectSpace.GetKeyPropertyName(GetType(AuditDataItemPersistent)), auditInfo)
				Dim ed As New EnumDescriptor(GetType(AuditOperationType))
				cs.Criteria("DeletedOnly") = CriteriaOperator.Parse("OperationType = ? OR OperationType = ?", ed.GetCaption(AuditOperationType.ObjectDeleted), ed.GetCaption(AuditOperationType.RemovedFromCollection))
				e.ShowViewParameters.Context = TemplateContext.View
				e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow
			Else
				XtraMessageBox.Show(String.Format("There is no audit info for the {0} type", View.ObjectTypeInfo.Type.Name), "Show Audit Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
			End If
		End Sub
		Private Sub showAuditInfoActionCore_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
			ShowAuditInfo(e)
		End Sub
		Public ReadOnly Property ShowAuditInfoAction() As SimpleAction
			Get
				Return showAuditInfoActionCore
			End Get
		End Property
	End Class
End Namespace