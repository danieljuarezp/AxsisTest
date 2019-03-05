using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AxsisTest.Enums
{
    public enum GenderEnum
    {
        [Display(Name = "Male")]
        Male = 0,

        [Display(Name = "Female")]
        Female = 1,
    }
}