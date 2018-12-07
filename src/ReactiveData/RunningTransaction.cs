using System;

namespace ReactiveData {
    internal static class RunningTransaction {
        [ThreadStatic] internal static Transaction Current;
    }
}
