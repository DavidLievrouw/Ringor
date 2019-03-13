namespace Dalion.Ringor.Utils {
    public interface IJsonSerializer {
        string Serialize(object model);
        string Serialize(object model, bool withIndentation);
        T Deserialize<T>(string json);
    }
}
