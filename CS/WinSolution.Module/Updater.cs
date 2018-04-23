using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            using (UnitOfWork uow = new UnitOfWork(Session.DataLayer)) {
                Master master = new Master(uow);
                master.Name = "Master";
                for (int i = 0; i < 10; i++) {
                    Child child = new Child(uow);
                    child.Master = master;
                    child.Name = "Child " + i.ToString();
                }
                uow.CommitChanges();
            }
        }
    }
}
