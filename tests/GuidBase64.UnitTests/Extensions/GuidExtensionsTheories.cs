namespace GuidBase64.UnitTests.Extensions {

    using System;
    using System.Collections.Generic;
    using GuidBase64.Extensions;
    using JetBrains.Annotations;
    using Xunit;

    public class GuidExtensionsTheories {

        [NotNull]
        public static IEnumerable<Object[]> Base64GuidPairData =>
            new List<Object[]> {
                new Object[] {
                    new Guid( "00000000-0000-0000-0000-000000000000" ), "AAAAAAAAAAAAAAAAAAAAAA"
                },
                new Object[] {
                    new Guid( "c6a44c9f-763a-4524-8c0b-04c599f001a6" ), "n0ykxjp2JEWMCwTFmfABpg"
                },
                new Object[] {
                    new Guid( "ffffffff-ffff-ffff-ffff-ffffffffffff" ), "_____________________w"
                }
            };

        public class ToBase64GuidMethod {

            [NotNull]
            public static IEnumerable<Object[]> ReturnsBase64GuidData => Base64GuidPairData;

            [NotNull]
            public static IEnumerable<Object[]> ReturnsBase64StringData => Base64GuidPairData;

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64Guid( Guid guid, String _ ) {
                var result = guid.ToBase64Guid();

                Assert.Equal( guid, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64GuidData ) )]
            public void ReturnsBase64GuidWithOptions( Guid guid, String _ ) {
                var result = guid.ToBase64Guid( options => {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                } );

                Assert.Equal( guid, result.Guid );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsBase64String( Guid guid, String expected ) {
                var result = guid.ToBase64String();

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsBase64StringWithPadding( Guid guid, String expected ) {
                expected += "==";

                var result = guid.ToBase64String( options => options.UsePadding() );

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsUrlUnsafeBase64String( Guid guid, String expected ) {
                expected = expected.Replace( "-", "+" ).Replace( "_", "/" );

                var result = guid.ToBase64String( options => options.UseStandardBase64Encoding() );

                Assert.Equal( expected, result );
            }

            [Theory]
            [MemberData( nameof( ReturnsBase64StringData ) )]
            public void ReturnsUrlUnsafeBase64StringWithPadding( Guid guid, String expected ) {
                expected = expected.Replace( "-", "+" ).Replace( "_", "/" ) + "==";

                var result = guid.ToBase64String( options => {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                } );

                Assert.Equal( expected, result );
            }
        }
    }
}