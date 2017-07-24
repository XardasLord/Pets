namespace Pets.Infrastructure.Exceptions
{
    public static class ErrorCodes
    {
        public static string EmailInUse => "email_in_use";
        public static string InvalidCredentials => "invalid_credentials";
        public static string InvalidEmail => "invalid_email";
        public static string UserAlreadyExist => "user_already_exist";
        public static string UserNotFound => "user_not_found";
        public static string AnimalNotFound => "animal_not_found";
        public static string AnimalNotAvailable => "animal_not_available";
    }
}
