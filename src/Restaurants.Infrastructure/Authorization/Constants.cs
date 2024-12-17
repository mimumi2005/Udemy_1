namespace Restaurants.Infrastructure.Authorization
{
    public static class PolicyNames
    {
        public const string HasNationality = "HasNationality";
        public const string AtLeast20 = "AtLeast20";
        public const string AtLeast2 = "Atleast2";

    }

    public static class AppClaimTypes
    {
        public const string Nationality = "HasNationality";
        public const string DateOfBirth = "DateOfBirth";
        public const string RestaurantCount = "RestaurantCount";
    }
}
