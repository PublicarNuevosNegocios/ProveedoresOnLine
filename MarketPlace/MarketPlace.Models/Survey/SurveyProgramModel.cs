using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Survey
{
    public class SurveyProgramModel
    {
        public bool RenderScripts { get; set; }

        public MarketPlace.Models.Project.ProjectViewModel CurrentProject { get; set; }
    }
}
