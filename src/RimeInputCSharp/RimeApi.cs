using System;
using System.Runtime.InteropServices;

namespace RimeInputCSharp
{
    internal static class RimeApi
    {
        public delegate void RimeNotificationHandler(IntPtr contextObject,
                                                     ulong  sessionId,
                                                     string messageType,
                                                     string messageValue);

        #region Constansts Definition

        internal const string DllPath = "librime.dll";

        #endregion

        #region Structs Definition

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeTraits
        {
            internal int DataSize;

            /// <summary>
            /// v0.9.
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr ShardDataDir;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr UserDataDir;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr DistributionName;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr DistributionCodeName;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr DistributionVersion;

            /// <summary>
            /// v1.0.
            /// Pass a C-string constant in the format "rime.x"
            /// where 'x' is the name of your application.
            /// Add prefix "rime." to ensure old log files are automatically cleaned.
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr AppName;

            /// <summary>
            /// A list of modules to load before initializing.
            /// CSharp Data Type : String Array.
            /// Original Cpp DataType Definition : char**
            /// </summary>
            internal readonly IntPtr Modules;

            /// <summary>
            /// v1.6
            /// Minimal level of logged messages.
            /// Value is passed to Glog library using FLAGS_minloglevel variable.
            /// 0 = INFO (default), 1 = WARNING, 2 = ERROR, 3 = FATAL
            /// </summary>
            internal int MinLogLevel;

            /// <summary>
            /// Directory of log files.
            /// Value is passed to Glog library using FLAGS_log_dir variable.
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr LogDir;

            /// <summary>
            /// prebuilt data directory. defaults to ${shared_data_dir}/build
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr PrebuiltDataDir;

            /// <summary>
            /// staging directory. defaults to ${user_data_dir}/build
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr StagingDir;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeComposition
        {
            internal int Length;

            internal int CursorPos;

            internal int SelStart;

            internal int SelEnd;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr RreEdit;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeCandidate
        {
            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr Text;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr Comment;

            /// <summary>
            /// CSharp Data Type : Function Delegation
            /// </summary>
            internal IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeMenu
        {
            internal int      PageSize;

            internal int      PageNo;

            internal bool     IsLastPage;

            internal int      HighlightedCandidateIndex;

            internal int      NumCandidates;

            /// <summary>
            /// CSharp Data Type : RimeCandidate
            /// </summary>
            internal IntPtr   Candidates;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr   SelectKeys;
        }

        internal struct RimeCommit
        {
            internal int DataSize;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr Text;
        }

        internal struct RimeContext
        {
            internal int DataSize;

            /// <summary>
            /// v0.9
            /// </summary>
            internal RimeComposition Composition;

            internal RimeMenu        Menu;

            /// <summary>
            /// v0.9.2
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr CommitTextPreview;

            /// <summary>
            /// CSharp Data Type : String Array.
            /// Original Cpp DataType Definition : char**
            /// </summary>
            internal IntPtr SelectLabels;
        }

        internal struct RimeStatus
        {
            internal int DataSize;

            /// <summary>
            /// v0.9
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr SchemaId;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr SchemaName;

            internal bool IsDisabled;

            internal bool IsComposing;

            internal bool IsAsciiMode;

            internal bool IsFullShape;

            internal bool IsSimplified;

            internal bool IsTraditional;

            internal bool IsAsciiPunct;
        }

        internal struct RimeCandidateListIterator
        {
            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr        Ptr;

            internal int           Index;

            internal RimeCandidate Candidate;
        }

        internal struct RimeConfig
        {
            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr Ptr;
        }

        internal struct RimeConfigIterator
        {
            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr List;

            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr Map;

            internal       int   Index;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr Key;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal readonly IntPtr Path;
        }

        internal struct RimeSchemaListItem
        {
            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr SchemaId;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr Name;

            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr Reserved;
        }

        //internal struct RimeSchemaList
        //{
        //    internal ulong  Size;

        //    /// <summary>
        //    /// Original Data Type : RimeSchemaListItem
        //    /// </summary>
        //    internal IntPtr List;
        //}

        //internal abstract class RimeCustomApi
        //{
        //    internal int DataSize;
        //}

        //internal abstract class RimeModule
        //{
        //    internal int DataSize;

        //    internal IntPtr ModuleName;

        //    internal abstract void Initialize();

        //    internal abstract void Finalize();

        //    internal abstract IntPtr GetApi();
        //}

        #endregion

        #region Function Definition


        /// <summary>
        /// Call this function before accessing any other API.
        /// </summary>
        /// <param name="traits">ptr for RimeTraits</param>
        [DllImport(DllPath)]
        internal static extern void RimeSetup(IntPtr traits);

        /// <summary>
        /// - on loading schema:
        ///   + message_type="schema", message_value="luna_pinyin/Luna Pinyin"
        /// - on changing mode:
        ///   + message_type="option", message_value="ascii_mode"
        ///   + message_type="option", message_value="!ascii_mode"
        /// - on deployment:
        ///   + session_id = 0, message_type="deploy", message_value="start"
        ///   + session_id = 0, message_type="deploy", message_value="success"
        ///   + session_id = 0, message_type="deploy", message_value="failure"
        ///
        ///   handler      will be         called with       context_object as the first parameter
        ///   every time an event occurs in librime, until RimeFinalize() is called.
        ///   when handler is NULL, notification is disabled.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="contextObject"></param>
        [DllImport(DllPath)]
        internal static extern void RimeSetNotificationHandler(RimeNotificationHandler handler,
                                                               IntPtr                  contextObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traits">ptr for RimeTraits</param>
        [DllImport(DllPath)]
        internal static extern void RimeInitialize(IntPtr traits);

        [DllImport(DllPath)]
        internal static extern void RimeFinalize();

        [DllImport(DllPath)]
        internal static extern bool RimeStartMaintenance(bool fullCheck);

        [DllImport(DllPath)]
        internal static extern bool RimeIsMaintenancing();

        [DllImport(DllPath)]
        internal static extern void RimeJoinMaintenanceThread();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traits">ptr for RimeTraits</param>
        [DllImport(DllPath)]
        internal static extern void RimeDeployerInitialize(IntPtr traits);

        [DllImport(DllPath)]
        internal static extern bool RimePrebuildAllSchemas();

        [DllImport(DllPath)]
        internal static extern bool RimeDeployWorkspace();


        [DllImport(DllPath)]
        internal static extern bool RimeDeploySchema(string schemaFile);

        [DllImport(DllPath)]
        internal static extern bool RimeDeployConfigFile(string fileName,  string versionKey);

        [DllImport(DllPath)]
        internal static extern bool RimeSyncUserData();

        [DllImport(DllPath)]
        internal static extern ulong RimeCreateSession();

        [DllImport(DllPath)]
        internal static extern bool RimeFindSession(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern bool RimeDestroySession(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern void RimeCleanupStaleSessions();

        [DllImport(DllPath)]
        internal static extern void RimeCleanupAllSessions();

        [DllImport(DllPath)]
        internal static extern bool RimeProcessKey(ulong sessionId, int keycode, int mask);

        [DllImport(DllPath)]
        internal static extern bool RimeCommitComposition(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern void RimeClearComposition(ulong sessionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="commit">ptr for RimeCommit</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeGetCommit(ulong sessionId, IntPtr commit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commit">RimeCommit</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeFreeCommit(IntPtr commit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="context">ptr for RimeContext</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeGetContext(ulong sessionId, IntPtr context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">ptr for RimeContext</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeFreeContext(IntPtr context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="status">ptr for RimeStatus</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeGetStatus(long sessionId, IntPtr status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">ptr for RimeStatus</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeFreeStatus(IntPtr status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeCandidateListBegin(ulong sessionId, IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeCandidateListNext(IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        [DllImport(DllPath)]
        internal static extern void RimeCandidateListEnd(IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeCandidateListFromIndex(ulong  sessionId,
                                                               IntPtr iterator,
                                                               int    index);

        [DllImport(DllPath)]
        internal static extern void RimeSetOption(ulong sessionId,  string option, bool value);

        [DllImport(DllPath)]
        internal static extern bool RimeGetOption(ulong sessionId,  string option);

        [DllImport(DllPath)]
        internal static extern void RimeSetProperty(ulong sessionId,  string prop,  string value);

        [DllImport(DllPath)]
        internal static extern void RimeGetProperty(ulong sessionId, string prop, out string value, ulong bufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaList">ptr for RimeSchemaList</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeGetSchemaList(IntPtr schemaList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaList">ptr for RimeSchemaList</param>
        [DllImport(DllPath)]
        internal static extern void RimeFreeSchemaList(IntPtr schemaList);

        [DllImport(DllPath)]
        internal static extern bool RimeGetCurrentSchema(ulong sessionId, out string schemaId, ulong bufferSize);

        [DllImport(DllPath)]
        internal static extern bool RimeSelectSchema(ulong sessionId,  string schemaId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeSchemaOpen(string schemaId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigOpen(string configId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeUserConfigOpen(string configId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigClose(IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigInit(IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="yaml"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigLoadString(IntPtr config, string yaml);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigGetBool(IntPtr config, string key, ref bool value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigGetInt(IntPtr config, string key, ref int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigGetDouble(IntPtr config, string key, ref double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigGetString(IntPtr config, string key, string value, ulong bufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern string RimeConfigGetCString(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigSetBool(IntPtr config, string key, bool value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigSetInt(IntPtr config,  string key,  int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigSetDouble(IntPtr config, string key,  double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigSetString(IntPtr config, string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigGetItem(IntPtr config,  string key, IntPtr value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigSetItem(IntPtr config, string key, IntPtr value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigClear(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigCreateList(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigCreateMap(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern ulong RimeConfigListSize(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigBeginList(IntPtr iterator, IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigBeginMap(IntPtr iterator, IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigNext(IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        [DllImport(DllPath)]
        internal static extern void RimeConfigEnd(IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="signer"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool RimeConfigUpdateSignature(IntPtr config, string signer);

        [DllImport(DllPath)]
        internal static extern bool RimeSimulateKeySequence(ulong sessionId,  string keySequence);

        [DllImport(DllPath)]
        internal static extern bool RimeRunTask(string taskName);

        [DllImport(DllPath)]
        internal static extern string RimeGetSharedDataDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetUserDataDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetSyncDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetUserId();

        #endregion
    }
}
