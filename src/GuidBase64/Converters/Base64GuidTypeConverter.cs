namespace GuidBase64.Converters {

    using System;
    using System.ComponentModel;
    using System.Globalization;
    using JetBrains.Annotations;

    public class Base64GuidTypeConverter : TypeConverter {

        public override Boolean CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) => sourceType == typeof( String );

        public override Boolean CanConvertTo( ITypeDescriptorContext context, Type destinationType ) => destinationType == typeof( String );

        [NotNull]
        public override Object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, Object value ) => new Base64Guid( value as String );

        [NotNull]
        public override Object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, [NotNull] Object value, Type destinationType ) =>
            ( ( Base64Guid )value ).ToString();
    }
}