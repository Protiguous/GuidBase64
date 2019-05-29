﻿namespace GuidBase64 {

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using Configuration;
    using Converters;
    using JetBrains.Annotations;

    [TypeConverter( typeof( Base64GuidTypeConverter ) )]
    public struct Base64Guid {

        private readonly Base64GuidOptions _options;

        public static readonly Base64Guid Empty = new Base64Guid( Guid.Empty );

        /// <summary>
        ///     Gets the Guid object.
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class with an empty GUID
        ///     and a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        public Base64Guid( Action<Base64GuidOptionsBuilder> optionsAction ) : this( new Guid(), optionsAction ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class by using the
        ///     specified array of bytes.
        /// </summary>
        /// <param name="buffer">A 16-element byte array containing values with which to initialize the GUID.</param>
        public Base64Guid( [NotNull] Byte[] buffer ) : this( new Guid( buffer ), options => { } ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class by using the
        ///     specified array of bytes and a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="buffer">A 16-element byte array containing values with which to initialize the GUID.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        public Base64Guid( [NotNull] Byte[] buffer, Action<Base64GuidOptionsBuilder> optionsAction ) : this( new Guid( buffer ), optionsAction ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class with an existing
        ///     <see cref="System.Guid" /> instance.
        /// </summary>
        /// <param name="guid">The <see cref="System.Guid" /> instance to initialise the <see cref="Base64Guid" /> instance with.</param>
        public Base64Guid( Guid guid ) : this( guid, options => { } ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class with an existing
        ///     <see cref="System.Guid" /> instance and a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="guid">The <see cref="System.Guid" /> instance to initialise the <see cref="Base64Guid" /> instance with.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        public Base64Guid( Guid guid, [NotNull] Action<Base64GuidOptionsBuilder> optionsAction ) {
            this._options = BuildOptions( optionsAction );
            this.Guid = guid;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class by using the value
        ///     represented by the specified string.
        /// </summary>
        /// <param name="encoded">
        ///     A string that contains a Guid which satisfies the regular expression "^[a-zA-Z0-9-_]{22}$".
        /// </param>
        public Base64Guid( String encoded ) : this( encoded, options => { } ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class by using the value
        ///     represented by the specified string with a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="encoded">
        ///     <para>A string that contains a Guid which satisfies one of the following regular expressions:</para>
        ///     <para>- "^[a-zA-Z0-9-_]{22}$" if <paramref name="optionsAction" /> is not configured.</para>
        ///     <para>- "^[a-zA-Z0-9\+/]{22}$" if <paramref name="optionsAction" /> is configured with UseStandardBase64Encoding().</para>
        ///     <para>- "^[a-zA-Z0-9-_]{22}==$" if <paramref name="optionsAction" /> is configured with UsePadding().</para>
        ///     <para>
        ///         - "^[a-zA-Z0-9\+/]{22}==$" if <paramref name="optionsAction" /> is configured with
        ///         UseStandardBase64Encoding() and UsePadding().
        ///     </para>
        /// </param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        public Base64Guid( String encoded, [NotNull] Action<Base64GuidOptionsBuilder> optionsAction ) {
            this._options = BuildOptions( optionsAction );
            this.Guid = new Guid( ParseToByteArray( encoded, this._options ) );
        }

        private static Base64GuidOptions BuildOptions( [NotNull] Action<Base64GuidOptionsBuilder> options ) {
            if ( options is null ) {
                throw new ArgumentNullException( nameof( options ) );
            }

            var builder = new Base64GuidOptionsBuilder();
            options( builder );

            return builder.Build();
        }

        [NotNull]
        private static Byte[] ParseToByteArray( String encoded, Base64GuidOptions options ) {
            if ( encoded is null ) {
                throw new ArgumentNullException( nameof( encoded ) );
            }

            if ( !options.Padding && ( encoded.Length < 22 || encoded.Length > 22 ) ) {
                throw new FormatException( $"{nameof( encoded )} is not 22 characters long" );
            }

            if ( options.Padding && ( encoded.Length < 24 || encoded.Length > 24 ) ) {
                throw new FormatException( $"{nameof( encoded )} is not 24 characters long" );
            }

            Regex regex;

            if ( !options.StandardBase64Encoding && !options.Padding ) {
                regex = new Regex( @"^[a-zA-Z0-9-_]*$" );
            }
            else if ( options.StandardBase64Encoding && !options.Padding ) {
                regex = new Regex( @"^[a-zA-Z0-9\+/]*$" );
            }
            else if ( !options.StandardBase64Encoding && options.Padding ) {
                regex = new Regex( @"^[a-zA-Z0-9-_]*={0,2}$" );
            }
            else {
                regex = new Regex( @"^[a-zA-Z0-9\+/]*={0,2}$" );
            }

            if ( !regex.IsMatch( encoded ) ) {
                throw new FormatException( $"{nameof( encoded )} is not encoded correctly" );
            }

            if ( !options.StandardBase64Encoding ) {
                encoded = encoded.Replace( "_", "/" );
                encoded = encoded.Replace( "-", "+" );
            }

            if ( options.Padding ) {
                return Convert.FromBase64String( encoded );
            }

            return Convert.FromBase64String( encoded + "==" );
        }

        /// <summary>
        ///     Implicitly converts a <see cref="String" /> object to its <see cref="Base64Guid" /> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="String" /> object to convert.</param>
        public static implicit operator Base64Guid( String s ) => new Base64Guid( s );

        /// <summary>
        ///     Implicitly converts a <see cref="System.Guid" /> object to its <see cref="Base64Guid" /> equivalent.
        /// </summary>
        /// <param name="g">The <see cref="System.Guid" /> object to convert.</param>
        public static implicit operator Base64Guid( Guid g ) => new Base64Guid( g );

        /// <summary>
        ///     Implicitly converts a <see cref="Base64Guid" /> object to its <see cref="System.Guid" /> equivalent.
        /// </summary>
        /// <param name="a">The <see cref="Base64Guid" /> object to convert.</param>
        public static implicit operator Guid( Base64Guid a ) => a.Guid;

        /// <summary>
        ///     Implicitly converts a <see cref="Base64Guid" /> object to its <see cref="String" /> equivalent.
        /// </summary>
        /// <param name="a">The <see cref="Base64Guid" /> object to convert.</param>
        [NotNull]
        public static implicit operator String( Base64Guid a ) => a.ToString();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class.
        /// </summary>
        /// <returns>A new <see cref="Base64Guid" /> object.</returns>
        public static Base64Guid NewBase64Guid() => new Base64Guid( Guid.NewGuid() );

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64Guid" /> class with
        ///     a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        /// <returns>A new <see cref="Base64Guid" /> object.</returns>
        public static Base64Guid NewBase64Guid( [NotNull] Action<Base64GuidOptionsBuilder> optionsAction ) => new Base64Guid( Guid.NewGuid(), optionsAction );

        /// <summary>
        ///     Indicates whether the values of two specified <see cref="Base64Guid" /> objects are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are not equal; otherwise false.</returns>
        public static Boolean operator !=( Base64Guid a, Base64Guid b ) => a.Guid != b.Guid;

        /// <summary>
        ///     Indicates whether the values of two specified <see cref="Base64Guid" /> objects are equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are equal; otherwise false.</returns>
        public static Boolean operator ==( Base64Guid a, Base64Guid b ) => a.Guid == b.Guid;

        /// <summary>
        ///     Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid" /> object.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <returns>A <see cref="Base64Guid" /> instance that contains the parsed value.</returns>
        public static Base64Guid Parse( String encoded ) => new Base64Guid( encoded );

        /// <summary>
        ///     Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid" /> object
        ///     with a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="options">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        /// <returns>A <see cref="Base64Guid" /> instance that contains the parsed value.</returns>
        public static Base64Guid Parse( String encoded, [NotNull] Action<Base64GuidOptionsBuilder> options ) => new Base64Guid( encoded, options );

        /// <summary>
        ///     Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid" /> object.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="result">
        ///     The object that will contain the parsed value. If the method returns true,
        ///     result contains a valid <see cref="Base64Guid" />. If the method returns false,
        ///     result is null.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise false.</returns>
        public static Boolean TryParse( String encoded, out Base64Guid result ) {
            try {
                result = new Base64Guid( encoded );

                return true;
            }
            catch {
                result = default;

                return false;
            }
        }

        /// <summary>
        ///     Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid" /> object
        ///     with a configurable <see cref="Base64GuidOptionsBuilder" />.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions" /> for the instance.</param>
        /// <param name="result">
        ///     The object that will contain the parsed value. If the method returns true,
        ///     result contains a valid <see cref="Base64Guid" />. If the method returns false,
        ///     result is null.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise false.</returns>
        public static Boolean TryParse( String encoded, Action<Base64GuidOptionsBuilder> optionsAction, out Base64Guid result ) {
            try {
                result = new Base64Guid( encoded, optionsAction );

                return true;
            }
            catch {
                result = default;

                return false;
            }
        }

        /// <summary>
        ///     Returns a value indicating whether this instance and a specified <see cref="Base64Guid" />
        ///     represent the same value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>
        ///     true if <paramref name="obj" /> is a <see cref="Base64Guid" /> that has the same value
        ///     as this instance; otherwise false.
        /// </returns>
        public override Boolean Equals( Object obj ) {

            if ( obj is Base64Guid b ) {
                return this.Guid == b.Guid;
            }

            return false;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override Int32 GetHashCode() {
            var hashCode = -1906274164;
            hashCode = hashCode * -1521134295 + EqualityComparer<Base64GuidOptions>.Default.GetHashCode( this._options );
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode( this.Guid );

            return hashCode;
        }

        /// <summary>
        ///     Returns a 16-element byte array that contains the value of this instance.
        /// </summary>
        /// <returns>A 16-element byte array containing the value of this instance.</returns>
        [NotNull]
        public Byte[] ToByteArray() => this.Guid.ToByteArray();

        /// <summary>
        ///     Returns a string representation of the value of this instance in base 64 format.
        /// </summary>
        /// <returns>
        ///     The value of this instance, formatted with the RFC 4648 Section 5 base 64 character set
        ///     and with padding stripped. If the instance has been configured to use the standard
        ///     base 64 encoding, the instance value will be formatted with the RFC 4648 Section 4 base
        ///     64 character set instead. If the instance has been configured to use padding, the instance value
        ///     will not have its padding stripped.
        /// </returns>
        public override String ToString() {
            var enc = Convert.ToBase64String( this.Guid.ToByteArray() );

            if ( !this._options.StandardBase64Encoding ) {
                enc = enc.Replace( "/", "_" );
                enc = enc.Replace( "+", "-" );
            }

            if ( !this._options.Padding ) {
                return enc.Substring( 0, 22 );
            }

            return enc;
        }
    }
}