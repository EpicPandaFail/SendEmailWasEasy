public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

		private readonly string _googleVerifyAddress = "https://www.google.com/recaptcha/api/siteverify";

		private readonly string _googleRecaptchaSecret = "secretCodeAyyyyy";

		protected readonly ISettingService _service;

		//private readonly object chkssl;

		IGenericServiceQuery<CompanyInfo> _companyService;
        IGenericServiceQuery<Service> _serviceService;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger, IGenericServiceQuery<CompanyInfo> companyService, IGenericServiceQuery<Service> serviceService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _companyService = companyService;
            _serviceService = serviceService;
        }

	[HttpPost]
        [ValidateAntiForgeryToken]
	public IActionResult SendMail(string name, string email, string subject, string message)
	{
		try
		{
			var mailMessage = new MailMessage();
			mailMessage.From = new MailAddress("tester@test.com");
			mailMessage.To.Add("careerboard.employer@outlook.com");
			mailMessage.Subject = subject;
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = $"<p>From: {name} </p><p>Email: {email}</p><p>Subject: {subject}</p><br><p>{message}</p>";

			var smtpClient = new SmtpClient("outlook.office365.com", 587);
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential("tester@test.com", "test");
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.EnableSsl = true;
			smtpClient.Send(mailMessage);

			return Json(new { success = true });
		}
		catch (Exception ex)
		{
			return Json(new { success = false, error = ex.Message });
		}
	}

	[HttpGet]
        public async Task<JsonResult> RecaptchaV3Vverify(string token)
        {
            var tokenResponse = new TokenResponse();

            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetStringAsync($"{_googleVerifyAddress}?secret={_googleRecaptchaSecret}&response={token}");
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response);
            }
            return Json(tokenResponse);
        }

}
