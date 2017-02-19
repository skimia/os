namespace Skimia.Plugins.StaticFront.Configuration
{
    public class RewriteOptions
    {
        public static readonly string ApacheDefaultFile = "rewrite.apache";

        public string Apache { get; set; } = ApacheDefaultFile;

        public RewriteOptions()
        {
            Apache = ApacheDefaultFile;
        }
    }
}
