using MFA_assignment.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using static MFA_assignment.Models.Confirmation;

namespace MFA_assignment.Models
{
    public class ConfirmationService : IConfirmationService,, IDisposable
    {
        private static readonly Dictionary<string, string> activeConfirmationCodes = new Dictionary<string, string>();
        private static readonly TimeSpan CodeExpirationDuration = TimeSpan.FromMinutes(5);
        private static readonly object locker = new object(); // For synchronization

        public IHttpActionResult SendConfirmationCode(SendConfirmationCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Phone))
            {
                return BadRequest("Phone number is required.");
            }

            if (activeConfirmationCodes.ContainsKey(request.Phone))
            {
                return BadRequest("A confirmation code is already sent for this phone number.");
            }

            var confirmationCode = GenerateConfirmationCode();
            activeConfirmationCodes[request.Phone] = confirmationCode;

            Console.WriteLine($"Confirmation code sent to {request.Phone}: {confirmationCode}");

            return Ok(new ApiResponse { Sent = true, GetThisCodeonPhone = confirmationCode });
        }

        public IHttpActionResult CheckConfirmationCode(CheckConfirmationCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.Code))
            {
                return BadRequest("Phone number and confirmation code are required.");
            }

            if (!activeConfirmationCodes.TryGetValue(request.Phone, out var activeCode))
            {
                return BadRequest("No active confirmation code found for this phone number.");
            }

            var isValid = string.Equals(request.Code, activeCode);

            lock (locker)
            {
                activeConfirmationCodes.Remove(request.Phone);
            }

            return Ok(new ApiResponse { Valid = isValid });
        }


        private string GenerateConfirmationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void CleanupExpiredConfirmationCodes()
        {
            var currentTime = DateTime.UtcNow;
            var keysToRemove = new List<string>();

            lock (locker)
            {
                foreach (var kvp in activeConfirmationCodes)
                {
                    if (currentTime - DateTime.Parse(kvp.Value) > CodeExpirationDuration)
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                foreach (var key in keysToRemove)
                {
                    activeConfirmationCodes.Remove(key);
                }
            }
        }
        public void Dispose()
        {
            CleanupExpiredConfirmationCodes();

            // Perform garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
