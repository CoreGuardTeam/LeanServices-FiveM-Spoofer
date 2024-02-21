using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace KeyAuth
{
	// Token: 0x02000003 RID: 3
	public class api
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000C140 File Offset: 0x0000A340
		public api(string name, string ownerid, string secret, string version)
		{
			bool flag = ownerid.Length != 10 || secret.Length != 64;
			if (flag)
			{
				Process.Start("https://youtube.com/watch?v=RfDTdiBq4_o");
				Process.Start("https://keyauth.cc/app/");
				Thread.Sleep(2000);
				api.error("Application not setup correctly. Please watch the YouTube video for setup.");
				Environment.Exit(0);
			}
			this.name = name;
			this.ownerid = ownerid;
			this.secret = secret;
			this.version = version;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public void init()
		{
			string text = encryption.iv_key();
			api.enckey = text + "-" + this.secret;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "init";
			nameValueCollection["ver"] = this.version;
			nameValueCollection["hash"] = api.checksum(Process.GetCurrentProcess().MainModule.FileName);
			nameValueCollection["enckey"] = text;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text2 = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			if (success)
			{
				bool newSession = response_structure.newSession;
				if (newSession)
				{
					Thread.Sleep(100);
				}
				api.sessionid = response_structure.sessionid;
				this.initialized = true;
			}
			else
			{
				bool flag = response_structure.message == "invalidver";
				if (flag)
				{
					this.app_data.downloadLink = response_structure.download;
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000C31C File Offset: 0x0000A51C
		public void CheckInit()
		{
			bool flag = !this.initialized;
			if (flag)
			{
				api.error("You must run the function KeyAuthApp.init(); first");
				Environment.Exit(0);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000C34C File Offset: 0x0000A54C
		public string expirydaysleft(string Type, int subscription)
		{
			this.CheckInit();
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
			dateTime = dateTime.AddSeconds((double)long.Parse(this.user_data.subscriptions[subscription].expiry)).ToLocalTime();
			TimeSpan timeSpan = dateTime - DateTime.Now;
			string text = Type.ToLower();
			bool flag = !(text == "months");
			string text2;
			if (flag)
			{
				bool flag2 = !(text == "days");
				if (flag2)
				{
					bool flag3 = !(text == "hours");
					if (flag3)
					{
						text2 = null;
					}
					else
					{
						text2 = Convert.ToString(timeSpan.Hours);
					}
				}
				else
				{
					text2 = Convert.ToString(timeSpan.Days);
				}
			}
			else
			{
				text2 = Convert.ToString(timeSpan.Days / 30);
			}
			return text2;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000C438 File Offset: 0x0000A638
		public void register(string username, string pass, string key, string email = "")
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "register";
			nameValueCollection["username"] = username;
			nameValueCollection["pass"] = pass;
			nameValueCollection["key"] = key;
			nameValueCollection["email"] = email;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			if (success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000C524 File Offset: 0x0000A724
		public void forgot(string username, string email)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "forgot";
			nameValueCollection["username"] = username;
			nameValueCollection["email"] = email;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000C5BC File Offset: 0x0000A7BC
		public void login(string username, string pass)
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "login";
			nameValueCollection["username"] = username;
			nameValueCollection["pass"] = pass;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			if (success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000C68C File Offset: 0x0000A88C
		public void logout()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "logout";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000C70C File Offset: 0x0000A90C
		public void web_login()
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			HttpListener httpListener;
			HttpListenerRequest request;
			HttpListenerResponse httpListenerResponse;
			for (;;)
			{
				httpListener = new HttpListener();
				string text = "handshake";
				text = "http://localhost:1337/" + text + "/";
				httpListener.Prefixes.Add(text);
				httpListener.Start();
				HttpListenerContext context = httpListener.GetContext();
				request = context.Request;
				httpListenerResponse = context.Response;
				httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
				httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
				httpListenerResponse.AddHeader("Via", "hugzho's big brain");
				httpListenerResponse.AddHeader("Location", "your kernel ;)");
				httpListenerResponse.AddHeader("Retry-After", "never lmao");
				httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
				bool flag = !(request.HttpMethod == "OPTIONS");
				if (flag)
				{
					break;
				}
				httpListenerResponse.StatusCode = 200;
				Thread.Sleep(1);
				httpListener.Stop();
			}
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			string text2 = request.RawUrl.Replace("/handshake?user=", "").Replace("&token=", " ");
			string text3 = text2.Split(Array.Empty<char>())[0];
			string text4 = text2.Split(new char[] { ' ' })[1];
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "login";
			nameValueCollection["username"] = text3;
			nameValueCollection["token"] = text4;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text5 = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text5);
			this.load_response_struct(response_structure);
			bool flag2 = true;
			bool success = response_structure.success;
			if (success)
			{
				this.load_user_data(response_structure.info);
				httpListenerResponse.StatusCode = 420;
				httpListenerResponse.StatusDescription = "SHEESH";
			}
			else
			{
				Console.WriteLine(response_structure.message);
				httpListenerResponse.StatusCode = 200;
				httpListenerResponse.StatusDescription = response_structure.message;
				flag2 = false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes("Whats up?");
			httpListenerResponse.ContentLength64 = (long)bytes.Length;
			httpListenerResponse.OutputStream.Write(bytes, 0, bytes.Length);
			Thread.Sleep(1);
			httpListener.Stop();
			bool flag3 = !flag2;
			if (flag3)
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000C9E8 File Offset: 0x0000ABE8
		public void button(string button)
		{
			this.CheckInit();
			HttpListener httpListener = new HttpListener();
			string text = "http://localhost:1337/" + button + "/";
			httpListener.Prefixes.Add(text);
			httpListener.Start();
			HttpListenerContext context = httpListener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse httpListenerResponse = context.Response;
			httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
			httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
			httpListenerResponse.AddHeader("Via", "hugzho's big brain");
			httpListenerResponse.AddHeader("Location", "your kernel ;)");
			httpListenerResponse.AddHeader("Retry-After", "never lmao");
			httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
			httpListenerResponse.StatusCode = 420;
			httpListenerResponse.StatusDescription = "SHEESH";
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			httpListener.Stop();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		public void upgrade(string username, string key)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "upgrade";
			nameValueCollection["username"] = username;
			nameValueCollection["key"] = key;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			response_structure.success = false;
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000CB88 File Offset: 0x0000AD88
		public void license(string key)
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "license";
			nameValueCollection["key"] = key;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			if (success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		public void check()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "check";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000CCCC File Offset: 0x0000AECC
		public void setvar(string var, string data)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "setvar";
			nameValueCollection["var"] = var;
			nameValueCollection["data"] = data;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000CD64 File Offset: 0x0000AF64
		public string getvar(string var)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "getvar";
			nameValueCollection["var"] = var;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			string text2;
			if (success)
			{
				text2 = response_structure.response;
			}
			else
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000CE10 File Offset: 0x0000B010
		public void ban(string reason = null)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "ban";
			nameValueCollection["reason"] = reason;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000CE9C File Offset: 0x0000B09C
		public string var(string varid)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "var";
			nameValueCollection["varid"] = varid;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			string text2;
			if (success)
			{
				text2 = response_structure.message;
			}
			else
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000CF48 File Offset: 0x0000B148
		public List<api.users> fetchOnline()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "fetchOnline";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			List<api.users> list;
			if (success)
			{
				list = response_structure.users;
			}
			else
			{
				list = null;
			}
			return list;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		public void fetchStats()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "fetchStats";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			if (success)
			{
				this.load_app_data(response_structure.appinfo);
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000D080 File Offset: 0x0000B280
		public List<api.msg> chatget(string channelname)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "chatget";
			nameValueCollection["channel"] = channelname;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			List<api.msg> list;
			if (success)
			{
				list = response_structure.messages;
			}
			else
			{
				list = null;
			}
			return list;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000D12C File Offset: 0x0000B32C
		public bool chatsend(string msg, string channelname)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "chatsend";
			nameValueCollection["message"] = msg;
			nameValueCollection["channel"] = channelname;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public bool checkblack()
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "checkblacklist";
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000D278 File Offset: 0x0000B478
		public string webhook(string webid, string param, string body = "", string conttype = "")
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "webhook";
			nameValueCollection["webid"] = webid;
			nameValueCollection["params"] = param;
			nameValueCollection["body"] = body;
			nameValueCollection["conttype"] = conttype;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			string text2;
			if (success)
			{
				text2 = response_structure.response;
			}
			else
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000D34C File Offset: 0x0000B54C
		public byte[] download(string fileid)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "file";
			nameValueCollection["fileid"] = fileid;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
			bool success = response_structure.success;
			byte[] array;
			if (success)
			{
				array = encryption.str_to_byte_arr(response_structure.contents);
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000D400 File Offset: 0x0000B600
		public void log(string message)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "log";
			nameValueCollection["pcuser"] = Environment.UserName;
			nameValueCollection["message"] = message;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			api.req(nameValueCollection);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000D488 File Offset: 0x0000B688
		public void changeUsername(string username)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "changeUsername";
			nameValueCollection["newUsername"] = username;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string text = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text);
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000D514 File Offset: 0x0000B714
		public static string checksum(string filename)
		{
			string text;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					text = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
				}
			}
			return text;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000D590 File Offset: 0x0000B790
		public static void error(string message)
		{
			string text = "Logs";
			string text2 = Path.Combine(text, "ErrorLogs.txt");
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			bool flag2 = !File.Exists(text2);
			if (flag2)
			{
				using (File.Create(text2))
				{
					File.AppendAllText(text2, DateTime.Now.ToString() + " > This is the start of your error logs file");
				}
			}
			File.AppendAllText(text2, DateTime.Now.ToString() + " > " + message + Environment.NewLine);
			Process.Start(new ProcessStartInfo("cmd.exe", "/c start cmd /C \"color b && title Error && echo " + message + " && timeout /t 5\"")
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			});
			Environment.Exit(0);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000D68C File Offset: 0x0000B88C
		private static string req(NameValueCollection post_data)
		{
			string text;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.Proxy = null;
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(api.assertSSL));
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					byte[] array = webClient.UploadValues("https://keyauth.win/api/1.2/", post_data);
					stopwatch.Stop();
					api.responseTime = stopwatch.ElapsedMilliseconds;
					api.sigCheck(Encoding.Default.GetString(array), webClient.ResponseHeaders["signature"], post_data.Get(0));
					text = Encoding.Default.GetString(array);
				}
			}
			catch (WebException ex)
			{
				bool flag = ((HttpWebResponse)ex.Response).StatusCode != (HttpStatusCode)429;
				if (flag)
				{
					api.error("Connection failure. Please try again, or contact us for help.");
					Environment.Exit(0);
					text = "";
				}
				else
				{
					api.error("You're connecting too fast to loader, slow down.");
					Environment.Exit(0);
					text = "";
				}
			}
			return text;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000D7BC File Offset: 0x0000B9BC
		private static bool assertSSL(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool flag = (!certificate.Issuer.Contains("Cloudflare Inc") && !certificate.Issuer.Contains("Google Trust Services") && !certificate.Issuer.Contains("Let's Encrypt")) || sslPolicyErrors > SslPolicyErrors.None;
			bool flag2;
			if (flag)
			{
				api.error("SSL assertion fail, make sure you're not debugging Network. Disable internet firewall on router if possible. & echo: & echo If not, ask the developer of the program to use custom domains to fix this.");
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000D828 File Offset: 0x0000BA28
		private static void sigCheck(string resp, string signature, string type)
		{
			bool flag = !(type == "log") && !(type == "file");
			if (flag)
			{
				try
				{
					bool flag2 = !encryption.CheckStringsFixedTime(encryption.HashHMAC((type == "init") ? api.enckey.Substring(17, 64) : api.enckey, resp), signature);
					if (flag2)
					{
						api.error("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " + resp);
						Environment.Exit(0);
					}
				}
				catch
				{
					api.error("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " + resp);
					Environment.Exit(0);
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000D8DC File Offset: 0x0000BADC
		private void load_app_data(api.app_data_structure data)
		{
			this.app_data.numUsers = data.numUsers;
			this.app_data.numOnlineUsers = data.numOnlineUsers;
			this.app_data.numKeys = data.numKeys;
			this.app_data.version = data.version;
			this.app_data.customerPanelLink = data.customerPanelLink;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000D944 File Offset: 0x0000BB44
		private void load_user_data(api.user_data_structure data)
		{
			this.user_data.username = data.username;
			this.user_data.ip = data.ip;
			this.user_data.hwid = data.hwid;
			this.user_data.createdate = data.createdate;
			this.user_data.lastlogin = data.lastlogin;
			this.user_data.subscriptions = data.subscriptions;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000BD71 File Offset: 0x00009F71
		private void load_response_struct(api.response_structure data)
		{
			this.response.success = data.success;
			this.response.message = data.message;
		}

		// Token: 0x04000002 RID: 2
		public string name;

		// Token: 0x04000003 RID: 3
		public string ownerid;

		// Token: 0x04000004 RID: 4
		public string secret;

		// Token: 0x04000005 RID: 5
		public string version;

		// Token: 0x04000006 RID: 6
		public static long responseTime;

		// Token: 0x04000007 RID: 7
		private static string sessionid;

		// Token: 0x04000008 RID: 8
		private static string enckey;

		// Token: 0x04000009 RID: 9
		private bool initialized;

		// Token: 0x0400000A RID: 10
		public api.app_data_class app_data = new api.app_data_class();

		// Token: 0x0400000B RID: 11
		public api.user_data_class user_data = new api.user_data_class();

		// Token: 0x0400000C RID: 12
		public api.response_class response = new api.response_class();

		// Token: 0x0400000D RID: 13
		private json_wrapper response_decoder = new json_wrapper(new api.response_structure());

		// Token: 0x02000004 RID: 4
		[DataContract]
		private class response_structure
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000025 RID: 37 RVA: 0x0000BD98 File Offset: 0x00009F98
			// (set) Token: 0x06000026 RID: 38 RVA: 0x0000BDA0 File Offset: 0x00009FA0
			[DataMember]
			public bool success { get; set; }

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000027 RID: 39 RVA: 0x0000BDA9 File Offset: 0x00009FA9
			// (set) Token: 0x06000028 RID: 40 RVA: 0x0000BDB1 File Offset: 0x00009FB1
			[DataMember]
			public bool newSession { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000029 RID: 41 RVA: 0x0000BDBA File Offset: 0x00009FBA
			// (set) Token: 0x0600002A RID: 42 RVA: 0x0000BDC2 File Offset: 0x00009FC2
			[DataMember]
			public string sessionid { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600002B RID: 43 RVA: 0x0000BDCB File Offset: 0x00009FCB
			// (set) Token: 0x0600002C RID: 44 RVA: 0x0000BDD3 File Offset: 0x00009FD3
			[DataMember]
			public string contents { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600002D RID: 45 RVA: 0x0000BDDC File Offset: 0x00009FDC
			// (set) Token: 0x0600002E RID: 46 RVA: 0x0000BDE4 File Offset: 0x00009FE4
			[DataMember]
			public string response { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600002F RID: 47 RVA: 0x0000BDED File Offset: 0x00009FED
			// (set) Token: 0x06000030 RID: 48 RVA: 0x0000BDF5 File Offset: 0x00009FF5
			[DataMember]
			public string message { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000031 RID: 49 RVA: 0x0000BDFE File Offset: 0x00009FFE
			// (set) Token: 0x06000032 RID: 50 RVA: 0x0000BE06 File Offset: 0x0000A006
			[DataMember]
			public string download { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000033 RID: 51 RVA: 0x0000BE0F File Offset: 0x0000A00F
			// (set) Token: 0x06000034 RID: 52 RVA: 0x0000BE17 File Offset: 0x0000A017
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.user_data_structure info { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000035 RID: 53 RVA: 0x0000BE20 File Offset: 0x0000A020
			// (set) Token: 0x06000036 RID: 54 RVA: 0x0000BE28 File Offset: 0x0000A028
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.app_data_structure appinfo { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000037 RID: 55 RVA: 0x0000BE31 File Offset: 0x0000A031
			// (set) Token: 0x06000038 RID: 56 RVA: 0x0000BE39 File Offset: 0x0000A039
			[DataMember]
			public List<api.msg> messages { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000039 RID: 57 RVA: 0x0000BE42 File Offset: 0x0000A042
			// (set) Token: 0x0600003A RID: 58 RVA: 0x0000BE4A File Offset: 0x0000A04A
			[DataMember]
			public List<api.users> users { get; set; }
		}

		// Token: 0x02000005 RID: 5
		public class msg
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600003C RID: 60 RVA: 0x0000BE53 File Offset: 0x0000A053
			// (set) Token: 0x0600003D RID: 61 RVA: 0x0000BE5B File Offset: 0x0000A05B
			public string message { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x0600003E RID: 62 RVA: 0x0000BE64 File Offset: 0x0000A064
			// (set) Token: 0x0600003F RID: 63 RVA: 0x0000BE6C File Offset: 0x0000A06C
			public string author { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000040 RID: 64 RVA: 0x0000BE75 File Offset: 0x0000A075
			// (set) Token: 0x06000041 RID: 65 RVA: 0x0000BE7D File Offset: 0x0000A07D
			public string timestamp { get; set; }
		}

		// Token: 0x02000006 RID: 6
		public class users
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000043 RID: 67 RVA: 0x0000BE86 File Offset: 0x0000A086
			// (set) Token: 0x06000044 RID: 68 RVA: 0x0000BE8E File Offset: 0x0000A08E
			public string credential { get; set; }
		}

		// Token: 0x02000007 RID: 7
		[DataContract]
		private class user_data_structure
		{
			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000046 RID: 70 RVA: 0x0000BE97 File Offset: 0x0000A097
			// (set) Token: 0x06000047 RID: 71 RVA: 0x0000BE9F File Offset: 0x0000A09F
			[DataMember]
			public string username { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000048 RID: 72 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
			// (set) Token: 0x06000049 RID: 73 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
			[DataMember]
			public string ip { get; set; }

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600004A RID: 74 RVA: 0x0000BEB9 File Offset: 0x0000A0B9
			// (set) Token: 0x0600004B RID: 75 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
			[DataMember]
			public string hwid { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600004C RID: 76 RVA: 0x0000BECA File Offset: 0x0000A0CA
			// (set) Token: 0x0600004D RID: 77 RVA: 0x0000BED2 File Offset: 0x0000A0D2
			[DataMember]
			public string createdate { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600004E RID: 78 RVA: 0x0000BEDB File Offset: 0x0000A0DB
			// (set) Token: 0x0600004F RID: 79 RVA: 0x0000BEE3 File Offset: 0x0000A0E3
			[DataMember]
			public string lastlogin { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000050 RID: 80 RVA: 0x0000BEEC File Offset: 0x0000A0EC
			// (set) Token: 0x06000051 RID: 81 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
			[DataMember]
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x02000008 RID: 8
		[DataContract]
		private class app_data_structure
		{
			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000053 RID: 83 RVA: 0x0000BEFD File Offset: 0x0000A0FD
			// (set) Token: 0x06000054 RID: 84 RVA: 0x0000BF05 File Offset: 0x0000A105
			[DataMember]
			public string numUsers { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000055 RID: 85 RVA: 0x0000BF0E File Offset: 0x0000A10E
			// (set) Token: 0x06000056 RID: 86 RVA: 0x0000BF16 File Offset: 0x0000A116
			[DataMember]
			public string numOnlineUsers { get; set; }

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000057 RID: 87 RVA: 0x0000BF1F File Offset: 0x0000A11F
			// (set) Token: 0x06000058 RID: 88 RVA: 0x0000BF27 File Offset: 0x0000A127
			[DataMember]
			public string numKeys { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000059 RID: 89 RVA: 0x0000BF30 File Offset: 0x0000A130
			// (set) Token: 0x0600005A RID: 90 RVA: 0x0000BF38 File Offset: 0x0000A138
			[DataMember]
			public string version { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x0600005B RID: 91 RVA: 0x0000BF41 File Offset: 0x0000A141
			// (set) Token: 0x0600005C RID: 92 RVA: 0x0000BF49 File Offset: 0x0000A149
			[DataMember]
			public string customerPanelLink { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x0600005D RID: 93 RVA: 0x0000BF52 File Offset: 0x0000A152
			// (set) Token: 0x0600005E RID: 94 RVA: 0x0000BF5A File Offset: 0x0000A15A
			[DataMember]
			public string downloadLink { get; set; }
		}

		// Token: 0x02000009 RID: 9
		public class app_data_class
		{
			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000060 RID: 96 RVA: 0x0000BF63 File Offset: 0x0000A163
			// (set) Token: 0x06000061 RID: 97 RVA: 0x0000BF6B File Offset: 0x0000A16B
			public string numUsers { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000062 RID: 98 RVA: 0x0000BF74 File Offset: 0x0000A174
			// (set) Token: 0x06000063 RID: 99 RVA: 0x0000BF7C File Offset: 0x0000A17C
			public string numOnlineUsers { get; set; }

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000064 RID: 100 RVA: 0x0000BF85 File Offset: 0x0000A185
			// (set) Token: 0x06000065 RID: 101 RVA: 0x0000BF8D File Offset: 0x0000A18D
			public string numKeys { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000066 RID: 102 RVA: 0x0000BF96 File Offset: 0x0000A196
			// (set) Token: 0x06000067 RID: 103 RVA: 0x0000BF9E File Offset: 0x0000A19E
			public string version { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x06000068 RID: 104 RVA: 0x0000BFA7 File Offset: 0x0000A1A7
			// (set) Token: 0x06000069 RID: 105 RVA: 0x0000BFAF File Offset: 0x0000A1AF
			public string customerPanelLink { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x0600006A RID: 106 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
			// (set) Token: 0x0600006B RID: 107 RVA: 0x0000BFC0 File Offset: 0x0000A1C0
			public string downloadLink { get; set; }
		}

		// Token: 0x0200000A RID: 10
		public class user_data_class
		{
			// Token: 0x17000022 RID: 34
			// (get) Token: 0x0600006D RID: 109 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
			// (set) Token: 0x0600006E RID: 110 RVA: 0x0000BFD1 File Offset: 0x0000A1D1
			public string username { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x0600006F RID: 111 RVA: 0x0000BFDA File Offset: 0x0000A1DA
			// (set) Token: 0x06000070 RID: 112 RVA: 0x0000BFE2 File Offset: 0x0000A1E2
			public string ip { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x06000071 RID: 113 RVA: 0x0000BFEB File Offset: 0x0000A1EB
			// (set) Token: 0x06000072 RID: 114 RVA: 0x0000BFF3 File Offset: 0x0000A1F3
			public string hwid { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000073 RID: 115 RVA: 0x0000BFFC File Offset: 0x0000A1FC
			// (set) Token: 0x06000074 RID: 116 RVA: 0x0000C004 File Offset: 0x0000A204
			public string createdate { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000C00D File Offset: 0x0000A20D
			// (set) Token: 0x06000076 RID: 118 RVA: 0x0000C015 File Offset: 0x0000A215
			public string lastlogin { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x06000077 RID: 119 RVA: 0x0000C01E File Offset: 0x0000A21E
			// (set) Token: 0x06000078 RID: 120 RVA: 0x0000C026 File Offset: 0x0000A226
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x0200000B RID: 11
		public class Data
		{
			// Token: 0x17000028 RID: 40
			// (get) Token: 0x0600007A RID: 122 RVA: 0x0000C02F File Offset: 0x0000A22F
			// (set) Token: 0x0600007B RID: 123 RVA: 0x0000C037 File Offset: 0x0000A237
			public string subscription { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x0600007C RID: 124 RVA: 0x0000C040 File Offset: 0x0000A240
			// (set) Token: 0x0600007D RID: 125 RVA: 0x0000C048 File Offset: 0x0000A248
			public string expiry { get; set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600007E RID: 126 RVA: 0x0000C051 File Offset: 0x0000A251
			// (set) Token: 0x0600007F RID: 127 RVA: 0x0000C059 File Offset: 0x0000A259
			public string timeleft { get; set; }
		}

		// Token: 0x0200000C RID: 12
		public class response_class
		{
			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000081 RID: 129 RVA: 0x0000C062 File Offset: 0x0000A262
			// (set) Token: 0x06000082 RID: 130 RVA: 0x0000C06A File Offset: 0x0000A26A
			public bool success { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000083 RID: 131 RVA: 0x0000C073 File Offset: 0x0000A273
			// (set) Token: 0x06000084 RID: 132 RVA: 0x0000C07B File Offset: 0x0000A27B
			public string message { get; set; }
		}
	}
}
