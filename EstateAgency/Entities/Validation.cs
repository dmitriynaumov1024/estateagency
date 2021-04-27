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
        public bool isValid { get; }
    }

    /// <summary>
    /// Validation exception. Useful to provide rich information about field invalidity.
    /// </summary>
    public class ValidationException: Exception 
    {
        public string ObjectTypeName;
        public string FieldName;
    }
}