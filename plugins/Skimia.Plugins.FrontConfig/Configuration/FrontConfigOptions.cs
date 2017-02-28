namespace Skimia.Plugins.FrontConfig.Configuration
{
    public class FrontConfigOptions
    {
        public static readonly string DefaultPath = "front_plugins";

        /// <value>
        /// The <see cref="Path"/> to store plugins config
        /// </value>
        public string Path { get; set; } = DefaultPath;
    }
}
