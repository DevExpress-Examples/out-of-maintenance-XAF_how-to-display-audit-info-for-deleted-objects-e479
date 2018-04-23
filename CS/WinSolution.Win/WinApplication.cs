using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.Data.Filtering;

namespace WinSolution.Win {
    public partial class WinSolutionWindowsFormsApplication : WinApplication {
        public WinSolutionWindowsFormsApplication() {
            InitializeComponent();
            CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(CreateCollectionSourceEvent);
        }

        private void WinSolutionWindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }

        public void CreateCollectionSourceEvent(object sender, CreateCustomCollectionSourceEventArgs args) {
            if(args.ListViewID.EndsWith("_Deleted")) {
               args.CollectionSource = new DeletedObjectsCollectionSource(args.ObjectSpace, args.ObjectType);
            }
        }
    }
    public class DeletedObjectsCollectionSource : CollectionSource {
        public static readonly CriteriaOperator DeletedObjectsCriteria = CriteriaOperator.Parse("GCRecord is not null");
        public DeletedObjectsCollectionSource(ObjectSpace objectSpace, Type objectType) : base(objectSpace, objectType) { }
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) {
            base.ApplyCriteriaCore(criteria & DeletedObjectsCriteria);
        }
        protected override object CreateCollection() {
            XPCollection collection = base.CreateCollection() as XPCollection;
            if (collection != null) {
                collection.SelectDeleted = true;
            }
            return collection;
        }
    }
}
