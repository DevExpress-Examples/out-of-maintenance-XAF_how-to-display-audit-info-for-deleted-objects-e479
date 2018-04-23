using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.AuditTrail;

namespace WinSolution.Module.Win {
    public class DeletedObjectsAuditInfoViewController : ViewController {
        private SimpleAction showAuditInfoActionCore;
        public DeletedObjectsAuditInfoViewController() {
            showAuditInfoActionCore = new SimpleAction(this, "ShowAuditInfo", DevExpress.Persistent.Base.PredefinedCategory.View);
            showAuditInfoActionCore.Caption = "Show  Audit Info";
            showAuditInfoActionCore.ImageName = "Attention";
            showAuditInfoActionCore.Execute += showAuditInfoActionCore_Execute;
            showAuditInfoActionCore.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
        }
        public XPCollection<AuditDataItemPersistent> GetAuditInfo(Session session, Type targetType) {
            AuditedObjectWeakReference auditObjectWeakReference = session.FindObject<AuditedObjectWeakReference>(
                    new BinaryOperator("TargetType", session.GetObjectType(session.GetClassInfo(targetType)))
            );
            if (auditObjectWeakReference != null) {
                auditObjectWeakReference.AuditDataItems.BindingBehavior = CollectionBindingBehavior.AllowNone;
                return auditObjectWeakReference.AuditDataItems;
            }
            return null;
        }
        protected virtual void ShowAuditInfo(SimpleActionExecuteEventArgs e) {
            XPCollection<AuditDataItemPersistent> auditInfo = GetAuditInfo(((ObjectSpace)View.ObjectSpace).Session, View.ObjectTypeInfo.Type);
            if (auditInfo != null) {
                CollectionSourceBase cs = new CollectionSource(View.ObjectSpace, typeof(AuditDataItemPersistent));
                e.ShowViewParameters.CreatedView = Application.CreateListView(
                    Application.FindListViewId(typeof(AuditDataItemPersistent)),
                    cs,
                    false
                );
                e.ShowViewParameters.CreatedView.Caption = String.Format("{0} History", e.ShowViewParameters.CreatedView.ObjectTypeInfo.Name);
                cs.Criteria["AllAuditInfo"] = new InOperator(View.ObjectSpace.GetKeyPropertyName(typeof(AuditDataItemPersistent)), auditInfo);
                EnumDescriptor ed = new EnumDescriptor(typeof(AuditOperationType));
                cs.Criteria["DeletedOnly"] = CriteriaOperator.Parse(
                    "OperationType = ? OR OperationType = ?",
                    ed.GetCaption(AuditOperationType.ObjectDeleted),
                    ed.GetCaption(AuditOperationType.RemovedFromCollection)
                );
                e.ShowViewParameters.Context = TemplateContext.View;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            }
            else {
                XtraMessageBox.Show(String.Format("There is no audit info for the {0} type", View.ObjectTypeInfo.Type.Name), "Show Audit Info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }
        private void showAuditInfoActionCore_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ShowAuditInfo(e);
        }
        public SimpleAction ShowAuditInfoAction {
            get { return showAuditInfoActionCore; }
        }
    }
}