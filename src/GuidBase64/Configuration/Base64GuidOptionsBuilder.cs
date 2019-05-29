namespace GuidBase64.Configuration {

    using JetBrains.Annotations;

    public class Base64GuidOptionsBuilder {

        private Base64GuidOptions _options;

        internal Base64GuidOptions Build() => this._options;

        [NotNull]
        public Base64GuidOptionsBuilder UsePadding() {
            this._options.Padding = true;

            return this;
        }

        [NotNull]
        public Base64GuidOptionsBuilder UseStandardBase64Encoding() {
            this._options.StandardBase64Encoding = true;

            return this;
        }
    }
}