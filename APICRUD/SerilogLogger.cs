using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.InteropServices;

namespace APICRUD
{
    public static class SerilogLogger
    {
        private static Logger log;

        static SerilogLogger()
        {
            InitialiseLogger();
        }
        public static void Error(string messageTemplate)
        {
            log.Error(messageTemplate);
        }
        public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Error(exception, messageTemplate, propertyValues);
        }
        public static void Information(string messageTemplate, params object[] propertyValues)
        {
            log.Information(messageTemplate, propertyValues);
        }
        
        public static void Debug(string messageTemplate)
        {
            log.Debug(messageTemplate);
        }
        public static void Debug(string messageTemplate, params object[] propertyValues)
        {
            log.Debug(messageTemplate, propertyValues);
        }
        public static void Fatal(string messageTemplate, params object[] propertyValues)
        {
            log.Fatal(messageTemplate, propertyValues);
        }
        public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            // MH added Fatal entry with Exception
            log.Fatal(exception, messageTemplate, propertyValues);
        }
        public static void Fatal(Exception exception, string messageTemplate)
        {
            // MH added Fatal entry with Exception
            log.Fatal(exception, messageTemplate);
        }
        public static void Warning(string messageTemplate)
        {
            log.Warning(messageTemplate);
        }
        public static void Warning(string messageTemplate, params object[] propertyValues)
        {
            log.Warning(messageTemplate, propertyValues);
        }
        private static void InitialiseLogger()
        {
            try
            {
                var _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

                var logLevel = _configuration["Serilog:MinimumLevel"];
                var applicationName = "";
                var connection = _configuration.GetConnectionString("DefaultConnection").ToString();

                if (string.IsNullOrEmpty(logLevel)) logLevel = "Debug";
                if (string.IsNullOrEmpty(applicationName)) applicationName = "APICRUD";

                var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel);
                var levelSwitch = new LoggingLevelSwitch(logEventLevel);

                var columnOptions = new ColumnOptions
                {
                    //AdditionalDataColumns = new Collection<DataColumn>
                    //{
                    //    new DataColumn { DataType = typeof(string), ColumnName = "Email" },
                    //    new DataColumn { DataType = typeof(string), ColumnName = "InfoMessage" },
                    //    new DataColumn { DataType = typeof(string), ColumnName = "ExceptionMessage" },
                    //    new DataColumn { DataType = typeof(string), ColumnName = "DeviceName" },
                    //    new DataColumn { DataType = typeof(string), ColumnName = "Type" },
                    //    new DataColumn { DataType = typeof(string), ColumnName = "OS" }
                    //}
                };

                log = new LoggerConfiguration().WriteTo
                    .MSSqlServer(connection, sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" }, columnOptions: columnOptions)
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .Enrich.WithProperty("Application", applicationName)
                    //.Enrich.WithThreadId()
                    //.Enrich.WithMachineName()
                    //.Enrich.WithHttpRequestId()
                    //.Enrich.WithHttpRequestUrl()
                    //.Enrich.WithUserName()
                    //.Enrich.WithClientAgent()
                    //.Enrich.WithClientIp()
                    //.Enrich.WithProcessId()
                    //.Enrich.WithProcessName()
                    .CreateLogger();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
