using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace WinSolution.Module
{
    [DefaultClassOptions]
    public class Master : BaseObject
    {
        public Master(Session session) : base(session) { }
        [Association("Master-Children"), Aggregated]
        public XPCollection<Child> Children {
            get {
                return GetCollection<Child>("Children");
            }
        }
        private string _name;
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }
    }
}
