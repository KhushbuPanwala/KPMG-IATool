using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.DomainModel.Constants
{
    public class RegexConstant
    {
        //To check decimal number
        public const string DecimalNumberRegex = "^[0-9]*([.][0-9]{1,2})?$";

        //To Check Whther a number is natural 
        public const string NaturalNumberRegex = "^[0-9]*$";

        //To check all types of emails
        public const string EmailAddressRegex = "^[a-zA-Z0-9#._-]+@[a-zA-Z0-9.-]+([.][a-zA-Z]{2,63})$";

        //To check all types of names
        public const string PersonNameRegex = "^[a-zA-Z' ]*$";

        //To verify any input with this mentioned charectors only 
        public const string CommonInputRegex = "^[a-zA-Z0-9&'!:;,.\\-_@(){}\\[\\]\\/ ]*$";

        //To verify country and state name
        public const string CountryStateRegex = "^[a-zA-Z'-()\\[\\] ]{2,}$";

        //To verify any alphanumeric string 
        public const string AlphanumericRegex = "^[a-zA-Z0-9 ]*$";

        // To verify only date
        public const string DateRegex = "^\\d{4}\\-(0?[1 - 9]|1[012])\\-(0?[1 - 9]|[12] [0-9]|3[01])$";

        // To verify only time
        public const string TimeRegex = "^([0 - 1]?[0 - 9]|2[0-3]):[0-5] [0-9]$";

        //Max length of textboxes
        
        public const int MaxInputLength = 256;
        //Max length of audit Ratings
        public const int RatingsLength = 12;

        //Max length of risk assessment year in auditable entity
        public const int YearLength = 4;

        //Max length of RCM process and subprocess  textboxes
        public const int ProcessMaxInputLength = 70;
        public const int SubProcessMaxInputLength = 140;

        //Max length for every process and subprocess
        public const int ProcessSubProcessMaxLength = 140;


    }
}
