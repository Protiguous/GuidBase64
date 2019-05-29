namespace GuidBase64.Extensions {

    using System;
    using Configuration;
    using JetBrains.Annotations;

    public static class GuidExtensions {

        /// <summary>
        ///     Returns a new <see cref="Base64Guid" /> instance with the same value as the original
        ///     <see cref="Guid" /> structure.
        /// </summary>
        /// <param name="guid">The original <see cref="Guid" /> structure.</param>
        /// <returns>A <see cref="Base64Guid" /> instance with the same value as <paramref name="guid" />.</returns>
        public static Base64Guid ToBase64Guid( this Guid guid ) => new Base64Guid( guid );

        /// <summary>
        ///     Returns a new <see cref="Base64Guid" /> instance with the same value as the original
        ///     <see cref="Guid" /> structure with a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="guid">The original <see cref="Guid" /> structure.</param>
        /// <param name="optionsAction">
        ///     The action to configure the <see cref="Base64GuidOptions" /> for the <see cref="Base64Guid" /> instance.
        /// </param>
        /// <returns>A <see cref="Base64Guid" /> instance with the same value as <paramref name="guid" />.</returns>
        public static Base64Guid ToBase64Guid( this Guid guid, Action<Base64GuidOptionsBuilder> optionsAction ) => new Base64Guid( guid, optionsAction );

        /// <summary>
        ///     Returns a string representation of the value of this instance in base 64 format.
        /// </summary>
        /// <param name="guid">The original <see cref="Guid" /> structure.</param>
        /// <returns>
        ///     The value of this instance, formatted with the RFC 4648 Section 5 base 64 character set
        ///     and with padding stripped.
        /// </returns>
        [NotNull]
        public static String ToBase64String( this Guid guid ) => new Base64Guid( guid ).ToString();

        /// <summary>
        ///     Returns a string representation of the value of this instance in base 64 format,
        ///     with a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="guid">The original <see cref="Guid" /> structure.</param>
        /// <param name="optionsAction">
        ///     The action to configure the <see cref="Base64GuidOptions" /> for the <see cref="Base64Guid" /> instance.
        /// </param>
        /// <returns>
        ///     The value of this instance, formatted with the RFC 4648 Section 5 base 64 character set
        ///     and with padding stripped. If the instance has been configured to use the standard
        ///     base 64 encoding, the instance value will be formatted with the RFC 4648 Section 4 base
        ///     64 character set instead. If the instance has been configured to use padding, the instance value
        ///     will not have its padding stripped.
        /// </returns>
        [NotNull]
        public static String ToBase64String( this Guid guid, Action<Base64GuidOptionsBuilder> optionsAction ) => new Base64Guid( guid, optionsAction ).ToString();
    }
}