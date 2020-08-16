using System.Linq;
using Dapper;

namespace NAVService
{
    public static class DataAccess
    {
        public static bool TraceWrite { get; set; } = true;

        public static System.Collections.Generic.List<NAVUserPreferencesModel> GetUserPreferences(int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $"pGetUserPreferencesByUserID { UserHelper.UserPropertiesModel.iUserID }";
                        System.Collections.Generic.List<NAVUserPreferencesModel> model = Database.Query<NAVUserPreferencesModel>(queryStr).ToList();
                        
                        if (ModelExists()) return model;

                        bool ModelExists()
                        {
                            if (model == null) throw new System.NullReferenceException();
                            return true;
                        }

                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetUserPreferences(++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetUserPreferences(++iRetry);
            }

            return null;
        }

        public static System.Collections.Generic.List<NAVUserPreferencesModel> GetUserPreferencesByPreferenceType(string nvClientPreferenceType, int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $@"pGetUserPreferencesByPreferenceType { UserHelper.UserPropertiesModel.iUserID }, '{ nvClientPreferenceType }'";
                        return Database.Query<NAVUserPreferencesModel>(queryStr).ToList();
                    }

                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetUserPreferencesByPreferenceType(nvClientPreferenceType, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetUserPreferencesByPreferenceType(nvClientPreferenceType, ++iRetry);
            }

            return null;
        }

        public static string GetUserPreferenceByPreferenceName(string nvClientPreferenceName, int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserPropertiesModel != null)
                {
                    if (TraceWrite) LogHelper.TraceWriteCurrentState();

                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $@"pGetUserPreferenceByPreferenceName { UserHelper.UserPropertiesModel.iUserID }, '{ nvClientPreferenceName }'";

                        NAVUserPreferencesModel UserPreferencesModel = Database.QuerySingle<NAVUserPreferencesModel>(queryStr);
                        return (UserPreferencesModel.bClientValueRequired == 1) ? $"{ UserPreferencesModel.nvUserPreferenceValue }" : $" { UserPreferencesModel.bUserPreference }";
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetUserPreferenceByPreferenceName(nvClientPreferenceName, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetUserPreferenceByPreferenceName(nvClientPreferenceName, ++iRetry);
            }

            return null;
        }

        public static string GetStringAbbreviations(string nvInputString, int iRetry = 0)
        {
            try
            {
                int FlaggedWordsOnly = UserHelper.GetReplaceDefaultWordsOnly() ? 1 : 0;

                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $@"pSetStringAbbreviations { UserHelper.UserPropertiesModel.iUserID }, '{ nvInputString }', { FlaggedWordsOnly }";

                        NAVComparisonStringModel ComparisonStringModel = Database.QuerySingle<NAVComparisonStringModel>(queryStr, commandTimeout: 0);
                        return $"{ ComparisonStringModel.nvString }";
                    }

                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetStringAbbreviations(nvInputString, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetStringAbbreviations(nvInputString, ++iRetry);
            }

            return null;
        }

        public static string GetPhraseAbbreviationsByUserID(string nvInputString, int iRetry = 0)
        {
            try
            {
                int FlaggedWordsOnly = UserHelper.GetReplaceDefaultWordsOnly() ? 1 : 0;

                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $@"pGetPhraseAbbreviationsByUserID { UserHelper.UserPropertiesModel.iUserID }, '{ nvInputString }', { FlaggedWordsOnly }";

                        NAVComparisonStringModel ComparisonStringModel = Database.QuerySingle<NAVComparisonStringModel>(queryStr);
                        return $"{ ComparisonStringModel.nvString }";
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetPhraseAbbreviationsByUserID(nvInputString, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetPhraseAbbreviationsByUserID(nvInputString, ++iRetry);
            }

            return null;
        }

        public static void UpdateUserPreference(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    _ = Database.Execute("pUpdateUserPreference", param, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) UpdateUserPreference(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) UpdateUserPreference(param, ++iRetry);
            }

        }

        public static System.Collections.Generic.List<NAVAbbreviationTypeModel> GetAbbreviationTypes(int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $"pGetAbbreviationTypeByLanguage { UserHelper.UserPropertiesModel.iLanguageID }";
                        return Database.Query<NAVAbbreviationTypeModel>(queryStr).ToList();
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetAbbreviationTypes(++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetAbbreviationTypes(++iRetry);
            }

            return null;
        }

        public static System.Collections.Generic.List<NAVAbbreviationModel> GetAbbreviationsByType(int iAbbreviationTypeID, int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $"pGetWordAbbreviationsByLanguageByType { UserHelper.UserPropertiesModel.iLanguageID }, { iAbbreviationTypeID }";
                        return Database.Query<NAVAbbreviationModel>(queryStr).ToList();
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetAbbreviationsByType(iAbbreviationTypeID, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetAbbreviationsByType(iAbbreviationTypeID, ++iRetry);
            }

            return null;

        }

