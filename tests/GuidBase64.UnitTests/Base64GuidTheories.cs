namespace GuidBase64.UnitTests {

    using System;
    using System.Collections.Generic;
    using CommonTestData;
    using Xunit;

    public class Base64GuidTheories {

        public class Base64GuidImplicitOperator {

            [Theory]
            [MemberData( nameof( TestData.Base64GuidPairs ), MemberType = typeof( TestData ) )]
            public void ReturnsBase64GuidFromGuid( Guid guid, String _ ) {
                var result = ( Base64Guid )guid;

                Assert.Equal( guid, result.Guid );
            }

            [Theory]
            [MemberData( nameof( TestData.Base64GuidPairs ), MemberType = typeof( TestData ) )]
            public void ReturnsBase64GuidFromString( Guid guid, String base64String ) {
                var result = ( Base64Guid )base64String;

                Assert.Equal( guid, result.Guid );
            }
        }

        public class GuidImplicitOperator {

            [Theory]
            [MemberData( nameof( TestData.Base64GuidPairs ), MemberType = typeof( TestData ) )]
            public void ReturnsBase64GuidAsGuid( Guid guid, String base64String ) {
                var a = new Base64Guid( base64String );

                var result = ( Guid )a;

                Assert.Equal( guid, result );
            }
        }

        public class ParseStaticMethod {

            public static IEnumerable<Object[]> ReturnsBase64GuidData => TestData.Base64GuidPairs;

            public static IEnumerable<Object[]> ThrowsWhenStringContentIsInvalidData => TestData.InvalidContentBase64GuidStrings;

            public static IEnumerable<Object[]> ThrowsWhenStringLengthIsInvalidData => TestData.InvalidLengthBase64GuidStrings;

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64Guid( Guid expected, String input ) {
                var result = Base64Guid.Parse( input );

                Assert.Equal( expected, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64GuidFromInputWithPadding( Guid expected, String input ) {
                input += "==";

                var result = Base64Guid.Parse( input, options => options.UsePadding() );

                Assert.Equal( expected, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64GuidFromUrlUnsafeInput( Guid expected, String input ) {
                input = input.Replace( "-", "+" ).Replace( "_", "/" );

                var result = Base64Guid.Parse( input, options => options.UseStandardBase64Encoding() );

                Assert.Equal( expected, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64GuidFromUrlUnsafeInputWithPadding( Guid expected, String input ) {
                input = input.Replace( "-", "+" ).Replace( "_", "/" ) + "==";

                var result = Base64Guid.Parse( input, options => {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                } );

                Assert.Equal( expected, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ThrowsWhenStringContentIsInvalidData ) )]
            public void ThrowsWhenStringContentIsInvalid( String input ) => Assert.Throws<FormatException>( () => Base64Guid.Parse( input ) );

            [Theory]
            [MemberData( nameof( ThrowsWhenStringContentIsInvalidData ) )]
            public void ThrowsWhenStringContentIsInvalidForUrlUnsafeInput( String input ) {
                input = input.Replace( "+", "-" ).Replace( "/", "_" );
                Assert.Throws<FormatException>( () => Base64Guid.Parse( input, options => options.UseStandardBase64Encoding() ) );
            }

            [Theory]
            [MemberData( nameof( ThrowsWhenStringLengthIsInvalidData ) )]
            public void ThrowsWhenStringLengthIsInvalid( String input ) => Assert.Throws<FormatException>( () => Base64Guid.Parse( input ) );

            [Theory]
            [MemberData( nameof( ThrowsWhenStringLengthIsInvalidData ) )]
            public void ThrowsWhenStringLengthIsInvalidForInputWithPadding( String input ) {
                input += "==";
                Assert.Throws<FormatException>( () => Base64Guid.Parse( input, options => options.UsePadding() ) );
            }
        }

        public class StringImplicitOperator {

            [Theory]
            [MemberData( nameof( TestData.Base64GuidPairs ), MemberType = typeof( TestData ) )]
            public void ReturnsBase64GuidAsString( Guid guid, String base64String ) {
                var a = new Base64Guid( guid );

                var result = ( String )a;

                Assert.Equal( base64String, result );
            }
        }

        public class ToByteArrayMethod {

            public static IEnumerable<Object[]> ReturnsByteArrayData => TestData.Base64GuidPairs;

            [Theory]
            [MemberData( nameof( ReturnsByteArrayData ) )]
            public void ReturnsByteArray( Guid guid, String encoded ) {
                var base64Guid = new Base64Guid( encoded );

                var result = base64Guid.ToByteArray();
                Assert.Equal( guid.ToByteArray(), result );
            }
        }

        public class ToStringMethod {

            public static IEnumerable<Object[]> ReturnsBase64StringData => TestData.Base64GuidPairs;

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsBase64String( Guid input, String expected ) {
                var base64Guid = new Base64Guid( input );

                var result = base64Guid.ToString();

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsBase64StringWithPadding( Guid input, String expected ) {
                expected += "==";
                var base64Guid = new Base64Guid( input, options => options.UsePadding() );

                var result = base64Guid.ToString();

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsUrlUnsafeBase64String( Guid input, String expected ) {
                expected = expected.Replace( "-", "+" ).Replace( "_", "/" );

                var base64Guid = new Base64Guid( input, options => options.UseStandardBase64Encoding() );

                var result = base64Guid.ToString();

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsUrlUnsafeBase64StringWithPadding( Guid input, String expected ) {
                expected = expected.Replace( "-", "+" ).Replace( "_", "/" ) + "==";

                var base64Guid = new Base64Guid( input, options => {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                } );

                var result = base64Guid.ToString();

                Assert.Equal( expected, result );
            }
        }

        public class TryParseStaticMethod {

            public static IEnumerable<Object[]> ReturnsFalseWhenStringContentIsInvalidData => TestData.InvalidContentBase64GuidStrings;

            public static IEnumerable<Object[]> ReturnsFalseWhenStringLengthIsInvalidData => TestData.InvalidLengthBase64GuidStrings;

            public static IEnumerable<Object[]> ReturnsTrueWithBase64GuidResultData => TestData.Base64GuidPairs;

            [Theory]
            [MemberData( nameof( ReturnsFalseWhenStringContentIsInvalidData ) )]
            public void ReturnsFalseWhenStringContentIsInvalid( String input ) {
                var result = Base64Guid.TryParse( input, out var output );

                Assert.False( result );
                Assert.Equal( default( Base64Guid ), output );
            }

            [Theory]
            [MemberData( nameof( ReturnsFalseWhenStringContentIsInvalidData ) )]
            public void ReturnsFalseWhenStringContentIsInvalidForUrlUnsafeInput( String input ) {
                input = input.Replace( "+", "-" ).Replace( "/", "_" );

                var result = Base64Guid.TryParse( input, options => options.UseStandardBase64Encoding(), out var output );

                Assert.False( result );
                Assert.Equal( default( Base64Guid ), output );
            }

            [Theory]
            [MemberData( nameof( ReturnsFalseWhenStringLengthIsInvalidData ) )]
            public void ReturnsFalseWhenStringLengthIsInvalid( String input ) {
                var result = Base64Guid.TryParse( input, out var output );

                Assert.False( result );
                Assert.Equal( default( Base64Guid ), output );
            }

            [Theory]
            [MemberData( nameof( ReturnsFalseWhenStringLengthIsInvalidData ) )]
            public void ReturnsFalseWhenStringLengthIsInvalidForInputWithPadding( String input ) {
                input += "==";

                var result = Base64Guid.TryParse( input, options => options.UsePadding(), out var output );

                Assert.False( result );
                Assert.Equal( default( Base64Guid ), output );
            }

            [Theory]
            [MemberData( nameof( ReturnsTrueWithBase64GuidResultData ) )]
            public void ReturnsTrueWithBase64GuidResult( Guid expected, String input ) {
                var result = Base64Guid.TryParse( input, out var output );

                Assert.True( result );
                Assert.Equal( expected, output.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsTrueWithBase64GuidResultData ) )]
            public void ReturnsTrueWithBase64GuidResultFromInputWithPadding( Guid expected, String input ) {
                input += "==";

                var result = Base64Guid.TryParse( input, options => options.UsePadding(), out var output );

                Assert.Equal( expected, output.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsTrueWithBase64GuidResultData ) )]
            public void ReturnsTrueWithBase64GuidResultFromUrlUnsafeInput( Guid expected, String input ) {
                input = input.Replace( "-", "+" ).Replace( "_", "/" );

                var result = Base64Guid.TryParse( input, options => options.UseStandardBase64Encoding(), out var output );

                Assert.Equal( expected, output.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsTrueWithBase64GuidResultData ) )]
            public void ReturnsTrueWithBase64GuidResultFromUrlUnsafeInputWithPadding( Guid expected, String input ) {
                input = input.Replace( "-", "+" ).Replace( "_", "/" ) + "==";

                var result = Base64Guid.TryParse( input, options => {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                }, out var output );

                Assert.Equal( expected, output.Guid );
            }
        }
    }
}