using Microsoft.AspNetCore.Identity;

namespace backend.Auth{
    public class Authenticator
    {
        private readonly PasswordHasher<object> _passwordHasher;

        //constructor
        public Authenticator(PasswordHasher<object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        // hashing password

        public string HashPassword(string plainPassword)
        {
            return _passwordHasher.HashPassword(new object(), plainPassword); //plain = unhashed
        }

        public bool MatchHashedPassword(string hashedPassword, string plainPassword)//hashedPassword = from db & hashed , plainPassword = from user & unhashed comes from form
        {
            var result = _passwordHasher.VerifyHashedPassword(new object(), hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}