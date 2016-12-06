
namespace Arxius.Services.PCL.Entities
{
    public enum ClassTypeEnum
    {
        Lecture,
        Excercise,
        Lab,
        Repetitory,
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
                case ClassTypeEnum.Repetitory:
                    return "Repetytorium";
                case ClassTypeEnum.Other:
                    return "Inne";
                default:
                    return "";
            }

        }
        public static string ToFriendlyShortString(this ClassTypeEnum me)
        {
            switch (me)
            {
                case ClassTypeEnum.Lecture:
                    return "W";
                case ClassTypeEnum.Excercise:
                    return "Ć";
                case ClassTypeEnum.Lab:
                    return "P";
                case ClassTypeEnum.Repetitory:
                    return "R";
                case ClassTypeEnum.Other:
                    return "I";
                default:
                    return "";
            }
        }
    }
    public enum DniTygodnia
    {
        Niedziela = 0,
        Poniedziałek = 1,
        Wtorek = 2,
        Środa = 3,
        Czwartek = 4,
        Piątek = 5,
        Sobota = 6
    }
}