        public static System.Collections.Generic.List<NAVAbbreviationsModel> GetAbbreviationsByUserID(int iRetry = 0)
        {
            try
            {
                int FlaggedWordsOnly = UserHelper.GetReplaceDefaultWordsOnly() ? 1 : 0;

                if (UserHelper.UserPropertiesModel != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        string queryStr = $"pGetStringAbbreviations { UserHelper.UserPropertiesModel.iUserID }, { FlaggedWordsOnly }";
                        return Database.Query<NAVAbbreviationsModel>(queryStr).ToList();
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetAbbreviationsByUserID(++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetAbbreviationsByUserID(++iRetry);
            }

            return null;

        }

        public static string GetAbbreviationType(int iAbbreviationTypeID, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    string queryStr = $"pGetAbbreviationType { iAbbreviationTypeID }";

                    NAVAbbreviationTypeModel AbbreviationTypeModel = Database.QuerySingle<NAVAbbreviationTypeModel>(queryStr);
                    return $"{ AbbreviationTypeModel.nvAbbreviationType }";
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetAbbreviationType(iAbbreviationTypeID, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetAbbreviationType(iAbbreviationTypeID, ++iRetry);
            }

            return null;
        }

        public static void UpdateAbbreviation(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    _ = Database.Execute("pUpdateAbbreviation", param, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) UpdateAbbreviation(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) UpdateAbbreviation(param, ++iRetry);
            }

        }

        public static int InsertAbbreviation(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                if (param != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        _ = Database.Execute("pInsertAbbreviation", param, commandType: System.Data.CommandType.StoredProcedure);
                        return param.Get<int>("@" + Constants.COLUMN_ABBREVIATION_RETURNCODE);
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) InsertAbbreviation(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) InsertAbbreviation(param, ++iRetry);
            }

            return 0;
        }

        public static int EditAbbreviation(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                if (param != null)
                {
                    using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                    {
                        _ = Database.Execute("pEditAbbreviation", param, commandType: System.Data.CommandType.StoredProcedure);
                        return param.Get<int>("@" + Constants.COLUMN_ABBREVIATION_RETURNCODE);
                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) EditAbbreviation(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) EditAbbreviation(param, ++iRetry);
            }

            return 0;
        }

        public static void DeleteAbbreviation(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    _ = Database.Execute("pDropWordByID", param, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) DeleteAbbreviation(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) DeleteAbbreviation(param, ++iRetry);
            }

        }

        public static void InsertUserStateLogEntry(DynamicParameters param, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    _ = Database.Execute("pInsertUserStateLogEntry", param, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) InsertUserStateLogEntry(param, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) InsertUserStateLogEntry(param, ++iRetry);
            }

        }

        public static NAVUserStateModel GetLastUserStateLogEntry(int iRetry = 0)
        {
            try
            {
                if (UserHelper.UserStateModel.bConfirmed)
                {
                    ApplyLogFileConfiguration();

                    if (UserHelper.UserPropertiesModel != null)
                    {
                        using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                        {
                            LogHelper.TraceWritePreviousState();

                            string queryStr = $"pGetLastUserStateLogEntry { UserHelper.UserPropertiesModel.iUserID }";

                            NAVUserStateModel UserStateModel = Database.QuerySingle<NAVUserStateModel>(queryStr);
                            return UserStateModel;
                        }

                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) GetLastUserStateLogEntry(++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) GetLastUserStateLogEntry(++iRetry);
            }

            void ApplyLogFileConfiguration()
            {
                if (log4net.LogManager.GetRepository() is log4net.Repository.Hierarchy.Hierarchy hierarchy)
                {
                    log4net.ThreadContext.Properties["logfilename"] = Properties.Resources.ERROR_LOG_FILENAME;
                    log4net.ThreadContext.Properties["userid"] = UserHelper.UserPropertiesModel.iUserID;
                    log4net.ThreadContext.Properties["client"] = UserHelper.UserPropertiesModel.nvClientName;
                }

            }

            return null;
        }

        public static NAVUserPropertiesModel InitialiseUserProperties(int iUserID, int iRetry = 0)
        {
            try
            {
                using (System.Data.IDbConnection Database = new System.Data.SqlClient.SqlConnection(ConnectionHelper.CONNECTION_LOCATION))
                {
                    System.DateTime logHelperStartTime = System.DateTime.Now;

                    string queryStr = $"pUserExists { iUserID }";
                    UserHelper.UserStateModel = Database.QuerySingle<NAVUserStateModel>(queryStr);

                    LogHelper.TraceTimeElapsedWriteLine(System.DateTime.Now, logHelperStartTime, "TRACE - Dapper initialisation (first Dapper invocation). Time Elapsed: ");

                    if (UserHelper.UserStateModel != null)
                    {
                        if (UserHelper.UserStateModel.bConfirmed)
                        {
                            LogHelper.ApplyLogAppenderConfiguration();

                            LogHelper.TraceWriteLine("TRACE - Default state initialisation");

                            queryStr = $"pGetUserProperties { iUserID }";
                            NAVUserPropertiesModel UserPropertiesModel = Database.QuerySingle<NAVUserPropertiesModel>(queryStr);

                            return UserPropertiesModel;
                        }

                    }

                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (LogHelper.RetrySQLException(ex, iRetry)) InitialiseUserProperties(iUserID, ++iRetry);
            }
            catch (System.NullReferenceException ex)
            {
                if (LogHelper.RetryNullReferenceException(ex, iRetry)) InitialiseUserProperties(iUserID, ++iRetry);
            }

            return null;
        }

    }

} 
