using System;

namespace EstateAgency.Entities
{
    /// <summary>
    /// IValidable interface provides isValid property for checking an instance.
    /// It is recommended to check isValid property before each insertion of data into the database.
    /// It is <i>critical</i> to check isValid property when object fields were set manually.
    /// isValid property reduces overall load because every field is validated at once.
    /// isValid should only check the correspondence to domain, not to look into the database
    /// for other records.
    /// </summary>
    public interface IValidable
    {
        /// <summary>
        /// Check whether fields of the instance correspond to domain, using predefined criterias.
        /// </summary>
        public ValidationResult Validate { get; }
    }

    /// <summary>
    /// Validation exception. Useful to provide rich information about field invalidity.
    /// </summary>
    public class ValidationException: Exception 
    {
        public string ObjectTypeName;
        public string FieldName;
        public ValidationException (string message): base(message) { }
        public ValidationException (string message, string objectTypeName, string fieldName): 
            base(message) 
        { 
            this.ObjectTypeName = objectTypeName;
            this.FieldName = fieldName;
        }
    }

    /// <summary>
    /// Validation result. Useful to provide information about field invalidity.
    /// </summary>
    public class ValidationResult
    {
        public string ObjectTypeName;
        public string FieldName;
        public string Message;
        public bool isValid;

        /// <summary>
        /// Successful validation result.
        /// </summary>
        public static readonly ValidationResult Success = new ValidationResult
        {
            isValid = true,
            ObjectTypeName = "unknown",
            FieldName = "unknown",
            Message = "Succesful validation"
        };

        /// <summary>
        /// Simple failed validation result.
        /// </summary>
        public static readonly ValidationResult Fail = new ValidationResult
        {
            isValid = false,
            ObjectTypeName = "unknown",
            FieldName = "unknown",
            Message = "Validation failed"
        };

        public ValidationResult () { }

        public ValidationResult (string message, string fieldName="unknown", string objectTypeName="unknown")
        {
            this.Message = message;
            this.FieldName = fieldName;
            this.ObjectTypeName = objectTypeName;
            this.isValid = false;
        }
    }
}