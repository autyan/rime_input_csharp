using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RimeInputCSharp
{
    internal static class RimeEngineCore
    {
        public delegate void RimeNotificationHandler(IntPtr contextObject,
                                                     ulong  sessionId,
                                                     string messageType,
                                                     string messageValue);

        public delegate void RimeProtoCommitBuilder();

        public delegate void RimeProtoContextBuilder();

        public delegate void RimeProtoStatusBuilder();

        #region Constansts Definition

        private const string DllPath = "rime.dll";

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
            internal IntPtr ShardDataDir;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr UserDataDir;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr DistributionName;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr DistributionCodeName;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr DistributionVersion;

            /// <summary>
            /// v1.0.
            /// Pass a C-string constant in the format "rime.x"
            /// where 'x' is the name of your application.
            /// Add prefix "rime." to ensure old log files are automatically cleaned.
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr AppName;

            /// <summary>
            /// A list of modules to load before initializing.
            /// CSharp Data Type : String Array.
            /// Original Cpp DataType Definition : char**
            /// </summary>
            internal IntPtr Modules;

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
            internal IntPtr LogDir;

            /// <summary>
            /// prebuilt data directory. defaults to ${shared_data_dir}/build
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr PrebuiltDataDir;

            /// <summary>
            /// staging directory. defaults to ${user_data_dir}/build
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr StagingDir;
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

            internal bool      IsLastPage;

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

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeCommit
        {
            internal int DataSize;

            /// <summary>
            /// CSharp Data Type : String
            /// </summary>
            internal IntPtr Text;
        }

        [StructLayout(LayoutKind.Sequential)]
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

        [StructLayout(LayoutKind.Sequential)]
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

            internal bool  IsDisabled;

            internal bool  IsComposing;

            internal bool  IsAsciiMode;

            internal bool  IsFullShape;

            internal bool  IsSimplified;

            internal bool  IsTraditional;

            internal bool  IsAsciiPunct;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeCandidateListIterator
        {
            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr        Ptr;

            internal int           Index;

            internal RimeCandidate Candidate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeConfig
        {
            /// <summary>
            /// Original Data Type : void*
            /// </summary>
            internal IntPtr Ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
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

        [StructLayout(LayoutKind.Sequential)]
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

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeSchemaList
        {
            internal ulong Size;

            /// <summary>
            /// Original Data Type : RimeSchemaListItem
            /// </summary>
            internal IntPtr List;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeCustomApi
        {
            internal int DataSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeModule
        {
            internal int dataSize;

            internal string moduleName;

            internal initialize Initialize { get; set; }

            internal finalize Finalize { get; set; }

            internal get_api GetApi { get; set; }

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void initialize();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void finalize();

            /// <summary>
            /// 
            /// </summary>
            /// <returns>ptr for RimeCustomApi</returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate IntPtr get_api();
        }
        ;


        [StructLayout(LayoutKind.Sequential)]
        internal struct RimeApi
        {
            internal int DataSize;

            internal setup SetUp { get; set; }

            internal set_notification_handler SetNotificationHandler { get; set; }

            internal initialize Initialize { get; set; }

            internal finalize Finalize { get; set; }

            internal start_maintenance StartMaintenance { get; set; }

            internal is_maintenance_mode IsMaintenanceMode { get; set; }

            internal join_maintenance_thread JoinMaintenanceThread { get; set; }

            internal deployer_initialize DeployerInitialize { get; set; }

            internal prebuild Prebuild { get; set; }

            internal deploy Deploy { get; set; }

            internal deploy_schema DeploySchema { get; set; }

            internal deploy_config_file DeployConfigFile { get; set; }

            internal sync_user_data SyncUserData { get; set; }

            internal create_session CreateSession { get; set; }

            internal find_session FindSession { get; set; }

            internal destroy_session DestroySession { get; set; }

            internal cleanup_stale_sessions CleanupStaleSessions { get; set; }

            internal cleanup_all_sessions CleanupAllSessions { get; set; }

            internal process_key ProcessKey { get; set; }

            internal commit_composition CommitComposition { get; set; }

            internal clear_composition ClearComposition { get; set; }

            internal get_commit GetCommit { get; set; }

            internal free_commit FreeCommit { get; set; }

            internal get_context GetContext { get; set; }

            internal free_context FreeContext { get; set; }

            internal get_status GetStatus { get; set; }

            internal free_status FreeStatus { get; set; }

            internal set_option SetOption { get; set; }

            internal get_option GetOption { get; set; }

            internal set_property SetProperty { get; set; }

            internal get_property GetProperty { get; set; }

            internal get_schema_list GetSchemaList { get; set; }

            internal free_schema_list FreeSchemaList { get; set; }

            internal get_current_schema GetCurrentSchema { get; set; }

            internal select_schema SelectSchema { get; set; }

            internal schema_open SchemaOpen { get; set; }

            internal config_open ConfigOpen { get; set; }

            internal config_close ConfigClose { get; set; }

            internal config_get_bool  ConfigGetbool  { get; set; }

            internal config_get_int ConfigGetInt { get; set; }

            internal config_get_double ConfigGetDouble { get; set; }

            internal config_get_string ConfigGetString { get; set; }

            internal config_get_cstring ConfigGetCString { get; set; }

            internal config_update_signature ConfigUpdateSignature { get; set; }

            internal config_begin_map ConfigBeginMap { get; set; }

            internal config_next ConfigNext { get; set; }

            internal config_end ConfigEnd { get; set; }

            internal simulate_key_sequence SimulateKeySequence { get; set; }

            internal register_module RegisterModule { get; set; }

            internal find_module FindModule { get; set; }

            internal run_task RunTask { get; set; }

            internal get_shared_data_dir GetSharedDataDir { get; set; }

            internal get_user_data_dir GetUserDataDir { get; set; }

            internal get_sync_dir GetSyncDir { get; set; }

            internal get_user_id GetUserId { get; set; }

            internal get_user_data_sync_dir GetUserDataSyncDir { get; set; }

            internal config_init ConfigInit { get; set; }
            
            internal config_load_string ConfigLoadString { get; set; }
            
            internal config_set_bool ConfigSetBool { get; set; }
            
            internal config_set_int ConfigSetInt { get; set; }

            internal config_set_double ConfigSetDouble {get; set;}

            internal config_set_string ConfigSetString { get; set; }

            internal config_get_item ConfigGetItem { get; set; }

            internal config_set_item ConfigSetItem { get; set; }

            internal config_clear ConfigClear { get; set; }

            internal config_create_list ConfigCreateList { get; set; }

            internal config_create_map ConfigCreateMap { get; set; }

            internal config_list_size ConfigListSize { get; set; }

            internal config_begin_list ConfigBeginList { get; set; }

            internal get_input GetInput { get; set; }

            
            internal get_caret_pos GetCaretPos { get; set; }


            internal select_candidate SelectCandidate { get; set; }

            internal get_version GetVersion { get; set; }

            internal set_caret_pos SetCaretPos { get; set; }

            internal select_candidate_on_current_page SelectCandidateOnCurrentPage { get; set; }

            internal candidate_list_begin CandidateListBegin { get; set; }

            internal candidate_list_next CandidateListNext { get; set; }

            internal candidate_list_end CandidateListEnd { get; set; }

            internal user_config_open UserConfigOpen { get; set; }

            internal candidate_list_from_index CandidateListFromIndex { get; set; }

            internal get_prebuilt_data_dir GetPrebuiltDataDir { get; set; }

            internal get_staging_dir GetStagingDir { get; set; }

            internal commit_proto CommitProto { get; set; }

            internal context_proto ContextProto { get; set; }

            internal status_proto StatusProto { get; set; }

            /*! setup
             *  Call this function before accessing any other API functions.
             */
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void setup(IntPtr traits);

            /*! Set up the notification callbacks
            *  Receive notifications
            *  - on loading schema:
            *    + message_type="schema", message_value="luna_pinyin/Luna Pinyin"
            *  - on changing mode:
            *    + message_type="option", message_value="ascii_mode"
            *    + message_type="option", message_value="!ascii_mode"
            *  - on deployment:
            *    + session_id = 0, message_type="deploy", message_value="start"
            *    + session_id = 0, message_type="deploy", message_value="success"
            *    + session_id = 0, message_type="deploy", message_value="failure"
            *
            *  handler will be called with context_object as the first parameter
            *  every time an event occurs in librime, until RimeFinalize() is called.
            *  when handler is NULL, notification is disabled.
            */
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void set_notification_handler(RimeNotificationHandler handler,
                                                            IntPtr                  contextObject);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="traits">ptr for RimeTraits</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void initialize(IntPtr traits);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void finalize();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  start_maintenance(bool  fullCheck);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  is_maintenance_mode();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void join_maintenance_thread();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="traits">ptr for RimeTraits</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void deployer_initialize (IntPtr traits);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  prebuild();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  deploy();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  deploy_schema(string schemaFile);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  deploy_config_file(string fileName, string versionKey);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  sync_user_data();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate ulong create_session();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  find_session(ulong sessionId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  destroy_session(ulong sessionId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void cleanup_stale_sessions();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void cleanup_all_sessions();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  process_key(ulong sessionId, int keycode, int mask);

            // return True if there is unread commit text
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  commit_composition(ulong sessionId);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void clear_composition(ulong sessionId);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="commit">ptr for RimeCommit</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_commit(ulong sessionId, IntPtr commit);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="commit">ptr for RimeCommit</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  free_commit(IntPtr commit);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="context">ptr for RimeContext</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_context(ulong sessionId, IntPtr context);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ctx">ptr for RimeContext</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  free_context(IntPtr ctx);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="status">ptr for RimeStatus</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_status(ulong sessionId, IntPtr status);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="status">ptr for RimeStatus</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  free_status(IntPtr status);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void set_option(ulong sessionId, string option, bool  value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_option(ulong sessionId, ref string option);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]

            internal delegate void set_property(ulong sessionId, string prop, string value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_property(ulong sessionId, string prop,  ref string value, ulong bufferSize);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="schemaList">ptr for RimeSchemaList</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_schema_list(IntPtr schemaList);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="schemaList">ptr for RimeSchemaList</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void free_schema_list (IntPtr schemaList);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  get_current_schema(ulong sessionId, string schemaIdd, ulong bufferSize);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  select_schema(ulong sessionId, string schemaId);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="schemaId"></param>
            /// <param name="config">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  schema_open(string schemaId, IntPtr config);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="configId"></param>
            /// <param name="config">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_open(string configId, IntPtr config);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_close(IntPtr config);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_get_bool (IntPtr config, string key, ref bool  value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_get_int(IntPtr config, string key, ref int value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_get_double(IntPtr config, string key, ref double value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <param name="bufferSize"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_get_string(IntPtr config, string key, ref string value, ulong bufferSize);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string config_get_cstring(IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="signer"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_update_signature(IntPtr config, string signer);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeConfigIterator</param>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_begin_map(IntPtr iterator, IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeConfigIterator</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  config_next(IntPtr iterator);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeConfigIterator</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void config_end(IntPtr iterator);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  simulate_key_sequence(ulong sessionId, string keySequence);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="module">ptr for RimeModule</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  register_module(IntPtr module);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="moduleName">ptr for RimeModule</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate IntPtr find_module(string moduleName);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool  run_task(string taskName);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_shared_data_dir();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_user_data_dir();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_sync_dir();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_user_id();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void get_user_data_sync_dir(ref string dir, ulong bufferSize);

            //! initialize an empty config object
            /*!
             * should call config_close() to free the object
             */
            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_init(IntPtr config);

            //! deserialize config from a yaml string
            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="yaml"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_load_string(IntPtr config, string yaml);

            // configuration: setters
            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_set_bool (IntPtr config, string key, bool value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_set_int(IntPtr config, string key, int value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_set_double(IntPtr config, string key, double value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_set_string(IntPtr config, string key,  string value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value">ptr for RimeConfig</param>
            /// <returns></returns>
            // configuration: manipulating complex structures
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_get_item(IntPtr config, string key, IntPtr value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <param name="value">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_set_item(IntPtr config, string key, IntPtr value);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_clear(IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_create_list(IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_create_map(IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate ulong config_list_size(IntPtr config, string key);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeConfigIterator</param>
            /// <param name="config">ptr for RimeConfig</param>
            /// <param name="key"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool config_begin_list(IntPtr iterator, IntPtr config, string key);

            //! get raw input
            /*!
             *  NULL is returned if session does not exist.
             *  the returned pointer to input string will become invalid upon editing.
             */
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_input(ulong sessionId);


            //! caret posistion in terms of raw input
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate ulong get_caret_pos(ulong sessionId);

            //! select a candidate at the given index in candidate list.
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool select_candidate(ulong sessionId, ulong index);

            //! get the version of librime
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_version();

            //! set caret posistion in terms of raw input
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void set_caret_pos(ulong sessionId, ulong caretPos);

            //! select a candidate from current page.
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool select_candidate_on_current_page(ulong sessionId, ulong index);

            //! access candidate list.
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="iterator">ptr for RimeCandidateListIterator</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool candidate_list_begin(ulong sessionId, IntPtr iterator);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeCandidateListIterator</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool candidate_list_next(IntPtr iterator);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="iterator">ptr for RimeCandidateListIterator</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void candidate_list_end(IntPtr iterator);

            //! access config files in user data directory, eg. user.yaml and installation.yaml
            /// <summary>
            /// 
            /// </summary>
            /// <param name="configId"></param>
            /// <param name="config">ptr for RimeConfig</param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool user_config_open(string configId, IntPtr config);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="iterator">ptr for RimeCandidateListIterator</param>
            /// <param name="index"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate bool candidate_list_from_index(ulong sessionId, IntPtr iterator, int index);

            //! prebuilt data directory.
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_prebuilt_data_dir();

            //! staging directory, stores data files deployed to a Rime client
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate string get_staging_dir();

            //! capnproto API.
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void commit_proto(ulong sessionId, RimeProtoCommitBuilder commitCommitBuilder);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void context_proto (ulong sessionId, RimeProtoContextBuilder contextBuilder);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void status_proto (ulong sessionId, RimeProtoStatusBuilder statusBuilder);
        }

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
        internal static extern bool  RimeStartMaintenance(bool  fullCheck);

        [DllImport(DllPath)]
        internal static extern bool  RimeIsMaintenancing();

        [DllImport(DllPath)]
        internal static extern void RimeJoinMaintenanceThread();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traits">ptr for RimeTraits</param>
        [DllImport(DllPath)]
        internal static extern void RimeDeployerInitialize(IntPtr traits);

        [DllImport(DllPath)]
        internal static extern bool  RimePrebuildAllSchemas();

        [DllImport(DllPath)]
        internal static extern bool  RimeDeployWorkspace();


        [DllImport(DllPath)]
        internal static extern bool  RimeDeploySchema(string schemaFile);

        [DllImport(DllPath)]
        internal static extern bool  RimeDeployConfigFile(string fileName,  string versionKey);

        [DllImport(DllPath)]
        internal static extern bool  RimeSyncUserData();

        [DllImport(DllPath)]
        internal static extern ulong RimeCreateSession();

        [DllImport(DllPath)]
        internal static extern bool  RimeFindSession(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern bool  RimeDestroySession(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern void RimeCleanupStaleSessions();

        [DllImport(DllPath)]
        internal static extern void RimeCleanupAllSessions();

        [DllImport(DllPath)]
        internal static extern bool  RimeProcessKey(ulong sessionId, int keycode, int mask);

        [DllImport(DllPath)]
        internal static extern bool  RimeCommitComposition(ulong sessionId);

        [DllImport(DllPath)]
        internal static extern void RimeClearComposition(ulong sessionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="commit">ptr for RimeCommit</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeGetCommit(ulong sessionId, IntPtr commit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commit">RimeCommit</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeFreeCommit(IntPtr commit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="context">ptr for RimeContext</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeGetContext(ulong sessionId, IntPtr context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">ptr for RimeContext</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeFreeContext(IntPtr context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="status">ptr for RimeStatus</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeGetStatus(long sessionId, IntPtr status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">ptr for RimeStatus</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeFreeStatus(IntPtr status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeCandidateListBegin(ulong sessionId, IntPtr iterator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeCandidateListIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeCandidateListNext(IntPtr iterator);

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
        internal static extern bool  RimeCandidateListFromIndex(ulong  sessionId,
                                                               IntPtr iterator,
                                                               int    index);

        [DllImport(DllPath)]
        internal static extern void RimeSetOption(ulong sessionId,  string option, bool  value);

        [DllImport(DllPath)]
        internal static extern bool  RimeGetOption(ulong sessionId,  string option);

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
        internal static extern bool  RimeGetSchemaList(IntPtr schemaList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaList">ptr for RimeSchemaList</param>
        [DllImport(DllPath)]
        internal static extern void RimeFreeSchemaList(IntPtr schemaList);

        [DllImport(DllPath)]
        internal static extern bool  RimeGetCurrentSchema(ulong sessionId, out string schemaId, ulong bufferSize);

        [DllImport(DllPath)]
        internal static extern bool  RimeSelectSchema(ulong sessionId,  string schemaId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeSchemaOpen(string schemaId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigOpen(string configId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeUserConfigOpen(string configId, IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigClose(IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigInit(IntPtr config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="yaml"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigLoadString(IntPtr config, string yaml);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigGetbool (IntPtr config, string key, ref bool  value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigGetInt(IntPtr config, string key, ref int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigGetDouble(IntPtr config, string key, ref double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigGetString(IntPtr config, string key, string value, ulong bufferSize);

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
        internal static extern bool  RimeConfigSetbool (IntPtr config, string key, bool  value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigSetInt(IntPtr config,  string key,  int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigSetDouble(IntPtr config, string key,  double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigSetString(IntPtr config, string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigGetItem(IntPtr config,  string key, IntPtr value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <param name="value">ptr for RimeConfig</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigSetItem(IntPtr config, string key, IntPtr value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigClear(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigCreateList(IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigCreateMap(IntPtr config, string key);

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
        internal static extern bool  RimeConfigBeginList(IntPtr iterator, IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        /// <param name="config">ptr for RimeConfig</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigBeginMap(IntPtr iterator, IntPtr config, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iterator">ptr for RimeConfigIterator</param>
        /// <returns></returns>
        [DllImport(DllPath)]
        internal static extern bool  RimeConfigNext(IntPtr iterator);

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
        internal static extern bool  RimeConfigUpdateSignature(IntPtr config, string signer);

        [DllImport(DllPath)]
        internal static extern bool  RimeSimulateKeySequence(ulong sessionId,  string keySequence);

        [DllImport(DllPath)]
        internal static extern bool  RimeRunTask(string taskName);

        [DllImport(DllPath)]
        internal static extern string RimeGetSharedDataDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetUserDataDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetSyncDir();

        [DllImport(DllPath)]
        internal static extern string RimeGetUserId();

        /// <summary>
        /// get rimeApi object
        /// </summary>
        /// <returns>ptr for RimeApi</returns>
        [DllImport(DllPath, EntryPoint = "rime_get_api")]
        internal static extern IntPtr RimeGetApi();

        #endregion
    }
}
