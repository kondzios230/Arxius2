
namespace Arxius.Services.PCL.Entities
{
    public enum ClassTypeEnum
    {
        Lecture,
        Excercise,
        Lab,
        Other
    }
    public static class ClassTypeEnums
    {
        public static string ToFriendlyString(this ClassTypeEnum me)
        {
            switch (me)
            {
                case ClassTypeEnum.Lecture:
                    return "Wykład";
                case ClassTypeEnum.Excercise:
                    return "Cwiczenia";
                case ClassTypeEnum.Lab:
                    return "Pracownia";
                case ClassTypeEnum.Other:
                    return "Inne";
                default:
                    return "";
            }
        }
    }
}
