namespace Dalion.Ringor {
    public static class Constants {
        public static class Headers {
            public const string ResponseType = "Dalion-ResponseType";
        } 

        public static class ViewData {
            public const string ApplicationInfo = "Dalion-ApplicationInfo";
            public const string Styles = "Dalion-Styles";
            public const string Scripts = "Dalion-Scripts";
            public const string ErrorPath = "Dalion-ErrorPath";
            public const string ErrorPathBase = "Dalion-ErrorPathBase";
            public const string ErrorQueryString = "Dalion-ErrorQueryString";
            public const string Error = "Dalion-Error";
        }

        public static class ResponseTypes {
            public const string SpaView = "SPAView";
            public const string ServerError = "ServerError";
            public const string NotFoundError = "NotFoundError";
            public const string CatchAllError = "CatchAllError";
        }
    }
}