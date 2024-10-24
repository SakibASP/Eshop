﻿using Eshop.Utils;
using System.Net.Mail;
using System.Net;
using System.Text;
using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;
using Serilog;
using Eshop.Interfaces.BusinessDetails;

namespace Eshop.Web.Repositories.BusinessDomains
{
    public class EmailOrderProcessor(EmailSettings settings, IWebHostEnvironment webHostEnvironment) : IOrderProcessor
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        public async Task ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.EnableSsl = settings.UseSsl;
                smtpClient.Host = settings.ServerName;
                smtpClient.Port = settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

                if (settings.WriteAsFile)
                {
                    var fileLocation = Path.Combine(_webHostEnvironment.WebRootPath, Constant.EmailFolderName);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = fileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                                        .AppendLine("A new order has been submitted")
                                        .AppendLine("---")
                                        .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}",
                    line.Quantity,
                    line.Product.Name,
                    subtotal);
                }
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                                                            .AppendLine("---")
                                                            .AppendLine("Ship to:")
                                                            .AppendLine(shippingInfo.Name)
                                                            .AppendLine(shippingInfo.Line1)
                                                            .AppendLine(shippingInfo.Line2 ?? "")
                                                            .AppendLine(shippingInfo.Line3 ?? "")
                                                            .AppendLine(shippingInfo.City)
                                                            .AppendLine(shippingInfo.State)
                                                            .AppendLine(shippingInfo.Country)
                                                            .AppendLine(shippingInfo.Zip ?? "")
                                                            .AppendLine("---")
                                                            .AppendFormat("Gift wrap: {0}", shippingInfo.Giftwrap ? "Yes" : "No");
                MailMessage mailMessage = new(settings.MailFromAddress, //From
                                            shippingInfo.Email!, //To
                                            "New order submitted!", //Subject
                                            body.ToString() // Body
                                            );
                if (settings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "I am from EmailOrderProcessor");
            }
        }
    }
}
