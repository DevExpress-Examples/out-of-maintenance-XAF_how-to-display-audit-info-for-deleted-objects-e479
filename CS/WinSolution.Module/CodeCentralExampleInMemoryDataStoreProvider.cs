using System;
using System.Data;
using DevExpress.Xpo.DB;

namespace WinSolution.Module {
    #region For demo purposes only!!!
    public class CodeCentralExampleInMemoryDataStoreProvider {
        private static readonly string fConnectionString;
        private static readonly DataSet fdataSet;
        public static string ConnectionString { get { return fConnectionString; } }
        static CodeCentralExampleInMemoryDataStoreProvider() {
            string providerKey = Guid.NewGuid().ToString();
            fConnectionString = "XpoProvider=" + providerKey;
            fdataSet = new DataSet();
            DataStoreBase.RegisterDataStoreProvider(providerKey, CreateProviderFromString);
        }
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            InMemoryDataStore result = new InMemoryDataStore(fdataSet, autoCreateOption);
            objectsToDisposeOnDisconnect = new IDisposable[] { };
            return result;
        }
    }
    #endregion
}