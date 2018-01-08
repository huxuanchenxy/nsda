using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MenuData
    {
        public string MenuCode { get; set; }
        public string MenuIcon { get; set; }
        public int MenuId { get; set; }
        public int MenuIndex { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public int ParentMenuId { get; set; }
    }

    public class ButtonData
    {
        public string ButtonAction { get; set; }
        public string ButtonCode { get; set; }
        public string ButtonIcon { get; set; }
        public int ButtonId { get; set; }
        public int ButtonIndex { get; set; }
        public string ButtonName { get; set; }
        public int MenuId { get; set; }
    }
}
