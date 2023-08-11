using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using Email = Ordering.Application.Models.Email;

namespace Ordering.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailSettings _emailSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options, IFluentEmail fluentEmail)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _emailSettings = options.Value;
    }
    
    public async Task<bool> SendEmail(Email email)
    {
        var sender = new SmtpSender(() => new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
        });

        _fluentEmail.Sender = sender;
        
        var response = await _fluentEmail.To(email.To)
            .Subject(email.Subject)
            .Body(email.Body)
            .SendAsync();

        if (!response.Successful)
            _logger.LogError("Email sending failed. Reason - {ErrorMessages}", response.ErrorMessages);

        return response.Successful;
    }
}