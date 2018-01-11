using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    public enum GradeEm
    {
        [Description("不限")]
        unlimited = -1,
        [Description("学前")]
        preschool = 0,
        [Description("1年级")]
        firstgrade = 1,
        [Description("2年级")]
        secondgrade = 2,
        [Description("3年级")]
        gradethree = 3,
        [Description("4年级")]
        fourthgrade = 4,
        [Description("5年级")]
        fifthgrade = 5,
        [Description("6年级")]
        gradesix = 6,
        [Description("7年级")]
        gradeseven = 7,
        [Description("8年级")]
        gradeeight = 8,
        [Description("9年级")]
        gradenine = 9,
        [Description("10年级")]
        gradeten = 10,
        [Description("11年级")]
        gradeeleven = 11,
        [Description("12年级")]
        gradetwelve = 12,
        [Description("13年级")]
        gradethirteen = 13
    }
}
