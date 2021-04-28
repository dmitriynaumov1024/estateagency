using System;

namespace EstateAgency.Database
{
    /// <summary>
    /// Represents a referential integrity exception - means that
    /// some key already exist or not exist yet.
    /// </summary>
    public class ReferentialException: Exception
    {
        public string Operation;
        public string TableName;
        public string FieldName;
        public string ReadableMessage;
        public ReferentialException () { }
        public ReferentialException (string message) : base(message) { }
    }
}