using System;
using System.Net;

namespace IWantMoreNames
{
	// Mono doesn't come with any SSL root certificates.
	// In order to support GitHub, we have to be able to download data via https.
	// When an instance of this is created, SSL ceritificate validation is disabled process-wide.
	// When the instance is destroyed, certificates are validated properly once again.
	// Use this class within a using statement.
	public class NoSSL : IDisposable
	{
		// A shorthand for a new instance
		public static NoSSL Instance
		{
			get { return new NoSSL(); }
		}

		// Disable SSL certificate validation
		public NoSSL()
		{
			ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
		}

		// Enable SSL certificate validation
		public void Dispose()
		{
			ServicePointManager.ServerCertificateValidationCallback = null;
		}
	}

}