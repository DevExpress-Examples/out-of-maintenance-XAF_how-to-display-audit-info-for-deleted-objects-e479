using DevExpress.ExpressApp;
using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(ObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
                Master master = ObjectSpace.CreateObject<Master>();
                master.Name = "Master";
                for (int i = 0; i < 10; i++) {
                    Child child = ObjectSpace.CreateObject<Child>();
                    child.Master = master;
                    child.Name = "Child " + i.ToString();
                }
                ObjectSpace.CommitChanges();
        }
    }
}
