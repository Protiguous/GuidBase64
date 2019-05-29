namespace GuidBase64.CommonTestData {

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TestData {

        public static IEnumerable<Object[]> Base64GuidPairs =>
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

        public static IEnumerable<Object[]> InvalidBase64GuidStrings => InvalidContentBase64GuidStrings.Concat( InvalidLengthBase64GuidStrings );

        public static IEnumerable<Object[]> InvalidContentBase64GuidStrings =>
            new List<Object[]> {
                new Object[] {
                    "abcdefghijABCDEFGHIJ1/"
                },
                new Object[] {
                    "+bcdefghijABCDEFGHIJ1/"
                },
                new Object[] {
                    "+bcdefghijABCDEFGHIJ12"
                }
            };

        public static IEnumerable<Object[]> InvalidLengthBase64GuidStrings =>
            new List<Object[]> {
                new Object[] {
                    "123456789012345678901"
                },
                new Object[] {
                    "12345678901234567890123"
                }
            };

    }

}