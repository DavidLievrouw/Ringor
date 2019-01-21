namespace Dalion.Ringor.Api.Serialization {
    public interface IJsonSerializer {
        string Serialize(object model);
        string Serialize(object model, bool withIndentation);
        T Deserialize<T>(string json);
    }
}
