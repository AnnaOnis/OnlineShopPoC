﻿using Microsoft.Extensions.Logging;
using OnlineShopPoC.Services;

namespace OnlineShopPoC.Decorators
{
    public class EmailSenderLoggingDecorator : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        public EmailSenderLoggingDecorator(IEmailSender emailSender,
                                           ILogger<EmailSenderLoggingDecorator> logger)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmail(string recepientEmail, string subject, string message)
        {
            _logger.LogInformation("Sending email to {recepientEmail}, subject: {subject}, message: {message}", recepientEmail, subject, message);
            await _emailSender.SendEmail(recepientEmail, subject, message);
        }
    }
}
