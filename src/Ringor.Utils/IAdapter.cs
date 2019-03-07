namespace Dalion.Ringor.Utils {
    public interface IAdapter<in TIn, out TOut> {
        TOut Adapt(TIn input);
    }
}