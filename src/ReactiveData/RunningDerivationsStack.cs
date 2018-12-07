using System;

namespace ReactiveData {
    internal static class RunningDerivationsStack {
        [ThreadStatic] internal static RunningDerivation Top;
    }
}
