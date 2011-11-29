//
// HttpRequestMessageTest.cs
//
// Authors:
//	Marek Safar  <marek.safar@gmail.com>
//
// Copyright (C) 2011 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Linq;

namespace MonoTests.System.Net.Http
{
	[TestFixture]
	public class HttpRequestMessageTest
	{
		[Test]
		public void Ctor_Invalid ()
		{
			try {
				new HttpRequestMessage (null, "");
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {
			}
		}

		[Test]
		public void Ctor_Default ()
		{
			var m = new HttpRequestMessage ();
			Assert.IsNull (m.Content, "#1");
			Assert.IsNotNull (m.Headers, "#2");
			Assert.AreEqual (HttpMethod.Get, m.Method, "#3");
			Assert.IsNotNull (m.Properties, "#4");
			Assert.IsNull (m.RequestUri, "#5");
			Assert.AreEqual (new Version (1, 1), m.Version, "#6");
		}

		[Test]
		public void Ctor_Uri ()
		{
			var c = new HttpRequestMessage (HttpMethod.Get, new Uri ("http://xamarin.com"));
			Assert.AreEqual ("http://xamarin.com/", c.RequestUri.AbsoluteUri, "#1");

			c = new HttpRequestMessage (HttpMethod.Get, new Uri ("https://xamarin.com"));
			Assert.AreEqual ("https://xamarin.com/", c.RequestUri.AbsoluteUri, "#2");

			c = new HttpRequestMessage (HttpMethod.Get, new Uri ("HTTP://xamarin.com:8080"));
			Assert.AreEqual ("http://xamarin.com:8080/", c.RequestUri.AbsoluteUri, "#3");

			var a = Uri.UriSchemeHttps;
			var b = new Uri ("http://xamarin.com");

			try {
				new HttpRequestMessage (HttpMethod.Get, new Uri ("ftp://xamarin.com"));
				Assert.Fail ("#4");
			} catch (ArgumentException) {
			}
		}

		[Test]
		public void Headers ()
		{
			HttpRequestMessage message = new HttpRequestMessage ();
			HttpRequestHeaders headers = message.Headers;

			headers.Accept.Add (new MediaTypeWithQualityHeaderValue ("audio/x"));
			headers.AcceptCharset.Add (new StringWithQualityHeaderValue ("str-v", 0.002));
			headers.AcceptEncoding.Add (new StringWithQualityHeaderValue ("str-enc", 0.44));
			headers.AcceptLanguage.Add (new StringWithQualityHeaderValue ("str-lang", 0.41));
			headers.Authorization = new AuthenticationHeaderValue ("sh-aut", "par");
			headers.CacheControl = new CacheControlHeaderValue () {
				MaxAge = TimeSpan.MaxValue
			};
			headers.Connection.Add ("test-value");
			headers.ConnectionClose = true;
			headers.Date = new DateTimeOffset (DateTime.Today);
			headers.Expect.Add (new NameValueWithParametersHeaderValue ("en", "ev"));
			headers.ExpectContinue = false;
			headers.From = "webmaster@w3.org";
			headers.Host = "host";
			headers.IfMatch.Add (new EntityTagHeaderValue ("\"tag\"", true));
			headers.IfModifiedSince = new DateTimeOffset (DateTime.Today);
			headers.IfNoneMatch.Add (new EntityTagHeaderValue ("\"tag2\"", true));
			headers.IfRange = new RangeConditionHeaderValue (new DateTimeOffset (DateTime.Today));
			headers.IfUnmodifiedSince = new DateTimeOffset? (DateTimeOffset.Now);
			headers.MaxForwards = 0x15b3;
			headers.Pragma.Add (new NameValueHeaderValue ("name", "value"));
			headers.ProxyAuthorization = new AuthenticationHeaderValue ("s", "p");
			headers.Range = new RangeHeaderValue (5L, 30L);
			headers.Referrer = new Uri ("http://xamarin.com");
			headers.TE.Add (new TransferCodingWithQualityHeaderValue ("TE", 0.3));
			headers.Trailer.Add ("value");
			headers.TransferEncoding.Add (new TransferCodingHeaderValue ("tchv"));
			headers.TransferEncodingChunked = true;
			headers.Upgrade.Add (new ProductHeaderValue ("prod", "ver"));
			headers.UserAgent.Add (new ProductInfoHeaderValue ("(comment)"));
			headers.Via.Add (new ViaHeaderValue ("protocol", "rec-by"));
			headers.Warning.Add (new WarningHeaderValue (5, "agent", "\"txt\""));

			try {
				headers.Add ("authorization", "");
				Assert.Fail ("Authorization");
			} catch (FormatException) {
			}

			try {
				headers.Add ("date", "");
				Assert.Fail ("Date");
			} catch (FormatException) {
			}

			try {
				headers.Add ("from", "a@w3.org");
				Assert.Fail ("From");
			} catch (FormatException) {
			}

			try {
				headers.Add ("hOst", "host");
				Assert.Fail ("Host");
			} catch (FormatException) {
			}

			try {
				headers.Add ("if-modified-since", "");
				Assert.Fail ("if-modified-since");
			} catch (FormatException) {
			}

			try {
				headers.Add ("if-range", "");
				Assert.Fail ("if-range");
			} catch (FormatException) {
			}

			try {
				headers.Add ("if-unmodified-since", "");
				Assert.Fail ("if-unmodified-since");
			} catch (FormatException) {
			}

			try {
				headers.Add ("max-forwards", "");
				Assert.Fail ("max-forwards");
			} catch (FormatException) {
			}

			try {
				headers.Add ("proxy-authorization", "");
				Assert.Fail ("proxy-authorization");
			} catch (FormatException) {
			}

			try {
				headers.Add ("range", "");
			} catch (FormatException) {
			}

			try {
				headers.Add ("referer", "");
				Assert.Fail ("referer");
			} catch (FormatException) {
			}

			headers.Add ("accept", "audio/y");
			headers.Add ("accept-charset", "achs");
			headers.Add ("accept-encoding", "aenc");
			headers.Add ("accept-language", "alan");
			headers.Add ("cache-control", "cc");
			headers.Add ("expect", "exp");
			headers.Add ("if-match", "\"v\"");
			headers.Add ("if-none-match", "\"v2\"");
			headers.Add ("pragma", "p");
			headers.Add ("TE", "0.8");
			headers.Add ("trailer", "value2");
			headers.Add ("transfer-encoding", "ttt");
			headers.Add ("upgrade", "uuu");
			headers.Add ("user-agent", "uaua");
			headers.Add ("via", "prot v");
			headers.Add ("warning", "4 ww \"t\"");

			Assert.IsTrue (headers.Accept.SequenceEqual (
				new[] {
					new MediaTypeWithQualityHeaderValue ("audio/x"),
					new MediaTypeWithQualityHeaderValue ("audio/y")
				}
			));

			Assert.IsTrue (headers.AcceptCharset.SequenceEqual (
				new[] {
					new StringWithQualityHeaderValue ("str-v", 0.002),
					new StringWithQualityHeaderValue ("achs")
				}
			));

			Assert.IsTrue (headers.AcceptEncoding.SequenceEqual (
				new[] {
					new StringWithQualityHeaderValue ("str-enc", 0.44),
					new StringWithQualityHeaderValue ("aenc")
				}
			));

			Assert.IsTrue (headers.AcceptLanguage.SequenceEqual (
				new[] {
					new StringWithQualityHeaderValue ("str-lang", 0.41),
					new StringWithQualityHeaderValue ("alan")
				}
			));

			Assert.AreEqual (new AuthenticationHeaderValue ("sh-aut", "par"), headers.Authorization);

			var cch = new CacheControlHeaderValue () {
					MaxAge = TimeSpan.MaxValue,
				};
			cch.Extensions.Add (new NameValueHeaderValue ("cc"));

			Assert.AreEqual (cch, headers.CacheControl);

			Assert.IsTrue (headers.Connection.SequenceEqual (
				new string[] { "test-value", "close" }));

			Assert.AreEqual (headers.Date, new DateTimeOffset (DateTime.Today));

			Assert.IsTrue (headers.Expect.SequenceEqual (
				new [] {
					new NameValueWithParametersHeaderValue ("en", "ev"),
					new NameValueWithParametersHeaderValue ("exp")
				}));

			Assert.AreEqual (headers.From, "webmaster@w3.org");

			Assert.IsTrue (headers.IfMatch.SequenceEqual (
				new EntityTagHeaderValue[] {
					new EntityTagHeaderValue ("\"tag\"", true),
					new EntityTagHeaderValue ("\"v\"", false)
				}
			));

			Assert.AreEqual (headers.IfModifiedSince, new DateTimeOffset (DateTime.Today));
			Assert.IsTrue (headers.IfNoneMatch.SequenceEqual (new EntityTagHeaderValue[] { new EntityTagHeaderValue ("\"tag2\"", true), new EntityTagHeaderValue ("\"v2\"", false) }));
			Assert.AreEqual (new DateTimeOffset (DateTime.Today), headers.IfRange.Date);
			Assert.AreEqual (headers.MaxForwards, 0x15b3);
			Assert.IsTrue (headers.Pragma.SequenceEqual (new NameValueHeaderValue[] { new NameValueHeaderValue ("name", "value"), new NameValueHeaderValue ("p", null) }));
			Assert.AreEqual ("p", headers.ProxyAuthorization.Parameter);
			Assert.AreEqual ("s", headers.ProxyAuthorization.Scheme);
			Assert.AreEqual (5, headers.Range.Ranges.First ().From);
			Assert.AreEqual (30, headers.Range.Ranges.First ().To);
			Assert.AreEqual ("bytes", headers.Range.Unit);
			Assert.AreEqual (headers.Referrer, new Uri ("http://xamarin.com"));
			Assert.IsTrue (headers.TE.SequenceEqual (new TransferCodingWithQualityHeaderValue[] { new TransferCodingWithQualityHeaderValue ("TE", 0.3), new TransferCodingWithQualityHeaderValue ("0.8") }), "29");
			Assert.IsTrue (headers.Trailer.SequenceEqual (
				new string[] {
					"value", "value2"
				}), "30");

			Assert.IsTrue (headers.TransferEncoding.SequenceEqual (
				new[] {
					new TransferCodingHeaderValue ("tchv"),
					new TransferCodingHeaderValue ("chunked"),
					new TransferCodingHeaderValue ("ttt")
				}
			));

			Assert.IsTrue (headers.Upgrade.SequenceEqual (
				new[] {
					new ProductHeaderValue ("prod", "ver"),
					new ProductHeaderValue ("uuu")
				}
			));

			Assert.IsTrue (headers.UserAgent.SequenceEqual (
				new[] {
					new ProductInfoHeaderValue ("(comment)"),
					new ProductInfoHeaderValue ("uaua", null)
				}
			));

			Assert.IsTrue (headers.Via.SequenceEqual (
				new[] {
					new ViaHeaderValue ("protocol", "rec-by"),
					new ViaHeaderValue ("prot", "v")
				}
			));

			Assert.IsTrue (headers.Warning.SequenceEqual (
				new[] {
					new WarningHeaderValue (5, "agent", "\"txt\""),
					new WarningHeaderValue (4, "ww", "\"t\"")
				}
			));

		}

		[Test]
		public void Headers_Invalid ()
		{
			HttpRequestMessage message = new HttpRequestMessage ();
			HttpRequestHeaders headers = message.Headers;
			try {
				headers.Add ("Age", "");
				Assert.Fail ("#1");
			} catch (InvalidOperationException) {
			}
			try {
				headers.AddWithoutValidation ("Age", "");
				Assert.Fail ("#2");
			} catch (InvalidOperationException) {
			}
		}

		[Test]
		public void Headers_Connection_Invalid ()
		{
			HttpRequestMessage message = new HttpRequestMessage ();
			HttpRequestHeaders headers = message.Headers;
			//headers.
		}

		[Test]
		public void Headers_From_Invalid ()
		{
			HttpRequestMessage message = new HttpRequestMessage ();
			HttpRequestHeaders headers = message.Headers;
			headers.From = null;
			headers.From = "";
			try {
				headers.From = " ";
				Assert.Fail ("#1");
			} catch (FormatException) {
			}
			try {
				headers.From = "test";
				Assert.Fail ("#2");
			} catch (FormatException) {
			}
		}

		[Test]
		public void Properties ()
		{
			var c = new HttpRequestMessage ();
			c.Content = null;
			c.Method = HttpMethod.Post;
			c.Properties.Add ("a", "test");
			c.RequestUri = null;
			c.Version = HttpVersion.Version10;

			Assert.IsNull (c.Content, "#1");
			Assert.AreEqual (HttpMethod.Post, c.Method, "#2");
			Assert.AreEqual ("test", c.Properties["a"], "#3");
			Assert.IsNull (c.RequestUri, "#4");
			Assert.AreEqual (HttpVersion.Version10, c.Version, "#5");
		}

		[Test]
		public void Properties_Invalid ()
		{
			var c = new HttpRequestMessage ();
			try {
				c.Method = null;
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {
			}

			try {
				c.RequestUri = new Uri ("ftp://xamarin.com");
				Assert.Fail ("#2");
			} catch (ArgumentException) {
			}

			try {
				c.Version = null;
				Assert.Fail ("#3");
			} catch (ArgumentNullException) {
			}
		}
	}
}