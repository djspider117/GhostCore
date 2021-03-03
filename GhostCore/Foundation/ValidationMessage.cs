namespace GhostCore.Foundation
{
    /// <summary>
    /// Provides data for error handling after content validation
    /// </summary>
    public class ValidationMessage
    {
        /// <summary>
        /// True if this message constitute an error, false for warnings or information. 
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; set; }

        public string Prefix { get; set; }

        /// <summary>
        /// Creates an instance of a <see cref="ValidationMessage"/>
        /// </summary>
        /// <param name="msg">The message</param>
        public ValidationMessage(string msg)
        {
            Message = msg;
        }
        /// <summary>
        /// Creates an instance of a <see cref="ValidationMessage"/>
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="isError">True if error, false otherwise</param>
        public ValidationMessage(bool isError, string msg) : this(msg)
        {
            IsError = isError;
        }

        public ValidationMessage WithPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
        }

        public override string ToString()
        {
            return $"{Prefix}[isError={IsError}][message={Message}]";
        }

    }
}
