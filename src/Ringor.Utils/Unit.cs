using System;

namespace Dalion.Ringor.Utils {
    /// <summary>
    ///     Represents a type with a single value. This type is often used to denote the successful completion of a
    ///     void-returning method (C#) or a Sub procedure (Visual Basic).
    /// </summary>
    [Serializable]
    public struct Unit : IEquatable<Unit> {
        public static Unit Default { get; } = new Unit();

        public bool Equals(Unit other) {
            return true;
        }

        public override bool Equals(object obj) {
            return obj is Unit;
        }

        public override int GetHashCode() {
            return 0;
        }

        public override string ToString() {
            return "()";
        }

        public static bool operator ==(Unit first, Unit second) {
            return true;
        }

        public static bool operator !=(Unit first, Unit second) {
            return false;
        }
    }
}