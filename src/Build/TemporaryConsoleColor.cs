using System;

namespace Dalion.Ringor.Build {
    public class TemporaryConsoleColor : IDisposable {
        private readonly ConsoleColor _backupColor;

        public TemporaryConsoleColor(ConsoleColor color) {
            _backupColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        public void Dispose() {
            Console.ForegroundColor = _backupColor;
        }
    }
}