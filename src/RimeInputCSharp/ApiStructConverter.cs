using System;
using System.Runtime.InteropServices;

namespace RimeInputCSharp
{
    internal static class ApiStructConverter
    {
        internal static RimeEngineCore.RimeTraits ToRimeTraits(this RimeTraits traits)
        {
            var traitsStruct = new RimeEngineCore.RimeTraits
                               {
                                   AppName              = Marshal.StringToHGlobalAnsi(traits.AppName),
                                   DistributionCodeName = Marshal.StringToHGlobalAnsi(traits.DistributionCodeName),
                                   DistributionName     = Marshal.StringToHGlobalAnsi(traits.DistributionName),
                                   DistributionVersion  = Marshal.StringToHGlobalAnsi(traits.DistributionVersion),
                                   LogDir               = Marshal.StringToHGlobalAnsi(traits.LogDir),
                                   MinLogLevel          = traits.MinLogLevel,
                                   Modules              = IntPtr.Zero,
                                   PrebuiltDataDir      = Marshal.StringToHGlobalAnsi(traits.PrebuiltDataDir),
                                   ShardDataDir         = Marshal.StringToHGlobalAnsi(traits.ShardDataDir),
                                   StagingDir           = Marshal.StringToHGlobalAnsi(traits.StagingDir)
                               };
            traitsStruct.DataSize = Marshal.SizeOf(traitsStruct);

            return traitsStruct;
        }

    }
}
