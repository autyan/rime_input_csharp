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

            internal RimeBool IsLastPage;

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
            internal IntPtr   SchemaName;

            internal RimeBool IsDisabled;

            internal RimeBool IsComposing;

            internal RimeBool IsAsciiMode;

            internal RimeBool IsFullShape;

            internal RimeBool IsSimplified;

            internal RimeBool IsTraditional;

            internal RimeBool IsAsciiPunct;
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

        internal struct RimeSchemaList
        {
            internal ulong  Size;

            /// <summary>
            /// Original Data Type : RimeSchemaListItem
            /// </summary>
            internal IntPtr List;
        }
        ;

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
        internal static extern RimeBool RimeStartMaintenance(RimeBool fullCheck);

        [DllImport(DllPath)]
        internal static extern RimeBool RimeIsMaintenancing();

        [DllImport(DllPath)]
        internal static extern void RimeJoinMaintenanceThread();

        #endregion
    }

    public enum RimeBool
    {
        False = 0,

        True = 1
    }
}
