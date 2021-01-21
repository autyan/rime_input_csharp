using System;
using System.Runtime.InteropServices;

namespace RimeInputCSharp
{
    public class RimeInput
    {
        private RimeEngineCore.RimeTraits _traitsStruct;

        private IntPtr _traitsStructPtr;

        private RimeEngineCore.RimeApi _rimeApi;

        private RimeInput(RimeTraits traits)
        {
            InitRimeApi(traits);
        }

        private void InitRimeApi(RimeTraits traits)
        {
            _traitsStruct    = traits.ToRimeTraits();
            _traitsStructPtr = Marshal.AllocHGlobal(Marshal.SizeOf(_traitsStruct));
            Marshal.StructureToPtr(traits.ToRimeTraits(), _traitsStructPtr, false);
            var apiPtr = RimeEngineCore.RimeGetApi();
            _rimeApi = Marshal.PtrToStructure<RimeEngineCore.RimeApi>(apiPtr);
            _rimeApi.RimeSetUp(_traitsStructPtr);
        }

        public static RimeInput CreateOrGetInstance(RimeTraits traits)
        {
            return new RimeInput(traits);
        }
    }
}
