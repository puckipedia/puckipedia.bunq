using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using puckipedia.bunq.ApiCalls;
using System.Net.Http;
using System.IO;
using puckipedia.bunq.MonetaryAccount;
using puckipedia.bunq.Payment;
using puckipedia.bunq.Shared;

namespace puckipedia.bunq.test
{
    public class Program
    {
        public static async Task<int> Do()
        {
            BunqSession session;
            session = await BunqSession.Create("sandbox.public.api.bunq.com", "puckipedia.bunq.test", "Test API client", "[put API key here]", null);

            File.WriteAllText("resume.json", session.StoreResumeData());

            var monetaryAccount = (await session.ListMonetaryAccountBank()).First();

            var lastPayment = (await monetaryAccount.ListPayment()).First();
            Console.WriteLine($"Last payment was {lastPayment.Description} to {lastPayment.CounterpartyAlias.DisplayName} - {lastPayment.Amount}");
            if (lastPayment.Amount.IsNegative) // payment went out
            {
                if (lastPayment.Type == PaymentType.BUNQ)
                {
                    await monetaryAccount.CreateAndGetRequestInquiry(new RequestInquiry.Request
                    {
                        AmountInquired = -lastPayment.Amount,
                        CounterpartyAlias = Pointer.IBAN(lastPayment.CounterpartyAlias.IBAN, lastPayment.CounterpartyAlias.DisplayName),
                        Description = "Revert payment " + lastPayment.Description
                    });

                    Console.WriteLine("Made a request to ask for money back");
                }
                else
                    Console.WriteLine("Can't 'revert' the payment, not a bunq payment");
            }
            else
            {
                var payment = await monetaryAccount.CreateAndGetPayment(new Payment.Payment.Request
                {
                    Amount = lastPayment.Amount,
                    CounterpartyAlias = Pointer.IBAN(lastPayment.CounterpartyAlias.IBAN, lastPayment.CounterpartyAlias.DisplayName),
                    Description = "Revert payment " + lastPayment.Description
                });

                Console.WriteLine("Sent payment to return money");
            }

            Console.Write("[press enter to exit] ");
            Console.ReadLine();
            return 0;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Example code: Sets up an installation and DeviceServer, then stores all the info required into resume.json.");
            var a = Do().Result;
        }
    }
}
