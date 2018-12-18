using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Test.Domain.Enum
{
    public enum UserStatusEnum
    {
        [Description("Default")]
        Default,

        [Description("Unable")]
        Unable,

        [Description("Activate")]
        Activate,

    }
}
