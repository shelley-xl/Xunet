using System;

[assembly: CLSCompliant(false)]

#if PLAT_SKIP_LOCALS_INIT
[module: System.Runtime.CompilerServices.SkipLocalsInit]
#endif
