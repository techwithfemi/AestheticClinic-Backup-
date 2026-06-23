// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// WhatsApp Message Templates
// ---------------------------------------

namespace AestheticEMR.Server.Services.WhatsApp
{
    public static class WhatsAppTemplates
    {
        /// <summary>
        /// Dictionary of WhatsApp message templates
        /// </summary>
        private static readonly Dictionary<string, string> Templates = new(StringComparer.OrdinalIgnoreCase)
        {
            // Appointment Templates
            {
                "appointment-confirmation",
                "Hello {0},\n\n" +
                "Your appointment has been confirmed!\n\n" +
                "📅 Date: {1}\n" +
                "🕐 Time: {2}\n" +
                "👨‍⚕️ Provider: {3}\n" +
                "📍 Location: {4}\n\n" +
                "Please arrive 10 minutes early. Reply CONFIRM to confirm or RESCHEDULE to change."
            },
            {
                "appointment-reminder",
                "Hello {0},\n\n" +
                "⏰ Reminder: You have an appointment tomorrow!\n\n" +
                "📅 Date: {1}\n" +
                "🕐 Time: {2}\n" +
                "👨‍⚕️ Provider: {3}\n\n" +
                "Reply CONFIRM if you'll attend or CANCEL if you need to reschedule."
            },
            {
                "appointment-cancelled",
                "Hello {0},\n\n" +
                "Your appointment scheduled for {1} at {2} has been cancelled.\n\n" +
                "Please contact us to reschedule: {3}"
            },
            {
                "appointment-rescheduled",
                "Hello {0},\n\n" +
                "✅ Your appointment has been rescheduled!\n\n" +
                "New Date: {1}\n" +
                "New Time: {2}\n" +
                "Provider: {3}\n\n" +
                "Please confirm by replying CONFIRM."
            },

            // Billing & Payment Templates
            {
                "invoice-sent",
                "Hello {0},\n\n" +
                "Your invoice has been prepared.\n\n" +
                "Invoice #: {1}\n" +
                "Amount Due: {2}\n" +
                "Due Date: {3}\n\n" +
                "Please arrange payment at your earliest convenience. Contact us if you have questions."
            },
            {
                "payment-reminder",
                "Hello {0},\n\n" +
                "💰 Payment Reminder\n\n" +
                "Invoice #: {1}\n" +
                "Amount Due: {2}\n" +
                "Due Date: {3}\n\n" +
                "Please arrange payment. Reply PAID if you've already sent payment."
            },
            {
                "payment-received",
                "Hello {0},\n\n" +
                "✅ Payment Received!\n\n" +
                "Thank you for your payment of {1}.\n\n" +
                "Invoice #: {2}\n" +
                "Reference: {3}\n\n" +
                "Your account balance is now {4}."
            },

            // Follow-up Templates
            {
                "followup-consultation",
                "Hello {0},\n\n" +
                "We hope you're pleased with your recent {1} consultation!\n\n" +
                "Your follow-up appointment is scheduled for:\n" +
                "📅 Date: {2}\n" +
                "🕐 Time: {3}\n\n" +
                "Please reply CONFIRM to confirm your attendance."
            },
            {
                "post-procedure-care",
                "Hello {0},\n\n" +
                "Thank you for choosing our clinic for your {1} procedure.\n\n" +
                "⚠️ Important Post-Care Instructions:\n" +
                "{2}\n\n" +
                "If you experience any complications, contact us immediately: {3}"
            },
            {
                "followup-survey",
                "Hello {0},\n\n" +
                "We'd love to know about your experience! 😊\n\n" +
                "Could you please rate your recent {1} visit?\n\n" +
                "Visit our survey link: {2}\n\n" +
                "Thank you for your feedback!"
            },

            // General Notifications
            {
                "account-verification",
                "Hello {0},\n\n" +
                "Welcome to {1}!\n\n" +
                "Your verification code is: {2}\n\n" +
                "This code expires in 10 minutes. Do not share this code with anyone."
            },
            {
                "welcome-new-patient",
                "Hello {0},\n\n" +
                "Welcome to {1}! 👋\n\n" +
                "We're delighted to have you. Complete your profile to get started: {2}\n\n" +
                "For any questions, reply to this message or call us at {3}."
            },
            {
                "clinic-update",
                "Hello {0},\n\n" +
                "📢 Update from {1}\n\n" +
                "{2}\n\n" +
                "For more information, visit: {3}"
            },

            // Service-Specific Templates
            {
                "aesthetic-consultation-offer",
                "Hello {0},\n\n" +
                "✨ Special Offer: {1}\n\n" +
                "Services: {2}\n" +
                "Discount: {3}\n" +
                "Valid Until: {4}\n\n" +
                "Book your free consultation today! Reply BOOK or call {5}"
            },
            {
                "dental-appointment-reminder",
                "Hello {0},\n\n" +
                "🦷 Dental Appointment Reminder\n\n" +
                "📅 {1}\n" +
                "🕐 {2}\n" +
                "Service: {3}\n\n" +
                "Please reply CONFIRM. For emergencies, call {4}"
            },
            {
                "lab-results-ready",
                "Hello {0},\n\n" +
                "Your lab results are ready!\n\n" +
                "Reference #: {1}\n" +
                "Test: {2}\n\n" +
                "Please visit us to collect your results or reply for home delivery details."
            }
        };

        /// <summary>
        /// Gets a template by name and substitutes variables
        /// </summary>
        public static string? GetTemplate(string templateName, params string[] variables)
        {
            if (!Templates.TryGetValue(templateName, out var template))
            {
                return null;
            }

            try
            {
                // Replace placeholders {0}, {1}, etc. with provided variables
                return string.Format(template, variables.Cast<object>().ToArray());
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(
                    $"Template '{templateName}' has {CountPlaceholders(template)} placeholders but {variables.Length} variables were provided.",
                    nameof(variables), ex);
            }
        }

        /// <summary>
        /// Gets all available template names
        /// </summary>
        public static IEnumerable<string> GetAvailableTemplates()
        {
            return Templates.Keys.ToList();
        }

        /// <summary>
        /// Checks if a template exists
        /// </summary>
        public static bool TemplateExists(string templateName)
        {
            return Templates.ContainsKey(templateName);
        }

        /// <summary>
        /// Counts the number of placeholders in a template
        /// </summary>
        private static int CountPlaceholders(string template)
        {
            int count = 0;
            for (int i = 0; i < template.Length; i++)
            {
                if (template[i] == '{' && i + 1 < template.Length && 
                    char.IsDigit(template[i + 1]))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Adds a custom template (for dynamic templates)
        /// </summary>
        public static void AddCustomTemplate(string templateName, string templateContent)
        {
            Templates[templateName] = templateContent;
        }
    }
}
