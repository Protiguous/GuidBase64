namespace GuidBase64.Configuration {

    using System;

    internal struct Base64GuidOptions {

        internal Boolean _padding;

        internal Boolean _standardBase64Encoding;

        internal Boolean Padding {
            get => this._padding;
            set => this._padding = value;
        }

        internal Boolean StandardBase64Encoding {
            get => this._standardBase64Encoding;
            set => this._standardBase64Encoding = value;
        }

        internal Base64GuidOptions( Boolean standardBase64Encoding, Boolean padding ) {
            this._standardBase64Encoding = standardBase64Encoding;
            this._padding = padding;
        }
    }
}