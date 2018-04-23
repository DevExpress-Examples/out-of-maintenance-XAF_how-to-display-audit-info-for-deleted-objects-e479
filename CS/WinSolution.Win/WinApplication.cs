using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.Data.Filtering;

namespace WinSolution.Win {
    public partial class WinSolutionWindowsFormsApplication : WinApplication {
        public WinSolutionWindowsFormsApplication() {
            InitializeComponent();
        }

        private void WinSolutionWindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
        protected override CollectionSourceBase CreateCollectionSourceCore(ObjectSpace objectSpace, Type objectType, string listViewID) {
            CollectionSourceBase cs = listViewID.EndsWith("_Deleted") ?
                new DeletedObjectsCollectionSource(objectSpace, objectType) :
                base.CreateCollectionSourceCore(objectSpace, objectType, listViewID);
            return cs;
        }
    }
    public class DeletedObjectsCollectionSource : CollectionSource {
        public static readonly CriteriaOperator DeletedObjectsCriteria = CriteriaOperator.Parse("GCRecord is not null");
        public DeletedObjectsCollectionSource(ObjectSpace objectSpace, Type objectType) : base(objectSpace, objectType) { }
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) {
            base.ApplyCriteriaCore(criteria & DeletedObjectsCriteria);
        }
        protected override object RecreateCollection(CriteriaOperator criteria, SortingCollection sortings) {
            XPCollection collection = (XPCollection)base.RecreateCollection(criteria & DeletedObjectsCriteria, sortings);
            collection.SelectDeleted = true;
            return collection;
        }
    }
}
