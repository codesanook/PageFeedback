using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.WindowsAzure;
using NHibernate;
using NHibernate.Event;
using NHibernate.SqlAzure;
using PageFeedback.Service.Mappings;
using PageFeedback.Service.SessionStorage;
using log4net;

namespace PageFeedback.Service.NhSessionFactory
{
    public class SessionFactory
    {
        private static ISessionFactory _sessionFactory;
        private static NHibernate.Cfg.Configuration _config;
        private static readonly ILog _log = LogManager.GetLogger(typeof(SessionFactory));

        public static NHibernate.Cfg.Configuration Config
        {
            get { return _config; }
        }

        public static void Init()
        {
            var connectionString = CloudConfigurationManager.GetSetting("SqlAzureConnectionString");
            Init(connectionString);
        }

        public static void Init(string connectionString)
        {
            try
            {
                var dbConfig = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)
                    .Driver<SqlAzureClientDriver>()//For Azure Retry connection
                    //.ql()
                    //.FormatSql()
                    .AdoNetBatchSize(100);

                _config = Fluently.Configure()
                    .Database(dbConfig)
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<PageCommentWordMap>();
                        //to se name query in xml file
                        m.HbmMappings.AddFromAssemblyOf<PageCommentWordMap>();
                    })
                    .ExposeConfiguration(TreatConfiguration)
                    .BuildConfiguration();

                _sessionFactory = _config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                LogExceptionRecursively(ex);
            }
        }

        private static void TreatConfiguration(NHibernate.Cfg.Configuration cfg)
        {
            cfg.EventListeners.PreInsertEventListeners
            = new IPreInsertEventListener[]
                                                 {
                                                    new AuditEventListener()
                                                 };

            cfg.EventListeners.PreUpdateEventListeners

                 = new IPreUpdateEventListener[]
                                                 {
                                                    new AuditEventListener()
                                                 };

            //https://github.com/MRCollective/NHibernate.SqlAzure
            //for unique constrain exception
            cfg.SetProperty(NHibernate.Cfg.Environment.SqlExceptionConverter,
                typeof(SqlServerExceptionConverter).AssemblyQualifiedName);
            //for transaction
            cfg.SetProperty(NHibernate.Cfg.Environment.TransactionStrategy,
                typeof(ReliableAdoNetWithDistributedTransactionFactory).AssemblyQualifiedName);
        }

        public static ISessionFactory GetNHSessionFactory()
        {
            if (_sessionFactory == null)
                Init();//we can move this to Global.ascx
            return _sessionFactory;
        }

        private static ISession GetNewSession()
        {
            return GetNHSessionFactory().OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            var sessionStorageContainer = SessionStorageFactory.GetStorageContainer();

            var currentSession = sessionStorageContainer.GetCurrentSession();
            if (currentSession == null)
            {
                currentSession = GetNewSession();
                sessionStorageContainer.Store(currentSession);
            }
            return currentSession;
        }

        public static void ClearCurrentSession()
        {
            var sessionStorageContainer =
                SessionStorageFactory.GetStorageContainer();
            sessionStorageContainer.Clear();

        }

        private static void LogExceptionRecursively(Exception ex)
        {
            if (ex == null)
            {
                _log.Fatal("SessionFactory init exception is null and return exit method");
            }
            else
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    foreach (var loaderException in loaderExceptions)
                    {
                        _log.Fatal(loaderException);
                    }
                }
                else
                {
                    _log.Fatal(ex);
                }
                LogExceptionRecursively(ex.InnerException);
            }
        }
    }
}
