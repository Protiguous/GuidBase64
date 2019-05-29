namespace GuidBase64.IntegrationTests {

    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CommonTestData;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestWebApi;
    using Xunit;

    public class Base64GuidModelBindingTest : IClassFixture<WebApplicationFactory<Startup>> {

        private readonly HttpClient _client;

        public static IEnumerable<Object[]> CanBindBase64GuidData => TestData.Base64GuidPairs;

        public static IEnumerable<Object[]> CannotBindBase64GuidData => TestData.InvalidBase64GuidStrings;

        public Base64GuidModelBindingTest( [NotNull] WebApplicationFactory<Startup> factory ) => this._client = factory.CreateClient();

        [Theory]
        [MemberData( nameof( CanBindBase64GuidData ) )]
        public async Task CanBindBase64GuidInQueryParameter( Guid guid, String encodedBase64Guid ) {
            var httpResponse = await this._client.GetAsync( $"/api/values?id={encodedBase64Guid}" );

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var resultGuid = new Guid( stringResponse );
            Assert.Equal( guid, resultGuid );
        }

        [Theory]
        [MemberData( nameof( CanBindBase64GuidData ) )]
        public async Task CanBindBase64GuidInRouteParameter( Guid guid, String encodedBase64Guid ) {
            var httpResponse = await this._client.GetAsync( $"/api/values/{encodedBase64Guid}" );

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var resultGuid = new Guid( stringResponse );
            Assert.Equal( guid, resultGuid );
        }

        [Theory]
        [MemberData( nameof( CannotBindBase64GuidData ) )]
        public async Task CannotBindBase64GuidInQueryParameter( String invalidBase64Guid ) {
            var httpResponse = await this._client.GetAsync( $"/api/values?id={invalidBase64Guid}" );

            Assert.Equal( HttpStatusCode.BadRequest, httpResponse.StatusCode );
        }

        [Theory]
        [MemberData( nameof( CannotBindBase64GuidData ) )]
        public async Task CannotBindBase64GuidInRouteParameter( String invalidBase64Guid ) {
            var httpResponse = await this._client.GetAsync( $"/api/values/{invalidBase64Guid}" );

            Assert.Equal( HttpStatusCode.BadRequest, httpResponse.StatusCode );
        }

    }

}