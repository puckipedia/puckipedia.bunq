# puckipedia.bunq

This is a library to communicate with bunq's API using .NET. It is written to be as simple as possible, while not abstracting too much.
Each async method call corresponds with one API call. For example:

    await monetaryAccount.CreatePayment(new Payment.Request
    {
        Amount = CurrencyAmount.EUR(200m),
        CounterpartyAlias = Pointer.IBAN("NL09BUNQ2025429452", "Puck Meerburg"),
        Description = "Send me money"
    });

This library call is equal to:

    POST /v1/user/24/monetary-account/12/payment HTTP/1.1
	Cache-Control: no-cache

	{"amount": {"currency": "EUR", "value": "200"}, "counterparty_alias": {"type": "IBAN", "name": "Puck Meerburg", "value": "NL09BUNQ2025429452"}, "description": "Send me money"}

	HTTP/1.1 200 OK

	{"Response": [{"Id": {"id": 1234}}]}

and thus the return value of `CreatePayment` is an int, 1234. There is also a helper called `CreateAndGetPayment`, which afterwards directly retrieves the payment object.

# Howto

All API calls in this library are implemented as extension methods on the object a layer below. For example, to get all Payments, you call an extension method on the MonetaryAccountBank.

To create a session, use `BunqSession.Create(string apiHostname, string userAgent, string description, string apiKey, List<string> permittedIPs)`. The values should speak for themselves.

Then, to get the monetary accounts, use `ListMonetaryAccountBank(this BunqSession session, int count = 10)`. For example, `session.ListMonetaryAccountBank(200)`. These return a `BunqPaginatedResponse<MonetaryAccountBank>` which can be enumerated.
See puckipedia.bunq.test for a small sample application that registers with the sandbox API, gets the latest payment, and 'reverts' it (sends the money back, or tries to send a RequestInquiry to ask for the money)

# Of note
Many POST API calls return only an ID or UUID. Because each API call costs money, I have decided to have the Create APIs return the ID that was created, as the API does. If you want to get the full object from the API, you will have to manually request it, or use CreateAndGet. Note that both ways cost 2 API calls, instead of 1.

On my todo list is also to add id-only variants of these APIs. So you could do Payment.Create(int monetaryAccountId); which is slightly more friendly if you don't want to keep GETting the MonetaryAccountBank.

# Implementation status
The base of the API is stable and can be easily iterated on, though not all objects and API calls are implemented.

- Installation
	- [x] Installation
	- [x] InstallationServerPublicKey (only the object)
	- [ ] Device
	- [x] DeviceServer (only the object)
	- [ ] PermittedIP
	- [ ] UserCredentialPasswordIP
- Session
	- [ ] Session (I don't extend session lifetimes, as it's easier to just set up a new session)
	- [x] SessionServer (POST only)
- User
	- [x] User (all the calls!)
		- [x] UserPerson
		- [x] UserCompany
	- [ ] ScheduleUser (unclear API)
- Monetary Accounts
	- [x] MonetaryAccount (empty object, but used as base anyways)
		- [x] MonetaryAccountBank (everything)
- Payments
	- [x] Payment (implemented all of it)
	- [x] DraftPayment (literally implemented, but could improve the library API, once the docs clear it up a bit)
	- [x] PaymentBatch (completely implemented)
	- [x] RequestInquiry
	- [x] RequestInquiryBatch
	- [x] RequestResponse
	- [ ] ScheduleInstance
	- [ ] Schedule
		- [ ] SchedulePayment
		- [ ] SchedulePaymentBatch
	- [ ] PaymentChat
	- [ ] RequestInquiryChat
	- [ ] RequestResponseChat
- Tabs
	- [x] CashRegister
	- [ ] CashRegisterQrCode (object only)
	- [ ] CashRegisterQrCodeContent (returns image)
	- [ ] Tab (object only)
	- [ ] TabItem (object only)
	- [ ] TabItemBatch (object only)
	- [ ] TabUsageSingle (object only)
	- [ ] TabUsageMultiple (object only)
	- [ ] TabQrCodeContent (returns image)
	- [ ] TabResultInquiry (object only)
	- [ ] TabResultResponse (object only)
	- note: the Tab API might have some issues when implementing, as these items don't have a parent ID, and are also nested. Still working on it though!
- Cards
	- [ ] Card (object only)
		- [ ] CardDebit (object only)
	- [ ] CardName
	- [ ] MasterCardAction
- Connect
	- [ ] ShareInviteBankInquiry
	- [ ] ShareInviteBankResponse
	- [ ] ShareInviteBankAmountUsed (just DELETE, for resetting budget)
	- [ ] DraftShareInviteBank (this is where my API design will shine, I think :P)
	- [ ] DraftShareInviteBankQrCodeContent
- Chat
	- [ ] ChatConversation
	- [ ] ChatMessage
		- [ ] ChatMessageAttachment
		- [ ] ChatMessageText
- Invoice
	- [ ] Invoice
	- [ ] InvoiceByUser
- Exports
	- [ ] CustomerStatementExport
	- [ ] CustomerStatementExportContent
	- [ ] ExportAnnualOverview
	- [ ] ExportAnnualOverviewContent
- Callbacks
	- [ ] CertificatePinned
- Attachments
	- [ ] Avatar (only as object)
	- [ ] AttachmentPublic (only as object)
	- [ ] AttachmentPublicContent
	- [ ] AttachmentMonetaryAccount
	- [ ] AttachmentTab (only as object)
	- [ ] AttachmentTabContent
	- [ ] TabAttachmentTab
	- [ ] TabAttachmentTabContent
	- [ ] AttachmentConversation (only as object)
	- [ ] AttachmentConversationContent
