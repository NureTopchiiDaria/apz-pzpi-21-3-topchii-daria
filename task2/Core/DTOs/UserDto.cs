using Core.Enums;

#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

#pragma warning disable CA1704
public class UserDto
#pragma warning restore CA1704
{
   public ExperienceLevel Exp
   {
      get;
      set;
   }

   public void Aa(ExperienceLevel es)
   {
      if (es == ExperienceLevel.Amateur)
      {
         Console.WriteLine();
      }

      if (es == ExperienceLevel.Beginner)
      {
         Console.WriteLine();
      }

      if (es == ExperienceLevel.Proficcient)
      {
         Console.WriteLine();
      }
   }
}