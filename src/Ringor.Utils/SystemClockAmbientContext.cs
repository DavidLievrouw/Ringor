namespace Dalion.Ringor.Utils {
    public class SystemClockAmbientContext : AmbientContext<ISystemClock> {
        public SystemClockAmbientContext(ISystemClock instance) : base(instance) { }
    }
}