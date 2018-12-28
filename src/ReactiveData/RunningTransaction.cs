using System;

namespace ReactiveData {
    static class RunningTransaction {
        [ThreadStatic] internal static Transaction Current;
    }
}
