using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HolisticWare.Core.Localization.Translate
{
	/// <summary>
	/// Google translate primitive.
	/// 
	/// Google translate not using API 
	/// </summary>
	/// <see cref="http://www.codeproject.com/Tips/644753/Language-Translator-Using-Google-Translate"/> 
	/// <see cref="http://www.mindstick.com/Articles/bec5290f-3fea-4f4e-9f27-ed7b13963132/Google%20Language%20Translator%20in%20C"/>
	/*
	google-language-api-for-dotnet

	http://code.google.com/p/google-language-api-for-dotnet/

	Google Translator

	http://www.codeproject.com/KB/IP/GoogleTranslator.aspx

	Translate your text using Google Api's

	http://blogs.msdn.com/shahpiyush/archive/2007/06/09/3188246.aspx

	Calling Google Ajax Language API for Translation and Language Detection from C#

	http://www.esotericdelights.com/post/2008/11/Calling-Google-Ajax-Language-API-for-Translation-and-Language-Detection-from-C.aspx

	Translation Web Service in C#

	http://www.codeproject.com/KB/cpp/translation.aspx

	Using Google's Translation API from .NET

	http://www.reimers.dk/blogs/jacob_reimers_weblog/archive/2008/06/18/using-google-s-translation-api-from-net.aspx

	Google Translate Kit, an open source library http://ggltranslate.codeplex.com/	

	https://cloud.google.com/translate/docs?csw=1
	*/




	/*
		http://alexmg.com/introduction-to-the-httpclient/
		http://massivescale.com/pages/custom-headers-with-httpclient/
	*/
	public class GoogleTranslatePrimitive
	{
		public string LanguageTo
		{
			get;
			set;
		}

		public string LanguageFrom
		{
			get;
			set;
		}


		public GoogleTranslatePrimitive ()
		{
		}


		private string user_agent = 
						"Mozilla/5.0"
						;

		private string text_to_translate = "Dobar dan!";
		private string text_translated = "";
		private string culture_to_translate = "hr";
		private string culture_translated = "en";


		/*
			http://stebet.net/httpclient-best-practices/
			http://tiku.io/questions/173712/make-https-call-using-httpclient
			http://www.dotnetcurry.com/showarticle.aspx?ID=1029
		*/

		public async Task<string> Translate ()
		{
			string url = string.Format
								(
				             uri_google_translate
								, Uri.EscapeUriString (text_to_translate)
								, culture_to_translate
								, culture_translated
			             );



			// Create a Language mapping
			var languageMap = new Dictionary<string, string> ();
			InitLanguageMap (languageMap);

			// Create an instance of WebClient in order to make the language translation
			//Uri address = new Uri(uri_google_translate);
			string address = 
					//@"http://holisticware.net/holisticware"
					@"http://google.com"
					;

			using (var client = new HttpClient ())
			{
				try
				{
					var handler = new HttpClientHandler();


					/*
						Problem:

						System.MissingMethodException: Method not found: 'System.Net.Http.HttpClientHandler.set_AutomaticDecompression'.
						at System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1[System.String].Start[<Translate>c__async0] (HolisticWare.Core.Localization.Translate.<Translate>c__async0& stateMachine) [0x0001b] 
							in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Runtime.CompilerServices/AsyncTaskMethodBuilder_T.cs:107
						at HolisticWare.Core.Localization.Translate.GoogleTranslatePrimitive.Translate () [0x00000] 
							in <filename unknown>:0
						at HolisticWare.BabelFish.MainPage+<buttonTranslate_Clicked>c__async0.MoveNext () [0x0002d] 
							in /Users/moljac/Projects/HolisticWare/HolisticWare.BabelFish/HolisticWare.BabelFish/HolisticWare.BabelFish/MainPage.xaml.cs:23

						Solution:

						add the portable HTTP client to BOTH your portable class library AND any app that consumes that assembly
					*/
					if (handler.SupportsAutomaticDecompression)
					{
						handler.AutomaticDecompression = 
							System.Net.DecompressionMethods.GZip 
							|
							System.Net.DecompressionMethods.Deflate
							;
					}

					var httpClient = new HttpClient (handler);

					client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
					client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
					client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0");
					client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");


        			string string_response = "";

					/*
						The code will not decompress

					var response_get_wit_uri = await client.GetAsync(new Uri (address));
					response_get_wit_uri.EnsureSuccessStatusCode ();
					string_response = await response_get_wit_uri.Content.ReadAsStringAsync ();
					*/


					// http://blogs.msdn.com/b/dotnet/archive/2013/06/06/portable-compression-and-httpclient-working-together.aspx
					var result = await client.GetStringAsync(url);
					// convert string to stream
					byte[] byteArray = Encoding.UTF8.GetBytes(result);
					//GZipStream bigStream = new GZipStream(new System.IO.MemoryStream(byteArray), CompressionMode.Decompress);
					System.IO.MemoryStream bigStreamOut = new System.IO.MemoryStream();
					//bigStream.CopyTo(bigStreamOut);
					string s = bigStreamOut.ToString();

					var response_get_wit_string = await client.GetAsync(address);
					response_get_wit_string.EnsureSuccessStatusCode ();
					string_response = await response_get_wit_string.Content.ReadAsStringAsync ();

				}
				catch (System.ArgumentException exc)
				{
					/*
						Problem:

						System.ArgumentException: Encoding name 'ISO-8859-2' not supported Parameter name: name   
							at System.Text.Encoding.GetEncoding (System.String name) [0x002f6] 
							in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Text/Encoding.cs:661    
							at System.Net.Http.HttpContent+<ReadAsStringAsync>c__async4.MoveNext () [0x0010e] 
							in ///Library/Frameworks/Xamarin.iOS.framework/Versions/8.6.0.51/src/mono/mcs/class/System.Net.Http/System.Net.Http/HttpContent.cs:176  
							--- End of stack trace from previous location where exception was thrown ---   
							at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000b] 
							in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Runtime.ExceptionServices/ExceptionDispatchInfo.cs:62
							at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1+ConfiguredTaskAwaiter[System.String].GetResult () 
							[0x00034] in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Runtime.CompilerServices/ConfiguredTaskAwaitable_T.cs:62    
							at System.Net.Http.HttpClient+<GetStringAsync>c__async5.MoveNext () [0x000a9] 
							in ///Library/Frameworks/Xamarin.iOS.framework/Versions/8.6.0.51/src/mono/mcs/class/System.Net.Http/System.Net.Http/HttpClient.cs:322  
							--- End of stack trace from previous location where exception was thrown ---   
							at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000b] 
							in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Runtime.ExceptionServices/ExceptionDispatchInfo.cs:62
							at System.Runtime.CompilerServices.TaskAwaiter`1[System.String].GetResult () [0x00034] 
							in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.Runtime.CompilerServices/TaskAwaiter_T.cs:59    
							at HolisticWare.Core.Localization.Translate.GoogleTranslatePrimitive+<Translate>c__async0.MoveNext () [0x0010e] 
							in /Users/moljac/Projects/HolisticWare/HolisticWare.BabelFish/HolisticWare.Core.Localization.Translate/GoogleTranslatePrimitive.cs:116 }	
							System.ArgumentException

							Related:

							http://blogs.msdn.com/b/dotnet/archive/2013/11/08/10435031.aspx?PageIndex=3

							Solution:

							client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0");
					*/
				}
				catch (System.Net.Http.HttpRequestException exc)
				{
					/*
						System.Net.Http.HttpRequestException: 403 (Forbidden: Access is denied.)   
							at System.Net.Http.HttpResponseMessage.EnsureSuccessStatusCode () [0x0000d] 
							in ///Library/Frameworks/Xamarin.iOS.framework/Versions/8.6.0.51/src/mono/mcs/class/System.Net.Http/System.Net.Http/HttpResponseMessage.cs:122    
							at HolisticWare.Core.Localization.Translate.GoogleTranslatePrimitive+<Translate>c__async0.MoveNext () [0x0013a] 
							in /Users/moljac/Projects/HolisticWare/HolisticWare.BabelFish/HolisticWare.Core.Localization.Translate/GoogleTranslatePrimitive.cs:125 }	
							System.Net.Http.HttpRequestException
					*/
					string msg = "";
				}
				catch (System.Exception exc)
				{

					string msg = "";
					System.Diagnostics.Debug.WriteLine(msg);
				}
			}
 
			/*
					Proxy for debugging 
						Fiddler, Charles

					for visual studio
					C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe.config


					 <system.net>
					    <defaultProxy useDefaultCredentials="true" enabled="true">
					      <proxy autoDetect="false" bypassonlocal="false" proxyaddress="http://127.0.0.1:8888" usesystemdefault="false" />
					    </defaultProxy>
					  </system.net>

					foa all apps 

					Open machine.config in the folder C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config. Note that if you are debugging a 64bit service (like ASP.NET) you will want to look in the Framework64 folder instead of the Framework folder. Similarly, if you are using a .NET version prior to 4.0, you will need to adjust the version part of the path.

Add the following XML block as a peer to the existing system.net element, replacing any existing defaultProxy element if present:

<!-- The following section is to force use of Fiddler for all applications, including those running in service accounts -->
 <system.net>
 <defaultProxy
                 enabled = "true"
                 useDefaultCredentials = "true">
 <proxy autoDetect="false" bypassonlocal="false" proxyaddress="http://127.0.0.1:8888" usesystemdefault="false" />
 </defaultProxy>
 </system.net>
  

				*/


			return text_translated;
		}

		private string uri_google_translate = 
						//@"http://translate.google.com/translate_t"
						@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}"
						;

		private string PostDataPrepare(string fromLanguage, string toLanguage, string content)
		{
			string google_translate_params = "hl=en&ie=UTF8&oe=UTF8submit=Translate&langpair={0}|{1}";

			string text_http_post_verb = string.Format
													(
													  google_translate_params
													, fromLanguage
													, toLanguage
													);

			// Encode the content and set the text query string param
			return text_http_post_verb += "&text=" + Uri.EscapeUriString(content);
		}

		public static void InitLanguageMap(Dictionary<string, string> language)
		{
			language.Add("Afrikaans", "af");
			language.Add("Albanian", "sq");
			language.Add("Arabic", "ar");
			language.Add("Armenian", "hy");
			language.Add("Azerbaijani", "az");
			language.Add("Basque", "eu");
			language.Add("Belarusian", "be");
			language.Add("Bengali", "bn");
			language.Add("Bulgarian", "bg");
			language.Add("Catalan", "ca");
			language.Add("Chinese", "zh-CN");
			language.Add("Croatian", "hr");
			language.Add("Czech", "cs");
			language.Add("Danish", "da");
			language.Add("Dutch", "nl"); 
			language.Add("English", "en");

			return;
		}




		public async void DownloadWikipedia()
		{
			var handler = new HttpClientHandler();
			if (handler.SupportsAutomaticDecompression)
			{
			    handler.AutomaticDecompression = 
			    					System.Net.DecompressionMethods.GZip 
			    					|
			    					System.Net.DecompressionMethods.Deflate
			    					;
			}
			var httpClient = new HttpClient(handler);
			var str = await httpClient.GetStringAsync("http://en.wikipedia.org/wiki/Gzip");   

	        return;
     	}

	}
}

