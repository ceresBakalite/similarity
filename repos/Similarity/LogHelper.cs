﻿using System.Linq;

namespace NAVService
{
    public static class LogHelper
    {
        private static readonly log4net.ILog log = GetLogger();

        private static void ApplicationKill() => System.Environment.Exit(0);

        internal static void GetLogState()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            if (args.Length == 1)
            {
                if (ClassLibraryStandard.HelperMethods.ObjectExists(UserHelper.UserPropertiesModel))
                {
                    UserHelper.UserStateModel = DataAccess.GetLastUserStateLogEntry();
                }
                else
                {
                    FatalUserVerificationException();
                }

            }
            else
            {
                RunCommandLineShell(args);
            }

        }

        internal static log4net.ILog GetLogger([System.Runtime.CompilerServices.CallerFilePath]string filename = "") 
        {
            return UserHelper.GetLogErrors() ? log4net.LogManager.GetLogger(filename) : null;
        }

        internal static void ApplyLogAppenderConfiguration()
        {
            if (log4net.LogManager.GetRepository() is log4net.Repository.Hierarchy.Hierarchy hierarchy)
            {
                TraceWriteLine("TRACE - Log4net appender initialisation");

                log4net.Appender.AdoNetAppender adoAppender = (log4net.Appender.AdoNetAppender)hierarchy.GetAppenders()
                    .Where(appender => appender.Name.Equals("AdoNetAppender", System.StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (adoAppender != null)
                {
                    adoAppender.ConnectionString = ConnectionHelper.CONNECTION_LOCATION;
                    adoAppender.ActivateOptions();
                }

            }

        }

        internal static bool RetrySQLException(System.Data.SqlClient.SqlException ex, int iRetryAttempt)
        {
            if (iRetryAttempt == Constants.CONNECTION_RETRY_LIMIT) FatalSqlException(ex);

            if (ex != null)
            {
                switch (ex.Number)
                {
                    case 258: break; // Connection Tmeout (inner Win32Exception)
                    case -2: break; // Execution Timeout Expired (SqlError.Number)
                    case 165: break; // login timeout expired 
                    case 298: break; // delay in login response
                    case 1205: break; // request deadlocked
                    case 3903: break; // no corresponding BEGIN TRANSACTION
                    case 3919: break; // transaction has already been committed or rolled back
                    case 3965: break; // local transaction active.
                    case 3989: break; // invalid transaction descriptor

                    default:

                        FatalSqlException(ex);
                        break;
                }

                TraceRetryWriteLine(ex, iRetryAttempt, ex.Number);
                ClassLibraryStandard.HelperMethods.ProcessSleep(2000);
            }

            return true;
        }

        internal static bool RetryNullReferenceException(System.NullReferenceException ex, int iRetryAttempt)
        {
            if (iRetryAttempt == Constants.CONNECTION_RETRY_LIMIT) FatalNullReferenceException(ex);

            if (ex != null)
            {
                TraceRetryWriteLine(ex, iRetryAttempt, ex.HResult);
                ClassLibraryStandard.HelperMethods.ProcessSleep(2000);
            }

            return true;
        }

        internal static void FatalNullReferenceException(System.Exception ex = null)
        {
            TraceWriteLine("FATAL NullReferenceException", ex);
            FatalException(Properties.Resources.NOTIFY_SQLNULLREFERENCE_ERROR, Properties.Resources.CAPTION_APPLICATION, ex);
        }

        internal static void FatalSqlException(System.Exception ex = null)
        {
            TraceWriteLine("FATAL SQLException", ex);
            FatalException(Properties.Resources.NOTIFY_SQLCONNECTION_ERROR, Properties.Resources.CAPTION_APPLICATION, ex);
        }

        internal static void FatalInternetUnavailableException(System.Exception ex = null)
        {
            TraceWriteLine("FATAL InternetUnavailableException");
            FatalException(Properties.Resources.NOTIFY_SQLNETWORK_UNAVAILABLE, Properties.Resources.CAPTION_INITIALISATION_ERROR, ex);
        }

        internal static void FatalUserVerificationException(System.Exception ex = null)
        {
            TraceWriteLine("FATAL UserVerificationException");
            FatalException(string.Format(UserHelper.culture, Constants.INVALID_USER_ID + ": " + Properties.Resources.NOTIFY_SQLINIITIALISATION_LOGON_ERROR, System.Environment.NewLine), Properties.Resources.CAPTION_INITIALISATION_ERROR, ex);
        }

        internal static void FatalException(string ErrorNotification, string Caption, System.Exception ex = null)
        {
            using (System.Windows.Forms.Form form = new System.Windows.Forms.Form { TopMost = true })
            {
                string Notification = string.Format(UserHelper.culture, ErrorNotification, System.Environment.NewLine);

                System.Windows.Forms.MessageBox.Show(form, Notification, Caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                if (log != null) log.Fatal(Notification, ex);
            }

            ApplicationKill();
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceWriteCurrentState()
        {
            TraceWriteLine("TRACE - Collecting user preferences");
            DataAccess.TraceWrite = false;
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceWritePreviousState()
        {
            TraceWriteLine("TRACE - Previous state initialisation");
            DataAccess.TraceWrite = true;
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceInitialisationComplete(string writeline = "TRACE - Application initialisation complete")
        {
            TraceWriteLine(writeline);
            TraceTimeElapsedWriteLine(System.DateTime.Now, UserHelper.ApplicationStartTime);
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceWriteLine(string trace = "TRACE - ", System.Exception ex = null)
        {
            System.Diagnostics.Trace.Indent();
            System.Diagnostics.Trace.WriteLine($"{trace}. DateTime: {System.DateTime.Now:MMMM dd, yyyy h:mm:ss tt}");
            TraceWriteException(ex);
            System.Diagnostics.Trace.Unindent();
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceRetryWriteLine(System.Exception ex = null, object iCount = null, object ExceptionNumber = null)
        {
            System.Diagnostics.Trace.Indent();
            System.Diagnostics.Trace.WriteLine(ClassLibraryStandard.HelperMethods.IsInteger(iCount) ? $"TRACE - Attempting to reconnect. Time: {System.DateTime.Now:h:mm:ss tt} Retry No.{iCount} - Exception No. {ExceptionNumber}" : $"TRACE - Attempting to reconnect. Time: {System.DateTime.Now:h:mm:ss tt}");
            TraceWriteException(ex);
            System.Diagnostics.Trace.Unindent();
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void TraceTimeElapsedWriteLine(System.DateTime fromThisTime, System.DateTime subtractThisTime, string trace = "TRACE - Time Elapsed: ")
        {
            System.Diagnostics.Trace.Indent();
            System.Diagnostics.Trace.WriteLine($"{ trace }{fromThisTime.Subtract(subtractThisTime).ToString("hh':'mm':'ss", UserHelper.culture)} seconds");
            System.Diagnostics.Trace.Unindent();
        }

        [System.Diagnostics.Conditional("TRACE")]
        internal static void ConfirmTraceState()
        {
            TraceWriteLine("TRACE - System Diagnostics Conditional TRACE is enabled");
            ConnectionHelper.ConfirmDebugState();
        }

        [System.Diagnostics.Conditional("TRACE")]
        private static void TraceWriteException(System.Exception ex)
        {
            if (ex != null)
            {
                System.Diagnostics.Trace.Indent();
                System.Diagnostics.Trace.WriteLine(ex.Message);
                System.Diagnostics.Trace.Unindent();
            }

        }

        private static void RunCommandLineShell(string[] args)
        {
            bool bDisplayHelp = (args.Length > 3 && args.Length < 11);

            System.Array.Resize(ref args, 11);

            string ParentString = args[1];
            string ChildString = args[2];

            int MatchingAlgorithm = ClassLibraryStandard.HelperMethods.IsInteger(args[3]) ? int.Parse(args[3], UserHelper.culture) : ExplorerForm.SetComparisonType(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_MATCHING_ALGORITHM));

            bool MakeCaseInsensitive = args[4] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[4]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_APPLY_CASE_INSENSITIVITY));
            bool PadToEqualLength = args[5] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[5]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PAD_TEXT));
            bool RemoveWhitespace = args[6] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[6]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REMOVE_NOISE_CHARACTERS));
            bool ReverseComparison = args[7] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[7]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REVERSE_COMPARE));
            bool PhoneticFilter = args[9] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[9]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PHONETIC_FILTER));
            bool WholeWordComparison = args[10] != null ? ClassLibraryStandard.HelperMethods.ToBoolean(args[10]) : ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_WHOLE_WORD_MATCH));

            if (bDisplayHelp || ((ParentString.ToLower(UserHelper.culture) == "help" || ParentString == "?") && ChildString == null))
            {
                System.Console.WriteLine(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_COMMANDLINE_HELP, System.Environment.NewLine, args[0], Constants.METHOD_RATCLIFF_OBERSHELP, Constants.METHOD_LEVENSHTEIN_DISTANCE, Constants.METHOD_HAMMING_DISTANCE, Constants.METHOD_RATCLIFF_OBERSHELP_VALUE, Constants.METHOD_LEVENSHTEIN_DISTANCE_VALUE, Constants.METHOD_HAMMING_DISTANCE_VALUE));
            }

            int iCompare = ExplorerForm.GetComparisonPercentage(MatchingAlgorithm, ParentString, ChildString, RemoveWhitespace, MakeCaseInsensitive, PadToEqualLength, ReverseComparison, PhoneticFilter, WholeWordComparison);
            TraceInitialisationComplete("TRACE - Commandline method complete");
            System.Diagnostics.Trace.WriteLine(null);

            System.Environment.Exit(iCompare);
        }

    }

}
