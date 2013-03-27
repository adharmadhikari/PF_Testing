using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;

namespace Pinsonault.Security
{

    /// <summary>
    /// Pinsonault.Web specific security methods and properties.
    /// </summary>
    public class Security
    {
        private Security() { }

        /// <summary>
        /// Static constructor loads defaults when application starts up.
        /// </summary>
        static Security()
        {
            try
            {
                //Password Strength
                string val = ConfigurationManager.AppSettings["passwordStrengthRegularExpression"];
                if ( !string.IsNullOrEmpty(val) )
                    _passwordStrengthRegularExpression = val;

                //Min Password Length
                val = ConfigurationManager.AppSettings["minPasswordLength"];
                if ( !string.IsNullOrEmpty(val) )
                {
                    int.TryParse(val, out _minPasswordLength);
                }

                //Min NonAlphanumeric Chars
                val = ConfigurationManager.AppSettings["minRequiredNonAlphanumericCharacters"];
                if ( !string.IsNullOrEmpty(val) )
                {
                    int.TryParse(val, out _minRequiredNonAlphanumericCharacters);
                }
            }
            catch
            {
                //avoid exceptions in static constructor
            }
        }

        static string _passwordStrengthRegularExpression = @"(?=.{6,})(?=(.*\d){1,})(?=(.*\W){1,})";
        /// <summary>
        /// Returns the configured regular expression used for testing the strength of a password.
        /// </summary>
        public static string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        //passwrod regular expression for alcon
        static string _passwordStrengthRegularExpressionAlcon = @"^(((?=.*[a-z])(?=.*[A-Z])(?=.*[\d]))|((?=.*[a-z])(?=.*[A-Z])(?=.*[\W]))|((?=.*[a-z])(?=.*[\d])(?=.*[\W]))|((?=.*[A-Z])(?=.*[\d])(?=.*[\W]))).{8,}$";
        /// <summary>
        /// Returns the configured regular expression used for testing the strength of a password for alcon client.
        /// </summary>
        public static string PasswordStrengthRegularExpressionAlcon
        {
            get { return _passwordStrengthRegularExpressionAlcon; }
        }

        static int _minPasswordLength = 6;
        /// <summary>
        /// Returns the configured minimum length for a password.
        /// </summary>
        public static int MinPasswordLength
        {
            get { return _minPasswordLength; }
        }

        static int _minPasswordLengthAlcon = 8;
        /// <summary>
        /// Returns the configured minimum length for a password.
        /// </summary>
        public static int MinPasswordLengthAlcon
        {
            get { return _minPasswordLengthAlcon; }
        }

        static int _minRequiredNonAlphanumericCharacters = 1;
        /// <summary>
        /// Returns the configured minimum number of non-alphanumeric characters required in a password.
        /// </summary>
        public static int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        /// <summary>
        /// Helper function for creating a hashed password for a user.  The UserName is used as a salt to make the hash unique for each user even if they have the same password.
        /// </summary>
        /// <param name="UserName">Unique name assigned to a user such as an email address.</param>
        /// <param name="Password">User's unencrypted password.</param>
        /// <returns>Returns a Base64 encoded string of the hashed password.</returns>
        public static string CreateHashedPassword(string UserName, string Password)
        {
            return HashValue(Password + UserName.ToLower());
        }

        /// <summary>
        /// Helper function for creating a hashed value from a specified string.
        /// </summary>
        /// <param name="Value">Value to hash.</param>
        /// <returns>Returns a Base64 encoded string of the hashed value.</returns>
        public static string HashValue(string Value)
        {
            if ( string.IsNullOrEmpty(Value) )
                throw new ArgumentNullException("Value", "HashValue requires a parameter.");

            SHA256 hash = SHA256Managed.Create();

            string result = Convert.ToBase64String(hash.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(Value)));
            hash.Clear();

            return result;
        }
    }
}