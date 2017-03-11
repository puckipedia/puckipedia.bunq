using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.User
{
    public enum UserPersonGender
    {
        MALE, FEMALE, UNKNOWN
    } // tag yourself

    [BunqEntity("UserPerson")]
    public class UserPerson : User
    {
        public struct TaxResidentLocation
        {
            [JsonProperty("country")] public string Country { get; private set; }
            [JsonProperty("tax_number")] public string TaxNumber { get; private set; }
        }

        [JsonProperty("first_name")] public string FirstName { get; private set; }
        [JsonProperty("middle_name")] public string MiddleName { get; private set; }
        [JsonProperty("last_name")] public string LastName { get; private set; }
        [JsonProperty("legal_name")] public string LegalName { get; private set; }
        [JsonProperty("tax_resident")] public TaxResidentLocation[] TaxResident { get; private set; }
        [JsonProperty("date_of_birth")] public DateTime DateOfBirth { get; private set; }
        [JsonProperty("place_of_birth")] public string PlaceOfBirth { get; private set; }
        [JsonProperty("country_of_birth")] public string CountryOfBirth { get; private set; }
        [JsonProperty("nationality")] public string Nationality { get; private set; }
        [JsonProperty("gender"), JsonConverter(typeof(StringEnumConverter))] public UserPersonGender Gender { get; private set; }
        [JsonProperty("legal_guardian_alias")] public Shared.LabelUser? LegalGuardianAlias { get; private set; }
        [JsonProperty("document_number")] public string DocumentNumber { get; private set; }
        [JsonProperty("document_type")] public string DocumentType { get; private set; } // xxx bunq: what document types?
        [JsonProperty("document_country_of_issuance")] public string DocumentCountryOfIssuance { get; private set; }
        [JsonProperty("document_front_attachment")] public Shared.Attachment DocumentFrontAttachment { get; private set; }
        [JsonProperty("document_back_attachment")] public Shared.Attachment? DocumentBackAttachment { get; private set; }
    }
}
